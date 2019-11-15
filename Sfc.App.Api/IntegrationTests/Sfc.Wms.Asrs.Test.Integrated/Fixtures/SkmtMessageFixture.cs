using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using Sfc.Core.OnPrem.Result;
using Newtonsoft.Json;
using Sfc.Wms.Interfaces.ParserAndTranslator.Contracts.Constants;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData;
using Sfc.Wms.Interfaces.Asrs.Dematic.Contracts.Dtos;
using ValidationMessage = Sfc.Wms.Api.Asrs.Test.Integrated.TestData.ValidationMessage;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures
{
    public class SkmtMessageFixture :DataBaseFixtureForSkmt
    {

        protected string CurrentSkuId;
        protected string CurrentActionCode;
        protected string InvalidSkuId;
        protected SkmtParams SkmtParameters;
        protected BaseResult Result;
        protected IRestResponse Response;
        protected BaseResult Negativecase;
        protected string SkmtUrl;
        protected string BaseUrl =ConfigurationManager.AppSettings["SkmtUrl"]; 

        protected void InitializeTestData()
        {
            GetDataBeforeTriggerSkmt();

        }
        protected void InitializeTestDataParent()
        {
            GetDataBeforeTriggerSkmtParent();

        }
        protected void InitializeTestDataChild()
        {
            GetDataBeforeTriggerSkmtChildSku();
        }
        protected void CurrentSkuIdForItemmaster()
        {
            CurrentSkuId = Normal.SkuId;
        }
        protected void CurrentSkuIdForParentSkuItemmaster()
        {
            CurrentSkuId = ParentSku.SkuId;
        }
        protected void CurrentSkuIdForChildSkuItemmaster()
        {
            CurrentSkuId = ChildSku.SkuId;
        }

        protected void InitializeInvalidTestData()
        {
            CurrentSkuId = DefaultValues.InvalidSku;
        }
        protected void CurrentActioncodeAdd()
        {
            CurrentActionCode = SkmtActionCode.Add;
        }
        protected void CurrentActioncodeUpdate()
        {
            CurrentActionCode = SkmtActionCode.Update;
        }
        protected void CurrentActioncodeDelete()
        {
            CurrentActionCode = SkmtActionCode.Delete;
        }

        protected IRestResponse ApiIsCalled(string url)
        {
            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);
            Response = client.Execute(request);
            return Response;
        }
        protected void ValidSkmtUrl()
        {
            SkmtUrl = $"{BaseUrl}?{"ActionCode"}={CurrentActionCode}&{"SkuId"}={CurrentSkuId}";
        }

        protected BaseResult SkmtResult()
        {
            var response = ApiIsCalled(SkmtUrl);
            var result = JsonConvert.DeserializeObject<BaseResult>(response.Content);
            return result;
        }
        protected void SkmtApiIsCalledCreatedIsReturned()
        {
            Result = SkmtResult();
            Assert.AreEqual(ResultType.Created, Result.ResultType.ToString());
        }

        protected void SkmtApiIsCalledCreatedForNegativeCases()
        {
            Result = SkmtResult();
            Assert.AreEqual(ResultType.NotFounds, Result.ResultType.ToString());
        }
        protected void SkmtApiIsCalledForInvalidSkuId()
        {
            Negativecase = SkmtResult();
        }
        protected void GetValidDataAfterTrigger()
        {
            GetDataAfterTrigger();
        }
        protected void VerifySkmtMessageWasInsertedForIntoSwmToMhe(string action, string act)
        {
            Assert.AreEqual(DefaultValues.Status, SwmToMheSkmt.SourceMessageStatus);
            Assert.AreEqual(TransactionCode.Skmt, Skmt.TransactionCode);
            Assert.AreEqual(MessageLength.Skmt, Skmt.MessageLength);
            Assert.AreEqual(action, act);
            Assert.AreEqual(ItemMaster.SkuId, Skmt.Sku);

        }
        protected void VerifySkmtMessageWasInsertedIntoWmsToEms()
        {
            VerifySkmtMessageWasInsertedIntoWmsToEms(WmsToEmsSkmt);
        }
        protected void VerifySkmtMessageWasInsertedIntoWmsToEms(WmsToEmsDto wte)
        {  
            Assert.AreEqual(SwmToMheSkmt.SourceMessageKey, wte.MessageKey);
            Assert.AreEqual(SwmToMheSkmt.SourceMessageText, wte.MessageText);
            Assert.AreEqual(SwmToMheSkmt.SourceMessageStatus, wte.Status);
            Assert.AreEqual(SwmToMheSkmt.SourceMessageResponseCode, wte.ResponseCode);
            Assert.AreEqual(TransactionCode.Skmt, wte.Transaction);
        }
        protected void VerifySkmtMessageWasNormalSku()
        {
            Assert.AreEqual(null, Skmt.ParentSku);
        }

        protected void VerifySkmtMessageWasParentSku()
        {
            Assert.AreEqual(ParentSku.Colordescription, Skmt.ParentSku);
        }

        protected void VerifySkmtMessageWasChildSku()
        {
            Assert.AreEqual(ParentSku.SkuId, ChildSku.SkuId);
        }

        protected void ValidateResultForInvalidSkuId()
        {
            Assert.AreEqual(ResultType.NotFounds, Negativecase.ResultType.ToString());
            Assert.AreEqual(1, Negativecase.ValidationMessages.Count);
            Assert.AreEqual(ValidationMessage.ItemMasters, Negativecase.ValidationMessages[0].FieldName);

        }

    }


   
}
