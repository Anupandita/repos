using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.App.Api.Tests.Unit.Constants;
using Sfc.Wms.App.Api.Tests.Unit.Fixtures;

namespace Sfc.Wms.App.Api.Tests.Unit.Controllers
{
    [TestClass]
    public class IvmtControllerTest : IvmtFixture
    {
        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Process_Ivmt_Message_With_Valid_Ivmt_Message()
        {
            ValidIvmtMessage();
            InsertMessageInvoked();
            IvmtMessageShouldBeProcessed();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Process_Ivmt_Message_With_Invalid_Ivmt_Message()
        {
            InvalidIvmtMessage();
            InsertMessageInvoked();
            IvmtMessageShouldNotBeProcessed();
        }
    }
}