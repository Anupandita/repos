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
    public class ComtGatewayTest : ComtGatewayFixture
    {
        [TestMethod]
        [TestCategory("UNIT")]
        public void Process_Comt_Message_With_Valid_Comt_Message()
        {
            this.Given(e=>e.ValidInputData())
                .When(el => el.ComtProcessorInvoked())
                .Then(el => el.ComtMessageShoulBeProcessed())
                .BDDfy();
        }

        [TestMethod]
        [TestCategory("UNIT")]
        public void Process_Comt_Message_With_Invalid_Comt_Message()
        {
            this.Given(e => e.InvalidInputData())
                .When(el => el.ComtProcessorInvoked())
                .Then(el => el.ComtMessageShoulNotBeProcessed())
                .BDDfy();
        }


    }
}