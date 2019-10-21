using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RestSharp;
using Sfc.Core.OnPrem.Result;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData;
using Sfc.Wms.InboundLpn.Contracts.Dtos;
using Sfc.Wms.Interface.ParserAndTranslator.Contracts.Constants;


namespace Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures
{
    public class OrmtMessageFixture: DataBaseFixtureForOrmt
    {
        protected BaseResult Result;
        protected string currentCartonNbr;
        protected string currentActionCode;
        protected string baseUrl = "http://localhost:59665/api/order-maintenance/carton-number";
        protected CaseDetailDto caseDetailDto;
        protected ComtParams OrmtParameters;
        protected IRestResponse Response;
        protected string ormtUrl;
        protected BaseResult negativecase1;
        protected BaseResult negativecase2;
        protected BaseResult negativeCase3;
        protected BaseResult negativeCase4;
        protected BaseResult negativeCase5;
        protected string waveUrl = "http://localhost:59665/api/order-maintenance/wave-number?waveNumber=20190804052";

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
            GetDataAfterCallingOrmtApiAfterWaveRelease();
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
            currentCartonNbr = printCarton.CartonNbr;
            currentActionCode = "AddRelease";
        }
        protected void CartonNumberForCancel()
        {
            currentCartonNbr = cancelOrder.CartonNbr;
            currentActionCode = "Cancel";
        }
       
        protected void CartonNumberForEPick()
        {
            currentCartonNbr = ePick.CartonNbr;
            currentActionCode = "AddRelease";
        }

        protected void CartonNumberForOrmtCountNotFound()
        {
            currentCartonNbr = activeOrmtCountNotFound.CartonNbr;
            currentActionCode = "AddRelease";
        }

        protected void CartonNumberForPickLocnNotFound()
        {
            currentCartonNbr = pickLocnNotFound.CartonNbr;
            currentActionCode = "AddRelease";
        }

        protected void CartonNumberForActiveLocnNotFound()
        {
            currentCartonNbr = ActiveLocnNotFound.CartonNbr;
            currentActionCode = "AddRelease";
        }

        protected void CartonNumberForInvalidCartonNumber()
        {
            currentCartonNbr = cancelOrder.CartonNbr;
            currentActionCode = "AddRelease";
        }

        protected void TestForInValidActionCode()
        {
            currentCartonNbr = printCarton.CartonNbr;
            currentActionCode = "Adddd";
        }

        protected void ValidOrmtUrl()
        {
            ormtUrl = $"{baseUrl}?{"cartonNumber"}={currentCartonNbr}&{"actionCode"}={currentActionCode}";            
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
            var response = ApiIsCalled(ormtUrl);
            var result = JsonConvert.DeserializeObject<BaseResult>(response.Content.ToString());
            return result;
        }

        protected void OrmtApiIsCalledCreatedIsReturned()
        {
            Result = OrmtResult();
            Assert.AreEqual("Created", Result.ResultType.ToString());
        }

        protected void OrmtApiIsCalledCreatedIsReturnedForWaveRelease()
        {
            var response = ApiIsCalled(waveUrl);
            var result = JsonConvert.DeserializeObject<BaseResult>(response.Content.ToString());
            Assert.AreEqual("Created", result.ResultType.ToString());
        }
        protected void OrmtApiIsCalledForNotEnoughInventory()
        {
            negativecase1 = OrmtResult();         
        }

        protected void OrmtApiIsCalledForPickLocationNotFound()
        {
            negativecase2 = OrmtResult();     
        }

        protected void OrmtApiIsCalledForActiveLocationNotFound()
        {
            negativeCase3 = OrmtResult();   
        }

        protected void OrmtApiIsCalledForInvalidCartonNumber()
        {
            negativeCase4 = OrmtResult();   
        }
        protected void OrmtApiIsCalledForInvalidActionCode()
        {
            negativeCase5 = OrmtResult();   
        }

        protected void ValidateResultForActiveOrmtNotFound()
        {
            Assert.AreEqual("PickLocationDetailsExtenstion", negativecase1.ValidationMessages[0].FieldName);
            Assert.AreEqual("Not Enough Inventory in Case", negativecase1.ValidationMessages[0].Message);
        }
        protected void ValidateResultForPickLocationNotFound()
        {
            Assert.AreEqual("ActiveLocationDto", negativecase2.ValidationMessages[0].FieldName);
            Assert.AreEqual("Not found", negativecase2.ValidationMessages[0].Message);
        }
        protected void ValidateResultForActiveLocationNotFound()
        {
            Assert.AreEqual("ActiveLocationDto", negativeCase3.ValidationMessages[0].FieldName);
            Assert.AreEqual("Not found", negativeCase3.ValidationMessages[0].Message);
        }
        protected void ValidateResultForInvalidCartonNumber()
        {
            Assert.AreEqual("cartonNumber-String", negativeCase4.ValidationMessages[0].FieldName);
            Assert.AreEqual("Invalid Data", negativeCase4.ValidationMessages[0].Message);
        }
        protected void ValidateResultForInvalidActionCode()
        {
            Assert.AreEqual("cartonNumber-String", negativeCase5.ValidationMessages[0].FieldName);
            Assert.AreEqual("Invalid Data", negativeCase5.ValidationMessages[0].Message);
        }
        protected void VerifyOrmtMessageWasInsertedInToSwmToMhe()
        {
            Assert.AreEqual("Ready", swmToMheAddRelease.SourceMessageStatus);
            Assert.AreEqual(TransactionCode.Ormt, ormt.TransactionCode);
            Assert.AreEqual(MessageLength.Ormt, ormt.MessageLength);
            Assert.AreEqual("AddRelease", ormt.ActionCode);
            Assert.AreEqual(printCarton.SkuId, ormt.Sku);
            Assert.AreEqual(printCarton.TotalQty, ormt.Quantity);
            Assert.AreEqual("Case", ormt.UnitOfMeasure);
            Assert.AreEqual(printCarton.CartonNbr, ormt.OrderId);
            Assert.AreEqual("1",ormt.OrderLineId);
            Assert.AreEqual("SHIPMENT",ormt.OrderType);
            Assert.AreEqual(printCarton.WaveNbr,ormt.WaveId);
            Assert.AreEqual("N",ormt.EndOfWaveFlag);
            Assert.AreEqual(printCarton.DestLocnId+"-"+printCarton.ShipWCtrlNbr,ormt.DestinationLocationId);
            Assert.AreEqual(printCarton.Whse + printCarton.Co+printCarton.Div,ormt.Owner);
            Assert.AreEqual("FIFO",ormt.OpRule);
        }

        protected void VerifyOrmtMessageWasInsertedInToSwmToMheForCancelOrders()
        {
            Assert.AreEqual("Ready", swmToMheCancelation.SourceMessageStatus);
            Assert.AreEqual(TransactionCode.Ormt, ormtCancel.TransactionCode);
            Assert.AreEqual(MessageLength.Ormt, ormtCancel.MessageLength);
            Assert.AreEqual("Cancel", ormtCancel.ActionCode);
            Assert.AreEqual(cancelOrder.SkuId, ormtCancel.Sku);
            Assert.AreEqual(cancelOrder.TotalQty, ormtCancel.Quantity);
            Assert.AreEqual("Case", ormtCancel.UnitOfMeasure);
            Assert.AreEqual(cancelOrder.CartonNbr, ormtCancel.OrderId);
            Assert.AreEqual("1", ormtCancel.OrderLineId);
            Assert.AreEqual("SHIPMENT", ormtCancel.OrderType);
            Assert.AreEqual(cancelOrder.WaveNbr, ormtCancel.WaveId);
            Assert.AreEqual("N", ormtCancel.EndOfWaveFlag);
            Assert.AreEqual(cancelOrder.DestLocnId +"-"+ cancelOrder.ShipWCtrlNbr, ormtCancel.DestinationLocationId);
            Assert.AreEqual(cancelOrder.Whse + cancelOrder.Co + cancelOrder.Div, ormtCancel.Owner);
            Assert.AreEqual("FIFO", ormtCancel.OpRule);
        }
        protected void VerifyOrmtMessageWasInsertedInToSwmToMheForEpick()
        {
            Assert.AreEqual("Ready", swmToMheEPick.SourceMessageStatus);
            Assert.AreEqual(TransactionCode.Ormt, ormtEPick.TransactionCode);
            Assert.AreEqual(MessageLength.Ormt, ormtEPick.MessageLength);
            Assert.AreEqual("AddRelease", ormtEPick.ActionCode);
            Assert.AreEqual(ePick.SkuId, ormtEPick.Sku);
            Assert.AreEqual(ePick.TotalQty, ormtEPick.Quantity);
            Assert.AreEqual("Case", ormtEPick.UnitOfMeasure);
            Assert.AreEqual(ePick.CartonNbr, ormtEPick.OrderId);
            Assert.AreEqual("1", ormtEPick.OrderLineId);
            Assert.AreEqual("SHIPMENT", ormtEPick.OrderType);
            Assert.AreEqual(ePick.WaveNbr, ormtEPick.WaveId);
            Assert.AreEqual("N", ormtEPick.EndOfWaveFlag);
            Assert.AreEqual(ePick.DestLocnId +"-"+ ePick.ShipWCtrlNbr, ormtEPick.DestinationLocationId);
            Assert.AreEqual(ePick.Whse + ePick.Co + ePick.Div, ormtEPick.Owner);
            Assert.AreEqual("FIFO", ormtEPick.OpRule);
        }

        protected void VerifyOrmtMessageWasInsertedInToWmsToEmsForPrintingOfOrder()
        {
            Assert.AreEqual(swmToMheAddRelease.SourceMessageStatus, wmsToEmsAddRelease.Status);
            Assert.AreEqual(TransactionCode.Ormt, wmsToEmsAddRelease.Transaction);
            Assert.AreEqual("MessageBuilder", wmsToEmsAddRelease.Process);
            Assert.AreEqual(swmToMheAddRelease.SourceMessageKey, wmsToEmsAddRelease.MessageKey);
            Assert.AreEqual(swmToMheAddRelease.SourceMessageTransactionCode, wmsToEmsAddRelease.Transaction);
            Assert.AreEqual(swmToMheAddRelease.SourceMessageText, wmsToEmsAddRelease.MessageText);
            Assert.AreEqual(swmToMheAddRelease.SourceMessageResponseCode, wmsToEmsAddRelease.ResponseCode);
            Assert.AreEqual(swmToMheAddRelease.ZplData, wmsToEmsAddRelease.ZplData);
        }

        protected void VerifyOrmtMessageWasInsertedInToWmsToEmsForCancelOrder()
        {
            Assert.AreEqual(swmToMheCancelation.SourceMessageStatus, wmsToEmsCancelation.Status);
            Assert.AreEqual(TransactionCode.Ormt, wmsToEmsCancelation.Transaction);
            Assert.AreEqual("MessageBuilder", wmsToEmsCancelation.Process);
            Assert.AreEqual(swmToMheCancelation.SourceMessageKey, wmsToEmsCancelation.MessageKey);
            Assert.AreEqual(swmToMheCancelation.SourceMessageTransactionCode, wmsToEmsCancelation.Transaction);
            Assert.AreEqual(swmToMheCancelation.SourceMessageText, wmsToEmsCancelation.MessageText);
            Assert.AreEqual(swmToMheCancelation.SourceMessageResponseCode, wmsToEmsCancelation.ResponseCode);
            Assert.AreEqual(swmToMheCancelation.ZplData, wmsToEmsCancelation.ZplData);
        }

        protected void VerifyOrmtMessageWasInsertedInToWmsToEmsForEpickOfOrder()
        {
            Assert.AreEqual(swmToMheEPick.SourceMessageStatus, wmsToEmsEPick.Status);
            Assert.AreEqual(TransactionCode.Ormt, wmsToEmsEPick.Transaction);
            Assert.AreEqual("MessageBuilder", wmsToEmsEPick.Process);
            Assert.AreEqual(swmToMheEPick.SourceMessageKey, wmsToEmsEPick.MessageKey);
            Assert.AreEqual(swmToMheEPick.SourceMessageTransactionCode, wmsToEmsEPick.Transaction);
            Assert.AreEqual(swmToMheEPick.SourceMessageText, wmsToEmsEPick.MessageText);
            Assert.AreEqual(swmToMheEPick.SourceMessageResponseCode, wmsToEmsEPick.ResponseCode);
            Assert.AreEqual(swmToMheEPick.ZplData, wmsToEmsEPick.ZplData);
        }
        protected void VerifyForOrmtCountInPickLocnDtlExt()
        {
            Assert.AreEqual(pickLcnDtlExtBeforeApi.ActiveOrmtCount + 1, pickLcnDtlExtAfterApi.ActiveOrmtCount);
        }

        protected void VerifyForStatusCodeinCartonHdrForAddRelease()
        {
            Assert.AreEqual(12, cartonHdr.StatusCode);
        }
           
        protected void VerifyForStatusCodeInCartonHdrForEPick()
        {
            Assert.AreEqual(12, cartonHdr.StatusCode);
        }
    }
}
