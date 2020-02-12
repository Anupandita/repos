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
        protected string ComtUrl = ConfigurationManager.AppSettings["BaseUrl"] +TestData.Parameter.ContainerMaintenance;
        protected string IvmtUrl = ConfigurationManager.AppSettings["BaseUrl"] +TestData.Parameter.InventoryMaintenance;
        protected ComtParams ComtParameters;
        protected IvmtParam IvmtParameters;
        protected IRestResponse Response;
        protected BaseResult Result;
        protected BaseResult ResultForNegativeCase;

        protected void InitializeTestData() 
        {
            GetDataBeforeTriggerComt();
        }

        protected void InitializeReceivedCaseTestData()
        {
            GetDataBeforeTriggerforReceivedCaseComt();
        }
        protected void InitializeTestDataForStatCode30WhenTriggerIsOn()
        {
            GetDataBeforeTriggerForStatCode30();
        }

        protected void InitializeTestDataForComtWhenTriggerIsOn()
        {
            GetDataBeforeTriggerForComtIvmt();
        }
        public void AValidNewComtMessageRecordWhereCaseNumberAndSkuIs(string currentCaseNbr,string skuId)
        {
            ComtParameters = new ComtParams
            {
                ActionCode = ActionCodeConstants.Create,                
                ContainerId = currentCaseNbr,
                ContainerType = DefaultValues.ContainerType,
                ParentContainerId = currentCaseNbr,
                AttributeBitmap = DefaultValues.AttributeBitMap,
                QuantityToInduct = DefaultValues.QuantityToInduct,
                SourceLpn = currentCaseNbr
            };
        }
        public void AValidNewIvmtMessageRecordWhereCaseNumberAndSkuIs(string currentCaseNbr, string skuId)
        {
            IvmtParameters = new IvmtParam
            {
                ActionCode = ActionCodeConstants.Create,                
                ContainerId = currentCaseNbr,
                ContainerType = DefaultValues.ContainerType,
                ParentContainerId = currentCaseNbr,
                AttributeBitmap = DefaultValues.AttributeBitMap,
                QuantityToInduct = DefaultValues.QuantityToInduct,
                SourceLpn = currentCaseNbr
            };
        }
        public void AValidNewCaseReturnedRecordWhereCaseNumberAndSkuIdIs(string currentCaseNbr,string skuId)
        {
            ComtParameters = new ComtParams
            {
                ActionCode = ActionCodeConstants.Create,               
                ContainerId = currentCaseNbr,
                ContainerType = DefaultValues.ContainerType,
                ParentContainerId = currentCaseNbr,
                AttributeBitmap = DefaultValues.AttributeBitMap,
                QuantityToInduct = DefaultValues.QuantityToInduct,
                SourceLpn = currentCaseNbr
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
        protected BaseResult IvmtResult(string url)
        {
            var response = ApiIsCalled(url, IvmtParameters);
            var result = JsonConvert.DeserializeObject<BaseResult>(response.Content);
            return result;
        }
        protected void IvmtApiIsCalledCreatedIsReturnedWithValidUrlIs(string url)
        {
            Result = IvmtResult(url);
            Assert.AreEqual(ResultType.Created, Result.ResultType.ToString());
        }
        protected void GetDataFromDataBaseForSingleSkuScenarios()
        {
            GetDataAfterTriggerOfComtForSingleSku();
        }
        protected void GetDataFromDataBaseAfterApiIsCalled()
        {
            GetDataAfterTriggerOfComtForReceivedCaseSingleSku();
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


        protected BaseResult ComtIvmtResult(string url)
        {
            var response = ApiIsCalled(url, ComtParameters);
            var result = JsonConvert.DeserializeObject<BaseResult>(response.Content);
            return result;
        }
        protected void ComtApiIsCalledCreatedIsReturnedWithValidUrlIs(string url)
        {
            Result = ComtIvmtResult(url);
            Assert.AreEqual(ResultType.Created, Result.ResultType.ToString());
        }

        protected void ComtApiIsCalledForNotEnoughInventoryInCaseAndUrlIs(string url)
        {
            ResultForNegativeCase = ComtIvmtResult(url);
        }
        protected void ValidateForNotEnoughInventoryInCase()
        {
            Assert.AreEqual(ValidationMessage.InboundLpn, ResultForNegativeCase.ValidationMessages[0].FieldName);
        }
        protected void VerifyIvmtMessageWasInsertedIntoSwmToMhe(int qty)
        {
            Assert.AreEqual(DefaultValues.Status, SwmToMheIvmt.SourceMessageStatus);
            Assert.AreEqual(TransactionCode.Ivmt, Ivmt.TransactionCode);
            Assert.AreEqual(MessageLength.Ivmt, Ivmt.MessageLength);
            Assert.AreEqual(ActionCodeConstants.Create, Ivmt.ActionCode);
            Assert.AreEqual(SingleSkuCase.SkuId, Ivmt.Sku);
            Assert.AreEqual(qty, Convert.ToDouble(Ivmt.Quantity));
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
        protected void VerifyTheQuantityIsIncreasedInToTransInventory()
        {
           Assert.AreEqual(SingleSkuCase.ActualInventoryUnits + Convert.ToDecimal(Ivmt.Quantity), CaseDtlAfterApi.ActualInventoryUnits);       
           Assert.AreEqual((UnitWeight * Convert.ToDecimal(Ivmt.Quantity))+ SingleSkuCase.ActualWeight, Convert.ToDecimal(CaseDtlAfterApi.ActualWeight));
        }      
        protected void VerifyQuantityisReducedIntoCaseDetail()
        {
            Assert.AreEqual(Constants.QuantityReducedToZero, CaseDtlAfterApi.TotalAllocQty);
        }

        protected void VerifyActualQuantityIsReducedInToCaseDtl()
        {
            Assert.AreEqual(Constants.QuantityReducedToZero, CaseDtlAfterApi.ActlQty);
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

        protected void VerifyQtyShouldBeIncreasedInPickLocnTableForToBeFilledQtyField()
        {
            Assert.AreEqual(PickLocnBeforeCallingApi.ToBeFilledQty + Convert.ToInt16(Ivmt.Quantity),PickLocnAfterCallingApi.ToBeFilledQty );
        }
    }
}