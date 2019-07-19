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