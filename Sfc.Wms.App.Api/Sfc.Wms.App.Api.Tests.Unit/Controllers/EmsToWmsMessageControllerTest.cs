using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.App.Api.Tests.Unit.Fixtures;

namespace Sfc.Wms.App.Api.Tests.Unit.Controllers
{
    [TestClass]
    public class EmsToWmsMessageControllerTes : EmsToWmsMessageFixture
    {
        [TestMethod]
        [TestCategory("UNIT")]
        public void Process_EmsToWms_Message_With_Valid_Message_Key()
        {
            ValidMessageKey();
            EmsToWmsMessageProcessorInvoked();
            EmsToWmsMessageShouldBeProcessed();
        }

        [TestMethod]
        [TestCategory("UNIT")]
        public void Process_EmsToWms_Message_With_Invalid_Message_Key()
        {
            InvalidMessageKey();
            EmsToWmsMessageProcessorInvoked();
            EmsToWmsMessageShouldNotBeProcessed();
        }
    }
}