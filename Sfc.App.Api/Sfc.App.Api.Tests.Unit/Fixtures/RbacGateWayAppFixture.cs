using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sfc.App.App.Gateways;
using Sfc.Wms.Result;
using Sfc.Wms.Security.Contracts.Dtos;
using Sfc.Wms.Security.Contracts.Interfaces;

namespace Sfc.App.Api.Tests.Unit.Fixtures
{
    public class RbacGateWayAppFixture
    {
        private readonly Mock<IUserRabcService> _mock;
        private readonly RbacGateway _rbacGateway;
        private bool isValid;

        private LoginCredentials request;
        private Task<BaseResult<UserInfoDto>> testResponse;

        protected RbacGateWayAppFixture()
        {
            _mock = new Mock<IUserRabcService>(MockBehavior.Default);
            _rbacGateway = new RbacGateway(_mock.Object);
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
            _mock.Setup(_ => _.GetRolesByUserNameAsync(request.UserName)).Returns(Task.FromResult(roles));
            _mock.Setup(_ => _.GetUserDetails(request.UserName)).Returns(Task.FromResult(userDetails));


            _mock.Setup(_ => _.SignInAsync(request)).Returns(Task.FromResult(authenticate));

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