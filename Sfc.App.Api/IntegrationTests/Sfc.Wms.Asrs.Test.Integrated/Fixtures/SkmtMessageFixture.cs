using System;
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

        protected string currentSkuId;
        protected string currentActionCode;
        protected string InvalidSkuId;
        protected SkmtParams SkmtParameters;
        protected BaseResult Result;
        protected IRestResponse Response;
        protected BaseResult negativecase;
        protected string SkmtUrl;
        protected string baseUrl =ConfigurationManager.AppSettings["SkmtUrl"]; 

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
            currentSkuId = Normal.SkuId;
        }
        protected void CurrentSkuIdForParentSkuItemmaster()
        {
            currentSkuId = parentSku.SkuId;
        }
        protected void CurrentSkuIdForChildSkuItemmaster()
        {
            currentSkuId = childSku.SkuId;
        }

        protected void InitializeInvalidTestData()
        {
            currentSkuId = DefaultValues.InvalidSku;
        }
        protected void CurrentActioncodeAdd()
        {
            currentActionCode = SkmtActionCode.Add;
        }
        protected void CurrentActioncodeUpdate()
        {
            currentActionCode = SkmtActionCode.Update;
        }
        protected void CurrentActioncodeDelete()
        {
            currentActionCode = SkmtActionCode.Delete;
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
            SkmtUrl = $"{baseUrl}?{"ActionCode"}={currentActionCode}&{"SkuId"}={currentSkuId}";
        }

        protected BaseResult SkmtResult()
        {
            var response = ApiIsCalled(SkmtUrl);
            var result = JsonConvert.DeserializeObject<BaseResult>(response.Content.ToString());
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
            negativecase = SkmtResult();
        }
        protected void GetValidDataAfterTrigger()
        {
            GetDataAfterTrigger();
        }
        protected void VerifySkmtMessageWasInsertedForIntoSwmToMhe(string action, string act)
        {
            Assert.AreEqual(DefaultValues.Status, swmToMheSkmt.SourceMessageStatus);
            Assert.AreEqual(TransactionCode.Skmt, skmt.TransactionCode);
            Assert.AreEqual(MessageLength.Skmt, skmt.MessageLength);
            Assert.AreEqual(action, act);
            Assert.AreEqual(ItemMaster.SkuId, skmt.Sku);

        }
        protected void VerifySkmtMessageWasInsertedIntoWmsToEms()
        {
            VerifySkmtMessageWasInsertedIntoWmsToEms(wmsToEmsSkmt);
        }
        protected void VerifySkmtMessageWasInsertedIntoWmsToEms(WmsToEmsDto wte)
        {  
            Assert.AreEqual(swmToMheSkmt.SourceMessageKey, wte.MessageKey);
            Assert.AreEqual(swmToMheSkmt.SourceMessageText, wte.MessageText);
            Assert.AreEqual(swmToMheSkmt.SourceMessageStatus, wte.Status);
            Assert.AreEqual(swmToMheSkmt.SourceMessageResponseCode, wte.ResponseCode);
            Assert.AreEqual(TransactionCode.Skmt, wte.Transaction);
        }
        protected void VerifySkmtMessageWasNormalSku()
        {
            Assert.AreEqual(null, skmt.ParentSku);
        }

        protected void VerifySkmtMessageWasParentSku()
        {
            Assert.AreEqual(parentSku.colordescription, skmt.ParentSku);
        }

        protected void VerifySkmtMessageWasChildSku()
        {
            Assert.AreEqual(parentSku.SkuId, childSku.SkuId);
        }

        protected void ValidateResultForInvalidSkuId()
        {
            Assert.AreEqual(ResultType.NotFounds, negativecase.ResultType.ToString());
            Assert.AreEqual(1, negativecase.ValidationMessages.Count);
            Assert.AreEqual(ValidationMessage.ItemMasters, negativecase.ValidationMessages[0].FieldName);

        }

    }


   
}
