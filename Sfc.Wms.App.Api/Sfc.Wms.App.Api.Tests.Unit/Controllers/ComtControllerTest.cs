using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.App.Api.Tests.Unit.Constants;
using Sfc.Wms.App.Api.Tests.Unit.Fixtures;

namespace Sfc.Wms.App.Api.Tests.Unit.Controllers
{
    [TestClass]
    public class ComtControllerTest : ComtFixture
    {
        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Process_Comt_Message_With_Valid_Comt_Message()
        {
            ValidComtMessage();
            InsertMessageInvoked();
            ComtMessageShouldBeProcessed();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Process_Comt_Message_With_Invalid_Comt_Message()
        {
            InvalidComtMessage();
            InsertMessageInvoked();
            ComtMessageShouldNotBeProcessed();
        }
    }
}