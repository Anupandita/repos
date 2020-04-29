using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures.UIFixtures;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData.Constant;
using TestStack.BDDfy;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Tests.UITests
{
    [TestClass]
    [Story(
    Title = "User Master Api Testing",
    AsA = "Authorized User test for User Master Apis",
    IWant = "To Test for User Master Api  ",
    SoThat = "I can vaidate User Preferences are being saved in Db",
    StoryUri = ""
     )]
    public class UserMasterApiTest:UserMasterFixture
    {

        [TestInitialize]
        public void AValidTestData()
        {
            LoginToFetchToken();
        }

        [TestMethod()]
        [TestCategory("UI_FUNCTIONAL")]
        public void UserMasterApiTesting()
        {
            this.Given(x => x.FetchUserPreferenceIdIfAnyFromDb())
            .And(x=>x.CreateUserMasterDto())
            .When(x=>x.CallUserMasterApi(UIConstants.UserMasterUrl))
            .Then(x => x.VerifyUserMasterApiOutputAgainstDbOutput(UIConstants.PreferenceId))
            .BDDfy("Test Case ID : ");
        }
    }
}
