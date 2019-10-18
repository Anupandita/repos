using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.App.Api.Tests.Unit.Constants;
using Sfc.Wms.App.Api.Tests.Unit.Fixtures;

namespace Sfc.Wms.App.Api.Tests.Unit.Controllers
{
    [TestClass]
    public class UserRbacControllerTest : UserRbacControllerFixture
    {
        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Should_Return_Unauthorized_As_Response_When_InValid_Login_Credentials_Are_Passed()
        {
            InvalidCredentials();
            UserLogsIn();
            TheReturnedResponseStatusIsUnauthorized();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Should_Return_Authorized_As_Response_When_Valid_Login_Credentials_Are_Passed()
        {
            ValidCredentials();
            UserLogsIn();
            TheReturnedResponseStatusIsAuthorized();
        }
    }
}