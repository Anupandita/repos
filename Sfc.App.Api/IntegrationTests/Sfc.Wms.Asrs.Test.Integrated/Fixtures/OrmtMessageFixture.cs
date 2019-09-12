using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RestSharp;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData;
using Sfc.Wms.InboundLpn.Contracts.Dtos;
using Sfc.Wms.ParserAndTranslator.Contracts.Constants;
using Sfc.Wms.Result;
using System.Configuration;


namespace Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures
{
    public class OrmtMessageFixture: DataBaseFixtureForOrmt
    {
        protected BaseResult Result;
        protected string currentCartonNbr;
        protected string currentActionCode;
        protected string ComtUrl = @ConfigurationManager.AppSettings["ComtUrl"];
        protected CaseDetailDto caseDetailDto;
        protected ComtParams OrmtParameters;
        protected ComtIvmtMessageFixture  comtIvmtMessageFixture;
        protected IRestResponse Response;
        

        protected void InitializeTestData()
        {
            GetDataBeforeTriggerOrmt();
        }

        protected void CartonNumberForAddRelease()
        {
            currentCartonNbr = ch.CartonNbr;
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

        protected IRestResponse ApiIsCalled(string url, ComtParams parameters)
        {
            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", Content.ContentType);
            request.AddJsonBody(parameters);
            request.RequestFormat = DataFormat.Json;
            Response = client.Execute(request);
            return Response;
        }
        protected BaseResult OrmtResult()
        {
            var response = ApiIsCalled("", OrmtParameters);
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
            Assert.AreEqual("", swmToMheAddRelease.SourceMessageStatus);
            Assert.AreEqual(TransactionCode.Ormt, ormt.TransactionCode);
            Assert.AreEqual(MessageLength.Ormt, ormt.MessageLength);
            Assert.AreEqual("AddRelease", ormt.ActionCode);
            Assert.AreEqual(ch.SkuId, ormt.Sku);
            Assert.AreEqual(ch.PickTktQty, ormt.Quantity);
            Assert.AreEqual("Case", ormt.UnitOfMeasure);
            Assert.AreEqual(ch.CartonNbr, ormt.OrderId);
            Assert.AreEqual("1",ormt.OrderLineId);
            Assert.AreEqual("Shipment",ormt.OrderType);
            Assert.AreEqual(ch.WaveNbr,ormt.WaveId);
            Assert.AreEqual("N",ormt.EndOfWaveFlag);
            Assert.AreEqual(ch.DestLocnId+"-"+ch.ShipWCtrlNbr,ormt.DestinationLocationId);
            Assert.AreEqual(ch.Whse + ch.Co+ch.Div,ormt.Owner);
            Assert.AreEqual("FIFO",ormt.OpRule);
        }

        protected void VerifyOrmtMessageWasInsertedInToSwmToMheForCancelOrders()
        {
            Assert.AreEqual("", swmToMheAddRelease.SourceMessageStatus);
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
            Assert.AreEqual(cancelOrder.DestLocnId + "-" + ch.ShipWCtrlNbr, ormtCancel.DestinationLocationId);
            Assert.AreEqual(cancelOrder.Whse + cancelOrder.Co + cancelOrder.Div, ormt.Owner);
            Assert.AreEqual("FIFO", ormt.OpRule);
        }
        protected void VerifyOrmtMessageWasInsertedInToSwmToMheForEpick()
        {
            Assert.AreEqual("", swmToMheAddRelease.SourceMessageStatus);
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
            Assert.AreEqual(ePick.DestLocnId + "-" + ch.ShipWCtrlNbr, ormtEPick.DestinationLocationId);
            Assert.AreEqual(ePick.Whse + ePick.Co + ePick.Div, ormtEPick.Owner);
            Assert.AreEqual("FIFO", ormtEPick.OpRule);
        }

        protected void VerifyOrmtMessageWasInsertedInToWmsToEms()
        {
            Assert.AreEqual(swmToMheAddRelease.SourceMessageStatus, wmsToEmsAddRelease.Status);
            Assert.AreEqual(TransactionCode.Ormt, wmsToEmsAddRelease.Transaction);
            Assert.AreEqual(swmToMheComt.SourceMessageResponseCode, wmsToEmsAddRelease.ResponseCode);
        }

        protected void VerifyForOrmtCountInPickLocnDtlExt()
        {

        }

        protected void VerifyForQtyInCartonDtl()
        {

        }

        protected void VerifyForStatusCodeinCartonHdrForAddRelease()
        {

        }
        
        protected void VerifyForStatusCodeInCartonHdrForCancel()
        {

        }
        protected void VerifyForStatusCodeInCartonHdrForEPick()
        {

        }

    }
}
