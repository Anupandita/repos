using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.App.Api.Tests.Unit.Constants;
using Sfc.Wms.App.Api.Tests.Unit.Fixtures;

namespace Sfc.Wms.App.Api.Tests.Unit.Nuget
{
    [TestClass]
    public class UserRbacGatewayTest : UserRbacGatewayFixture
    {
        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Should_Return_Unauthorized_As_Response_When_InValid_Login_Credentials_Are_Passed()
        {
            InValidLoginCredentials();
            UserAuthenticationServiceIsInvoked();
            TheServiceCallReturnedUnAuthorizesAsResponseStatus();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Should_Return_Authorized_As_Response_When_Valid_Login_Credentials_Are_Passed()
        {
            ValidLoginCredentials();
            UserAuthenticationServiceIsInvoked();
            TheServiceCallReturnedAuthorizedAsResponseStatus();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Should_Return_BadRequest_As_Response_When_Empty_Login_Credentials_Are_Passed()
        {
            EmptyLoginCredentials();
            UserAuthenticationServiceIsInvoked();
            TheServiceCallReturnedBadRequestAsResponseStatus();
        }
    }
}