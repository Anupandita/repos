using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using RestSharp;
using Sfc.App.Api.Nuget.Gateways;
using Sfc.Wms.Interface.Asrs.Dtos;
using Sfc.Wms.Result;
using System.Net;
using System.Threading.Tasks;

namespace Sfc.App.Api.Tests.Unit.Fixtures
{
    public abstract class EmsToWmsMessageGatewayFixture
    {
        private readonly EmsToWmsMessageGateway _emsToWmsMessageGateway;

        private readonly Mock<IRestClient> _restClient;

        private BaseResult manipulationTestResult;

        protected EmsToWmsMessageGatewayFixture()
        {
            _restClient = new Mock<IRestClient>();
            _emsToWmsMessageGateway = new EmsToWmsMessageGateway(_restClient.Object);
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
            var result = new BaseResult { ResultType = ResultTypes.BadRequest };
            GetRestResponse1(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void ValidInputData()
        {
            var result = new BaseResult { ResultType = ResultTypes.Created };
            GetRestResponse1(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void EmsToWmsMessageProcessorInvoked()
        {
            manipulationTestResult = _emsToWmsMessageGateway.CreateAsync(It.IsAny<long>()).Result;
        }

        protected void EmsToWmsMessageMessageShoulBeProcessed()
        {
            Assert.IsNotNull(manipulationTestResult);
            Assert.AreEqual(manipulationTestResult.ResultType, ResultTypes.Created);
        }

        protected void EmsToWmsMessageMessageShoulNotBeProcessed()
        {
            Assert.IsNotNull(manipulationTestResult);
            Assert.AreEqual(manipulationTestResult.ResultType, ResultTypes.BadRequest);
        }
    }
}