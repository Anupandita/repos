using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures.UIFixtures;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData.Constant;
using TestStack.BDDfy;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Tests.UITests
{
    [TestClass]
    [Story(
    Title = "Corba Api Testing",
    AsA = "Authorized User test for Corba Apis",
    IWant = "To Test for Corba Api  ",
    SoThat = "I can vaidate Corba function calls using the Api",
    StoryUri = ""
     )]
    public class CorbaApiTest:CorbaFixture
    {

        [TestInitialize]
        public void AValidTestData()
        {
            LoginToFetchToken();
        }

        [TestMethod()]
        [TestCategory("UI_FUNCTIONAL")]
        public void CorbaApiTesting()
        {
            this.Given(x=>x.CreateCorbaInputParamAndUrl(UIConstants.Single,UIConstants.CorbaFunctionName))
            .Then(x => x.CallCorbaApi(UIConstants.CorbaUrl))
            .Then(x => x.VerifyCorbaApiOutputInDb())
            .BDDfy("Test Case ID : ");
        }
    }
}
