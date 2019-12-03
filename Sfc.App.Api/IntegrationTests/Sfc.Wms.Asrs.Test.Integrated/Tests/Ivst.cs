using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures;
using TestStack.BDDfy;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Tests
{
    [TestClass]
    [Story(
        AsA = "Authorized User test for Ivst message from emstowms",
       IWant = "To Verify IVST message is inserted in to SWM_FROM_MHE table with appropriate data" +
        " And verify IVST message is inserted in to swm_from_mhe table with appropriate data " +
        " Verify in TransInventory table for Allocation Inventory units and actual weight" +
        " Verify in Case Detail table for Quantity, CaseHeader and task detail table for status code ",
       SoThat = "I can validate for message fields in IVST message, in Internal Table SWM_FROM_MHE" +
        " and validate the quantity,weight,statuscode in the caseheader, casedetail, task header tables"
        )]
    public class Ivst : IvstMessageFixture
    {
        [TestInitialize]
        public void TestData()
        {
            TestInitialize();
        }
         //this code will be tested only for outbound process
        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        [Priority(1)]
        public void VerifyForValidIvstMessageScenariosCycleCount()
        {
            //this.Given(x => x.MsgKeyCycleCount())
            //    .And(x => x.ValidIvstUrl())
            //    .When(x => x.IvstApiIsCalledWithValidMsgKey())
            //    .And(x => x.GetValidDataAfterTrigger())
            //    .Then(x => x.VerifyIvstMessageWasInsertedIntoSwmFromMhe())
            //    .And(x => x.VerifyCycleCountMessage())
            //    .And(x => x.PixTransactionValidationForCycleCountAdjustmentPlus())
            //    .BDDfy();
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        [Priority(1)]
        [DataRow(2)]
        public void TestForIvstUnexpectedOverageExceptionScenarios(int count)
        {
            this.Given(x => x.TestDataForUnexpectedOverageException())
            .And(x => x.MsgKeyForUnexpectedOverageException())
            .And(x => x.ValidIvstUrl())
            .When(x => x.IvstApiIsCalledCreatedIsReturned())
            .And(x => x.GetValidDataAfterTrigger())
            .Then(x => x.VerifyIvstMessageWasInsertedIntoSwmFromMheUnExceptedOverage())
            .And(x => x.VerifyTheQuantityForUnexpectedOverageExceptionIntoTransInventoryTable())
            .And(x => x.VerifyTheRecordInsertedIntoPixTransactionTablereasonCodeForUnexpectedOverageException())
            .BDDfy();
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        [Priority(2)]
        public void TestForIvstInventoryShortageExceptionScenarios()
        {
            this.Given(x => x.TestDataForInventoryException())
            .And(x => x.MsgKeyForInventoryShortageException())
            .And(x => x.ValidIvstUrl())
            .When(x => x.IvstApiIsCalledCreatedIsReturned())
            .And(x => x.GetValidDataAfterTrigger())
            .Then(x => x.VerifyIvstMessageWasInsertedIntoSwmFromMheInventoryShortage())
            .And(x => x.VerifyTheQuantityForInventoryShortageExceptionIntoTransInventoryTable())
            .And(x => x.VerifyTheRecordInsertedIntoPixTransactionTablereasonCodeForInventoryShortageException())
            .BDDfy();
        }
        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        [Priority(3)]
        public void TestForIvstIvstDamageMessageScenarios()
        {
            this.Given(x => x.TestDataForDamageException())
            .And(x => x.MsgKeyForDamageException())
            .And(x => x.ValidIvstUrl())
            .When(x => x.IvstApiIsCalledCreatedIsReturned())
            .And(x => x.GetValidDataAfterTrigger())
            .Then(x => x.VerifyIvstMessageWasInsertedIntoSwmFromMheDamage())
            .And(x => x.VerifyTheQuantityForDamageExceptionIntoTransInventoryTable())
            .And(x => x.VerifyTheRecordInsertedIntoPixTransactionTablereasonCodeForDamageException())
            .BDDfy();
        }
        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        [Priority(4)]
        public void TestForIvstWrongSkuMessageScenarios()
        {
            this.Given(x => x.TestDataForWrongSkuException())
            .And(x => x.MsgKeyForWrongSkuException())
            .And(x => x.ValidIvstUrl())
            .When(x => x.IvstApiIsCalledCreatedIsReturned())
            .And(x => x.GetValidDataAfterTrigger())
            .Then(x => x.VerifyIvstMessageWasInsertedIntoSwmFromMheWrongSku())
            .And(x => x.VerifyTheQuantityForWrongSkuExceptionIntoTransInventoryTable())
            .And(x => x.VerifyTheRecordInsertedIntoPixTransactionTablereasonCodeForWrongSkuException())
            .BDDfy();
        }
    }
}
