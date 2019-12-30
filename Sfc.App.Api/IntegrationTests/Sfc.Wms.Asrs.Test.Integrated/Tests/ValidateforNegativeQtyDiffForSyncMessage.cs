using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures;
using Sfc.Wms.Interfaces.ParserAndTranslator.Contracts.Constants;
using TestStack.BDDfy;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Tests
{

    [TestClass]
    [Story(
          Title = "Synchronization Messages",
          AsA = "Authorized User test for Synr,Synd and Sync messages",
          IWant = "To Verify All the Records in pick location table w.r.t active location should be inserted into pld_snap_shot table" +
                   " And Validate for Synd data table for one or two row inserted where there is quantity mismatch from dematic pick_locn tables.from pld snap shot w.r.t sync id , sku, location combination" +
                   "And Verify the quanties in both table should be same ",
          SoThat = "I can validate for SYNR message should insert into Swm_to_mhe" +
                   "Validate Pix transaction Table for Miss match Quatity sku"+
                   "Validate SYND message should insert into Swm_from_mhe " +
                   "Validate SYNC message should insert into Swm_from_mhe",
          StoryUri = "http://tfsapp1:8080/tfs/ShamrockCollection/Portfolio-SOWL/_workitems?id=143479&_a=edit"

      )]
    public class ValidateforNegativeQtyDiffForSyncMessage : SynrQtyDefferenceMessageFixture
    {
        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void Test1SynrMessageTestForQtyDifferenceScenarios()
        {
            this.Given(x => x.TestInitializeForValidMessage())
               .When(x => x.SynrApiIsCalledCreatedIsReturnedWithValidUrlAndSyncIdIs(SynrUrl, Nextupcnt + 1))
                .And(x => x.GetDataFromDataBaseForSynrScenarios())
                .And(x => x.VerifySynrMessageWasInsertedIntoSwmToMhe())
                .And(x => x.VerifySynrMessageWasInsertedIntoWmsToEms())
                .And(x => x.VerifyCountPickLocationTableAndSnapshotTable())
                .BDDfy("Test Case ID : 144444 TestCase: SYNR-Synchronization Request");
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void Test2SyndMessageTesForQtyDifferencetScenarios()
        {
            this.Given(x => x.TestInitialize())
                .And(x => x.ValidateForSyndMessagesInsertedIntoSwmToMheTableAndSyndDataTableWithAppropiateValues())
                .BDDfy("Test Case ID :146038 TestCase: SYND -Synchronization Data: Test for message where there is quantity mismatch from dematic pick_locn tables");
        }
        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void Test3SyncMessageTestForQtyDifferenceScenarios()
        {
            this.Given(x => x.SyncTestInitialize())
                .And(x => x.SyncTestInitializeForValidMessage())
                .And(x => x.ValidSyncUrlMsgKeyAndProcessorIs(SyncUrl, SyncData.MsgKey, DefaultPossibleValue.MessageProcessor))
                .When(x => x.SyncApiIsCalledWithValidMsgKey())
                .And(x => x.GetValidDataAfterTriggerSync())
                 .Then(x => x.VerifySyncMessageWasInsertedIntoEmsToWms())
                 .And(x => x.ValidateForQtyShouldBeUpdatedInPickLocationTable())
                 .And(x => x.ValidateForPixTran())
                .BDDfy("Test Case ID:146661 TestCase:SYNC -Test for message if sku Quantity is more in SWM_SYNR_PLD_SNAPSHOT and less in SWM_SYND_DATA for each sku_id");
        }

      

    }
}
