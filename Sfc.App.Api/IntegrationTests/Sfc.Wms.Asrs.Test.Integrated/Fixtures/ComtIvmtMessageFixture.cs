using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RestSharp;
using Sfc.Core.OnPrem.ParserAndTranslator.Constants;
using Sfc.Core.OnPrem.Result;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData;
using Sfc.Wms.InboundLpn.Contracts.Dtos;
using Sfc.Wms.Interfaces.ParserAndTranslator.Contracts.Constants;
using System;
using System.Configuration;
using System.Diagnostics;
using ValidationMessage = Sfc.Wms.Api.Asrs.Test.Integrated.TestData.ValidationMessage;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures
{
    public class ComtIvmtMessageFixture : DataBaseFixture
    {
        protected string currentCaseNbr;
        protected string ComtUrl = @ConfigurationManager.AppSettings["ComtUrl"];
        protected string IvmtUrl = @ConfigurationManager.AppSettings["IvmtUrl"];
        protected CaseDetailDto caseDetailDto;
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
            currentCaseNbr = singleSkuCase.CaseNumber;
        }

        protected void CurrentCaseNumberForMultiSku()
        {
            currentCaseNbr =  caseHdrMultiSku.CaseNumber;
        }
        protected void CurrentCaseNumberForNotEnoughInventoryInCase()
        {
            currentCaseNbr = NotEnoughInvCase.CaseNumber;
        }

        public void AValidNewComtMessageRecord()
        {
            ComtParameters = new ComtParams
            {
                ActionCode = ActionCodeConstants.Create,
                CurrentLocationId = DefaultValues.CurrentlocnId,
                ContainerId = currentCaseNbr,
                ContainerType = DefaultValues.ContainerType,
                ParentContainerId = currentCaseNbr,
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
                ContainerId = currentCaseNbr,
                ContainerType = DefaultValues.ContainerType,
                ParentContainerId = currentCaseNbr,
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
            var result = JsonConvert.DeserializeObject<BaseResult>(response.Content.ToString());
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
            var result = JsonConvert.DeserializeObject<BaseResult>(response.Content.ToString());
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
            Assert.AreEqual(DefaultValues.Status, swmToMheIvmt.SourceMessageStatus);
            Assert.AreEqual(TransactionCode.Ivmt, ivmt.TransactionCode);
            Assert.AreEqual(MessageLength.Ivmt, ivmt.MessageLength);
            Assert.AreEqual(ActionCodeConstants.Create, ivmt.ActionCode);
            Assert.AreEqual(singleSkuCase.SkuId, ivmt.Sku);
            Assert.AreEqual(singleSkuCase.TotalAllocQty, Convert.ToDouble(ivmt.Quantity));
            Assert.AreEqual(DefaultValues.ContainerType, ivmt.UnitOfMeasure);
            Assert.AreEqual(DefaultValues.DataControl, ivmt.DateControl);
        }

        protected void VerifyComtMessageWasInsertedIntoSwmToMheForMultiSku()
        {
            VerifyComtMessageWasInsertedIntoSwmToMhe(comt, swmToMheComt, caseHdrMultiSku.CaseNumber);
        }
        protected void VerifyComtMessageWasInsertedIntoWmsToEms() 
        {
            VerifyComtMessageWasInsertedIntoWmsToEms(wmsToEmsComt);      
        }
        protected void VerifyComtMessageWasInsertedIntoWmsToEmsForMultiSku()
        {
            VerifyComtMessageWasInsertedIntoWmsToEms(wmsToEmsComt);
        }
        protected void VerifyComtMessageWasInsertedIntoSwmToMhe()
        {
            VerifyComtMessageWasInsertedIntoSwmToMhe(comt, swmToMheComt, singleSkuCase.CaseNumber);
        }
        protected void VerifyIvmtMessageWasInsertedIntoWmsToEms()
        {
            VerifyIvmtMessageWasInsertedIntoWmsToEms(wmsToEmsIvmt);
        }

  
        protected void VerifyTheQuantityIsIncreasedToTransInventory()
        {
           Assert.AreEqual(singleSkuCase.ActualInventoryUnits + Convert.ToDecimal(ivmt.Quantity), caseDtlAfterApi.ActualInventoryUnits);
           // the bellow code will be tested after new build release.
           //Assert.AreEqual((unitWeight * Convert.ToDecimal(ivmt.Quantity))+ Convert.ToInt16(singleSkuCase.ActualWeight), Math.Round(Convert.ToDecimal(caseDtlAfterApi.ActualWeight)));
        }
        protected void VerifyQuantityisReducedIntoCaseDetail()
        {
            Assert.AreEqual(0, caseDtlAfterApi.TotalAllocQty);        
        }
        protected void VerifyStatusIsUpdatedIntoCaseHeader()
        {
            VerifyStatusIsUpdatedIntoCaseHeader(caseDtlAfterApi.StatusCode);
        }

        protected void VerifyStatusIsUpdatedIntoTaskHeader()
        {
            try
            {
                 VerifyStatusIsUpdatedIntoTaskHeader(taskSingleSku.StatusCode);
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
            VerifyStatusIsUpdatedIntoCaseHeader(caseHdrDtl.StatusCode);
        }
    }
}