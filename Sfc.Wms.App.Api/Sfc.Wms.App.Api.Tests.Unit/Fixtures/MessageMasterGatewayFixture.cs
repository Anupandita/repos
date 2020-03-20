using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using DataGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using RestSharp;
using Sfc.Core.OnPrem.Result;
using Sfc.Core.RestResponse;
using Sfc.Wms.App.Api.Controllers;
using Sfc.Wms.App.Api.Nuget.Gateways;
using Sfc.Wms.App.Api.Nuget.Interfaces;
using Sfc.Wms.Configuration.MessageLogger.Contracts.Interfaces;
using Sfc.Wms.Configuration.MessageMaster.Contracts.Dtos;

namespace Sfc.Wms.App.Api.Tests.Unit.Fixtures
{
    public class MessageMasterGatewayFixture
    {
        private readonly IMessageMasterGateway _messageMasterGateway;
        private readonly Mock<IRestCsharpClient> _restClient;
        private BaseResult<IEnumerable<MessageDetailDto>> messageDetails;

        protected MessageMasterGatewayFixture()
        {
            _restClient = new Mock<IRestCsharpClient>();
            _messageMasterGateway = new MessageMasterGateway(new ResponseBuilder(), _restClient.Object);
        }

        private void GetRestResponse<T>(T entity, HttpStatusCode statusCode, ResponseStatus responseStatus)
            where T : new()
        {
            var response = new Mock<IRestResponse<T>>();
            response.Setup(_ => _.StatusCode).Returns(statusCode);
            response.Setup(_ => _.ResponseStatus).Returns(responseStatus);
            response.Setup(_ => _.Content).Returns(JsonConvert.SerializeObject(entity));
            _restClient.Setup(x => x.ExecuteTaskAsync<T>(It.IsAny<IRestRequest>()))
                .Returns(Task.FromResult(response.Object));
        }

        private void VerifyRestClientInvocation<T>() where T : new()
        {
            _restClient.Verify(x => x.ExecuteTaskAsync<T>(It.IsAny<IRestRequest>()));
        }

        protected void InputForUiSpecificMessageDetails()
        {
            var result = new BaseResult<IEnumerable<MessageDetailDto>>
                { ResultType = ResultTypes.Ok, Payload = Generator.Default.List<MessageDetailDto>(10) };
            GetRestResponse(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void InputForWhichNoRecordsExists()
        {
            var result = new BaseResult<IEnumerable<MessageDetailDto>> { ResultType = ResultTypes.BadRequest };
            GetRestResponse(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void GetMessageDetailsOperationInvoked()
        {
            messageDetails = _messageMasterGateway.GetMessageDetailsAsync(It.IsAny<string>()).Result;
        }

        protected void TheGetOperationReturnedOkAsResponseStatus()
        {
            VerifyRestClientInvocation<BaseResult<IEnumerable<MessageDetailDto>>>();
            Assert.IsNotNull(messageDetails);
            Assert.AreEqual(ResultTypes.Ok, messageDetails.ResultType);
        }

        protected void TheGetOperationReturnedBadRequestAsResponseStatus()
        {
            VerifyRestClientInvocation<BaseResult<IEnumerable<MessageDetailDto>>>();
            Assert.IsNotNull(messageDetails);
            Assert.AreEqual(ResultTypes.BadRequest, messageDetails.ResultType);
        }
    }
}