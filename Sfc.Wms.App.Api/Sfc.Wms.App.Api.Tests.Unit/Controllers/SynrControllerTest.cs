using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.App.Api.Tests.Unit.Constants;
using Sfc.Wms.App.Api.Tests.Unit.Fixtures;

namespace Sfc.Wms.App.Api.Tests.Unit.Controllers
{
    [TestClass]
    public class SynrControllerTest : SynrFixture
    {
        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Process_Synr_Message_With_Valid_SynchronizationRequest_Message()
        {
            ValidSynchronizationRequestMessage();
            InsertMessageInvoked();
            SynchronizationRequestMessageShouldBeProcessed();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Process_Synr_Message_With_Invalid_SynchronizationRequest_Message()
        {
            InvalidSynchronizationRequestMessage();
            InsertMessageInvoked();
            SynchronizationRequestMessageShouldNotBeProcessed();
        }
    }
}