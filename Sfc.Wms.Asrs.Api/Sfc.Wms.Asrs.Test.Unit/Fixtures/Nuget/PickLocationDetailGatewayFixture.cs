using DataGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using RestSharp;
using Sfc.Wms.Asrs.Nuget.Gateways;
using Sfc.Wms.DematicMessage.Contracts.Dto;
using Sfc.Wms.Result;
using System.Net;
using System.Threading.Tasks;

namespace Sfc.Wms.Asrs.Test.Unit.Fixtures.Nuget
{
    public abstract class PickLocationDetailGatewayFixture
    {
        private readonly PickLocationDtlGateway _pickLocationDtlGateway;

        private readonly Mock<IRestClient> _restClient;

        private BaseResult manipulationTestResult;

        protected PickLocationDetailGatewayFixture()
        {
            _restClient = new Mock<IRestClient>();
            _pickLocationDtlGateway = new PickLocationDtlGateway(_restClient.Object);
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

        protected void ValidData()
        {
            var result = new BaseResult { ResultType = ResultTypes.Created };
            GetRestResponse1(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void UpdatePickLocationDetailInvoked()
        {
            var request = Generator.Default.Single<CostDto>();
            manipulationTestResult = _pickLocationDtlGateway.UpdateAsync(request).Result;
        }

        protected void PickLocationDetailShouldBeUpdated()
        {
            Assert.IsNotNull(manipulationTestResult);
            Assert.AreEqual(manipulationTestResult.ResultType, ResultTypes.Created);
        }

        protected void InvalidData()
        {
            var result = new BaseResult {  ResultType = ResultTypes.BadRequest };
            GetRestResponse1(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void PickLocationDetailShouldNotBeUpdated()
        {
            Assert.IsNotNull(manipulationTestResult);
            Assert.AreEqual(manipulationTestResult.ResultType, ResultTypes.BadRequest);
        }
    }
}