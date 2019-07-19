using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.Asrs.Test.Integrated.TestData;
using RestSharp;
using Sfc.Wms.Amh.Dematic.Contracts.Dtos;
using Newtonsoft.Json;
using DefaultPossibleValue = Sfc.Wms.DematicMessage.Contracts.Constants;
using Sfc.Wms.Result;

namespace Sfc.Wms.Asrs.Test.Integrated.Fixtures
{
    public class ComtIvmtMessageFixture : DataBaseFixture
    {
        protected string currentCaseNbr;
        protected string ComtUrl = "http://localhost:59351/api/comt";
        protected CaseDetailDto caseDetailDto;
        protected ComtParams ComtParameters;
        protected IRestResponse Response;
        private dynamic testResult;

        protected void GetDataBeforeCallingComtApi() 
        { 
            GetDataBeforeBeforeTriggerComt();
        }
        
        protected void SetCurrentCaseNumberToSingleSku()
        {
            currentCaseNbr = caseNumberSingleSku;
        }

        protected void SetCurrentCaseNumberToMultiSku()
        {
            currentCaseNbr = caseNumberMultiSku;
        }

        protected void AValidNewComtMessageRecord()
        {
            ComtParameters = new ComtParams
            {
                ActionCode = DefaultPossibleValue.ActionCodeConstants.create,
                CurrentLocationId = "123",
                ContainerId = currentCaseNbr,
                ContainerType = "Case",
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
            Assert.AreEqual("Created",result.ResultType.ToString());         
        }
        protected void VerifyComtMessageWasInsertedIntoSwmToMhe() 
        {
            Assert.AreEqual("Ready",swmToMhe.SourceMessageStatus);                    
            Assert.AreEqual("Case", swmToMhe.ContainerType);
            Assert.AreEqual(0, caseHeader.ActlWt);           
            Assert.AreEqual(DefaultPossibleValue.TransactionCode.Comt, comt.TransactionCode);
            Assert.AreEqual(DefaultPossibleValue.MessageLength.Comt, comt.MessageLength);
            Assert.AreEqual(DefaultPossibleValue.ActionCodeConstants.create,comt.ActionCode);
            Assert.AreEqual(caseHeader.CaseNumber,swmToMhe.ContainerId);
            Assert.AreEqual("Case", swmToMhe.ContainerType);                       
        }
        protected void VerifyComtMessageWasInsertedIntoWmsToEms() 
        {          
            Assert.AreEqual(swmToMhe.SourceMessageStatus,w.Status);
            Assert.AreEqual(DefaultPossibleValue.TransactionCode.Comt,w.Transaction);
            Assert.AreEqual(swmToMhe.SourceMessageProcess,w.AddWho);
            Assert.AreEqual(Convert.ToInt16(swmToMhe.SourceMessageResponseCode), w.ResponseCode);
        }
        protected void VerifyIvmtMessageWasInsertedIntoSwmToMhe() 
        {
            Assert.AreEqual("Ready", stm.SourceMessageStatus);            
            Assert.AreEqual("Warehouse",ivmt.Owner);
            Assert.AreEqual(DefaultPossibleValue.TransactionCode.Ivmt, ivmt.TransactionCode);
            Assert.AreEqual(DefaultPossibleValue.MessageLength.Ivmt, ivmt.MessageLength);
            Assert.AreEqual(DefaultPossibleValue.ActionCodeConstants.create, ivmt.ActionCode);
            Assert.AreEqual(cd.SkuId,ivmt.Sku);
            Assert.AreEqual(cd.ActlQty,Convert.ToDouble(ivmt.Quantity));
            Assert.AreEqual("Case", ivmt.UnitOfMeasure);
            Assert.AreEqual(caseHeader.ManufacturingDate, ivmt.ManufactureDate);
            Assert.AreEqual(caseHeader.ShipByDate,ivmt.FifoDate);
            Assert.AreEqual(caseHeader.XpireDate,ivmt.ExpirationDate);
            Assert.AreEqual("F",ivmt.DateControl);           
        }
        protected void VerifyIvmtMessageWasInsertedIntoWmsToEms() 
        {
            Assert.AreEqual(stm.SourceMessageStatus, wte.Status);
            Assert.AreEqual(DefaultPossibleValue.TransactionCode.Ivmt, wte.Transaction);
            Assert.AreEqual(stm.SourceMessageProcess, wte.AddWho);
            Assert.AreEqual(Convert.ToInt16(stm.SourceMessageResponseCode),wte.ResponseCode);
        }
        protected void VerifyTheQuantityIsIncreasedToTransInventory()
        {
           Assert.AreEqual(transInvn.ActualInventoryUnits + Convert.ToDecimal(ivmt.Quantity), trn.ActualInventoryUnits);
           Assert.AreEqual(((unitWeight * Convert.ToDecimal(ivmt.Quantity))+ Convert.ToInt16(transInvn.ActualWeight)), Convert.ToDecimal(trn.ActualWeight));
        }
        protected void VerifyQuantityisReducedIntoCaseDetail()
        {
            Assert.AreEqual(0, caseDtl.ActlQty);        
        }
        protected void VerifyStatusIsUpdatedIntoCaseHeader()
        {
            Assert.AreNotEqual(50, caseHdr.StatCode);
            Assert.AreEqual(96, caseHdr.StatCode);
        }
        protected void VerifyStatusIsUpdatedIntoTaskHeader()
        {
            Assert.AreEqual(90,task.StatusCode);
        }
        protected void ValidateForCaseDetailForQuantity()
        {
            VerifyQuantityisReducedIntoCaseDetailForMultiSku();
        }

        protected void ValidateForCaseHeaderForStatusCode()
        {
            VerifyStatusIsUpdatedIntoCaseHeaderForMultiSku();
        }
    }
}