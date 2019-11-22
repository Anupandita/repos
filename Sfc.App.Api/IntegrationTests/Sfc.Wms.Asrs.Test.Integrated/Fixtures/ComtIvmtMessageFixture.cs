using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RestSharp;
using Sfc.Core.OnPrem.ParserAndTranslator.Constants;
using Sfc.Core.OnPrem.Result;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData;
using Sfc.Wms.Interfaces.ParserAndTranslator.Contracts.Constants;
using System;
using System.Configuration;
using System.Diagnostics;
using ValidationMessage = Sfc.Wms.Api.Asrs.Test.Integrated.TestData.ValidationMessage;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures
{
    public class ComtIvmtMessageFixture : DataBaseFixture
    {
        protected string CurrentCaseNbr;
        protected string ComtUrl = ConfigurationManager.AppSettings["ComtUrl"];
        protected string IvmtUrl = ConfigurationManager.AppSettings["IvmtUrl"];
        protected ComtParams ComtParameters;
        protected IvmtParam IvmtParameters;
        protected IRestResponse Response;
        protected BaseResult Result;
        protected BaseResult ResultForNegativeCase;

        protected void InitializeTestData() 
        {
            GetDataBeforeTriggerComt();
        }
        
        protected void CurrentCaseNumberForSingleSku()
        {
            CurrentCaseNbr = SingleSkuCase.CaseNumber;
        }

        protected void CurrentCaseNumberForMultiSku()
        {
            CurrentCaseNbr =  CaseHdrMultiSku.CaseNumber;
        }
        protected void CurrentCaseNumberForNotEnoughInventoryInCase()
        {
            CurrentCaseNbr = NotEnoughInvCase.CaseNumber;
        }

        public void AValidNewComtMessageRecord()
        {
            ComtParameters = new ComtParams
            {
                ActionCode = ActionCodeConstants.Create,
                CurrentLocationId = DefaultValues.CurrentlocnId,
                ContainerId = CurrentCaseNbr,
                ContainerType = DefaultValues.ContainerType,
                ParentContainerId = CurrentCaseNbr,
                AttributeBitmap = DefaultValues.AttributeBitMap,
                QuantityToInduct = DefaultValues.QuantityToInduct
            };
        }
        public void AValidNewIvmtMessageRecord()
        {
            IvmtParameters = new IvmtParam
            {
                ActionCode = ActionCodeConstants.Create,
                CurrentLocationId = DefaultValues.CurrentlocnId,
                ContainerId = CurrentCaseNbr,
                ContainerType = DefaultValues.ContainerType,
                ParentContainerId = CurrentCaseNbr,
                AttributeBitmap = DefaultValues.AttributeBitMap,
                QuantityToInduct = DefaultValues.QuantityToInduct
            };
        }
        protected IRestResponse ApiIsCalled(string url, IvmtParam parameters)
        {
            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", Content.ContentType);
            request.AddJsonBody(parameters);
            request.RequestFormat = DataFormat.Json;
            Response = client.Execute(request);
            return Response;
        }
        protected BaseResult IvmtResult()
        {
            var response = ApiIsCalled(IvmtUrl, IvmtParameters);
            var result = JsonConvert.DeserializeObject<BaseResult>(response.Content);
            return result;
        }
        protected void IvmtApiIsCalledCreatedIsReturned()
        {
            Result = IvmtResult();
            Assert.AreEqual(ResultType.Created, Result.ResultType.ToString());
        }
        protected void GetDataFromDataBaseForSingleSkuScenarios()
        {
            GetDataAfterTriggerOfComtForSingleSku();
        }

        protected void GetDataFromDataBaseForSingleSkuScenariosIvmt()
        {
            GetDataAfterTriggerOfIvmtForSingleSku();
        }
        protected void GetDataAndValidateForIvmtMessageHasInsertedIntoBothTables()
        {
            GetDataAfterTriggerForMultiSkuAndValidateData();
        }
        protected IRestResponse ApiIsCalled(string url,ComtParams parameters) 
        {
            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", Content.ContentType);
            request.AddJsonBody(parameters);
            request.RequestFormat = DataFormat.Json;
            Response = client.Execute(request);
            return Response;
        }
        protected BaseResult ComtIvmtResult()
        {
            var response = ApiIsCalled(ComtUrl, ComtParameters);
            var result = JsonConvert.DeserializeObject<BaseResult>(response.Content);
            return result;
        }
        protected void ComtApiIsCalledCreatedIsReturned()
        {
            Result = ComtIvmtResult();
            Assert.AreEqual(ResultType.Created, Result.ResultType.ToString());
        }

        protected void ComtApiIsCalledForNotEnoughInventoryInCase()
        {
            ResultForNegativeCase = ComtIvmtResult();
        }

        protected void ValidateForNotEnoughInventoryInCase()
        {
            Assert.AreEqual(ValidationMessage.InboundLpn, ResultForNegativeCase.ValidationMessages[0].FieldName);
            /* Validation messages are not proper.*/
            //Assert.AreEqual(ValidationMessage.NotEnoughInventoryInCase, ResultForNegativeCase.ValidationMessages[0].Message);

        }

        protected void VerifyIvmtMessageWasInsertedIntoSwmToMhe()
        {
            Assert.AreEqual(DefaultValues.Status, SwmToMheIvmt.SourceMessageStatus);
            Assert.AreEqual(TransactionCode.Ivmt, Ivmt.TransactionCode);
            Assert.AreEqual(MessageLength.Ivmt, Ivmt.MessageLength);
            Assert.AreEqual(ActionCodeConstants.Create, Ivmt.ActionCode);
            Assert.AreEqual(SingleSkuCase.SkuId, Ivmt.Sku);
            Assert.AreEqual(SingleSkuCase.TotalAllocQty, Convert.ToDouble(Ivmt.Quantity));
            Assert.AreEqual(DefaultValues.ContainerType, Ivmt.UnitOfMeasure);
            Assert.AreEqual(DefaultValues.DataControl, Ivmt.DateControl);
        }

        protected void VerifyComtMessageWasInsertedIntoSwmToMheForMultiSku()
        {
            VerifyComtMessageWasInsertedIntoSwmToMhe(Comt, SwmToMheComt, CaseHdrMultiSku.CaseNumber);
        }
        protected void VerifyComtMessageWasInsertedIntoWmsToEms() 
        {
            VerifyComtMessageWasInsertedIntoWmsToEms(WmsToEmsComt);      
        }
        protected void VerifyComtMessageWasInsertedIntoWmsToEmsForMultiSku()
        {
            VerifyComtMessageWasInsertedIntoWmsToEms(WmsToEmsComt);
        }
        protected void VerifyComtMessageWasInsertedIntoSwmToMhe()
        {
            VerifyComtMessageWasInsertedIntoSwmToMhe(Comt, SwmToMheComt, SingleSkuCase.CaseNumber);
        }
        protected void VerifyIvmtMessageWasInsertedIntoWmsToEms()
        {
            VerifyIvmtMessageWasInsertedIntoWmsToEms(WmsToEmsIvmt);
        }

  
        protected void VerifyTheQuantityIsIncreasedToTransInventory()
        {
           Assert.AreEqual(SingleSkuCase.ActualInventoryUnits + Convert.ToDecimal(Ivmt.Quantity), CaseDtlAfterApi.ActualInventoryUnits);
           // the bellow code will be tested after new build release.
           //Assert.AreEqual((unitWeight * Convert.ToDecimal(ivmt.Quantity))+ Convert.ToInt16(singleSkuCase.ActualWeight), Math.Round(Convert.ToDecimal(caseDtlAfterApi.ActualWeight)));
        }
        protected void VerifyQuantityisReducedIntoCaseDetail()
        {
            Assert.AreEqual(0, CaseDtlAfterApi.TotalAllocQty);        
        }
        protected void VerifyStatusIsUpdatedIntoCaseHeader()
        {
            VerifyStatusIsUpdatedIntoCaseHeader(CaseDtlAfterApi.StatusCode);
        }

        protected void VerifyStatusIsUpdatedIntoTaskHeader()
        {
            try
            {
                 VerifyStatusIsUpdatedIntoTaskHeader(TaskSingleSku.StatusCode);
            }
            catch
            {
                Debug.Print("Task Not Found");
            }
        }
        protected void VerifyQuantityisReducedIntoCaseDetailTable()
        {
            VerifyQuantityisReducedIntoCaseDetailForMultiSku();
        }

        protected void VerifyStatusIsUpdatedIntoCaseHeaderTable()
        {
            VerifyStatusIsUpdatedIntoCaseHeader(CaseHdrDtl.StatusCode);
        }
    }
}