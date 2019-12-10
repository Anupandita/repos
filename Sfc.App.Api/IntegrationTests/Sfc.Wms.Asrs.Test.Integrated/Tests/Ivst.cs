using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData;
using TestStack.BDDfy;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Tests
{
    [TestClass]
    [Story(
        AsA = "Authorized User test for Ivst message from emstowms",
       IWant = "To Test for all Ivst exceptions and verify IVST message was inserted in to SWM_FROM_MHE table ",
       SoThat = "I can validate for message fields in IVST message definition, in Internal Table SWM_FROM_MHE" +
        " and validate for quantity and weight in trans_invn table when inbound pallet is yes for all exceptions which are applicable "+
        " and validate for quantity in pick location table when inbound pallet is No for all exceptions(applicable) "+
        " and validate for pix transaction table the record should be exists for each message w.r.t reason codes"
        )]
    public class Ivst : IvstMessageFixture
    {
        [TestInitialize]
        public void TestData()
        {
            TestInitialize();
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        [Priority(3)]
        public void Ivst3UnexpectedOverageExceptionTestScenariosForInboundPalletIsY()
        {
            this.Given(x => x.TestDataForUnexpectedOverageException())
            .And(x => x.MsgKeyForUnexpectedOverageException())
            .And(x => x.ValidIvstUrl())
            .When(x => x.IvstApiIsCalledCreatedIsReturned())
            .And(x => x.GetValidDataAfterTrigger())
            .Then(x => x.VerifyIvstMessageWasInsertedIntoSwmFromMheUnExceptedOverage())
            .And(x => x.VerifyTheQuantityForUnexpectedOverageExceptionIntoTransInventoryTable())
            .And(x => x.VerifyTheRecordInsertedIntoPixTransactionAndValidateReasonCodeForUnexpectedOverageException())
            .BDDfy();
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        [Priority(4)]
        public void Ivst4InventoryShortageExceptionTestScenariosForInboundPalletIsY()
        {
            this.Given(x => x.TestDataForInventoryException())
            .And(x => x.MsgKeyForInventoryShortageException())
            .And(x =>x.ValidUrl(IvstUrl))
            .And(x => x.ValidIvstUrl())
            .When(x => x.IvstApiIsCalledCreatedIsReturned())
            .And(x => x.GetValidDataAfterTrigger())
            .Then(x => x.VerifyIvstMessageWasInsertedIntoSwmFromMheForInventoryShortage(InvShortageInbound.Key))
            .And(x => x.VerifyTheQuantityAndWeightShouldBeReducedByIvstQtyInTransInventoryForInboundPalletIsY())
            .And(x => x.VerifyTheRecordInsertedIntoPixTransactionAndValidateReasonCodeForInventoryShortageException())
            .BDDfy();
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        [Priority(5)]

        public void Ivst5InventoryShortageExceptionTestScenariosForInboundPalletIsN()
        {
            this.Given(x => x.TestDataForInventoryShortageInboundPalletIsN())
            .And(x => x.MsgKeyForInventoryShortageForInboundPalletIsN())
            .And(x => x.ValidIvstUrl())
            .When(x => x.IvstApiIsCalledCreatedIsReturned())
            .And(x => x.GetValidDataAfterTrigger())
            .Then(x => x.VerifyIvstMessageWasInsertedIntoSwmFromMheForInventoryShortage(InvShortageOutbound.Key))
            .And(x =>x.VerifyTheQuantityShouldBeReducedByIvstQtyInPickLocationForInboundPalletIsN())
            .And(x => x.VerifyTheRecordInsertedIntoPixTransactionAndValidateReasonCodeForInventoryShortageException())
            .BDDfy();
        }


        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        [Priority(6)]
        public void Ivst6DamageExceptionTestScenariosForInboundPalletIsY()
        {
            this.Given(x => x.TestDataForDamageException())
            .And(x => x.MsgKeyForDamageException())
            .And(x => x.ValidIvstUrl())
            .When(x => x.IvstApiIsCalledCreatedIsReturned())
            .And(x => x.GetValidDataAfterTrigger())
            .Then(x => x.VerifyIvstMessageWasInsertedIntoSwmFromMheForDamage(DamageInbound.Key))
            .And(x => x.VerifyTheQuantityAndWeightShouldBeReducedByIvstQtyInTransInventoryForInboundPalletIsY())
            .And(x => x.VerifyTheRecordInsertedIntoPixTransactionAndValidateReasonCodeForDamageException())
            .BDDfy();
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        [Priority(7)]
        public void Ivst7DamageExceptionTestScenariosForInboundPalletIsN()
        {
            this.Given(x =>x.TestDataForDamageInboundPalletIsN())
                .And(x =>x.MsgKeyForDamageOutbound())
                .And(x =>x.ValidIvstUrl())
                .When(x => x.IvstApiIsCalledCreatedIsReturned())
                .And(x => x.GetValidDataAfterTrigger())
                .Then(x => x.VerifyIvstMessageWasInsertedIntoSwmFromMheForDamage(DamageOutbound.Key))
                .And(x => x.VerifyTheQuantityShouldBeReducedByIvstQtyInPickLocationForInboundPalletIsN())
                .And(x => x.VerifyTheRecordInsertedIntoPixTransactionAndValidateReasonCodeForDamageException())
                .BDDfy();
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        [Priority(8)]
        public void Ivst8WrongSkuMessageTestScenariosForInboundPalletIsY()
        {
            this.Given(x => x.TestDataForWrongSkuException())
            .And(x => x.MsgKeyForWrongSkuException())
            .And(x => x.ValidIvstUrl())
            .When(x => x.IvstApiIsCalledCreatedIsReturned())
            .And(x => x.GetValidDataAfterTrigger())
            .Then(x => x.VerifyIvstMessageWasInsertedIntoSwmFromMheWrongSku(WrongSku.Key))
            .And(x => x.VerifyTheQuantityAndWeightShouldBeReducedByIvstQtyInTransInventoryForInboundPalletIsY())
            .And(x => x.VerifyTheRecordInsertedIntoPixTransactionAndValidateReasonCodeForWrongSkuException())
            .BDDfy();
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        [Priority(9)]
        public void Ivst9WrongSkuTestScenariosForInboundPalletIsN()
        {
            this.Given(x => x.TestDataForWrongSkuInboundPalletIsN())
            .And(x => x.MsgKeyForWrongSkuInboundIsN())
            .And(x => x.ValidIvstUrl())
            .When(x => x.IvstApiIsCalledCreatedIsReturned())
            .And(x => x.GetValidDataAfterTrigger())
            .Then(x => x.VerifyIvstMessageWasInsertedIntoSwmFromMheWrongSku(WrongSkuOutbound.Key))
            .And(x => x.VerifyTheQuantityShouldBeReducedByIvstQtyInPickLocationForInboundPalletIsN())
            .And(x => x.VerifyTheRecordInsertedIntoPixTransactionAndValidateReasonCodeForWrongSkuException())
            .BDDfy();
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        [Priority(1)]
        public void Ivst1CycleCountTestScenariosForAdjustmentPlus()
        {
            this.Given(x => x.TestDataForCycleCountAdjustmentPlus())
            .And(x => x.MsgKeyForCycleCountAdjustmentPlus())
            .And(x => x.ValidIvstUrl())
            .When(x => x.IvstApiIsCalledCreatedIsReturned())
            .And(x => x.GetValidDataAfterTrigger())
            .Then(x => x.VerifyIvstMessageWasInsertedIntoSwmFromMheForCycleCount(CycleCountAdjustmentPlus.Key,"AdjustmentPlus"))
            .And(x => x.VerifyTheQuantityHasIncreasedInPickLocationForCycleCountAdjustmentPlus())
            .And(x => x.VerifyTheRecordInsertedIntoPixTransactionAndValidateReasonCodeForCycleCountAdjustmentPlus())
            .BDDfy();
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        [Priority(2)]
        public void Ivst2CycleCountTestScenariosForAdjustmentMinus()
        {
            this.Given(x => x.TestDataForCycleCountAdjustmentMinus())
            .And(x => x.MsgKeyForCycleCountAdjustmentMinus())
            .And(x => x.ValidIvstUrl())
            .When(x => x.IvstApiIsCalledCreatedIsReturned())
            .And(x => x.GetValidDataAfterTrigger())
            .Then(x => x.VerifyIvstMessageWasInsertedIntoSwmFromMheForCycleCount(CycleCountAdjustmentMinus.Key, "AdjustmentMinus"))
            .And(x => x.VerifyTheQuantityShouldBeReducedByIvstQtyInPickLocationForInboundPalletIsN())
            .And(x => x.VerifyTheRecordInsertedIntoPixTransactionAndValidateReasonCodeForCycleCountAdjustmentPlus())
            .BDDfy();
        }

    }
}
