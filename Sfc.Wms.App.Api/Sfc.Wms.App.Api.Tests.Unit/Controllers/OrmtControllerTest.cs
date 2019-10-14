using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.App.Api.Tests.Unit.Fixtures;
using TestStack.BDDfy;

namespace Sfc.Wms.App.Api.Tests.Unit.Controllers
{
    [TestClass]
    [Story(
        AsA = "End user",
        IWant = "To build a order maintenance message",
        SoThat = "can be processed"
    )]
    public class OrmtControllerTest : OrmtFixture
    {
        [TestMethod]
        [TestCategory("UNIT")]
        public void Process_Ormt_Message_With_Valid_Carton_Number()
        {
            this.Given(el => el.ValidInput())
                .When(el => el.CreateOrmtMessagesByCartonNumber())
                .Then(el => el.OrmtMessageShouldBeProcessed()).BDDfy();
        }

        [TestMethod]
        [TestCategory("UNIT")]
        public void Process_Ormt_Message_With_Invalid_Carton_Number()
        {
            this.Given(el => el.InvalidInput())
                .When(el => el.CreateOrmtMessagesByCartonNumber())
                .Then(el => el.OrmtMessageShouldNotBeProcessed()).BDDfy();
        }

        [TestMethod]
        [TestCategory("UNIT")]
        public void Process_Ormt_Message_With_Valid_Wave_Number()
        {
            this.Given(el => el.ValidInput())
                .When(el => el.CreateOrmtMessagesByCartonNumber())
                .Then(el => el.OrmtMessageShouldBeProcessed()).BDDfy();
        }

        [TestMethod]
        [TestCategory("UNIT")]
        public void Process_Ormt_Message_With_Invalid_Wave_Number()
        {
            this.Given(el => el.InvalidInput())
                .When(el => el.CreateOrmtMessagesByCartonNumber())
                .Then(el => el.OrmtMessageShouldNotBeProcessed()).BDDfy();
        }
    }
}