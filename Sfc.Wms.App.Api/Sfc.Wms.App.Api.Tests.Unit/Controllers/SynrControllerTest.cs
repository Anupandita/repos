using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.App.Api.Tests.Unit.Fixtures;

namespace Sfc.Wms.App.Api.Tests.Unit.Controllers
{
    [TestClass]
    public class SynrControllerTest : SynrFixture
    {
        [TestMethod]
        [TestCategory("UNIT")]
        public void Process_Synr_Message_With_Valid_SynchronizationRequest_Message()
        {
            ValidSynchronizationRequestMessage();
            InsertMessageInvoked();
            SynchronizationRequestMessageShouldBeProcessed();
        }

        [TestMethod]
        [TestCategory("UNIT")]
        public void Process_Synr_Message_With_Invalid_SynchronizationRequest_Message()
        {
            InvalidSynchronizationRequestMessage();
            InsertMessageInvoked();
            SynchronizationRequestMessageShouldNotBeProcessed();
        }
    }
}