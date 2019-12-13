using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures;
using TestStack.BDDfy;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Tests
{
    [TestClass]
    [Story(
       AsA = "Authorized User test for skmt message from wmstoems",
      IWant = "To Verify SKMT message is inserted in to swm_to_mhe table with appropriate data" +
       "And verify SKMT message is inserted in to wmstoems table with appropriate data ",
      SoThat = "I can validate for message fields in SKMT message, in Internal Table SWM_TO_MHE and in wmstoems"
       )]
    public class SkmtParentSku:SkmtMessageFixture
    {
        [TestInitialize]
        public void AValidTestData()
        {
            InitializeTestDataParent();
        }


        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void SkmtMessageTest1ForParentSkuAddScenarios()
        {
            this.Given(x => x.CurrentSkuIdForParentSkuItemmaster())
                .And(x => x.CurrentActioncodeAdd())
                .And(x => x.ValidSkmtUrl())
                .When(x => x.SkmtApiIsCalledCreatedIsReturned())
                .And(x => x.GetValidDataAfterTrigger())
                .And(x => x.VerifySkmtMessageWasInsertedForIntoSwmToMhe("Add", Skmt.ActionCode))
                .And(x => x.VerifySkmtMessageWasInsertedIntoWmsToEms())
                .And(x => x.VerifySkmtMessageWasParentSku())
            .BDDfy();
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void SkmtMessageTest2ForParentSkuUpdateScenarios()
        {
            this.Given(x => x.CurrentSkuIdForParentSkuItemmaster())
                .And(x => x.CurrentActioncodeUpdate())
                .And(x => x.ValidSkmtUrl())
                .When(x => x.SkmtApiIsCalledCreatedIsReturned())
                .And(x => x.GetValidDataAfterTrigger())
                .And(x => x.VerifySkmtMessageWasInsertedForIntoSwmToMhe("Update", Skmt.ActionCode))
                .And(x => x.VerifySkmtMessageWasInsertedIntoWmsToEms())
                .And(x => x.VerifySkmtMessageWasParentSku())

            .BDDfy();

        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void SkmtMessageTest3ForParentSkuDeleteScenarios()
        {
            this.Given(x => x.CurrentSkuIdForParentSkuItemmaster())
                .And(x => x.CurrentActioncodeDelete())
                .And(x => x.ValidSkmtUrl())
                .When(x => x.SkmtApiIsCalledCreatedIsReturned())
                .And(x => x.GetValidDataAfterTrigger())
                .And(x => x.VerifySkmtMessageWasInsertedForIntoSwmToMhe("Delete", Skmt.ActionCode))
                .And(x => x.VerifySkmtMessageWasInsertedIntoWmsToEms())
                .And(x => x.VerifySkmtMessageWasParentSku())

            .BDDfy();

        }

    }
}
