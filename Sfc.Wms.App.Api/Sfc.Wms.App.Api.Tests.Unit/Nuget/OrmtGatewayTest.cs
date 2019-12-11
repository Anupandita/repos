using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.App.Api.Tests.Unit.Constants;
using Sfc.Wms.App.Api.Tests.Unit.Fixtures;

namespace Sfc.Wms.App.Api.Tests.Unit.Nuget
{
    [TestClass]
    public class OrmtGatewayTest : OrmtGatewayFixture
    {
        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Process_Ormt_Message_With_Valid_CartonNumber_Message()
        {
            ValidInputData();
            CreateOrmtByCartonNumberMessageBuilderInvoked();
            OrmtMessageShouldBeProcessed();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Process_Ormt_Message_With_Invalid_CartonNumber_Message()
        {
            InvalidInputData();
            CreateOrmtByCartonNumberMessageBuilderInvoked();
            OrmtMessageShouldNotBeProcessed();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Process_Ormt_Message_With_Valid_WaveNumber_Message()
        {
            ValidInputData();
            CreateOrmtByWaveNumberMessageBuilderInvoked();
            OrmtMessageShouldBeProcessed();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Process_Ormt_Message_With_Invalid_WaveNumbe_Message()
        {
            InvalidInputData();
            CreateOrmtByWaveNumberMessageBuilderInvoked();
            OrmtMessageShouldNotBeProcessed();
        }
    }
}