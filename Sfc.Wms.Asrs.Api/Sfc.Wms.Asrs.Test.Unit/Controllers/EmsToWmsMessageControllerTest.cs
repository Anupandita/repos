using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.Asrs.Test.Unit.Fixtures;
using TestStack.BDDfy;

namespace Sfc.Wms.Asrs.Test.Unit.Controllers
{
    [TestClass]
    [Story(
        AsA = "End user",
        IWant = "To Parse a EMSTOWMS message",
        SoThat = "It can be processed"
    )]
    public class EmsToWmsMessageControllerTes : EmsToWmsMessageFixture
    {
        [TestMethod]
        [TestCategory("UNIT")]
        public void Process_EmsToWms_Message_With_Valid_Message_Key()
        {
            this.Given(el => el.ValidMessageKey())
                .When(el => el.EmsToWmsMessageProcessorInvoked())
                .Then(el => el.EmsToWmsMessageShouldBeProcessed()).BDDfy();
        }

        [TestMethod]
        [TestCategory("UNIT")]
        public void Process_EmsToWms_Message_With_Invalid_Message_Key()
        {
            this.Given(el => el.InvalidMessageKey())
                .When(el => el.EmsToWmsMessageProcessorInvoked())
                .Then(el => el.EmsToWmsMessageShouldNotBeProcessed()).BDDfy();
        }
    }
}