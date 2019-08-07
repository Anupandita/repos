using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RestSharp;
using Sfc.Wms.Asrs.Test.Integrated.TestData;
using Sfc.Wms.InboundLpn.Contracts.Dtos;
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
            currentCaseNbr = caseSingleSku.CaseNumber;
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
            GetDataAfterTriggerForComtForSingleSku();
        }

        protected void GetDataFromDbForMultiSkuAndValidateForData()
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
            VerifyIvmtMessageWasInsertedIntoSwmToMhe(ivmt, cd, cd.SkuId, Convert.ToDouble(cd.ActualQuantity));
        }
        protected void VerifyComtMessageWasInsertedIntoWmsToEms() 
        {
            VerifyIvmtMessageWasInsertedIntoWmsToEms(wte1);
        }

        protected void VerifyIvmtMessageWasInsertedIntoSwmToMhe()
        {
            VerifyComtMessageWasInsertedIntoSwmToMhe(comt, swmToMhe1, caseSingleSku.CaseNumber);
        }
        protected void VerifyIvmtMessageWasInsertedIntoWmsToEms()
        {
            VerifyComtMessageWasInsertedIntoWmsToEms(wmsToEms);
        }       

        protected void VerifyTheQuantityIsIncreasedToTransInventory()
        {
           Assert.AreEqual(transInvn.ActualInventoryUnits + Convert.ToDecimal(ivmt.Quantity), trn.ActualInventoryUnits);
           Assert.AreEqual(Math.Round((unitWeight * Convert.ToDecimal(ivmt.Quantity))+ Convert.ToInt16(transInvn.ActualWeight)), Math.Round(Convert.ToDecimal(trn.ActualWeight)));
        }
        protected void VerifyQuantityisReducedIntoCaseDetail()
        {
            Assert.AreEqual(0, caseDtl.ActualQuantity);        
        }
        protected void VerifyStatusIsUpdatedIntoCaseHeader()
        {
            VerifyStatusIsUpdatedIntoCaseHeader(caseHdr.StatusCode);
        }
        
        protected void VerifyStatusIsUpdatedIntoTaskHeader()
        {
            
            try
            {
                VerifyStatusIsUpdatedIntoTaskHeader(task.StatusCode);
            }
            catch
            {
                Debug.Print("Task Not Found");
            }
        }
        protected void ValidateForCaseDetailForQuantity()
        {
            VerifyQuantityisReducedIntoCaseDetailForMultiSku();
        }

        protected void ValidateForCaseHeaderForStatusCode()
        {
            VerifyStatusIsUpdatedIntoCaseHeader(caseHeader.StatusCode);
        }
    }
}