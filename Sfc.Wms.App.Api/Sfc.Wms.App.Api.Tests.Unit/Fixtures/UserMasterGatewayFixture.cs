using DataGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using RestSharp;
using Sfc.Core.OnPrem.Result;
using Sfc.Core.RestResponse;
using Sfc.Wms.App.Api.Nuget.Gateways;
using System.Net;
using System.Threading.Tasks;
using Sfc.Core.OnPrem.Security.Contracts.Dtos;
using System.Collections.Generic;
using Sfc.Wms.App.Api.Nuget.Interfaces;

namespace Sfc.Wms.App.Api.Tests.Unit.Fixtures
{
    public abstract class UserMasterGatewayFixture
    {
        private readonly UserMasterGateway _userMasterGateway;
        private readonly Mock<IRestCsharpClient> _restClient;
        private readonly IEnumerable <PreferencesDto> _preferencesDto;
        private BaseResult manipulationTestResult;

        protected UserMasterGatewayFixture()
        {
            _restClient = new Mock<IRestCsharpClient>();
            _preferencesDto = Generator.Default.List<PreferencesDto>();
            _userMasterGateway = new UserMasterGateway(new ResponseBuilder(), _restClient.Object);
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
        
        #region Update User Preferences 

        protected void ValidParametersToUpdateUserPreferences()
        {
            var result = new BaseResult<IEnumerable<PreferencesDto>>
            {
                ResultType = ResultTypes.Ok
            };
            GetRestResponse(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void InvalidParametersToUpdateUserPreferences()
        {
            var result = new BaseResult<IEnumerable<PreferencesDto>>
            {
                ResultType = ResultTypes.BadRequest
            };
            GetRestResponse(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void UpdateUserPreferencesInvoked()
        {
            manipulationTestResult = _userMasterGateway.UpdateUserPreferences(_preferencesDto, It.IsAny<string>()).Result;
        }

        protected void UpdateUserPreferencesReturnedOkResponse()
        {
            VerifyRestClientInvocation<BaseResult<IEnumerable<PreferencesDto>>>();
            Assert.IsNotNull(manipulationTestResult);
            Assert.AreEqual(ResultTypes.Ok, manipulationTestResult.ResultType);
        }

        protected void UpdateUserPreferencesReturnedNotFoundResponse()
        {
            VerifyRestClientInvocation<BaseResult<IEnumerable<PreferencesDto>>>();
            Assert.IsNotNull(manipulationTestResult);
            Assert.AreEqual(ResultTypes.BadRequest, manipulationTestResult.ResultType);
        }

        #endregion Update User Preferences 
    }
}
