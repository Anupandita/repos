using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.App.Api.Tests.Unit.Fixtures;
using TestStack.BDDfy;

namespace Sfc.Wms.App.Api.Tests.Unit.Controllers
{
    [TestClass]
    [Story(
        AsA = "End user",
        IWant = "To build a IVMT message",
        SoThat = "It can be processed"
    )]
    public class IvmtControllerTest : IvmtFixture
    {
        [TestMethod]
        [TestCategory("UNIT")]
        public void Process_Ivmt_Message_With_Valid_Ivmt_Message()
        {
            this.Given(el => el.ValidIvmtMessage())
                .When(el => el.InsertMessageInvoked())
                .Then(el => el.IvmtMessageShouldBeProcessed()).BDDfy();
        }

        [TestMethod]
        [TestCategory("UNIT")]
        public void Process_Ivmt_Message_With_Invalid_Ivmt_Message()
        {
            this.Given(el => el.InvalidIvmtMessage())
                .When(el => el.InsertMessageInvoked())
                .Then(el => el.IvmtMessageShouldNotBeProcessed()).BDDfy();
        }
    }
}