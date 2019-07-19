using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.Asrs.Test.Integrated.TestData;
using RestSharp;
using Sfc.Wms.Amh.Dematic.Contracts.Dtos;
namespace Sfc.Wms.Asrs.Test.Integrated.Fixtures
{

    public class ComtIvmtFixtureForMultiSku : DataBaseFixtureForMultiSku
    {
        protected string currentCaseNbr;
        protected string ComtUrl = "http://localhost:59351/api/comt";
        protected CaseDetailDto caseDetailDto;
        protected ComtParams ComtParameters;

        protected void GetDataBeforeCallingApi()
        {
            GetDataBeforeTriggerForComt();
        }

        protected void AValidNewComtMessageRecord()
        {
            ComtParameters = new ComtParams();
            ComtParameters.ActionCode = "Create";
            ComtParameters.CurrentLocationId = "123";
            ComtParameters.ContainerId = cn.CaseNumber;
            ComtParameters.ContainerType = "Case";
            ComtParameters.ParentContainerId = cn.CaseNumber;
            ComtParameters.AttributeBitmap = "";
            ComtParameters.QuantityToInduct = "30";
        }

        public void AComtApiIsCalled()
        {
            var client = new RestClient(ComtUrl);
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", Content.ContentType);
            request.AddJsonBody(ComtParameters);
            request.RequestFormat = DataFormat.Json;
            var response = client.Execute(request);
          //Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        protected void GetDataAfterCallingApiAndValidateData()
        {
            GetDataAndValidateForComtAndIvmt();

        }
        protected void CheckMessageIsInsertedInSwmToMheForComtMessage()
        {
            Assert.AreEqual("Ready", swmToMhe.SourceMessageStatus);
            //Assert.AreEqual(caseHeader.LocationId, s.LocationId);
            Assert.AreEqual("Case", swmToMhe.ContainerType);
            Assert.AreEqual(0, caseHeader.ActlWt);
            Assert.AreEqual("COMT", comt.TransactionCode);
            Assert.AreEqual("00241", comt.MessageLength);
            Assert.AreEqual("Create", comt.ActionCode);
            Assert.AreEqual(caseHeader.CaseNumber, swmToMhe.ContainerId);
            Assert.AreEqual("Case", swmToMhe.ContainerType);
        }

        protected void CheckMessageIsInsertedInWmsToEmsForComtMessage()
        {
            Assert.AreEqual(swmToMhe.SourceMessageStatus, wmsToEms.Status);
            Assert.AreEqual("COMT", wmsToEms.Transaction);
            Assert.AreEqual(swmToMhe.SourceMessageProcess, wmsToEms.AddWho);
            Assert.AreEqual(Convert.ToInt16(swmToMhe.SourceMessageResponseCode), wmsToEms.ResponseCode);
        }

        protected void ValidateForCaseDetailForQuantity()
       {
            CheckInCaseDetailForQuantity();
       }

        protected void ValidateForCaseHdrForStatusCode()
        {
            CheckInCaseHdrForStatusCode();
        }
    }
}
