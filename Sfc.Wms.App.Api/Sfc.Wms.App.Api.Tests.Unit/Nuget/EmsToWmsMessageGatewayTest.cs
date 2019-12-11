using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.App.Api.Tests.Unit.Constants;
using Sfc.Wms.App.Api.Tests.Unit.Fixtures;

namespace Sfc.Wms.App.Api.Tests.Unit.Nuget
{
    [TestClass]
    public class EmsToWmsMessageGatewayTest : EmsToWmsMessageGatewayFixture
    {
        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Process_EmsToWmsMessage_Message_With_Valid_EmsToWmsMessage_Message()
        {
            ValidInputData();
            EmsToWmsMessageProcessorInvoked();
            EmsToWmsMessageMessageShouldBeProcessed();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Process_EmsToWmsMessage_Message_With_Invalid_EmsToWmsMessage_Message()
        {
            InvalidInputData();
            EmsToWmsMessageProcessorInvoked();
            EmsToWmsMessageMessageShouldNotBeProcessed();
        }
    }
}