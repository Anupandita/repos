using DataGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using RestSharp;
using Sfc.Core.OnPrem.Result;
using Sfc.Core.RestResponse;
using Sfc.Wms.App.Api.Contracts.Dto;
using Sfc.Wms.App.Api.Nuget.Gateways;
using Sfc.Wms.Configuration.SystemCode.Contracts.Dtos;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Sfc.Wms.App.Api.Nuget.Interfaces;

namespace Sfc.Wms.App.Api.Tests.Unit.Fixtures
{
    public class CommonGatewayFixture
    {
        private readonly CommonGateway _commonGateway;
        private readonly Mock<IRestCsharpClient> _restClient;
        private SystemCodeInputDto systemCodeInputDto;
        private BaseResult<IEnumerable<SysCodeDto>> testBaseResult;

        protected CommonGatewayFixture()
        {
            _restClient = new Mock<IRestCsharpClient>();
            _commonGateway = new CommonGateway(new ResponseBuilder(), _restClient.Object);
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

        protected void InputParametersToGetCodeIds()
        {
            systemCodeInputDto = Generator.Default.Single<SystemCodeInputDto>();
            var result = new BaseResult<IEnumerable<SysCodeDto>>
            { ResultType = ResultTypes.Ok, Payload = Generator.Default.List<SysCodeDto>(10) };
            GetRestResponse(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void InValidInputParametersToGetCodeIds()
        {
            systemCodeInputDto = Generator.Default.Single<SystemCodeInputDto>();
            var result = new BaseResult<IEnumerable<SysCodeDto>> { ResultType = ResultTypes.BadRequest };
            GetRestResponse(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void GetCodeIdsOperationInvoked()
        {
            testBaseResult = _commonGateway.GetCodeIdsAsync(systemCodeInputDto, It.IsAny<string>()).Result;
        }

        protected void TheGetCodeIdsOperationReturnedOkAsResponseStatus()
        {
            VerifyRestClientInvocation<BaseResult<IEnumerable<SysCodeDto>>>();
            Assert.IsNotNull(testBaseResult);
            Assert.AreEqual(ResultTypes.Ok, testBaseResult.ResultType);
        }

        protected void TheGetCodeIdsOperationReturnedBadRequestAsResponseStatus()
        {
            VerifyRestClientInvocation<BaseResult<IEnumerable<SysCodeDto>>>();
            Assert.IsNotNull(testBaseResult);
            Assert.AreEqual(ResultTypes.BadRequest, testBaseResult.ResultType);
        }
    }
}