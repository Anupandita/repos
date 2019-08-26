using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.App.Api.Tests.Unit.Constants;
using Sfc.App.Api.Tests.Unit.Fixtures;
using TestStack.BDDfy;

namespace Sfc.App.Api.Tests.Unit.App
{
    [TestClass]
    [Story(
        AsA = "To do user Role based authentication",
        IWant = "User to get authenticated",
        SoThat = "Authenticated user can access the restricted resources"
    )]
    public class RbacGatewayTest : RbacGatewayFixture
    {
        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Should_Return_Unauthorized_As_Response_When_InValid_Login_Credentials_Are_Passed()
        {
            this.Given(el => InvalidCredentials())
                .When(el => UserLogsIn())
                .Then(el => TheReturnedResponseStatusIsUnauthorized())
                .BDDfy();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Should_Return_Ok_As_Response_When_Valid_Login_Credentials_Are_Passed()
        {
            this.Given(el => ValidCredentials())
                .When(el => UserLogsIn())
                .Then(el => TheReturnedResponseStatusIsOk())
                .BDDfy();
        }
    }
}