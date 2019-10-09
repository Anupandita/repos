using DataGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sfc.Core.OnPrem.Result;
using Sfc.Core.OnPrem.Security.Contracts.Dtos;
using Sfc.Core.OnPrem.Security.Contracts.Interfaces;
using Sfc.Wms.App.App.Gateways;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfc.App.Api.Tests.Unit.Fixtures
{
    public class RbacGatewayFixture
    {
        private readonly Mock<IUserRbacService> _mockUserRbacService;
        private readonly RbacGateway _rbacGateway;
        private bool isValid;
        private LoginCredentials loginRequest;
        private Task<BaseResult<UserInfoDto>> userDetailsResponse;

        protected RbacGatewayFixture()
        {
            _mockUserRbacService = new Mock<IUserRbacService>(MockBehavior.Default);
            _rbacGateway = new RbacGateway(_mockUserRbacService.Object);
        }

        protected void InvalidCredentials()
        {
            isValid = false;
            loginRequest = Generator.Default.Single<LoginCredentials>();
        }

        protected void ValidCredentials()
        {
            isValid = true;
            loginRequest = Generator.Default.Single<LoginCredentials>();
        }

        protected void UserLogsIn()
        {
            var authenticate = new BaseResult<int>
            {
                Payload = Generator.Default.Single<UserInfoDto>().UserId,
                ResultType = isValid ? ResultTypes.Ok : ResultTypes.Unauthorized
            };

            var roles = new BaseResult<List<RolesDto>>
            {
                Payload = Generator.Default.List<RolesDto>(2).ToList(),
                ResultType = ResultTypes.Ok
            };

            var userDetails = new BaseResult<UserInfoDto>
            {
                Payload = Generator.Default.Single<UserInfoDto>(),
                ResultType = ResultTypes.Ok
            };
            _mockUserRbacService.Setup(m => m.GetRolesByUserNameAsync(loginRequest.UserName))
                .Returns(Task.FromResult(roles));
            _mockUserRbacService.Setup(m => m.GetUserDetailsAsync(loginRequest.UserName))
                .Returns(Task.FromResult(userDetails));

            _mockUserRbacService.Setup(m => m.SignInAsync(loginRequest)).Returns(Task.FromResult(authenticate));

            userDetailsResponse = _rbacGateway.SignInAsync(loginRequest);
        }

        protected void TheReturnedResponseStatusIsOk()
        {
            Assert.IsNotNull(userDetailsResponse);
            Assert.IsNotNull(userDetailsResponse.Result);
            Assert.AreEqual(userDetailsResponse.Result.ResultType, ResultTypes.Ok);
        }

        protected void TheReturnedResponseStatusIsUnauthorized()
        {
            Assert.IsNotNull(userDetailsResponse);
            Assert.IsNotNull(userDetailsResponse.Result);
            Assert.AreEqual(userDetailsResponse.Result.ResultType, ResultTypes.Unauthorized);
        }
    }
}