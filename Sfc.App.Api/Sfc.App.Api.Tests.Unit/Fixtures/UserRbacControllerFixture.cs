using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using DataGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sfc.App.Api.Controllers;
using Sfc.App.App.Interfaces;
using Sfc.Wms.Result;
using Sfc.Wms.Security.Rbac.Contracts.Dtos;
using Sfc.Wms.Security.Rbac.Contracts.Dtos.UI;

namespace Sfc.App.Api.Tests.Unit.Fixtures
{
    public abstract class UserRbacControllerFixture
    {
        private readonly Mock<IRbacGateway> _rabcGateway;
        private readonly UserRbacController _userRbacController;
        private bool isValid;

        private LoginCredentials request;
        private Task<IHttpActionResult> testResponse;

        protected UserRbacControllerFixture()
        {
            _rabcGateway = new Mock<IRbacGateway>(MockBehavior.Default);
            _userRbacController = new UserRbacController(_rabcGateway.Object);
        }

        protected void InvalidCredentials()
        {
            isValid = false;
            request = Generator.Default.Single<LoginCredentials>();
        }

        protected void ValidCredentials()
        {
            isValid = true;
            request = Generator.Default.Single<LoginCredentials>();
        }

        protected void UserLogsIn()
        {
            var userDetails = new BaseResult<UserDetailsDto>
            {
                Payload = Generator.Default.Single<UserDetailsDto>(),
                ResultType = isValid ? ResultTypes.Ok : ResultTypes.Unauthorized
            };

            _rabcGateway.Setup(_ => _.SignInAsync(request)).Returns(Task.FromResult(userDetails));

            testResponse = _userRbacController.SignInAsync(request);
        }


        protected void TheReturnedResponseStatusIsAuthorized()
        {
            Assert.IsNotNull(testResponse);
            var result = testResponse.Result as OkNegotiatedContentResult<BaseResult<UserDetailsDto>>;
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
            Assert.AreEqual(result.Content.ResultType, ResultTypes.Ok);
        }

        protected void TheReturnedResponseStatusIsUnauthorized()
        {
            Assert.IsNotNull(testResponse);
            var result = testResponse.Result as NegotiatedContentResult<BaseResult<UserDetailsDto>>;
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
            Assert.AreEqual(result.Content.ResultType, ResultTypes.Unauthorized);
        }
    }
}