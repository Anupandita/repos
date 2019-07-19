using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.Asrs.Test.Unit.Fixtures;
using TestStack.BDDfy;

namespace Sfc.Wms.Asrs.Test.Unit.Services
{
    [TestClass]
    [Story(
        AsA = "End user",
        IWant = "To build a WmsToEms message",
        SoThat = "It can be processed"
    )]
    public class WmsToEmsMessageServiceTest : WmsToEmsMessageServiceFixture
    {
        [TestMethod]
        [TestCategory("UNIT")]
        public void Process_Ivmt_Message_With_Valid_Ivmt_Message()
        {
            this.Given(el => el.ValidIvmtMessage())
                .When(el => el.ProcessIvmtMessageIsInvoked())
                .Then(el => el.IvmtMessageShouldBeProcessed()).BDDfy();
        }

        [TestMethod]
        [TestCategory("UNIT")]
        public void Process_Ivmt_Message_With_Invalid_Ivmt_Message()
        {
            this.Given(el => el.InvalidIvmtMessage())
                .When(el => el.ProcessIvmtMessageIsInvoked())
                .Then(el => el.IvmtMessageShouldNotBeProcessed()).BDDfy();
        }

        [TestMethod]
        [TestCategory("UNIT")]
        public void Process_Comt_Message_With_Valid_Comt_Message()
        {
            this.Given(el => el.ValidComtMessage())
                .When(el => el.ProcessComtMessageIsInvoked())
                .Then(el => el.ComtMessageShouldBeProcessed()).BDDfy();
        }

        [TestMethod]
        [TestCategory("UNIT")]
        public void Process_Comt_Message_With_Invalid_Comt_Message()
        {
            this.Given(el => el.InvalidComtMessage())
                .When(el => el.ProcessComtMessageIsInvoked())
                .Then(el => el.ComtMessageShouldNotBeProcessed()).BDDfy();
        }
    }
}
