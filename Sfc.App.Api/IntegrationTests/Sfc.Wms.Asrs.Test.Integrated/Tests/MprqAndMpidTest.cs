using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures;
using TestStack.BDDfy;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Tests
{
    [TestClass]
    [Story(
       AsA = "Authorized User test for MPRQ message from emstowms",
      IWant = "To Test MPRQ message followed by MPID message and "+
        "To Verify MPRQ message is inserted in to SWM_FROM_MHE table with appropriate data",
      SoThat = "I can validate for message fields in MPRQ message and MPID, in Internal Table SWM_FROM_MHE")]
    public class MprqAndMpidTest :MprqMessageFixture
    {
        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void VerifyForValidMprqAndMpidMessageScenarios()
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
