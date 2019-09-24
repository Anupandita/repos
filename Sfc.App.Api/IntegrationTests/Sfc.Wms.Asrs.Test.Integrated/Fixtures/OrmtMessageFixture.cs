using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RestSharp;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData;
using Sfc.Wms.InboundLpn.Contracts.Dtos;
using Sfc.Wms.Interface.ParserAndTranslator.Contracts.Constants;
using Sfc.Wms.Result;
using System.Configuration;


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
        protected ComtIvmtMessageFixture  comtIvmtMessageFixture;
        protected IRestResponse Response;
        protected string url;

        protected void InitializeTestData()
        {
            GetDataBeforeTriggerOrmt();
        }

        protected void ReadDataAfterApiForPrintingOfCarton()
        {
            GetDataAfterCallingOrmtApiForAddRelease();
        }
        protected void ReadDataAfterApiForCancelOfCarton()
        {
            GetDataAfterCallingApiForCancellationOfOrders();
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

        protected void AValidNewOrmtMessageRecord()
        {
            OrmtParameters = new ComtParams
            {
                ActionCode = currentActionCode,
                CurrentLocationId = "",
                ContainerId = currentCartonNbr,
                ContainerType = DefaultValues.ContainerType,  
            };
        }

        protected IRestResponse ApiIsCalled()
        {
            url = $"{baseUrl}?{"cartonNumber"}={"00000999994081749031"}&{"actionCode"}={"AddRelease"}";
            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);
            Response = client.Execute(request);
            return Response;
        }
        protected BaseResult OrmtResult()
        {
            var response = ApiIsCalled();
            var result = JsonConvert.DeserializeObject<BaseResult>(response.Content.ToString());
            return result;
        }

        protected void OrmtApiIsCalledCreatedIsReturned()
        {
            Result = OrmtResult();
            Assert.AreEqual("Created", Result.ResultType.ToString());
        }
        protected void VerifyOrmtMessageWasInsertedInToSwmToMhe()
        {
            Assert.AreEqual("Ready", swmToMheAddRelease.SourceMessageStatus);
            Assert.AreEqual(TransactionCode.Ormt, ormt.TransactionCode);
            Assert.AreEqual(MessageLength.Ormt, ormt.MessageLength);
            Assert.AreEqual("AddRelease", ormt.ActionCode);
            Assert.AreEqual(printCarton.SkuId, ormt.Sku);
            Assert.AreEqual(printCarton.PickTktQty, ormt.Quantity);
            Assert.AreEqual("Case", ormt.UnitOfMeasure);
            Assert.AreEqual(printCarton.CartonNbr, ormt.OrderId);
            Assert.AreEqual("1",ormt.OrderLineId);
            Assert.AreEqual("Shipment",ormt.OrderType);
            Assert.AreEqual(printCarton.WaveNbr,ormt.WaveId);
            Assert.AreEqual("N",ormt.EndOfWaveFlag);
            Assert.AreEqual(printCarton.DestLocnId+"-"+printCarton.ShipWCtrlNbr,ormt.DestinationLocationId);
            Assert.AreEqual(printCarton.Whse + printCarton.Co+printCarton.Div,ormt.Owner);
            Assert.AreEqual("FIFO",ormt.OpRule);
        }

        protected void VerifyOrmtMessageWasInsertedInToSwmToMheForCancelOrders()
        {
            Assert.AreEqual("Ready", swmToMheAddRelease.SourceMessageStatus);
            Assert.AreEqual(TransactionCode.Ormt, ormtCancel.TransactionCode);
            Assert.AreEqual(MessageLength.Ormt, ormtCancel.MessageLength);
            Assert.AreEqual("Cancel", ormtCancel.ActionCode);
            Assert.AreEqual(cancelOrder.SkuId, ormtCancel.Sku);
            Assert.AreEqual(cancelOrder.PickTktQty, ormtCancel.Quantity);
            Assert.AreEqual("Case", ormtCancel.UnitOfMeasure);
            Assert.AreEqual(cancelOrder.CartonNbr, ormtCancel.OrderId);
            Assert.AreEqual("1", ormtCancel.OrderLineId);
            Assert.AreEqual("Shipment", ormtCancel.OrderType);
            Assert.AreEqual(cancelOrder.WaveNbr, ormtCancel.WaveId);
            Assert.AreEqual("N", ormtCancel.EndOfWaveFlag);
            Assert.AreEqual(cancelOrder.DestLocnId + "-" + printCarton.ShipWCtrlNbr, ormtCancel.DestinationLocationId);
            Assert.AreEqual(cancelOrder.Whse + cancelOrder.Co + cancelOrder.Div, ormt.Owner);
            Assert.AreEqual("FIFO", ormt.OpRule);
        }
        protected void VerifyOrmtMessageWasInsertedInToSwmToMheForEpick()
        {
            Assert.AreEqual("Ready", swmToMheAddRelease.SourceMessageStatus);
            Assert.AreEqual(TransactionCode.Ormt, ormtEPick.TransactionCode);
            Assert.AreEqual(MessageLength.Ormt, ormtEPick.MessageLength);
            Assert.AreEqual("AddRelease", ormtEPick.ActionCode);
            Assert.AreEqual(ePick.SkuId, ormtEPick.Sku);
            Assert.AreEqual(ePick.PickTktQty, ormtEPick.Quantity);
            Assert.AreEqual("Case", ormtEPick.UnitOfMeasure);
            Assert.AreEqual(ePick.CartonNbr, ormtEPick.OrderId);
            Assert.AreEqual("1", ormtEPick.OrderLineId);
            Assert.AreEqual("Shipment", ormtEPick.OrderType);
            Assert.AreEqual(ePick.WaveNbr, ormtEPick.WaveId);
            Assert.AreEqual("N", ormtEPick.EndOfWaveFlag);
            Assert.AreEqual(ePick.DestLocnId + "-" + printCarton.ShipWCtrlNbr, ormtEPick.DestinationLocationId);
            Assert.AreEqual(ePick.Whse + ePick.Co + ePick.Div, ormtEPick.Owner);
            Assert.AreEqual("FIFO", ormtEPick.OpRule);
        }

        protected void VerifyOrmtMessageWasInsertedInToWmsToEms()
        {
            Assert.AreEqual(swmToMheAddRelease.SourceMessageStatus, wmsToEmsAddRelease.Status);
            Assert.AreEqual(TransactionCode.Ormt, wmsToEmsAddRelease.Transaction);
            //Assert.AreEqual(swmToMheComt.SourceMessageResponseCode, wmsToEmsAddRelease.ResponseCode);
        }

        protected void VerifyForOrmtCountInPickLocnDtlExt()
        {
            Assert.AreEqual(pickLcnDtlExtBeforeApi.ToString() + 1, pickLcnDtlExtBeforeApi);
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
