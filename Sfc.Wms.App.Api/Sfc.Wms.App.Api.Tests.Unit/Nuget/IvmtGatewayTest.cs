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
    public class IvmtGatewayTest : IvmtGatewayFixture
    {
        [TestMethod]
        [TestCategory("UNIT")]
        public void Process_Ivmt_Message_With_Valid_Ivmt_Message()
        {
            this.Given(e => e.ValidInputData())
                .When(el => el.IvmtProcessorInvoked())
                .Then(el => el.IvmtMessageShouldBeProcessed())
                .BDDfy();
        }

        [TestMethod]
        [TestCategory("UNIT")]
        public void Process_Ivmt_Message_With_Invalid_Ivmt_Message()
        {
            this.Given(e => e.InvalidInputData())
                .When(el => el.IvmtProcessorInvoked())
                .Then(el => el.IvmtMessageShouldNotBeProcessed())
                .BDDfy();
        }
    }
}