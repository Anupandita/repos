using System.Threading.Tasks;
using DataGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sfc.App.Gateways;
using Sfc.Wms.Result;
using Sfc.Wms.Security.Rbac.Contracts.Dtos;
using Sfc.Wms.Security.Rbac.Contracts.Dtos.UI;
using Sfc.Wms.Security.Rbac.Contracts.Interfaces;

namespace Sfc.App.Api.Tests.Unit.Fixtures
{
    public class RbacGateWayAppFixture
    {
        private readonly Mock<IUserRabcGateway> _mock;
        private readonly RbacGateway _rbacGateway;
        private bool _isValid;

        private LoginCredentials _request;
        private Task<BaseResult<UserDetailsDto>> _testResponse;

        protected RbacGateWayAppFixture()
        {
            _mock = new Mock<IUserRabcGateway>(MockBehavior.Default);
            _rbacGateway = new RbacGateway(_mock.Object);
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

            _mock.Setup(_ => _.SignInAsync(_request)).Returns(Task.FromResult(userDetails));

            _testResponse = _rbacGateway.SignInAsync(_request);
        }


        protected void TheReturnedResponseStatusIsOk()
        {
            Assert.IsNotNull(_testResponse);
            Assert.IsNotNull(_testResponse.Result);
            Assert.AreEqual(_testResponse.Result.ResultType, ResultTypes.Ok);
        }

        protected void TheReturnedResponseStatusIsUnauthorized()
        {
            Assert.IsNotNull(_testResponse);
            Assert.IsNotNull(_testResponse.Result);
            Assert.AreEqual(_testResponse.Result.ResultType, ResultTypes.Unauthorized);
        }
    }
}