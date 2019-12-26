using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures;
using TestStack.BDDfy;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Tests
{

    [TestClass]
    [Story(
       Title = "123786 - Ivst exception handling",
       AsA = "Authorized User test for Ivst message from emstowms",
       IWant = "To Test for all Ivst exceptions and verify IVST message was inserted in to SWM_FROM_MHE table ",
       SoThat = "I can validate for message fields in IVST message definition, in Internal Table SWM_FROM_MHE" +
        " and validate for quantity and weight in trans_invn table when inbound pallet is yes for all exceptions which are applicable "+
        " and validate for quantity in pick location table when inbound pallet is No for all exceptions(applicable) "+
        " and validate for pix transaction table the record should be exists for each message w.r.t reason codes",
       StoryUri = "http://tfsapp1:8080/tfs/ShamrockCollection/Portfolio-SOWL/_workitems?id=123786&_a=edit"    
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
            .And(x => x.ValidIvstUrlMsgKeyAndMsgProcessorIs(IvstUrl,IvstData.Key,EmsToWmsParameters.Process))
            .When(x => x.IvstApiIsCalledCreatedIsReturned())
            .And(x => x.GetValidDataAfterTriggerForKey(IvstData.Key))
            .Then(x => x.VerifyIvstMessageWasInsertedIntoSwmFromMheForUnexceptedOverage())
            .And(x => x.VerifyTheQuantityForUnexpectedOverageExceptionIntoTransInventoryTable())
            .And(x => x.VerifyTheRecordInsertedIntoPixTransactionAndValidateReasonCodeForUnexpectedOverageException())        
            .BDDfy("Test Case ID : 124721 Dematic - IVST- Test for UnExpected overage /Residual Quantity");
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        [Priority(4)]
        public void Ivst4InventoryShortageExceptionTestScenariosForInboundPalletIsY()
        {
            this.Given(x => x.TestDataForInventoryException())            
            .And(x => x.ValidIvstUrlMsgKeyAndMsgProcessorIs(IvstUrl, InvShortageInbound.Key, EmsToWmsParametersInventoryShortage.Process))
            .When(x => x.IvstApiIsCalledCreatedIsReturned())
            .And(x => x.GetValidDataAfterTriggerForKey(InvShortageInbound.Key))
            .Then(x => x.VerifyIvstMessageWasInsertedIntoSwmFromMheForInventoryShortageAndMsgKeyShouldBe(InvShortageInbound.Key))
            .And(x => x.VerifyTheQuantityAndWeightShouldBeReducedByIvstQtyInTransInventoryForInboundPalletIsY())
            .And(x => x.VerifyTheRecordInsertedIntoPixTransactionAndValidateReasonCodeForInventoryShortageException())
            .BDDfy("Test Case ID: 124725- Dematic - IVST- Test for Inventory Shortage Validate for all functionalities .");
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        [Priority(5)]

        public void Ivst5InventoryShortageExceptionTestScenariosForInboundPalletIsN()
        {
            this.Given(x => x.TestDataForInventoryShortageInboundPalletIsN())            
            .And(x => x.ValidIvstUrlMsgKeyAndMsgProcessorIs(IvstUrl, InvShortageOutbound.Key, EmsToWmsParametersInventoryShortage.Process))
            .When(x => x.IvstApiIsCalledCreatedIsReturned())
            .And(x => x.GetValidDataAfterTriggerForKey(InvShortageOutbound.Key))
            .Then(x => x.VerifyIvstMessageWasInsertedIntoSwmFromMheForInventoryShortageAndMsgKeyShouldBe(InvShortageOutbound.Key))
            .And(x =>x.VerifyTheQuantityShouldBeReducedByIvstQtyInPickLocationForInboundPalletIsN())
            .And(x => x.VerifyTheRecordInsertedIntoPixTransactionAndValidateReasonCodeForInventoryShortageException())
            .BDDfy("Test Case ID: 124725- Dematic - IVST- Test for Inventory Shortage. Validate for all functionalities .");
        }


        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        [Priority(6)]
        public void Ivst6DamageExceptionTestScenariosForInboundPalletIsY()
        {
            this.Given(x => x.TestDataForDamageException())            
            .And(x => x.ValidIvstUrlMsgKeyAndMsgProcessorIs(IvstUrl, DamageInbound.Key, EmsToWmsParametersDamage.Process))
            .When(x => x.IvstApiIsCalledCreatedIsReturned())
            .And(x => x.GetValidDataAfterTriggerForKey(DamageInbound.Key))
            .Then(x => x.VerifyIvstMessageWasInsertedIntoSwmFromMheForDamageAndMsgKeyShouldBe(DamageInbound.Key))
            .And(x => x.VerifyTheQuantityAndWeightShouldBeReducedByIvstQtyInTransInventoryForInboundPalletIsY())
            .And(x => x.VerifyTheRecordInsertedIntoPixTransactionAndValidateReasonCodeForDamageException())
            .BDDfy("Test Case ID :124726- Dematic - IVST- Test for Damage Exceptions for Inbound Pallet = Y and Validate for all functionalities");
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        [Priority(7)]
        public void Ivst7DamageExceptionTestScenariosForInboundPalletIsN()
        {
            this.Given(x =>x.TestDataForDamageInboundPalletIsN())              
                .And(x =>x.ValidIvstUrlMsgKeyAndMsgProcessorIs(IvstUrl,DamageOutbound.Key, EmsToWmsParametersDamage.Process))
                .When(x => x.IvstApiIsCalledCreatedIsReturned())
                .And(x => x.GetValidDataAfterTriggerForKey(DamageOutbound.Key))
                .Then(x => x.VerifyIvstMessageWasInsertedIntoSwmFromMheForDamageAndMsgKeyShouldBe(DamageOutbound.Key))
                .And(x => x.VerifyTheQuantityShouldBeReducedByIvstQtyInPickLocationForInboundPalletIsN())
                .And(x => x.VerifyTheRecordInsertedIntoPixTransactionAndValidateReasonCodeForDamageException())
                .BDDfy("Test Case ID : 124726- Dematic - IVST- Test for Damage Exceptions for Inbound Pallet = N and Validate for all functionalities");
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        [Priority(8)]
        public void Ivst8WrongSkuMessageTestScenariosForInboundPalletIsY()
        {
            this.Given(x => x.TestDataForWrongSkuException())           
            .And(x => x.ValidIvstUrlMsgKeyAndMsgProcessorIs(IvstUrl,WrongSku.Key,EmsToWmsParametersWrongSku.Process))
            .When(x => x.IvstApiIsCalledCreatedIsReturned())
            .And(x => x.GetValidDataAfterTriggerForKey(WrongSku.Key))
            .Then(x => x.VerifyIvstMessageWasInsertedIntoSwmFromMheForWrongSkuAndMsgKeyShouldBe(WrongSku.Key))
            .And(x => x.VerifyTheQuantityAndWeightShouldBeReducedByIvstQtyInTransInventoryForInboundPalletIsY())
            .And(x => x.VerifyTheRecordInsertedIntoPixTransactionAndValidateReasonCodeForWrongSkuException())
            .BDDfy("Test Case ID : 124727-Dematic - IVST- Test for Wrong SKU for Inbound Pallet = Y and validate for all functionalities");
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        [Priority(9)]
        public void Ivst9WrongSkuTestScenariosForInboundPalletIsN()
        {
            this.Given(x => x.TestDataForWrongSkuInboundPalletIsN())          
            .And(x => x.ValidIvstUrlMsgKeyAndMsgProcessorIs(IvstUrl, WrongSkuOutbound.Key, EmsToWmsParametersWrongSku.Process))
            .When(x => x.IvstApiIsCalledCreatedIsReturned())
            .And(x => x.GetValidDataAfterTriggerForKey(WrongSkuOutbound.Key))
            .Then(x => x.VerifyIvstMessageWasInsertedIntoSwmFromMheForWrongSkuAndMsgKeyShouldBe(WrongSkuOutbound.Key))
            .And(x => x.VerifyTheQuantityShouldBeReducedByIvstQtyInPickLocationForInboundPalletIsN())
            .And(x => x.VerifyTheRecordInsertedIntoPixTransactionAndValidateReasonCodeForWrongSkuException())
            .BDDfy("Test Case ID:124727 -Dematic - IVST- Test for Wrong SKU for Inbound Pallet = N and validate for all functionalities");
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        [Priority(1)]
        public void Ivst1CycleCountTestScenariosForAdjustmentPlus()
        {
            this.Given(x => x.TestDataForCycleCountAdjustmentPlus())            
            .And(x => x.ValidIvstUrlMsgKeyAndMsgProcessorIs(IvstUrl, CycleCountAdjustmentPlus.Key,EmsToWmsParametersCycleCount.Process))
            .When(x => x.IvstApiIsCalledCreatedIsReturned())
            .And(x => x.GetValidDataAfterTriggerForKey(CycleCountAdjustmentPlus.Key))
            .Then(x => x.VerifyIvstMessageWasInsertedIntoSwmFromMheForCycleCountAndMsgKeyShouldBe(CycleCountAdjustmentPlus.Key,"AdjustmentPlus"))
            .And(x => x.VerifyTheQuantityHasIncreasedInPickLocationForCycleCountAdjustmentPlus())
            .And(x => x.VerifyTheRecordInsertedIntoPixTransactionAndValidateReasonCodeForCycleCountAdjustmentPlus())
            .BDDfy("Test Case ID: 124720 - Dematic - IVST - Test for IVST api when the Cycle count is done. for both action codes AdjustmentPlus") ;
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        [Priority(2)]
        public void Ivst2CycleCountTestScenariosForAdjustmentMinus()
        {
            this.Given(x => x.TestDataForCycleCountAdjustmentMinus())           
            .And(x => x.ValidIvstUrlMsgKeyAndMsgProcessorIs(IvstUrl,CycleCountAdjustmentMinus.Key, EmsToWmsParametersCycleCount.Process))
            .When(x => x.IvstApiIsCalledCreatedIsReturned())
            .And(x => x.GetValidDataAfterTriggerForKey(CycleCountAdjustmentMinus.Key))
            .Then(x => x.VerifyIvstMessageWasInsertedIntoSwmFromMheForCycleCountAndMsgKeyShouldBe(CycleCountAdjustmentMinus.Key, "AdjustmentMinus"))
            .And(x => x.VerifyTheQuantityShouldBeReducedByIvstQtyInPickLocationForInboundPalletIsN())
            .And(x => x.VerifyTheRecordInsertedIntoPixTransactionAndValidateReasonCodeForCycleCountAdjustmentPlus())
            .BDDfy("Test Case ID: 124720 - Dematic - IVST - Test for IVST api when the Cycle count is done. for both action codes AdjustmentPlusMinus");
        }
    }
}
