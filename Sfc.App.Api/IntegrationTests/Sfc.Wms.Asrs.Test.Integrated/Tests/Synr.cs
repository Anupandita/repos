using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures;
using TestStack.BDDfy;
using Sfc.Wms.Interfaces.ParserAndTranslator.Contracts.Constants;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Tests
{
    [TestClass]
    [Story(
        Title = "Synchronization Messages",
        AsA = "Authorized User test for Synr,Synd and Sync messages",
        IWant = "To Verify All the Records in pick location table w.r.t active location should be inserted into pld_snap_shot table"+
                 " And Validate for Synd data table for all rows inserted from pld snap shot w.r.t sync id , sku, location combination"+
                 "And Verify the quanties in both table should be same ",
        SoThat = "I can validate for SYNR message should insert into Swm_to_mhe "+
                 "Validate SYND message should insert into Swm_from_mhe "+
                 "Validate SYNC message should insert into Swm_from_mhe",
        StoryUri = "http://tfsapp1:8080/tfs/ShamrockCollection/Portfolio-SOWL/_workitems?id=143479&_a=edit"

    )]
    public class Synr:SynrMessageFixture
    {
        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        protected void Test1SynrMessageTestScenarios()
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
        protected void Test2SyndMessageTestScenarios()
        {
            this.Given(x => x.TestInitialize())
                .And(x => x.ValidateForSyndMessagesInsertedIntoSwmToMheTableAndSyndDataTableWithAppropiateValues())
                .BDDfy("Test Case ID : 144446 TestCase: SYND- Synchronization Data : Test for message where the record should be same as in pick_locan");
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        protected void Test3SyncMessageTestScenarios()
        {
            this.Given(x => x.SyncTestInitialize())
                .And(x => x.SyncTestInitializeForValidMessage())
                //.And(x => x.ValidSyncUrlMsgKeyAndProcessorIs(SyncUrl, SyncData.MsgKey, DefaultPossibleValue.MessageProcessor))
                //.When(x => x.SyncApiIsCalledWithValidMsgKey())
                //.And(x => x.GetValidDataAfterTriggerSync())
                //.And(x=>x.VerifySyncMessageWasInsertedIntoSwmFromMhe())
                //.And(x=>x.VerfySkuCountPldsnapTableAndSyndDataTable())
                //.And(x=>x.VerifyQuantityPldsnapTableAndSyndDataTable())
                .BDDfy("Test Case ID:146661 TestCase: SYNC -test for message if the sku count is same in both the tables ( SWM_SYNR_PLD_SNAPSHOT and SWM_SYND_DATA). for sync_id");
        }
    }
}
