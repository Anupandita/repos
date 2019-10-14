using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.App.Api.Tests.Unit.Fixtures;
using TestStack.BDDfy;

namespace Sfc.Wms.App.Api.Tests.Unit.Nuget
{
    [TestClass]
    [Story(
        AsA = "End user",
        IWant = "To build a EmsToWmsMessage message",
        SoThat = "can be processed"
    )]
    public class EmsToWmsMessageGatewayTest : EmsToWmsMessageGatewayFixture
    {
        [TestMethod]
        [TestCategory("UNIT")]
        public void Process_EmsToWmsMessage_Message_With_Valid_EmsToWmsMessage_Message()
        {
            this.Given(e => e.ValidInputData())
                .When(el => el.EmsToWmsMessageProcessorInvoked())
                .Then(el => el.EmsToWmsMessageMessageShouldBeProcessed())
                .BDDfy();
        }

        [TestMethod]
        [TestCategory("UNIT")]
        public void Process_EmsToWmsMessage_Message_With_Invalid_EmsToWmsMessage_Message()
        {
            this.Given(e => e.InvalidInputData())
                .When(el => el.EmsToWmsMessageProcessorInvoked())
                .Then(el => el.EmsToWmsMessageMessageShouldNotBeProcessed())
                .BDDfy();
        }
    }
}