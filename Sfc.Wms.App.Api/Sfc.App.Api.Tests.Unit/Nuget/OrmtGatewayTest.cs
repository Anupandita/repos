using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.App.Api.Tests.Unit.Fixtures;
using TestStack.BDDfy;

namespace Sfc.Wms.App.Api.Tests.Unit.Nuget
{
    [TestClass]
    [Story(
        AsA = "End user",
        IWant = "To build a IVMT message",
        SoThat = "can be processed"
    )]
    public class OrmtGatewayTest : OrmtGatewayFixture
    {
        [TestMethod]
        [TestCategory("UNIT")]
        public void Process_Ormt_Message_With_Valid_CartonNumber_Message()
        {
            this.Given(e => e.ValidInputData())
                .When(el => el.CreateOrmtByCartonNumberMessageBuilderInvoked())
                .Then(el => el.OrmtMessageShouldBeProcessed())
                .BDDfy();
        }

        [TestMethod]
        [TestCategory("UNIT")]
        public void Process_Ormt_Message_With_Invalid_CartonNumber_Message()
        {
            this.Given(e => e.InvalidInputData())
                .When(el => el.CreateOrmtByCartonNumberMessageBuilderInvoked())
                .Then(el => el.OrmtMessageShouldNotBeProcessed())
                .BDDfy();
        }

        [TestMethod]
        [TestCategory("UNIT")]
        public void Process_Ormt_Message_With_Valid_WaveNumber_Message()
        {
            this.Given(e => e.ValidInputData())
                .When(el => el.CreateOrmtByWaveNumberMessageBuilderInvoked())
                .Then(el => el.OrmtMessageShouldBeProcessed())
                .BDDfy();
        }

        [TestMethod]
        [TestCategory("UNIT")]
        public void Process_Ormt_Message_With_Invalid_WaveNumbe_Message()
        {
            this.Given(e => e.InvalidInputData())
                .When(el => el.CreateOrmtByWaveNumberMessageBuilderInvoked())
                .Then(el => el.OrmtMessageShouldNotBeProcessed())
                .BDDfy();
        }
    }
}