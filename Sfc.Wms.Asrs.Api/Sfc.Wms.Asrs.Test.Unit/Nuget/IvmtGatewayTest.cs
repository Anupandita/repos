using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.Asrs.Test.Unit.Fixtures.Nuget;
using TestStack.BDDfy;

namespace Sfc.Wms.Asrs.Test.Unit.Nuget
{
    [TestClass]
    [Story(
        AsA = "End user",
        IWant = "To build a IVMT message",
        SoThat = "can be processed"
    )]
    public class IvmtGatewayTest : IvmtGatewayFixture
    {
        [TestMethod]
        [TestCategory("UNIT")]
        public void Process_Ivmt_Message_With_Valid_Ivmt_Message()
        {
            this.Given(e => e.ValidInputData())
                .When(el => el.IvmtProcessorInvoked())
                .Then(el => el.IvmtMessageShoulBeProcessed())
                .BDDfy();
        }

        [TestMethod]
        [TestCategory("UNIT")]
        public void Process_Ivmt_Message_With_Invalid_Ivmt_Message()
        {
            this.Given(e => e.InvalidInputData())
                .When(el => el.IvmtProcessorInvoked())
                .Then(el => el.IvmtMessageShoulNotBeProcessed())
                .BDDfy();
        }
    }
}