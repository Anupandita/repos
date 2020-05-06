using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures;
using Sfc.Wms.Interfaces.ParserAndTranslator.Contracts.Constants;
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

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]       
        public void Ivst3UnexpectedOverageExceptionTestScenariosForInboundPalletIsY() //105,121
        {
            this.Given(x => x.TestDataForUnexpectedOverageExceptionScenario1())          
            .And(x => x.ValidIvstUrlMsgKeyAndMsgProcessorIs(IvstUrl,IvstData.Key,EmsToWmsParameters.Process))
            .When(x => x.IvstApiIsCalledCreatedIsReturned())
            .And(x => x.GetValidDataAfterTriggerForKey(IvstData.Key))
            .Then(x => x.VerifyIvstMessageWasInsertedIntoSwmFromMheForUnexceptedOverage(IvstData.Key))
            .And(x=>x.ValidationForQuuantityInCaseDtlTableForUnExpectedOverageScenario1())
            .And(x => x.VerifyTheQuantityForUnexpectedOverageExceptionIntoTransInventoryTableAndPickLocnTable())            
            .BDDfy("Test Case ID : 124721 Dematic - IVST- Test for UnExpected overage /Residual Quantity");
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]      
        public void Ivst3UnexpectedOverageExceptionTestScenariosForInboundPalletIsYScenario2() //106, 122
        {
            this.Given(x => x.TestDataForUnexpectedOverageExceptionScenario2())
            .And(x => x.ValidIvstUrlMsgKeyAndMsgProcessorIs(IvstUrl, IvstData.Key, EmsToWmsParameters.Process))
            .When(x => x.IvstApiIsCalledCreatedIsReturned())
            .And(x => x.GetValidDataAfterTriggerForKey(IvstData.Key))
            .Then(x => x.VerifyIvstMessageWasInsertedIntoSwmFromMheForUnexceptedOverage(IvstData.Key))
            .And(x => x.ValidationForStatCodeShouldBeUpdatedTo96ForScenario2())
            .And(x => x.VerifyTheQuantityForUnexpectedOverageExceptionIntoTransInventoryTableAndPickLocnTable())           
            .BDDfy("Test Case ID : 124721 Dematic - IVST- Test for UnExpected overage /Residual Quantity");
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]      
        public void Ivst3UnexpectedOverageExceptionTestScenariosForInboundPalletIsYScenario3()
        {
            this.Given(x => x.TestDataForUnexpectedOverageExceptionScenario3())
            .And(x => x.ValidIvstUrlMsgKeyAndMsgProcessorIs(IvstUrl, IvstData.Key, EmsToWmsParameters.Process))
            .When(x => x.IvstApiIsCalledCreatedIsReturned())
            .And(x => x.GetValidDataAfterTriggerForKey(IvstData.Key))
            .Then(x => x.VerifyIvstMessageWasInsertedIntoSwmFromMheForUnexceptedOverage(IvstData.Key))
            .And(x => x.ValidationForUnExpectedOverageWhereCaseHdrStatCode96AndNegativePickAlreadyExists())     
            .And(x => x.VerifyTheQuantityForUnexpectedOverageExceptionIntoTransInventoryTableAndPickLocnTable())
            .BDDfy("Test Case ID : 124721 Dematic - IVST- Test for UnExpected overage /Residual Quantity");
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]       
        public void Ivst3UnexpectedOverageExceptionTestScenariosForInboundPalletIsYScenario4() //123
        {
            this.Given(x => x.TestDataForUnexpectedOverageExceptionScenario4())
            .And(x => x.ValidIvstUrlMsgKeyAndMsgProcessorIs(IvstUrl, IvstData.Key, EmsToWmsParameters.Process))
            .When(x => x.IvstApiIsCalledCreatedIsReturned())
            .And(x => x.GetValidDataAfterTriggerForKey(IvstData.Key))
            .Then(x => x.VerifyIvstMessageWasInsertedIntoSwmFromMheForUnexceptedOverage(IvstData.Key))
            .And(x => x.ValidationForUnExpectedOverageWhereCaseHdrStatCode96AndNegativePickDoesNotExists())
            .And(x => x.VerifyTheQuantityForUnexpectedOverageExceptionIntoTransInventoryTableAndPickLocnTable())
            .BDDfy("Test Case ID : 124721 Dematic - IVST- Test for UnExpected overage /Residual Quantity");
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]        
        public void Ivst4InventoryShortageExceptionTestScenariosForInboundPalletIsY() //124
        {
            this.Given(x => x.TestDataForInventoryException())            
            .And(x => x.ValidIvstUrlMsgKeyAndMsgProcessorIs(IvstUrl, InvShortageInbound.Key, EmsToWmsParametersInventoryShortage.Process))
            .When(x => x.IvstApiIsCalledCreatedIsReturned())
            .And(x => x.GetValidDataAfterTriggerForKey(InvShortageInbound.Key))
            .Then(x => x.VerifyIvstMessageWasInsertedIntoSwmFromMheForInventoryShortageAndMsgKeyShouldBe(InvShortageInbound.Key))
            .And(x => x.VerifyTheQuantityAndWeightShouldBeReducedByIvstQtyInTransInventoryAndPicklocnForToBeFilledAndrInboundPalletIsY())
            .And(x => x.VerifyTheRecordInsertedIntoPixTransactionAndValidateReasonCodeForInventoryShortageException())
            .BDDfy("Test Case ID: 124725- Dematic - IVST- Test for Inventory Shortage Validate for all functionalities .");
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]      
        public void Ivst5InventoryShortageExceptionTestScenariosForInboundPalletIsN() //125
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
        public void Ivst5InventoryShortageExceptionTestScenariosForInboundPalletIsNScenario3() //126 
        {
            this.Given(x => x.TestDataForInventoryShortageScenario3())
            .And(x => x.ValidIvstUrlMsgKeyAndMsgProcessorIs(IvstUrl, InvShortageOutbound.Key, EmsToWmsParametersInventoryShortage.Process))
            .When(x => x.IvstApiIsCalledCreatedIsReturned())
            .And(x => x.GetValidDataAfterTriggerForKey(InvShortageOutbound.Key))
            .Then(x => x.VerifyIvstMessageWasInsertedIntoSwmFromMheForInventoryShortageAndMsgKeyShouldBe(InvShortageOutbound.Key))
            .And(x => x.ValidateForInventoryShortageScenario3())
            .And(x =>x.VerifyQtyReducedToZeroInPickLocnDtlTable())
            .And(x => x.VerifyForPixTransactionAndValidateReasonCodeForInventoryShortageExceptionWherePickLocnQtyIsSmallerThanIvstQty())
            .BDDfy("Test Case ID: 124725- Dematic - IVST- Test for Inventory Shortage. Validate for all functionalities .");
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]     
        public void Ivst5InventoryShortageExceptionTestScenariosForInboundPalletIsNScenario4() 
        {
            this.Given(x => x.TestDataForInventoryShortageScenario4())
            .And(x => x.ValidIvstUrlMsgKeyAndMsgProcessorIs(IvstUrl, InvShortageOutbound.Key, EmsToWmsParametersInventoryShortage.Process))
            .When(x => x.IvstApiIsCalledCreatedIsReturned())
            .And(x => x.GetValidDataAfterTriggerForKey(InvShortageOutbound.Key))
            .Then(x => x.VerifyIvstMessageWasInsertedIntoSwmFromMheForInventoryShortageAndMsgKeyShouldBe(InvShortageOutbound.Key))
            .And(x => x.ValidateForInventoryShortageScenario4())
            .And(x => x.VerifyQtyReducedToZeroInPickLocnDtlTable())
            .And(x => x.VerifyForPixTransactionAndValidateReasonCodeForInventoryShortageExceptionWherePickLocnQtyIsSmallerThanIvstQty())
            .BDDfy("Test Case ID: 124725- Dematic - IVST- Test for Inventory Shortage. Validate for all functionalities .");
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]      
        public void Ivst5InventoryShortageExceptionTestScenariosForInboundPalletIsNScenario5() //127
        {
            this.Given(x => x.TestDataForInventoryShortageScenario5())
            .And(x => x.ValidIvstUrlMsgKeyAndMsgProcessorIs(IvstUrl, InvShortageOutbound.Key, EmsToWmsParametersInventoryShortage.Process))
            .When(x => x.IvstApiIsCalledCreatedIsReturned())
            .And(x => x.GetValidDataAfterTriggerForKey(InvShortageOutbound.Key))
            .Then(x => x.VerifyIvstMessageWasInsertedIntoSwmFromMheForInventoryShortageAndMsgKeyShouldBe(InvShortageOutbound.Key))
            .And(x => x.ValidateForInventoryShortageScenario5())             
            .BDDfy("Test Case ID: 124725- Dematic - IVST- Test for Inventory Shortage. Validate for all functionalities .");
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]    
        public void Ivst6DamageExceptionTestScenariosForInboundPalletIsY()
        {
            this.Given(x => x.TestDataForDamageException())
            .And(x => x.ValidIvstUrlMsgKeyAndMsgProcessorIs(IvstUrl, DamageInbound.Key, EmsToWmsParametersDamage.Process))
            .When(x => x.IvstApiIsCalledCreatedIsReturned())
            .And(x => x.GetValidDataAfterTriggerForKey(DamageInbound.Key))
            .Then(x => x.VerifyIvstMessageWasInsertedIntoSwmFromMheForDamageAndMsgKeyShouldBe(DamageInbound.Key))
            .And(x => x.VerifyTheQuantityAndWeightShouldBeReducedByIvstQtyInTransInventoryAndPicklocnForToBeFilledAndrInboundPalletIsY())
            .And(x => x.VerifyForCorbaHdrDtlRecords())
            .And(x => x.VerifyForParmname())
            .And(x => x.VerifyParamType())
            .And(x => x.VerifyParamValue())
           // .And(x => x.VerifyForCaseHdrAndCaseDtlRecordInsertedOrNotForDamageAndWrongSkuExceptions())
            .BDDfy("Test Case ID :124726- Dematic - IVST- Test for Damage Exceptions for Inbound Pallet = Y and Validate for all functionalities");
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
      
        public void Ivst7DamageExceptionTestScenariosForInboundPalletIsN() //129
        {
            this.Given(x =>x.TestDataForDamageInboundPalletIsN())              
                .And(x =>x.ValidIvstUrlMsgKeyAndMsgProcessorIs(IvstUrl,DamageOutbound.Key, EmsToWmsParametersDamage.Process))
                //.When(x => x.IvstApiIsCalledCreatedIsReturned())
                .And(x => x.GetValidDataAfterTriggerForKey(DamageOutbound.Key))
                .Then(x => x.VerifyIvstMessageWasInsertedIntoSwmFromMheForDamageAndMsgKeyShouldBe(DamageOutbound.Key))
                .And(x => x.VerifyTheQuantityShouldBeReducedByIvstQtyInPickLocationForInboundPalletIsN())
                .And(x => x.VerifyForCorbaHdrDtlRecords())
                .And(x => x.VerifyForParmname())
                .And(x => x.VerifyParamType())
                .And(x => x.VerifyParamValue())
               // .And(x => x.VerifyForCaseHdrAndCaseDtlRecordInsertedOrNotForDamageAndWrongSkuExceptions())
                .BDDfy("Test Case ID : 124726- Dematic - IVST- Test for Damage Exceptions for Inbound Pallet = N and Validate for all functionalities");
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]        
        public void Ivst7DamageExceptionTestScenariosForInboundPalletIsNScenario3() //130
        {
            this.Given(x => x.TestDataForDamageScenario3())
                .And(x => x.ValidIvstUrlMsgKeyAndMsgProcessorIs(IvstUrl, DamageOutbound.Key, EmsToWmsParametersDamage.Process))
                .When(x => x.IvstApiIsCalledCreatedIsReturned())
                .And(x => x.GetValidDataAfterTriggerForKey(DamageOutbound.Key))
                .Then(x => x.VerifyIvstMessageWasInsertedIntoSwmFromMheForDamageAndMsgKeyShouldBe(DamageOutbound.Key))
                .And(x => x.VerifyQtyReducedToZeroInPickLocnDtlTable())
                .And(x => x.VerifyForCorbaHdrDtlRecords())
                .And(x => x.VerifyForParmname())
                .And(x => x.VerifyParamType())
                .And(x => x.VerifyParamValue())
               // .And(x => x.VerifyForCaseHdrAndCaseDtlRecordInsertedOrNotForDamageAndWrongSkuExceptions())
                .BDDfy("Test Case ID : 124726- Dematic - IVST- Test for Damage Exceptions for Inbound Pallet = N and Validate for all functionalities");
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
       
        public void Ivst8WrongSkuMessageTestScenariosForInboundPalletIsY() //95
        {
            this.Given(x => x.TestDataForWrongSkuException())           
            .And(x => x.ValidIvstUrlMsgKeyAndMsgProcessorIs(IvstUrl,WrongSku.Key,EmsToWmsParametersWrongSku.Process))
            .When(x => x.IvstApiIsCalledCreatedIsReturned())
            .And(x => x.GetValidDataAfterTriggerForKey(WrongSku.Key))
            .Then(x => x.VerifyIvstMessageWasInsertedIntoSwmFromMheForWrongSkuAndMsgKeyShouldBe(WrongSku.Key))
            .And(x => x.VerifyTheQuantityAndWeightShouldBeReducedByIvstQtyInTransInventoryAndPicklocnForToBeFilledAndrInboundPalletIsY())    
            .And(x => x.VerifyForCorbaHdrDtlRecords())
            .And(x => x.VerifyForParmname())
            .And(x => x.VerifyParamType())
            .And(x => x.VerifyParamValue())
            //.And(x => x.VerifyForCaseHdrAndCaseDtlRecordInsertedOrNotForDamageAndWrongSkuExceptions())
            .BDDfy("Test Case ID : 124727-Dematic - IVST- Test for Wrong SKU for Inbound Pallet = Y and validate for all functionalities");
        }
     
        [TestMethod()]
        [TestCategory("FUNCTIONAL")]     
        public void Ivst1CycleCountTestScenariosForAdjustmentPlus() //96
        {
            this.Given(x => x.TestDataForCycleCountAdjustmentPlus())            
            .And(x => x.ValidIvstUrlMsgKeyAndMsgProcessorIs(IvstUrl, CycleCountAdjustmentPlus.Key,EmsToWmsParametersCycleCount.Process))
            .When(x => x.IvstApiIsCalledCreatedIsReturned())
            .And(x => x.GetValidDataAfterTriggerForKey(CycleCountAdjustmentPlus.Key))
            .Then(x => x.VerifyIvstMessageWasInsertedIntoSwmFromMheForCycleCountAndMsgKeyShouldBe(CycleCountAdjustmentPlus.Key,"AdjustmentPlus"))
            .And(x => x.VerifyTheQuantityHasIncreasedInPickLocationForCycleCountAdjustmentPlus())
            .And(x => x.VerifyTheRecordInsertedIntoPixTransactionAndValidateReasonCodeForCycleCountAdjustmentPlus("A"))
            .BDDfy("Test Case ID: 124720 - Dematic - IVST - Test for IVST api when the Cycle count is done. for both action codes AdjustmentPlus") ;
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]       
        public void Ivst2CycleCountTestScenariosForAdjustmentMinus() //97,133
        {
            this.Given(x => x.TestDataForCycleCountAdjustmentMinus())           
            .And(x => x.ValidIvstUrlMsgKeyAndMsgProcessorIs(IvstUrl,CycleCountAdjustmentMinus.Key, EmsToWmsParametersCycleCount.Process))
            .When(x => x.IvstApiIsCalledCreatedIsReturned())
            .And(x => x.GetValidDataAfterTriggerForKey(CycleCountAdjustmentMinus.Key))
            .Then(x => x.VerifyIvstMessageWasInsertedIntoSwmFromMheForCycleCountAndMsgKeyShouldBe(CycleCountAdjustmentMinus.Key, "AdjustmentMinus"))
            .And(x => x.VerifyTheQuantityShouldBeReducedByIvstQtyInPickLocationForInboundPalletIsN())
            .And(x => x.VerifyTheRecordInsertedIntoPixTransactionAndValidateReasonCodeForCycleCountAdjustmentPlus("S"))
            .BDDfy("Test Case ID: 124720 - Dematic - IVST - Test for IVST api when the Cycle count is done. for both action codes AdjustmentPlusMinus");
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]       
        public void Ivst10MixedInventoryTestScenariosForAdjustmentMinus() //98
        {
            this.Given(x => x.TestDataForMixedInventory())
                .And(x => x.ValidIvstUrlMsgKeyAndMsgProcessorIs(IvstUrl, MixedOutbound.Key, EmsToWmsParametersMixedInventory.Process))
                .When(x => x.IvstApiIsCalledCreatedIsReturned())
                .And(x => x.GetValidDataAfterTriggerForKey(MixedOutbound.Key))
                .Then(x => x.VerifyIvstMessageWasInsertedIntoSwmFromMheForMixedInventoryAndMsgKeyShouldBe(MixedOutbound.Key, "AdjustmentMinus"))
                .And(x => x.VerifyTheQuantityShouldNotBeChanged())
                .BDDfy();
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void IvstTestForActionCodeAdjustment() //98
        {
            this.Given(x => x.TestDataForActionCodeAdjustment())
                .And(x => x.ValidIvstUrlMsgKeyAndMsgProcessorIs(IvstUrl, MixedOutbound.Key, EmsToWmsParametersMixedInventory.Process))
                //.When(x => x.IvstApiIsCalledCreatedIsReturned())
                .And(x => x.GetValidDataAfterTriggerForKey(MixedOutbound.Key))
                .Then(x => x.VerifyIvstMessageWasInsertedIntoSwmFromMheForMixedInventoryAndMsgKeyShouldBe(MixedOutbound.Key, "AdjustmentMinus"))
                .And(x => x.VerifyTheQuantityShouldNotBeChanged())
                .BDDfy();
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]        
        public void Ivst11NoExceptionTestScenariosForAdjustmentMinus() //99
        {
            this.Given(x => x.TestDataForNoException())
                //.And(x => x.ValidIvstUrlMsgKeyAndMsgProcessorIs(IvstUrl, NoExceptionInBound.Key, EmsToWmsParametersNoException.Process))
                //.When(x => x.IvstApiIsCalledCreatedIsReturned())
                //.And(x => x.GetValidDataAfterTriggerForKey(NoExceptionInBound.Key))
                //.Then(x => x.VerifyIvstMessageWasInsertedIntoSwmFromMheForNoExceptionAndMsgKeyShouldBe(NoExceptionInBound.Key, "AdjustmentMinus"))
                //.And(x => x.VerifyTheQuantityShouldNotBeChanged())
                .BDDfy();
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]     
        public void Ivst12CycleCountForActionCodeAdjustMentMinusScenario3()
        {
            this.Given(x => x.TestDataForCcAdjustmentMinusScenario3())
                .And(x => x.ValidIvstUrlMsgKeyAndMsgProcessorIs(IvstUrl, CycleCountAdjustmentMinus.Key, EmsToWmsParametersCycleCount.Process))
                .When(x => x.IvstApiIsCalledCreatedIsReturned())
                .And(x => x.GetValidDataAfterTriggerForKey(CycleCountAdjustmentMinus.Key))          
                .Then(x => x.VerifyIvstMessageWasInsertedIntoSwmFromMheForCycleCountAndMsgKeyShouldBe(CycleCountAdjustmentMinus.Key, "AdjustmentMinus"))
                .And(x => x.VerifyForCycleCountNegativePickRecordAlreadyExists())
                .And(x => x.VerifyTheRecordInsertedInToPixTransactionForCycleCountNegativePickScenario("S"))
                .BDDfy();
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void Ivst12CycleCountForActionCodeAdjustMentMinusScenario4() //100
        {
            this.Given(x => x.TestDataForAdjustmentMinusScenario4())
                .And(x => x.ValidIvstUrlMsgKeyAndMsgProcessorIs(IvstUrl, CycleCountAdjustmentMinus.Key, EmsToWmsParametersCycleCount.Process))
                .When(x => x.IvstApiIsCalledCreatedIsReturned())
                .And(x => x.GetValidDataAfterTriggerForKey(CycleCountAdjustmentMinus.Key))
                .Then(x => x.VerifyIvstMessageWasInsertedIntoSwmFromMheForCycleCountAndMsgKeyShouldBe(CycleCountAdjustmentMinus.Key, "AdjustmentMinus"))
                .And(x => x.VerifyQtyReducedToZeroInPickLocnDtlTable())
                .And(x =>x.VerifyTheRecordInsertedInToPixTransactionForCycleCountNegativePickScenario("S"))
                .BDDfy();
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void Ivst12CycleCountForActionCodeAdjustMentMinusScenario5() //101
        {
            this.Given(x => x.TestDataForAdjustmentMinusScenario5())
                .And(x => x.ValidIvstUrlMsgKeyAndMsgProcessorIs(IvstUrl, CycleCountAdjustmentMinus.Key, EmsToWmsParametersCycleCount.Process))
                .When(x => x.IvstApiIsCalledCreatedIsReturned())
                .And(x => x.GetValidDataAfterTriggerForKey(CycleCountAdjustmentMinus.Key))
                .Then(x => x.VerifyIvstMessageWasInsertedIntoSwmFromMheForCycleCountAndMsgKeyShouldBe(CycleCountAdjustmentMinus.Key, "AdjustmentMinus"))
                .And(x => x.VerifyForCycleCountWherePickLocnQtyIsLessThanIvstQtyandLessthanOrEqualToZero())              
                .BDDfy();
        }
    }
}
