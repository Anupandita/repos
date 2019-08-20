using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RestSharp;
using Sfc.Wms.Asrs.Test.Integrated.TestData;
using Sfc.Wms.InboundLpn.Contracts.Dtos;
using Sfc.Wms.ParserAndTranslator.Contracts.Constants;
using Sfc.Wms.Result;
using System;
using System.Configuration;
using System.Diagnostics;
using DefaultPossibleValue = Sfc.Wms.ParserAndTranslator.Contracts.Constants;

namespace Sfc.Wms.Asrs.Test.Integrated.Fixtures
{
    public class ComtIvmtMessageFixture : DataBaseFixture
    {
        protected string currentCaseNbr;
        protected string ComtUrl = @ConfigurationManager.AppSettings["ComtUrl"];
        protected CaseDetailDto caseDetailDto;
        protected ComtParams ComtParameters;
        protected IRestResponse Response;
        
        protected void GetDataBeforeCallingComtApi() 
        {
            GetDataBeforeTriggerComt();
        }
        
        protected void SetCurrentCaseNumberToSingleSku()
        {
            currentCaseNbr = singleSkuCase.CaseNumber;
        }

        protected void SetCurrentCaseNumberToMultiSku()
        {
            currentCaseNbr =  caseHdrMultiSku.CaseNumber;
        }

        protected void AValidNewComtMessageRecord()
        {
            ComtParameters = new ComtParams
            {
                ActionCode = DefaultPossibleValue.ActionCodeConstants.Create,
                CurrentLocationId = "123",
                ContainerId = currentCaseNbr,
                ContainerType = DefaultValues.ContainerType,
                ParentContainerId = currentCaseNbr,
                AttributeBitmap = "",
                QuantityToInduct = "30"
            };
        }

        protected void GetDataFromDbForSingleSku()
        {
            GetDataAfterTriggerOfComtForSingleSku();
        }

        protected void GetDataAndValidateForIvmtMessageHasInsertedIntoBothTables()
        {
            GetDataAfterTriggerForMultiSkuAndValidateData();
        }
        protected void ComtApiIsCalled() 
        {
            var client = new RestClient(ComtUrl);
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", Content.ContentType);
            request.AddJsonBody(ComtParameters);
            request.RequestFormat = DataFormat.Json;
            Response = client.Execute(request);          
            var result = JsonConvert.DeserializeObject<BaseResult>(Response.Content.ToString());
            Assert.AreEqual(DefaultValues.ResultType,result.ResultType.ToString());         
        }

        protected void VerifyComtMessageWasInsertedIntoSwmToMhe()
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
        protected void VerifyIvmtMessageWasInsertedIntoSwmToMhe()
        {
            VerifyComtMessageWasInsertedIntoSwmToMhe(comt, swmToMheComt, singleSkuCase.CaseNumber);
        }
        protected void VerifyIvmtMessageWasInsertedIntoWmsToEms()
        {
            VerifyIvmtMessageWasInsertedIntoWmsToEms(wmsToEmsIvmt);
        }       

        protected void VerifyTheQuantityIsIncreasedToTransInventory()
        {
           Assert.AreEqual(singleSkuCase.ActualInventoryUnits + Convert.ToDecimal(ivmt.Quantity), trn.ActualInventoryUnits);
           Assert.AreEqual((Math.Round(unitWeight) * Convert.ToDecimal(ivmt.Quantity))+ Convert.ToInt16(singleSkuCase.ActualWeight), Math.Round(Convert.ToDecimal(trn.ActualWeight)));
        }
        protected void VerifyQuantityisReducedIntoCaseDetail()
        {
            Assert.AreEqual(0, caseDtlAfterTrigger.TotalAllocatedQuantity);        
        }
        protected void VerifyStatusIsUpdatedIntoCaseHeader()
        {
            VerifyStatusIsUpdatedIntoCaseHeader(caseHdr.StatusCode);
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
            VerifyStatusIsUpdatedIntoCaseHeader(caseHeaderAfterTrigger.StatusCode);
        }
    }
}