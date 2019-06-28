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
        private bool isValid;

        private LoginCredentials request;
        private Task<BaseResult<UserDetailsDto>> testResponse;

        protected RbacGateWayAppFixture()
        {
            _mock = new Mock<IUserRabcGateway>(MockBehavior.Default);
            _rbacGateway = new RbacGateway(_mock.Object);
        }

        protected void InvalidDataInRequest()
        {
            isValid = false;
            request = Generator.Default.Single<LoginCredentials>();
        }

        protected void ValidInputDataInRequest()
        {
            isValid = true;
            request = Generator.Default.Single<LoginCredentials>();
        }

        protected void InvokeSignInUser()
        {
            var userDetails = new BaseResult<UserDetailsDto>
            {
                Payload = Generator.Default.Single<UserDetailsDto>(),
                ResultType = isValid ? ResultTypes.Ok : ResultTypes.Unauthorized
            };

            _mock.Setup(_ => _.SignInAsync(request)).Returns(Task.FromResult(userDetails));

            testResponse = _rbacGateway.SignInAsync(request);
        }


        protected void TheReturnedResponseStatusIsOk()
        {
            Assert.IsNotNull(testResponse);
            Assert.IsNotNull(testResponse.Result);
            Assert.AreEqual(testResponse.Result.ResultType, ResultTypes.Ok);
        }

        protected void TheReturnedResponseStatusIsUnauthorized()
        {
            Assert.IsNotNull(testResponse);
            Assert.IsNotNull(testResponse.Result);
            Assert.AreEqual(testResponse.Result.ResultType, ResultTypes.Unauthorized);
        }
    }
}