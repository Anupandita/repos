using System.Net;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using RestSharp;
using Sfc.Core.OnPrem.Result;
using Sfc.Core.RestResponse;
using Sfc.Wms.App.Api.Nuget.Gateways;

namespace Sfc.Wms.App.Api.Tests.Unit.Fixtures
{
    public abstract class EmsToWmsMessageGatewayFixture
    {
        private readonly EmsToWmsMessageGateway _emsToWmsMessageGateway;

        private readonly Mock<IRestClient> _restClient;

        private BaseResult manipulationTestResult;

        protected EmsToWmsMessageGatewayFixture()
        {
            _restClient = new Mock<IRestClient>();
            var reponseBuilder = new ResponseBuilder();
            _emsToWmsMessageGateway = new EmsToWmsMessageGateway(_restClient.Object, reponseBuilder);
        }

        private void GetRestResponse1<T>(T entity, HttpStatusCode statusCode, ResponseStatus responseStatus)
            where T : new()
        {
            var response = new Mock<IRestResponse<T>>();
            response.Setup(_ => _.StatusCode).Returns(statusCode);
            response.Setup(_ => _.ResponseStatus).Returns(responseStatus);
            response.Setup(_ => _.Content).Returns(JsonConvert.SerializeObject(entity));
            _restClient.Setup(x => x.ExecuteTaskAsync<T>(It.IsAny<IRestRequest>()))
                .Returns(Task.FromResult(response.Object));
        }

        protected void InvalidInputData()
        {
            var result = new BaseResult {ResultType = ResultTypes.BadRequest};
            GetRestResponse1(result, HttpStatusCode.BadRequest, ResponseStatus.Completed);
        }

        protected void ValidInputData()
        {
            var result = new BaseResult {ResultType = ResultTypes.Created};
            GetRestResponse1(result, HttpStatusCode.Created, ResponseStatus.Completed);
        }

        protected void EmsToWmsMessageProcessorInvoked()
        {
            manipulationTestResult = _emsToWmsMessageGateway.CreateAsync(It.IsAny<long>()).Result;
        }

        protected void EmsToWmsMessageMessageShouldBeProcessed()
        {
            Assert.IsNotNull(manipulationTestResult);
            Assert.AreEqual(manipulationTestResult.ResultType, ResultTypes.Created);
        }

        protected void EmsToWmsMessageMessageShouldNotBeProcessed()
        {
            Assert.IsNotNull(manipulationTestResult);
            Assert.AreEqual(manipulationTestResult.ResultType, ResultTypes.BadRequest);
        }
    }
}