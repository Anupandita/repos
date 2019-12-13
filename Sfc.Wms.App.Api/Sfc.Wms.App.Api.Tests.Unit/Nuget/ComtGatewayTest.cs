using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.App.Api.Tests.Unit.Constants;
using Sfc.Wms.App.Api.Tests.Unit.Fixtures;

namespace Sfc.Wms.App.Api.Tests.Unit.Nuget
{
    [TestClass]
    public class ComtGatewayTest : ComtGatewayFixture
    {
        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Process_Comt_Message_With_Valid_Comt_Message()
        {
            ValidInputData();
            ComtProcessorInvoked();
            ComtMessageShouldBeProcessed();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Process_Comt_Message_With_Invalid_Comt_Message()
        {
            InvalidInputData();
            ComtProcessorInvoked();
            ComtMessageShouldNotBeProcessed();
        }
    }
}