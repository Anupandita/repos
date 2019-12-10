using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RestSharp;
using Sfc.Core.OnPrem.Result;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData;
using Sfc.Wms.Foundation.InboundLpn.Contracts.Dtos;
using Sfc.Wms.Interfaces.ParserAndTranslator.Contracts.Constants;
using System;
using System.Configuration;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures
{
    public class OrmtMessageFixture: DataBaseFixtureForOrmt
    {
        protected BaseResult Result;
        protected string CurrentCartonNbr;
        protected string CurrentActionCode;
        protected string Url = @ConfigurationManager.AppSettings["OrmtUrl"];
        protected string BaseUrl = @ConfigurationManager.AppSettings["OrmtWaveUrl"];
        protected CaseDetailDto CaseDetailDto;
        protected ComtParams OrmtParameters;
        protected IRestResponse Response;
        protected string OrmtUrl;
        protected BaseResult Negativecase1;
        protected BaseResult Negativecase2;
        protected BaseResult NegativeCase3;
        protected BaseResult NegativeCase4;
        protected BaseResult NegativeCase5;
        protected string WaveUrl;

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

        protected void InitializeTestDataForOnProcessCostMessage()
        {
            GetDataBeforeCallingApiForOnProcessCostMessage();
        }

        protected void InitializeTestDataForWaveRelease()
        {
            GetValidDataBeforeTriggerOrmtForPrintingOfCartonsThroughWaveNumber();
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

        protected void ReadDataAfterApiForOnprocessCostOfCarton()
        {
            GetDataAfterCallingApiForOnProcessCost();
        }
        protected void CartonNumberForAddRelease()
        {
            CurrentCartonNbr = PrintCarton.CartonNbr;
            CurrentActionCode = OrmtActionCode.AddRelease;
        }
        protected void CartonNumberForCancel()
        {
            CurrentCartonNbr = CancelOrder.CartonNbr;
            CurrentActionCode = OrmtActionCode.Cancel;
        }
       
        protected void CartonNumberForEPick()
        {
            CurrentCartonNbr = EPick.CartonNbr;
            CurrentActionCode = OrmtActionCode.AddRelease;
        }

        protected void CartonNumberForOnProcessCost()
        {
            CurrentCartonNbr = OnProCost.CartonNbr;
            CurrentActionCode = OrmtActionCode.AddRelease;
        }

        protected void CartonNumberForOrmtCountNotFound()
        {
            CurrentCartonNbr = ActiveOrmtCountNotFound.CartonNbr;
            CurrentActionCode = OrmtActionCode.AddRelease;
        }

        protected void CartonNumberForPickLocnNotFound()
        {
            CurrentCartonNbr = PickLocnNotFound.CartonNbr;
            CurrentActionCode = OrmtActionCode.AddRelease;
        }

        protected void CartonNumberForActiveLocnNotFound()
        {
            CurrentCartonNbr = ActiveLocnNotFound.CartonNbr;
            CurrentActionCode = OrmtActionCode.AddRelease;
        }

        protected void CartonNumberForInvalidCartonNumber()
        {
            CurrentCartonNbr = CancelOrder.CartonNbr;
            CurrentActionCode = OrmtActionCode.AddRelease;
        }

        protected void TestForInValidActionCode()
        {
            CurrentCartonNbr = PrintCarton.CartonNbr;
            CurrentActionCode = Constants.InvalidOrmtActionCode;
        }

        protected void ValidOrmtUrl()
        {
            OrmtUrl = $"{Url}?{"cartonNumber"}={CurrentCartonNbr}&{"actionCode"}={CurrentActionCode}";            
        }

        protected void ValidOrmtWaveUrl()
        {
            WaveUrl = $"{BaseUrl}?{"waveNumber"}={OrderList[0].WaveNbr}";
        }

        protected IRestResponse ApiIsCalled(string Url)
        {
            var client = new RestClient(Url);
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
            Assert.AreEqual(ResultType.Created, Result.ResultType.ToString());
        }

        protected void OrmtApiIsCalledCreatedIsReturnedForWaveRelease()
        {
            var response = ApiIsCalled(WaveUrl);
            var result = JsonConvert.DeserializeObject<BaseResult>(response.Content);
            Assert.AreEqual(ResultType.Created, result.ResultType.ToString());
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
            // Not implemented due to message change.
        }
        protected void ValidateResultForPickLocationNotFound()
        {            
            Assert.AreEqual(Constants.ValidationMessage, Negativecase2.ValidationMessages[0].Message);
        }
        protected void ValidateResultForActiveLocationNotFound()
        {
            Assert.AreEqual(Constants.ValidationMessage, NegativeCase3.ValidationMessages[0].Message);
        }
        protected void ValidateResultForInvalidCartonNumber()
        {
            Assert.AreEqual(Constants.ValidationMessage, NegativeCase4.ValidationMessages[0].Message);
        }
        protected void ValidateResultForInvalidActionCode()
        {
          // not implemented.
        }
        protected void VerifyOrmtMessageWasInsertedInToSwmToMhe()
        {
            Assert.AreEqual(DefaultValues.Status, SwmToMheAddRelease.SourceMessageStatus);
            Assert.AreEqual(TransactionCode.Ormt, Ormt.TransactionCode);
            Assert.AreEqual(MessageLength.Ormt, Ormt.MessageLength);
            Assert.AreEqual(OrmtActionCode.AddRelease, Ormt.ActionCode);
            Assert.AreEqual(PrintCarton.SkuId, Ormt.Sku);
            Assert.AreEqual(PrintCarton.TotalQty, Ormt.Quantity);
            Assert.AreEqual(UnitOfMeasures.Case, Ormt.UnitOfMeasure);
            Assert.AreEqual(PrintCarton.CartonNbr, Ormt.OrderId);
            Assert.AreEqual(Constants.OrderLineId,Ormt.OrderLineId);
            Assert.AreEqual(DefaultPossibleValue.OrderType,Ormt.OrderType);
            Assert.AreEqual(PrintCarton.WaveNbr,Ormt.WaveId);
            Assert.AreEqual(Constants.EndOfWaveFlag,Ormt.EndOfWaveFlag);
            Assert.AreEqual(PrintCarton.DestLocnId+"-"+PrintCarton.ShipWCtrlNbr,Ormt.DestinationLocationId);
            Assert.AreEqual(PrintCarton.Whse,Ormt.Owner);
            Assert.AreEqual(DefaultPossibleValue.OpRule,Ormt.OpRule);
        }

        protected void VerifyOrmtMessageWasInsertedInToSwmToMheForCancelOrders()
        {
            Assert.AreEqual(DefaultValues.Status, SwmToMheCancelation.SourceMessageStatus);
            Assert.AreEqual(TransactionCode.Ormt, OrmtCancel.TransactionCode);
            Assert.AreEqual(MessageLength.Ormt, OrmtCancel.MessageLength);
            Assert.AreEqual(OrmtActionCode.Cancel, OrmtCancel.ActionCode);
            Assert.AreEqual(CancelOrder.SkuId, OrmtCancel.Sku);
            Assert.AreEqual(CancelOrder.TotalQty, OrmtCancel.Quantity);
            Assert.AreEqual(UnitOfMeasures.Case, OrmtCancel.UnitOfMeasure);
            Assert.AreEqual(CancelOrder.CartonNbr, OrmtCancel.OrderId);
            Assert.AreEqual(Constants.OrderLineId, OrmtCancel.OrderLineId);
            Assert.AreEqual(DefaultPossibleValue.OrderType, OrmtCancel.OrderType);
            Assert.AreEqual(CancelOrder.WaveNbr, OrmtCancel.WaveId);
            Assert.AreEqual(Constants.EndOfWaveFlag, OrmtCancel.EndOfWaveFlag);
            Assert.AreEqual(CancelOrder.DestLocnId +"-"+ CancelOrder.ShipWCtrlNbr, OrmtCancel.DestinationLocationId);
            Assert.AreEqual(CancelOrder.Whse, OrmtCancel.Owner);
            Assert.AreEqual(DefaultPossibleValue.OpRule, OrmtCancel.OpRule);
        }
        protected void VerifyOrmtMessageWasInsertedInToSwmToMheForEpick()
        {
            Assert.AreEqual(DefaultValues.Status, SwmToMheEPick.SourceMessageStatus);
            Assert.AreEqual(TransactionCode.Ormt, OrmtEPick.TransactionCode);
            Assert.AreEqual(MessageLength.Ormt, OrmtEPick.MessageLength);
            Assert.AreEqual(OrmtActionCode.AddRelease, OrmtEPick.ActionCode);
            Assert.AreEqual(EPick.SkuId, OrmtEPick.Sku);
            Assert.AreEqual(EPick.TotalQty, OrmtEPick.Quantity);
            Assert.AreEqual(UnitOfMeasures.Case, OrmtEPick.UnitOfMeasure);
            Assert.AreEqual(EPick.CartonNbr, OrmtEPick.OrderId);
            Assert.AreEqual(Constants.OrderLineId, OrmtEPick.OrderLineId);
            Assert.AreEqual(DefaultPossibleValue.OrderType, OrmtEPick.OrderType);
            Assert.AreEqual(EPick.WaveNbr, OrmtEPick.WaveId);
            Assert.AreEqual(Constants.EndOfWaveFlag, OrmtEPick.EndOfWaveFlag);
            Assert.AreEqual(EPick.DestLocnId +"-"+ EPick.ShipWCtrlNbr, OrmtEPick.DestinationLocationId);
            Assert.AreEqual(EPick.Whse , OrmtEPick.Owner);
            Assert.AreEqual(DefaultPossibleValue.OpRule, OrmtEPick.OpRule);
        }

        protected void VerifyOrmtMessageWasInsertedInToSwmToMheForOnProcessCost()
        {
            Assert.AreEqual(DefaultValues.Status, SwmToMheOnProcess.SourceMessageStatus);
            Assert.AreEqual(TransactionCode.Ormt, OrmtOnprocess.TransactionCode);
            Assert.AreEqual(MessageLength.Ormt, OrmtOnprocess.MessageLength);
            Assert.AreEqual(OrmtActionCode.AddRelease, OrmtOnprocess.ActionCode);
            Assert.AreEqual(OnProCost.SkuId, OrmtOnprocess.Sku);
            Assert.AreEqual(OnProCost.TotalQty, OrmtOnprocess.Quantity);
            Assert.AreEqual(UnitOfMeasures.Case, OrmtOnprocess.UnitOfMeasure);
            Assert.AreEqual(OnProCost.CartonNbr, OrmtOnprocess.OrderId);
        }

        protected void VerifyOrmtMessageWasInsertedInToWmsToEmsForPrintingOfOrder()
        {
            Assert.AreEqual(SwmToMheAddRelease.SourceMessageStatus, WmsToEmsAddRelease.Status);
            Assert.AreEqual(TransactionCode.Ormt, WmsToEmsAddRelease.Transaction);
            Assert.AreEqual(SwmToMheAddRelease.SourceMessageProcess, WmsToEmsAddRelease.Process);
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
            Assert.AreEqual(SwmToMheCancelation.SourceMessageProcess, WmsToEmsCancelation.Process);
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
            Assert.AreEqual(SwmToMheEPick.SourceMessageProcess, WmsToEmsEPick.Process);
            Assert.AreEqual(SwmToMheEPick.SourceMessageKey, WmsToEmsEPick.MessageKey);
            Assert.AreEqual(SwmToMheEPick.SourceMessageTransactionCode, WmsToEmsEPick.Transaction);
            Assert.AreEqual(SwmToMheEPick.SourceMessageText, WmsToEmsEPick.MessageText);
            Assert.AreEqual(SwmToMheEPick.SourceMessageResponseCode, WmsToEmsEPick.ResponseCode);
            Assert.AreEqual(SwmToMheEPick.ZplData, WmsToEmsEPick.ZplData);
        }

        protected void VerifyOrmtMessageWasInsertedInToWmsToEmsForOnProcessCostOfOrder()
        {
            Assert.AreEqual(SwmToMheOnProcess.SourceMessageStatus, WmsToEmsOnPrc.Status);
            Assert.AreEqual(TransactionCode.Ormt, WmsToEmsOnPrc.Transaction);
            Assert.AreEqual(SwmToMheOnProcess.SourceMessageProcess, WmsToEmsOnPrc.Process);
            Assert.AreEqual(SwmToMheOnProcess.SourceMessageKey, WmsToEmsOnPrc.MessageKey);
            Assert.AreEqual(SwmToMheOnProcess.SourceMessageTransactionCode, WmsToEmsOnPrc.Transaction);
            Assert.AreEqual(SwmToMheOnProcess.SourceMessageText, WmsToEmsOnPrc.MessageText);
            Assert.AreEqual(SwmToMheOnProcess.SourceMessageResponseCode, WmsToEmsOnPrc.ResponseCode);
            Assert.AreEqual(SwmToMheOnProcess.ZplData, WmsToEmsOnPrc.ZplData);
        }
        protected void VerifyForOrmtCountInPickLocnDtlExt()
        {
            Assert.AreEqual(PickLcnDtlExtBeforeApi.ActiveOrmtCount + 1, PickLcnDtlExtAfterApi.ActiveOrmtCount);
        }

        protected void VerifyForStatusCodeinCartonHdrForAddRelease()
        {
            Assert.AreEqual(Constants.CartonStatusForReleased, CartonHdr.StatusCode);
        }
           
        protected void VerifyForStatusCodeInCartonHdrForEPick()
        {
            Assert.AreEqual(Constants.CartonStatusForReleased, CartonHdr.StatusCode);
        }

        protected void VerifyForStatusInSwmEligibleOrmtCartons()
        {
            Assert.AreEqual(Constants.OrmtMessageSent, Convert.ToInt32(Status.Status));
        }

    }
}
