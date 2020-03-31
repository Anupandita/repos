using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures;
using TestStack.BDDfy;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Tests
{
    [TestClass]
    [Story(
       AsA = "Authorized User test for orst message from emstowms",
       IWant = "To Verify ORST message is inserted in to swm_to_mhe table with appropriate data" +
        "For Bellow Cases" +
        "Case1: ActionCode = Allocated " +
        "Case2: ActionCode = Completed and OrderReasonCodeMap = '0'" +
        "Case3: ActionCode = DeAllocate " +
        "Case4: ActionCode = Cancelled" +
        "To Verify Carton Status"
           ,
       SoThat = "I can validate for message fields in ORMT message, in Internal Table SWM_TO_MHE and in wmstoems for all caseTypes" +
        "Verify For Pick Ticket Status and Carton Status for all the Action Codes" +
        "Validate For Quantities InTo CartonDetail, PickTicketDetail,PickLocationDetail for all case types " +
        "Validate for Ormt count for all case types" +
        "Validate for AllocationStatus for the Task Completion",
      StoryUri = "http://tfsapp1:8080/tfs/ShamrockCollection/Portfolio-SOWL/_workitems?id=129459&_a=edit"
        )]

    public class OrstMessageTest : OrstMessageFixture
    {
        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        [DataRow(4)]
        public void OrstMessageTest1ForActionCodeAllocated(int count)
        {
            this.Given(x=>x.InitializeTestData())                
                .And(x => x.ValidMsgKeyMsgProcessorAndOrstUrlIs(MsgKeyForAllocated.MsgKey, EmsToWmsAllocated.Process,OrstUrl))
                //.When(x => x.OrstApiIsCalledCreatedIsReturned())
                .And(x => x.ReadDataAfterApiForActionCodeAllocated())
                .Then(x => x.VerifyOrstMessageWasInsertedIntoSwmFromMheForActionCodeAllocated())
                .And(x => x.VerifyPickTicketStatusHasChangedToInPickingForActionCodeAllocated())
                .And(x => x.VerifyCartonStatusHasChangedToInPackingForActionCodeAllocated())                  
             .BDDfy("Test Case Id:134866 -Dematic :  ORST : Test For Message when ActionCode = 'Allocated'");
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
       
        public void OrstMessageTest2ForActionCodeCompleted()
        {
            this.Given(x => x.TestDataForActionCodeComplete())             
                .And(x => x.ValidMsgKeyMsgProcessorAndOrstUrlIs(MsgKeyForCompleted.MsgKey, EmsToWmsCompleted.Process,OrstUrl))
                //.When(x => x.OrstApiIsCalledCreatedIsReturned())
                .Then(x => x.ReadDataAfterApiForActionCodeComplete())
                .And(x => x.VerifyOrstMessageWasInsertedIntoSwmFromMheForActionCodeComplete())
                .And(x => x.VerifyCartonStatusHasChangedToPickedForActionCodeComplete())
                .And(x => x.ValidateForQuantitiesInTocartonDetailTableForActionCodeComplete())
                .And(x => x.ValidateForQuantitiesInToPickTicketDetailTableForActionCodeComplete())
                .And(x => x.VerifyPickTicketStatusHasChangedToWeighedForStatusCodeComplete())
                .And(x => x.ValidateForQuantitiesInToPickLocationTableForActionCodeComplete())
                .And(x => x.ValidateForOrmtCountHasReducedForActionCodeComplete())
                .And(x => x.VerifyAllocationStatusHasChangedToCompleteForActionCodeComplete())
                .And(x =>x.ValidateForMessageToSvCountForOrstCompletedMessage())
                .And(x =>x.ValidateForMessageToCWVCount())
             .BDDfy("Test Case Id:134867 -Dematic : ORST : Test Message when Action Code = complete with order reason code map = 0");
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void OrstMessageTest3ForActionCodeDeAllocate()
        {
            this.Given(x=>x.TestDataForActionCodeDeAllocate())                           
                .And(x => x.ValidMsgKeyMsgProcessorAndOrstUrlIs(MsgKeyForDeallocated.MsgKey, EmsToWmsDeallocated.Process,OrstUrl))
                .When(x => x.OrstApiIsCalledCreatedIsReturned())
                .Then(x => x.ReadDataAfterApiForActionCodeDeAllocate())
                .And(x => x.VerifyOrstMessageWasInsertedIntoSwmFromMheForActionCodeDeAllocate())               
                .BDDfy("Test Case Id:134868 -Dematic : ORST : Test Message when Action Code = De-Allocate");
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void OrstMessageTest4ForActionCodeCancel()
        {
            this.Given(x => x.TestDataForActionCodeCancel())       
                .And(x => x.ValidMsgKeyMsgProcessorAndOrstUrlIs(MsgKeyForCanceled.MsgKey, EmsToWmsCanceled.Process,OrstUrl))
                .When(x => x.OrstApiIsCalledCreatedIsReturned())
                .Then(x => x.ReadDataAfterApiForActionCodeCancel())
                .And(x => x.VerifyCartonStatusHasUpdatedToAllocatedOrWaitingForActionCodeCancel())        
                .And(x => x.ValidateForOrmtCountHasReducedForActionCodeCancel())
                .BDDfy("Test Case Id:134869 -Dematic : ORST : Test Message when Action Code = Cancel");
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void OrstMessageTest5ForActionCodeCompleteWhenBitCodeIsEnabled()
        {
            this.Given(x => x.ReadDataBeforeCallingApiForActionCodeCompleteWithBitsEnabled())                
                .And(x => x.ValidMsgKeyMsgProcessorAndOrstUrlIs(MsgKeysForCase5.MsgKey, EmsToWmsCompleted.Process,OrstUrl))
                .When(x => x.OrstApiIsCalledCreatedIsReturned())
                .Then(x => x.ReadDataAfterCallingApiForActionCodeCompleteWithBitsEnabled())
                .And(x => x.VerifyOrstMessageWasInsertedIntoSwmFromMheForActionCodeComplete())
                .And(x => x.VerifyCartonStatusHasChangedTo5ForActionCodeCompleteWithBitsEnabled())
                .And(x => x.VerifyForQuantitiesInToPickLocationTableForActionCodeCompleteWithBitsEnabled())
                .And(x => x.VerifyForOrmtCountForActionCodeCompleteWithBitsEnabled())
                .BDDfy("Test Case Id:134871 -Dematic : ORST : Test Message when Action Code = complete with order reason code map <> 0");
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void OrstMessageTest6ForActionCodeCompleteWhenPickTicketSeqNbrIsSmallerThan1()
        {
            this.Given(x => x.ReadDataBeforeApiForNegativeCaseWherePickTicketSeqNumberIsLessThan1())            
                .And(x => x.ValidMsgKeyMsgProcessorAndOrstUrlIs(MsgKeyForCompleted.MsgKey, EmsToWmsCompleted.Process,OrstUrl))
                .When(x => x.OrstApiIsCalledForNegativeCase())
                .BDDfy("Test Case Id:146384 -OrstMessage:  Test For ActionCode Complete When PickTicket SeqNbr Is Smaller Than 1");            
        }
    }
}
