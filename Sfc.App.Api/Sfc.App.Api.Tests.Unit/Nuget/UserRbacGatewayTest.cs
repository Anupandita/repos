using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.App.Api.Tests.Unit.Fixtures;
using TestStack.BDDfy;

namespace Sfc.App.Api.Tests.Unit.Nuget
{
    [TestClass]
    [Story(
        AsA = "To do user Role based authentication",
        IWant = "User to get authenticated",
        SoThat = "Authenticated user can access the restricted resources"
    )]
    public class UserRbacGatewayTest : UserRbacGatewayFixture
    {
        [TestMethod]
        [TestCategory("UNIT")]
        public void Should_Return_Unauthorized_As_Response_When_InValid_Login_Credentials_Are_Passed()
        {
            this.Given(el => InValidLoginCredentials())
                .When(el => UserAuthenticationServiceIsInvoked())
                .Then(el => TheServiceCallReturnedUnAuthorizesAsResponseStatus())
                .BDDfy();
        }

        [TestMethod]
        [TestCategory("UNIT")]
        public void Should_Return_Authorized_As_Response_When_Valid_Login_Credentials_Are_Passed()
        {
            this.Given(el => ValidLoginCredentials())
                .When(el => UserAuthenticationServiceIsInvoked())
                .Then(el => TheServiceCallReturnedAuthorizedAsResponseStatus())
                .BDDfy();
        }

        [TestMethod]
        [TestCategory("UNIT")]
        public void Should_Return_BadRequest_As_Response_When_Empty_Login_Credentials_Are_Passed()
        {
            this.Given(el => EmptyValidLoginCredentials())
                .When(el => UserAuthenticationServiceIsInvoked())
                .Then(el => TheServiceCallReturnedBadRequestAsResponseStatus())
                .BDDfy();
        }
    }
}