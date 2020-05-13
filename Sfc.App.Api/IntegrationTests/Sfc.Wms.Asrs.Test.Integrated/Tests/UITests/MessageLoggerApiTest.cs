using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures.UIFixtures;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData.Constant;
using TestStack.BDDfy;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Tests.UITests
{
    [TestClass]
    [Story(
         Title = "Message Logger Api Testing",
         AsA = "Authorized User test for Message Logger Page Apis",
         IWant = "To Test for all Apis Used on Message Logger Page on Web UI ",
         SoThat = "I can vaidate message islogged in db ",
         StoryUri = ""
          )]
   public class MessageLoggerApiTest : MessageLoggerFixture
    {
        [TestInitialize]
        protected void AValidTestData()
        {
            LoginToFetchToken();
            
        }
        [TestMethod()]
        [TestCategory("UI_FUNCTIONAL")]
        protected void MessageLoggerApi()
        {
            this.Given(x => x.CreateInputDtoForMessageLoggerApi())
                .When(x => x.CallMessageLoggerApiWithUrl(UIConstants.MessageLoggerUrl))
                .Then(x => x.VerifyLogInsteredInMessageLogTableInDb())
            .BDDfy("Test Case ID : "); 
        }
    }
}
