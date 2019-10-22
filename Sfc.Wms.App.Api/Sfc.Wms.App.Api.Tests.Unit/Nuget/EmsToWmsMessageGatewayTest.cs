using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.App.Api.Tests.Unit.Fixtures;

namespace Sfc.Wms.App.Api.Tests.Unit.Nuget
{
    [TestClass]
    public class EmsToWmsMessageGatewayTest : EmsToWmsMessageGatewayFixture
    {
        [TestMethod]
        [TestCategory("UNIT")]
        public void Process_EmsToWmsMessage_Message_With_Valid_EmsToWmsMessage_Message()
        {
            ValidInputData();
            EmsToWmsMessageProcessorInvoked();
            EmsToWmsMessageMessageShouldBeProcessed();
        }

        [TestMethod]
        [TestCategory("UNIT")]
        public void Process_EmsToWmsMessage_Message_With_Invalid_EmsToWmsMessage_Message()
        {
            InvalidInputData();
            EmsToWmsMessageProcessorInvoked();
            EmsToWmsMessageMessageShouldNotBeProcessed();
        }
    }
}