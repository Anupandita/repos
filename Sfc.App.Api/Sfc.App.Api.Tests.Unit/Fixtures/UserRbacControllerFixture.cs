using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using DataGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sfc.App.Api.Controllers;
using Sfc.App.Gateways;
using Sfc.App.Interfaces;
using Sfc.Wms.Result;
using Sfc.Wms.Security.Rbac.Contracts.Dtos;
using Sfc.Wms.Security.Rbac.Contracts.Dtos.UI;

namespace Sfc.App.Api.Tests.Unit.Fixtures
{
    public abstract class UserRbacControllerFixture
    {
        private readonly Mock<IRbacGateway> _rabcGateway;
        private readonly UserRbacController _userRbacController;
        private bool _isValid;
        
        private LoginCredentials _request;
        private Task<IHttpActionResult> _testResponse;

        protected UserRbacControllerFixture()
        {
            _rabcGateway = new Mock<IRbacGateway>(MockBehavior.Default);
            _userRbacController = new UserRbacController(_rabcGateway.Object);
        }

        protected void InvalidDataInRequest()
        {
            _isValid = false;
            _request = Generator.Default.Single<LoginCredentials>();
        }

        protected void ValidInputDataInRequest()
        {
            _isValid = true;
            _request = Generator.Default.Single<LoginCredentials>();
        }

        protected void InvokeSignInUser()
        {
            var userDetails = new BaseResult<UserDetailsDto>
            {
                Payload = Generator.Default.Single<UserDetailsDto>(),
                ResultType = _isValid ? ResultTypes.Ok : ResultTypes.Unauthorized
            };

            _rabcGateway.Setup(_ => _.SignInAsync(_request)).Returns(Task.FromResult(userDetails));

            _testResponse = _userRbacController.SignInAsync(_request);
        }


        protected void TheReturnedResponseStatusIsAuthorized()
        {
            Assert.IsNotNull(_testResponse);
            var result = _testResponse.Result as OkNegotiatedContentResult<BaseResult<UserDetailsDto>>;
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
            Assert.AreEqual(result.Content.ResultType, ResultTypes.Ok);
        }

        protected void TheReturnedResponseStatusIsUnauthorized()
        {
            Assert.IsNotNull(_testResponse);
            var result = _testResponse.Result as NegotiatedContentResult<BaseResult<UserDetailsDto>>;
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
            Assert.AreEqual(result.Content.ResultType, ResultTypes.Unauthorized);
        }


    }
}
