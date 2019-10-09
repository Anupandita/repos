using DataGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sfc.Core.OnPrem.Result;
using Sfc.Core.OnPrem.Security.Contracts.Dtos;
using Sfc.Wms.App.Api.Controllers;
using Sfc.Wms.App.App.Interfaces;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Sfc.Wms.App.Api.Interfaces;

namespace Sfc.App.Api.Tests.Unit.Fixtures
{
    public abstract class UserRbacControllerFixture
    {
        private readonly Mock<IRbacService> _mockRbacGateway;
        private readonly UserRbacController _userRbacController;
        private bool isValid;

        private LoginCredentials request;
        private Task<IHttpActionResult> testResponse;

        protected UserRbacControllerFixture()
        {
            _mockRbacGateway = new Mock<IRbacService>(MockBehavior.Default);
            _userRbacController = new UserRbacController(_mockRbacGateway.Object);
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
            var userDetails = new BaseResult<UserInfoDto>
            {
                Payload = Generator.Default.Single<UserInfoDto>(),
                ResultType = isValid ? ResultTypes.Ok : ResultTypes.Unauthorized
            };

            _mockRbacGateway.Setup(m => m.SignInAsync(request)).Returns(Task.FromResult(userDetails));
            _mockRbacGateway.Setup(m => m.GetPrinterValuesAsyc(It.IsAny<UserInfoDto>())).Returns(Task.FromResult(userDetails));
            testResponse = _userRbacController.SignInAsync(request);
        }

        protected void TheReturnedResponseStatusIsAuthorized()
        {
            Assert.IsNotNull(testResponse);
            var result = testResponse.Result as OkNegotiatedContentResult<BaseResult<UserInfoDto>>;
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
            Assert.AreEqual(result.Content.ResultType, ResultTypes.Ok);
        }

        protected void TheReturnedResponseStatusIsUnauthorized()
        {
            Assert.IsNotNull(testResponse);
            var result = testResponse.Result as NegotiatedContentResult<BaseResult<UserInfoDto>>;
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
            Assert.AreEqual(result.Content.ResultType, ResultTypes.Unauthorized);
        }
    }
}