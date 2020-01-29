using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using DataGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using RestSharp;
using Sfc.Core.OnPrem.Result;
using Sfc.Core.OnPrem.Security.Contracts.Dtos;
using Sfc.Core.RestResponse;
using Sfc.Wms.App.Api.Nuget.Gateways;
using Sfc.Wms.App.Api.Nuget.Interfaces;

namespace Sfc.Wms.App.Api.Tests.Unit.Fixtures
{
    public class UserRbacGatewayFixture
    {
        private readonly Mock<IRestCsharpClient> _mockRestClient;
        private readonly UserRbacGateway _userRbacGateway;
        private LoginCredentials loginCredentials;
        private BaseResult<UserInfoDto> signInResponse;
        private BaseResult<UserInfoDto> userDetails;

        protected UserRbacGatewayFixture()
        {
            _mockRestClient = new Mock<IRestCsharpClient>(MockBehavior.Default);
            var reponseBuilder = new ResponseBuilder();
            _userRbacGateway = new UserRbacGateway(_mockRestClient.Object, reponseBuilder);
        }

        private void GetRestResponse<T>(T entity, HttpStatusCode statusCode, ResponseStatus responseStatus)
            where T : new()
        {
            var response = new Mock<IRestResponse<T>>();
            response.Setup(el => el.StatusCode).Returns(statusCode);
            response.Setup(el => el.ResponseStatus).Returns(responseStatus);
            response.Setup(el => el.Headers).Returns(new List<Parameter>
                {new Parameter("Bearer", "Bearer Value", ParameterType.HttpHeader)});
            response.Setup(el => el.Content).Returns(JsonConvert.SerializeObject(entity));
            _mockRestClient.Setup(el => el.ExecuteTaskAsync<T>(It.IsAny<IRestRequest>()))
                .Returns(Task.FromResult(response.Object));
        }

        #region Sign In

        private void SetUserDetails(ResultTypes resultTypes)
        {
            userDetails = new BaseResult<UserInfoDto>
            {
                Payload = Generator.Default.Single<UserInfoDto>(),
                ResultType = resultTypes
            };
        }

        protected void InValidLoginCredentials()
        {
            loginCredentials = Generator.Default.Single<LoginCredentials>();
            SetUserDetails(ResultTypes.Unauthorized);
            GetRestResponse(userDetails, HttpStatusCode.Unauthorized, ResponseStatus.Completed);
        }

        protected void ValidLoginCredentials()
        {
            loginCredentials = Generator.Default.Single<LoginCredentials>();
            SetUserDetails(ResultTypes.Ok);
            GetRestResponse(userDetails, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void EmptyLoginCredentials()
        {
            loginCredentials = new LoginCredentials();
            SetUserDetails(ResultTypes.BadRequest);
            GetRestResponse(userDetails, HttpStatusCode.BadRequest, ResponseStatus.Completed);
        }

        protected void UserAuthenticationServiceIsInvoked()
        {
            signInResponse = _userRbacGateway.SignInAsync(loginCredentials).Result;
        }

        protected void TheServiceCallReturnedBadRequestAsResponseStatus()
        {
            Assert.IsNotNull(signInResponse);
            Assert.AreEqual(signInResponse.ResultType, ResultTypes.BadRequest);
        }

        protected void TheServiceCallReturnedUnAuthorizesAsResponseStatus()
        {
            Assert.IsNotNull(signInResponse);
            Assert.AreEqual(signInResponse.ResultType, ResultTypes.Unauthorized);
        }

        protected void TheServiceCallReturnedAuthorizedAsResponseStatus()
        {
            Assert.IsNotNull(signInResponse);
            Assert.IsNotNull(signInResponse.Payload);
            Assert.AreEqual(signInResponse.ResultType, ResultTypes.Ok);
        }

        #endregion Sign In
    }
}