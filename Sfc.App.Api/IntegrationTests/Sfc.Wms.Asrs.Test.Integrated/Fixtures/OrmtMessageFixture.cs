using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RestSharp;
using Sfc.Core.OnPrem.Result;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData;
using Sfc.Wms.Foundation.InboundLpn.Contracts.Dtos;
using Sfc.Wms.Interfaces.ParserAndTranslator.Contracts.Constants;
using System.Configuration;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures
{
    public class OrmtMessageFixture: DataBaseFixtureForOrmt
    {
        protected BaseResult Result;
        protected string CurrentCartonNbr;
        protected string CurrentActionCode;
        protected string Url = @ConfigurationManager.AppSettings["OrmtUrl"];
        protected CaseDetailDto CaseDetailDto;
        protected ComtParams OrmtParameters;
        protected IRestResponse Response;
        protected string OrmtUrl;
        protected BaseResult Negativecase1;
        protected BaseResult Negativecase2;
        protected BaseResult NegativeCase3;
        protected BaseResult NegativeCase4;
        protected BaseResult NegativeCase5;
        protected string WaveUrl = "";

        protected void InitializeTestDataForPrintingOfCartons()
        {
            GetDataBeforeTriggerOrmtForPrintingOfCartons();
        }
        protected void InitializeTestDataForCancelOfCarton()
        {
            GetDataBeforeTriggerOrmtForCancellationOfOrders();
        }

        protected void InitializeTestDataForEpickOfCarton()
        {
            GetDataBeforeCallingApiForEpickOfOrders();
        }

        protected void InitializeTestDataForWaveRelease()
        {
            //GetValidDataBeforeTriggerOrmtForPrintingOfCartonsThroughWaveNumber();
        }
        protected void ReadDataAndValidateTheFieldsInInternalTables()
        {
            //GetDataAfterCallingOrmtApiAfterWaveRelease();
        }
        protected void ReadDataAfterApiForPrintingOfCarton()
        {
            GetDataAfterCallingOrmtApiForAddRelease();
        }
        protected void ReadDataAfterApiForCancelOfCarton()
        {
            GetDataAfterCallingApiForCancellationOfOrders();
        }
        protected void InitializeTestDataForNegativeCases()
        {
            GetDataForNegativeCases();
        }

        protected void ReadDataAfterApiForEPickOfCarton()
        {
            GetDataAfterCallingApiForEPickOrders();
        }
        protected void CartonNumberForAddRelease()
        {
            CurrentCartonNbr = PrintCarton.CartonNbr;
            CurrentActionCode = "AddRelease";
        }
        protected void CartonNumberForCancel()
        {
            CurrentCartonNbr = CancelOrder.CartonNbr;
            CurrentActionCode = "Cancel";
        }
       
        protected void CartonNumberForEPick()
        {
            CurrentCartonNbr = EPick.CartonNbr;
            CurrentActionCode = "AddRelease";
        }

        protected void CartonNumberForOrmtCountNotFound()
        {
            CurrentCartonNbr = ActiveOrmtCountNotFound.CartonNbr;
            CurrentActionCode = "AddRelease";
        }

        protected void CartonNumberForPickLocnNotFound()
        {
            CurrentCartonNbr = PickLocnNotFound.CartonNbr;
            CurrentActionCode = "AddRelease";
        }

        protected void CartonNumberForActiveLocnNotFound()
        {
            CurrentCartonNbr = ActiveLocnNotFound.CartonNbr;
            CurrentActionCode = "AddRelease";
        }

        protected void CartonNumberForInvalidCartonNumber()
        {
            CurrentCartonNbr = CancelOrder.CartonNbr;
            CurrentActionCode = "AddRelease";
        }

        protected void TestForInValidActionCode()
        {
            CurrentCartonNbr = PrintCarton.CartonNbr;
            CurrentActionCode = "Adddd";
        }

        protected void ValidOrmtUrl()
        {
            OrmtUrl = $"{Url}?{"cartonNumber"}={CurrentCartonNbr}&{"actionCode"}={CurrentActionCode}";            
        }

        protected IRestResponse ApiIsCalled(string url)
        {
            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);     
            Response = client.Execute(request);
            return Response;
        }

        protected BaseResult OrmtResult()
        {
            var response = ApiIsCalled(OrmtUrl);
            BaseResult result = JsonConvert.DeserializeObject<BaseResult>(response.Content);
            return result;
        }

        protected void OrmtApiIsCalledCreatedIsReturned()
        {
            Result = OrmtResult();
            Assert.AreEqual("Created", Result.ResultType.ToString());
        }

        protected void OrmtApiIsCalledCreatedIsReturnedForWaveRelease()
        {
            var response = ApiIsCalled(WaveUrl);
            var result = JsonConvert.DeserializeObject<BaseResult>(response.Content);
            Assert.AreEqual("Created", result.ResultType.ToString());
        }
        protected void OrmtApiIsCalledForNotEnoughInventory()
        {
            Negativecase1 = OrmtResult();         
        }

        protected void OrmtApiIsCalledForPickLocationNotFound()
        {
            Negativecase2 = OrmtResult();     
        }

        protected void OrmtApiIsCalledForActiveLocationNotFound()
        {
            NegativeCase3 = OrmtResult();   
        }

        protected void OrmtApiIsCalledForInvalidCartonNumber()
        {
            NegativeCase4 = OrmtResult();   
        }
        protected void OrmtApiIsCalledForInvalidActionCode()
        {
            NegativeCase5 = OrmtResult();   
        }

        protected void ValidateResultForActiveOrmtNotFound()
        {
            Assert.AreEqual("PickLocationDetailsExtenstion", Negativecase1.ValidationMessages[0].FieldName);
            Assert.AreEqual("Not Enough Inventory in Case", Negativecase1.ValidationMessages[0].Message);
        }
        protected void ValidateResultForPickLocationNotFound()
        {
            Assert.AreEqual("ActiveLocationDto", Negativecase2.ValidationMessages[0].FieldName);
            Assert.AreEqual("Not found", Negativecase2.ValidationMessages[0].Message);
        }
        protected void ValidateResultForActiveLocationNotFound()
        {
            Assert.AreEqual("ActiveLocationDto", NegativeCase3.ValidationMessages[0].FieldName);
            Assert.AreEqual("Not found", NegativeCase3.ValidationMessages[0].Message);
        }
        protected void ValidateResultForInvalidCartonNumber()
        {
            Assert.AreEqual("cartonNumber-String", NegativeCase4.ValidationMessages[0].FieldName);
            Assert.AreEqual("Invalid Data", NegativeCase4.ValidationMessages[0].Message);
        }
        protected void ValidateResultForInvalidActionCode()
        {
            /* once the validation messages is up then will test this code*/
           // Assert.AreEqual("cartonNumber-String", negativeCase5.ValidationMessages[0].FieldName);
           // Assert.AreEqual("Invalid Data", negativeCase5.ValidationMessages[0].Message);
        }
        protected void VerifyOrmtMessageWasInsertedInToSwmToMhe()
        {
            Assert.AreEqual("Ready", SwmToMheAddRelease.SourceMessageStatus);
            Assert.AreEqual(TransactionCode.Ormt, Ormt.TransactionCode);
            Assert.AreEqual(MessageLength.Ormt, Ormt.MessageLength);
            Assert.AreEqual("AddRelease", Ormt.ActionCode);
            Assert.AreEqual(PrintCarton.SkuId, Ormt.Sku);
            Assert.AreEqual(PrintCarton.TotalQty, Ormt.Quantity);
            Assert.AreEqual("Case", Ormt.UnitOfMeasure);
            Assert.AreEqual(PrintCarton.CartonNbr, Ormt.OrderId);
            Assert.AreEqual("1",Ormt.OrderLineId);
            Assert.AreEqual("SHIPMENT",Ormt.OrderType);
            Assert.AreEqual(PrintCarton.WaveNbr,Ormt.WaveId);
            Assert.AreEqual("N",Ormt.EndOfWaveFlag);
            Assert.AreEqual(PrintCarton.DestLocnId+"-"+PrintCarton.ShipWCtrlNbr,Ormt.DestinationLocationId);
            Assert.AreEqual(PrintCarton.Whse + PrintCarton.Co+PrintCarton.Div,Ormt.Owner);
            Assert.AreEqual("FIFO",Ormt.OpRule);
        }

        protected void VerifyOrmtMessageWasInsertedInToSwmToMheForCancelOrders()
        {
            Assert.AreEqual("Ready", SwmToMheCancelation.SourceMessageStatus);
            Assert.AreEqual(TransactionCode.Ormt, OrmtCancel.TransactionCode);
            Assert.AreEqual(MessageLength.Ormt, OrmtCancel.MessageLength);
            Assert.AreEqual("Cancel", OrmtCancel.ActionCode);
            Assert.AreEqual(CancelOrder.SkuId, OrmtCancel.Sku);
            Assert.AreEqual(CancelOrder.TotalQty, OrmtCancel.Quantity);
            Assert.AreEqual("Case", OrmtCancel.UnitOfMeasure);
            Assert.AreEqual(CancelOrder.CartonNbr, OrmtCancel.OrderId);
            Assert.AreEqual("1", OrmtCancel.OrderLineId);
            Assert.AreEqual("SHIPMENT", OrmtCancel.OrderType);
            Assert.AreEqual(CancelOrder.WaveNbr, OrmtCancel.WaveId);
            Assert.AreEqual("N", OrmtCancel.EndOfWaveFlag);
            Assert.AreEqual(CancelOrder.DestLocnId +"-"+ CancelOrder.ShipWCtrlNbr, OrmtCancel.DestinationLocationId);
            Assert.AreEqual(CancelOrder.Whse + CancelOrder.Co + CancelOrder.Div, OrmtCancel.Owner);
            Assert.AreEqual("FIFO", OrmtCancel.OpRule);
        }
        protected void VerifyOrmtMessageWasInsertedInToSwmToMheForEpick()
        {
            Assert.AreEqual("Ready", SwmToMheEPick.SourceMessageStatus);
            Assert.AreEqual(TransactionCode.Ormt, OrmtEPick.TransactionCode);
            Assert.AreEqual(MessageLength.Ormt, OrmtEPick.MessageLength);
            Assert.AreEqual("AddRelease", OrmtEPick.ActionCode);
            Assert.AreEqual(EPick.SkuId, OrmtEPick.Sku);
            Assert.AreEqual(EPick.TotalQty, OrmtEPick.Quantity);
            Assert.AreEqual("Case", OrmtEPick.UnitOfMeasure);
            Assert.AreEqual(EPick.CartonNbr, OrmtEPick.OrderId);
            Assert.AreEqual("1", OrmtEPick.OrderLineId);
            Assert.AreEqual("SHIPMENT", OrmtEPick.OrderType);
            Assert.AreEqual(EPick.WaveNbr, OrmtEPick.WaveId);
            Assert.AreEqual("N", OrmtEPick.EndOfWaveFlag);
            Assert.AreEqual(EPick.DestLocnId +"-"+ EPick.ShipWCtrlNbr, OrmtEPick.DestinationLocationId);
            Assert.AreEqual(EPick.Whse + EPick.Co + EPick.Div, OrmtEPick.Owner);
            Assert.AreEqual("FIFO", OrmtEPick.OpRule);
        }

        protected void VerifyOrmtMessageWasInsertedInToWmsToEmsForPrintingOfOrder()
        {
            Assert.AreEqual(SwmToMheAddRelease.SourceMessageStatus, WmsToEmsAddRelease.Status);
            Assert.AreEqual(TransactionCode.Ormt, WmsToEmsAddRelease.Transaction);
            Assert.AreEqual("MessageBuilder", WmsToEmsAddRelease.Process);
            Assert.AreEqual(SwmToMheAddRelease.SourceMessageKey, WmsToEmsAddRelease.MessageKey);
            Assert.AreEqual(SwmToMheAddRelease.SourceMessageTransactionCode, WmsToEmsAddRelease.Transaction);
            Assert.AreEqual(SwmToMheAddRelease.SourceMessageText, WmsToEmsAddRelease.MessageText);
            Assert.AreEqual(SwmToMheAddRelease.SourceMessageResponseCode, WmsToEmsAddRelease.ResponseCode);
            Assert.AreEqual(SwmToMheAddRelease.ZplData, WmsToEmsAddRelease.ZplData);
        }

        protected void VerifyOrmtMessageWasInsertedInToWmsToEmsForCancelOrder()
        {
            Assert.AreEqual(SwmToMheCancelation.SourceMessageStatus, WmsToEmsCancelation.Status);
            Assert.AreEqual(TransactionCode.Ormt, WmsToEmsCancelation.Transaction);
            Assert.AreEqual("MessageBuilder", WmsToEmsCancelation.Process);
            Assert.AreEqual(SwmToMheCancelation.SourceMessageKey, WmsToEmsCancelation.MessageKey);
            Assert.AreEqual(SwmToMheCancelation.SourceMessageTransactionCode, WmsToEmsCancelation.Transaction);
            Assert.AreEqual(SwmToMheCancelation.SourceMessageText, WmsToEmsCancelation.MessageText);
            Assert.AreEqual(SwmToMheCancelation.SourceMessageResponseCode, WmsToEmsCancelation.ResponseCode);
            Assert.AreEqual(SwmToMheCancelation.ZplData, WmsToEmsCancelation.ZplData);
        }

        protected void VerifyOrmtMessageWasInsertedInToWmsToEmsForEpickOfOrder()
        {
            Assert.AreEqual(SwmToMheEPick.SourceMessageStatus, WmsToEmsEPick.Status);
            Assert.AreEqual(TransactionCode.Ormt, WmsToEmsEPick.Transaction);
            Assert.AreEqual("MessageBuilder", WmsToEmsEPick.Process);
            Assert.AreEqual(SwmToMheEPick.SourceMessageKey, WmsToEmsEPick.MessageKey);
            Assert.AreEqual(SwmToMheEPick.SourceMessageTransactionCode, WmsToEmsEPick.Transaction);
            Assert.AreEqual(SwmToMheEPick.SourceMessageText, WmsToEmsEPick.MessageText);
            Assert.AreEqual(SwmToMheEPick.SourceMessageResponseCode, WmsToEmsEPick.ResponseCode);
            Assert.AreEqual(SwmToMheEPick.ZplData, WmsToEmsEPick.ZplData);
        }
        protected void VerifyForOrmtCountInPickLocnDtlExt()
        {
           // Assert.AreEqual(pickLcnDtlExtBeforeApi.ActiveOrmtCount + 1, pickLcnDtlExtAfterApi.ActiveOrmtCount);
        }

        protected void VerifyForStatusCodeinCartonHdrForAddRelease()
        {
            Assert.AreEqual(12, CartonHdr.StatusCode);
        }
           
        protected void VerifyForStatusCodeInCartonHdrForEPick()
        {
            Assert.AreEqual(12, CartonHdr.StatusCode);
        }
    }
}
