using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures.UIFixtures;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestStack.BDDfy;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Tests.UITests
{
    [TestClass]
    [Story(
         Title = "Message Logger Api Testing",
         AsA = "Authorized User test for Item Attribute Page Apis",
         IWant = "To Test for all Apis Used on Message Logger Page on Web UI ",
         SoThat = "I can vaidate message that is going to be displayed on UI ",
         StoryUri = ""
          )]
   public class MessageLoggerApiTest : MessageLoggerFixture
    {
        [TestInitialize]
        public void AValidTestData()
        {
            LoginToFetchToken();
            
        }
        [TestMethod()]
        [TestCategory("UI_FUNCTIONAL")]
        public void MessageLoggerApi()
        {
            this.Given(x => x.CreateInputDtoForMessageLoggerApi())
                .When(x => x.CallMessageLoggerApiWithUrl(UIConstants.MessageLoggerUrl))
                .Then(x => x.VerifyLogInsteredInMessageLogTableInDb())
            .BDDfy("Test Case ID : "); 
        }
    }
}
