using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.App.Api.Tests.Unit.Fixtures;
using TestStack.BDDfy;

namespace Sfc.App.Api.Tests.Unit.Controllers
{
    [TestClass]
    [Story(
        AsA = "End user",
        IWant = "To build a COMT message",
        SoThat = "can be processed"
    )]
    public class ComtControllerTest : ComtFixture
    {
        [TestMethod]
        [TestCategory("UNIT")]
        public void Process_Comt_Message_With_Valid_Comt_Message()
        {
            this.Given(el => el.ValidComtMessage())
                .When(el => el.InsertMessageInvoked())
                .Then(el => el.ComtMessageShouldBeProcessed()).BDDfy();
        }

        [TestMethod]
        [TestCategory("UNIT")]
        public void Process_Comt_Message_With_Invalid_Comt_Message()
        {
            this.Given(el => el.InvalidComtMessage())
                .When(el => el.InsertMessageInvoked())
                .Then(el => el.ComtMessageShouldNotBeProcessed()).BDDfy();
        }
    }
}