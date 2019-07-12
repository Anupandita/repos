using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using RestSharp;
using Sfc.App.Api.Nuget.Gateways;
using Sfc.Wms.Result;
using System.Net;
using System.Threading.Tasks;
using Sfc.Wms.Interface.Asrs.Dtos;

namespace Sfc.App.Api.Tests.Unit.Fixtures
{
    public abstract class ComtGatewayFixture
    {
        private readonly ComtGateway _comtGateway;

        private readonly Mock<IRestClient> _restClient;

        private BaseResult manipulationTestResult;

        protected ComtGatewayFixture()
        {
            _restClient = new Mock<IRestClient>();
            _comtGateway = new ComtGateway(_restClient.Object);
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

        protected void ComtProcessorInvoked()
        {
            manipulationTestResult = _comtGateway.CreateAsync(It.IsAny<ComtTriggerInputDto>()).Result;
        }

        protected void ComtMessageShoulBeProcessed()
        {
            Assert.IsNotNull(manipulationTestResult);
            Assert.AreEqual(manipulationTestResult.ResultType, ResultTypes.Created);
        }

        protected void ComtMessageShoulNotBeProcessed()
        {
            Assert.IsNotNull(manipulationTestResult);
            Assert.AreEqual(manipulationTestResult.ResultType, ResultTypes.BadRequest);
        }
    }
}