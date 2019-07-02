using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using DataGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using RestSharp;
using Sfc.App.Api.Nuget.Gateways;
using Sfc.Wms.Result;
using Sfc.Wms.Security.Rbac.Contracts.Dtos;
using Sfc.Wms.Security.Rbac.Contracts.Dtos.UI;

namespace Sfc.App.Api.Tests.Unit.Fixtures
{
    public class UserRbacGatewayFixture
    {
        private readonly Mock<IRestClient> _restClient;
        private readonly UserRbacGateway _userRbacGateway;
        private LoginCredentials loginCredentials;
        private BaseResult<UserDetailsDto> signInBaseResult;

        public UserRbacGatewayFixture()
        {
            _restClient = new Mock<IRestClient>(MockBehavior.Default);
            _userRbacGateway = new UserRbacGateway(_restClient.Object);
        }

        private void GetRestResponse<T>(T entity, HttpStatusCode statusCode, ResponseStatus responseStatus)
            where T : new()
        {
            var response = new Mock<IRestResponse<T>>();
            response.Setup(_ => _.StatusCode).Returns(statusCode);
            response.Setup(_ => _.ResponseStatus).Returns(responseStatus);
            response.Setup(_ => _.Headers).Returns(new List<Parameter>
                {new Parameter("Bearer", "Bearer Value", ParameterType.HttpHeader)});
            response.Setup(_ => _.Content).Returns(JsonConvert.SerializeObject(entity));
            _restClient.Setup(x => x.ExecuteTaskAsync<T>(It.IsAny<IRestRequest>()))
                .Returns(Task.FromResult(response.Object));
        }

        #region Sign In

        protected void InValidLoginCredentials()
        {
            loginCredentials = Generator.Default.Single<LoginCredentials>();
            var result = new BaseResult<UserDetailsDto>
            {
                Payload = Generator.Default.Single<UserDetailsDto>(),
                ResultType = ResultTypes.Unauthorized
            };
            GetRestResponse(result, HttpStatusCode.Unauthorized, ResponseStatus.Completed);
        }

        protected void ValidLoginCredentials()
        {
            loginCredentials = Generator.Default.Single<LoginCredentials>();
            var result = new BaseResult<UserDetailsDto>
            {
                Payload = Generator.Default.Single<UserDetailsDto>(),
                ResultType = ResultTypes.Ok
            };
            GetRestResponse(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void EmptyLoginCredentials()
        {
            loginCredentials = new LoginCredentials();
            var result = new BaseResult<UserDetailsDto>
            {
                Payload = Generator.Default.Single<UserDetailsDto>(),
                ResultType = ResultTypes.BadRequest
            };
            GetRestResponse(result, HttpStatusCode.BadRequest, ResponseStatus.Completed);
        }

        protected void UserAuthenticationServiceIsInvoked()
        {
            signInBaseResult = _userRbacGateway.SignInAsync(loginCredentials).Result;
        }

        protected void TheServiceCallReturnedBadRequestAsResponseStatus()
        {
            Assert.IsNotNull(signInBaseResult);
            Assert.AreEqual(signInBaseResult.ResultType, ResultTypes.BadRequest);
        }

        protected void TheServiceCallReturnedUnAuthorizesAsResponseStatus()
        {
            Assert.IsNotNull(signInBaseResult);
            Assert.AreEqual(signInBaseResult.ResultType, ResultTypes.Unauthorized);
        }

        protected void TheServiceCallReturnedAuthorizedAsResponseStatus()
        {
            Assert.IsNotNull(signInBaseResult);
            Assert.IsNotNull(signInBaseResult.Payload);
            Assert.AreEqual(signInBaseResult.ResultType, ResultTypes.Ok);
        }

        #endregion
    }
}