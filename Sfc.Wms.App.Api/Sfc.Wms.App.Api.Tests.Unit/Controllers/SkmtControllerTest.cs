using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.App.Api.Tests.Unit.Fixtures;

namespace Sfc.Wms.App.Api.Tests.Unit.Controllers
{
    [TestClass]
    public class SkmtControllerTest : SkmtFixture
    {
        [TestMethod]
        [TestCategory("UNIT")]
        public void Process_Skmt_Message_With_Valid_Skmt_Message()
        {
            ValidSkmtMessage();
            InsertMessageInvoked();
            SkmtMessageShouldBeProcessed();
        }

        [TestMethod]
        [TestCategory("UNIT")]
        public void Process_Skmt_Message_With_Invalid_Skmt_Message()
        {
            InvalidSkmtMessage();
            InsertMessageInvoked();
            SkmtMessageShouldNotBeProcessed();
        }
    }
}