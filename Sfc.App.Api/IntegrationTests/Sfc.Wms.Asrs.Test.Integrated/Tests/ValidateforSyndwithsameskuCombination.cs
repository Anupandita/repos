
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
                " And Validate for Synd data table for all rows inserted from pld snap shot w.r.t sync id , sku, location combination" +
                "And Verify the quanties in both table should be same ",
        SoThat = "I can validate for SYNR message should insert into Swm_to_mhe " +
                 "Validate SYND message should insert into Swm_from_mhe " +
                 "Validate SYNC message should insert into Swm_from_mhe",
        StoryUri = "http://tfsapp1:8080/tfs/ShamrockCollection/Portfolio-SOWL/_workitems?id=143479&_a=edit"

    )]
    public class ValidateforSyndwithsameskuCombination : SyndwithsameskuCombinationMessageFixture
    {
        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        protected void Test1SynrMessageTestForQtyDifferenceScenarios()
        {
            this.Given(x => x.TestInitializeForValidMessage())
                .When(x => x.SynrApiIsCalledCreatedIsReturnedWithValidUrlAndSyncIdIs(SynrUrl, Nextupcnt + 1))
                .And(x => x.GetDataFromDataBaseForSynrScenarios())
                .And(x => x.VerifySynrMessageWasInsertedIntoSwmToMhe())
                .And(x => x.VerifySynrMessageWasInsertedIntoWmsToEms())
                .And(x => x.VerifyCountPickLocationTableAndSnapshotTable())
                .BDDfy("Test Case 144444: SYNR:Synchronization Request");
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        
        protected void Test2SyndMessageTesForQtyDifferencetScenarios()
        {
            this.Given(x => x.TestInitializeForDupilcateSyndForaSameSkuWithQuantities())
                .And(x => x.ValidSyndSameSkuWithDifferentQtyUrl())
                .When(x=>x.SyndSameSkuWithDifferentQtyApiIsCalledWithValidMsgKey())
                .And(x => x.GetDataFromDataBaseForSyndSameSkuWithDifferentQtyScenarios())
                .And(x => x.VerifySyndMessageWasInsertedIntoSwmFromMhe())
                .And(x=>x.VerifySyndMessageWasInsertedIntoSwmFromMheTableandSyndDataTable())
                .BDDfy("Test Case 146039: SYND: send the duplicate synd message for a same sku with quantities same (split quantity and send)");
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]

        protected void Test3SyndMessageTesForQtyDifferencetScenarios()
        {
            this.Given(x => x.TestInitializeForDupilcateSyndForaSameSkuWithQuantities())
                .And(x => x.ValidSyndSameSkuWithDifferentQtyUrl())
                .When(x => x.SyndSameSkuWithDifferentQtyApiIsCalledWithValidMsgKey())
                .And(x => x.GetDataFromDataBaseForSyndSameSkuWithDifferentQtyScenarios())
                .And(x => x.VerifySyndMessageWasInsertedIntoSwmFromMhe())
                .And(x => x.VerifySyndMessageWasInsertedIntoSwmFromMheTableandSyndDataTable())
                .BDDfy("Test Case 146038: SYND: Test for message where there is quantity mismatch from dematic pld tables");
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        protected void Test4SyncMessageTestForQtyDifferenceScenarios()
        {
            this.Given(x => x.SyncTestInitialize())
                .And(x => x.SyncTestInitializeForValidMessage())
                .And(x => x.ValidSyncUrlMsgKeyAndProcessorIs(SyncUrl, SyncData.MsgKey, DefaultPossibleValue.MessageProcessor))
                .When(x => x.SyncApiIsCalledWithValidMsgKey())
                .And(x => x.GetValidDataAfterTriggerSync())
                .Then(x => x.VerifySyncMessageWasInsertedIntoEmsToWms())
                .And(x => x.ValidateForQtyDifferenceShouldBeUpdatedInPickLocationTable())
                .BDDfy("Test Case 146667: SYNC: test for message if sku 'x' has a quantity of '10' in SWM_SYNR_PLD_SNAPSHOT and sku 'y' has quantity of '5+1+4' which is splited in to 3 synd messages");
        }
    }
}
