using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures.UIFixtures;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData.Constant;
using TestStack.BDDfy;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Tests.UITests
{
    [TestClass]
    [Story(
       Title = "Message Master Api Testing",
       AsA = "Authorized User test for Message Master Apis",
       IWant = "To Test for Message Master Api  ",
       SoThat = "I can vaidate Data the Messages used on UI to display in errors/info",
       StoryUri = ""
        )]
    public class MessageMasterApiTest:MessageMasterFixture
    {

        [TestInitialize]
        protected void AValidTestData()
        {
            LoginToFetchToken();
            GetMessageMasterRecordsRelatedToUIFromDb();
        }

        [TestMethod()]
        [TestCategory("UI_FUNCTIONAL")]
        protected void MessageMasterApiTesting()
        {
            this.Given(x => x.CallMessageMasterApi(UIConstants.MessageMasterUrl))
            .Then(x => x.VerifyMessageMasterApiOutputAgainstDbOutput())
            .BDDfy("Test Case ID : ");
        }
    }
}
