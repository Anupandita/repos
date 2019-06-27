using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.App.Api.Tests.Unit.Fixtures;
using TestStack.BDDfy;

namespace Sfc.App.Api.Tests.Unit.App
{
    [TestClass]
    [Story(
        AsA = "",
        IWant = "",
        SoThat = ""
    )]
    public class RbacGateWayTest : RbacGateWayAppFixture
    {
        [TestMethod]
        [TestCategory("UNIT")]
        public void Should_Return_Unauthorized_As_Response_When_InValid_Login_Credentials_Are_Passed()
        {
            this.Given(_ => _.InvalidDataInRequest())
                .When(_ => _.InvokeSignInUser())
                .Then(_ => _.TheReturnedResponseStatusIsUnauthorized())
                .BDDfy();
        }

        [TestMethod]
        [TestCategory("UNIT")]
        public void Should_Return_Ok_As_Response_When_Valid_Login_Credentials_Are_Passed()
        {
            this.Given(_ => _.ValidInputDataInRequest())
                .When(_ => _.InvokeSignInUser())
                .Then(_ => _.TheReturnedResponseStatusIsOk())
                .BDDfy();
        }
    }
}