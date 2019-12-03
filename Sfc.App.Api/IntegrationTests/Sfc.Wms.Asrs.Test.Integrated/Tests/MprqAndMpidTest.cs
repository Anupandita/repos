using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures;
using TestStack.BDDfy;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Tests
{
    [TestClass]
    [Story(
       AsA = "Authorized User test for MPRQ message from emstowms",
      IWant = "To Verify MPRQ message is inserted in to SWM_FROM_MHE table with appropriate data" +
       " And verify MPRQ message is inserted in to swm_from_mhe table with appropriate data ",
      SoThat = "I can validate for message fields in MPRQ message, in Internal Table SWM_FROM_MHE")]
    public class MprqAndMpidTest :MprqMessageFixture
    {
        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void VerifyForValidMprqMessageScenarios()
        {
            this.Given(x => x.TestInitializeForValidMessage())
                .And(x => x.AValidMsgKey())
                .And(x=>x.AValidMprqUrl())
                .When(x => x.MprqApiIsCalledWithValidMsgKey())
                .And(x => x.GetValidDataAfterTrigger())
                .And(x => x.VerifyMprqMessageWasInsertedIntoSwmFromMhe())
                .And(x => x.VerifyMpidMessageWasInsertedIntoswmToMhe())
                .And(x => x.VerifyLocationMpid())
                .BDDfy();
        }
    }
}
