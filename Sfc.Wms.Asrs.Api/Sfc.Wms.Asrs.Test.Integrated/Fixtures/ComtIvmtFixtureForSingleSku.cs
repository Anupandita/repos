using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.Asrs.Test.Integrated.TestData;
using RestSharp;
using System.Net;
using System.Diagnostics;
using Sfc.Wms.DematicMessage.Contracts.Dto;
using Sfc.Wms.Asrs.Shamrock.Contracts.Dtos;
using Sfc.Wms.Amh.Dematic.Contracts.Dtos;
using Sfc.Wms.Asrs.Dematic.Contracts.Dtos;
using Newtonsoft.Json;
using System.Linq;
using Oracle.ManagedDataAccess.Client;
using DefaultPossibleValue = Sfc.Wms.DematicMessage.Contracts.Constants;
using Sfc.Wms.Result;

namespace Sfc.Wms.Asrs.Test.Integrated.Fixtures
{
    public class ComtIvmtFixtureForSingleSku : DataBaseFixture
    {
        protected string currentCaseNbr;
        protected string ComtUrl = "http://localhost:59351/api/comt";
        protected CaseDetailDto caseDetailDto;
        protected ComtParams ComtParameters;
        protected IRestResponse Response;

        protected void GetDataBeforeCallingComtApi() 
        { 
            GetDataBeforeBeforeTriggerComt();
        }

        protected void GetDataAfterCallingApi()
        {
            GetDataAfterTriggerForComt();
        }
       
        protected void AValidNewComtMessageRecord()
        {
            ComtParameters = new ComtParams();
            ComtParameters.ActionCode = DefaultPossibleValue.ActionCodeConstants.create;
            ComtParameters.CurrentLocationId = "123";
            ComtParameters.ContainerId = cn.CaseNumber;
            ComtParameters.ContainerType = "Case";
            ComtParameters.ParentContainerId = cn.CaseNumber;
            ComtParameters.AttributeBitmap = "";
            ComtParameters.QuantityToInduct = "30";
        }

        public void AComtApiIsCalled() 
        {   var client = new RestClient(ComtUrl);
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", Content.ContentType);
            request.AddJsonBody(ComtParameters);
            request.RequestFormat = DataFormat.Json;
            Response = client.Execute(request);
         //   Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        protected void CheckMessageIsInsertedInSwmToMheForComtMessage() 
        {
             Assert.AreEqual("Ready",swmToMhe.SourceMessageStatus);          
            //Assert.AreEqual(caseHeader.LocationId, s.LocationId);
            Assert.AreEqual("Case", swmToMhe.ContainerType);
            Assert.AreEqual(0, caseHeader.ActlWt);           
            Assert.AreEqual(DefaultPossibleValue.TransactionCode.Comt, comt.TransactionCode);
            Assert.AreEqual(DefaultPossibleValue.MessageLength.Comt, comt.MessageLength);
            Assert.AreEqual(DefaultPossibleValue.ActionCodeConstants.create,comt.ActionCode);
            Assert.AreEqual(caseHeader.CaseNumber,swmToMhe.ContainerId);
            Assert.AreEqual("Case", swmToMhe.ContainerType);                       
        }

        protected void CheckMessageIsInsertedInWmsToEmsForComtMessage() 
        {
            Assert.AreEqual(swmToMhe.SourceMessageStatus,w.Status);
            Assert.AreEqual(DefaultPossibleValue.TransactionCode.Comt,w.Transaction);
            Assert.AreEqual(swmToMhe.SourceMessageProcess,w.AddWho);
            Assert.AreEqual(Convert.ToInt16(swmToMhe.SourceMessageResponseCode), w.ResponseCode);
        }

        protected void CheckMessageIsInsertedInSwmToMheForIvmtMessage() 
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

        protected void CheckMessageIsInsertedInWmsToEmsForIvmtMessage() 
        {
            Assert.AreEqual(stm.SourceMessageStatus, wte.Status);
            Assert.AreEqual(DefaultPossibleValue.TransactionCode.Ivmt, wte.Transaction);
            Assert.AreEqual(stm.SourceMessageProcess, wte.AddWho);
            Assert.AreEqual(Convert.ToInt16(stm.SourceMessageResponseCode),wte.ResponseCode);
        }

        protected void CheckInTransInventoryTable()
        {
           Assert.AreEqual(transInvn.ActualInventoryUnits + Convert.ToDecimal(ivmt.Quantity), trn.ActualInventoryUnits);
           Assert.AreEqual(((unitWeight * Convert.ToDecimal(ivmt.Quantity))+ Convert.ToInt16(transInvn.ActualWeight)), Convert.ToDecimal(trn.ActualWeight));
        }

        protected void CheckInCaseDetailForQuantity()
        {
            Assert.AreEqual(0, caseDtl.ActlQty);
            Assert.AreEqual(0, caseDtl.TotalAllocQty);
        }

        protected void CheckInCaseHdrForStatusCode()
        {
            Assert.AreEqual(96, caseHdr.StatCode);
        }

        protected void CheckInTaskHeaderForStatusCode()
        {
            Assert.AreEqual(90,task.StatusCode);
        }

    }
}