using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures.UIFixtures;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData.Constant;
using TestStack.BDDfy;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Tests.UITests
{
    [TestClass]
    [Story(
    Title = "Rbac Api Testing",
    AsA = "Authorized User test for Rbac Api",
    IWant = "To Test for Rbac Api  ",
    SoThat = "I can Login and validate Menus and Permissions fetched in the response",
    StoryUri = ""
     )]
    public class RbacApiTest:RbacFixture
    {        
        [TestMethod()]
        [TestCategory("UI_FUNCTIONAL")]
        public void RbacApiTesting()
        {
            this.Given(x => x.CreateLoginDtoUsingCredentials())
            .And(x=>x.FetchDataFromDb())
            .When(x => x.CallLoginApi(UIConstants.LoginUrl))
            .Then(x => x.VerifyMenusListAgainstDb())
            .And(x => x.VerifyPermissionsListAgainstDb())
            .And(x => x.VerifyPrintersListAgainstDb())
            .And(x => x.VerifyPreferencesListAgainstDb())
            .BDDfy("Test Case ID : ");
        }
    }
}
