using DataGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using RestSharp;
using Sfc.Core.OnPrem.Result;
using Sfc.Core.RestResponse;
using Sfc.Wms.App.Api.Nuget.Gateways;
using Sfc.Wms.App.Api.Nuget.Interfaces;
using Sfc.Wms.Configuration.MessageLogger.Contracts.UoW.Dtos;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Sfc.Wms.App.Api.Tests.Unit.Fixtures
{
    public class MessageLogGatewayFixture
    {
        private readonly IMessageLoggerGateway _messageLogController;
        private IEnumerable<MessageLogDto> logs;
        private BaseResult testResponse;
        private readonly Mock<IRestCsharpClient> _restClient;


        protected MessageLogGatewayFixture()
        {
            _restClient = new Mock<IRestCsharpClient>();
            _messageLogController = new MessageLoggerGateway(new ResponseBuilder(), _restClient.Object);
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

        protected void InputParametersForBatchInsertion()
        {
            logs = Generator.Default.List<MessageLogDto>(2);
            var result = new BaseResult { ResultType = ResultTypes.Created };
            GetRestResponse(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void EmptyOrNullInputForInsertion()
        {
            logs = null;
            var result = new BaseResult { ResultType = ResultTypes.BadRequest };
            GetRestResponse(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void BatchInsertionOperationInvoked()
        {
            testResponse = _messageLogController.BatchInsertAsync(logs, It.IsAny<string>()).Result;
        }

        protected void TheReturnedBadRequestResponse()
        {
            VerifyRestClientInvocation<BaseResult>();
            Assert.IsNotNull(testResponse);
            Assert.AreEqual(ResultTypes.BadRequest, testResponse.ResultType);
        }

        protected void TheReturnedCreatedResponse()
        {
            VerifyRestClientInvocation<BaseResult>();
            Assert.IsNotNull(testResponse);
            Assert.AreEqual(ResultTypes.Created, testResponse.ResultType);
        }

    }
}