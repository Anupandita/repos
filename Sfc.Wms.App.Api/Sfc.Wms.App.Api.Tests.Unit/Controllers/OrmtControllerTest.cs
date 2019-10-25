using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.App.Api.Tests.Unit.Fixtures;

namespace Sfc.Wms.App.Api.Tests.Unit.Controllers
{
    [TestClass]
    public class OrmtControllerTest : OrmtFixture
    {
        [TestMethod]
        [TestCategory("UNIT")]
        public void Process_Ormt_Message_With_Valid_Carton_Number()
        {
            ValidInput();
            CreateOrmtMessagesByCartonNumber();
            OrmtMessageShouldBeProcessed();
        }

        [TestMethod]
        [TestCategory("UNIT")]
        public void Process_Ormt_Message_With_Invalid_Carton_Number()
        {
            InvalidInput();
            CreateOrmtMessagesByCartonNumber();
            OrmtMessageShouldNotBeProcessed();
        }

        [TestMethod]
        [TestCategory("UNIT")]
        public void Process_Ormt_Message_With_Valid_Wave_Number()
        {
            ValidInput();
            CreateOrmtMessagesByCartonNumber();
            OrmtMessageShouldBeProcessed();
        }

        [TestMethod]
        [TestCategory("UNIT")]
        public void Process_Ormt_Message_With_Invalid_Wave_Number()
        {
            InvalidInput();
            CreateOrmtMessagesByCartonNumber();
            OrmtMessageShouldNotBeProcessed();
        }
    }
}