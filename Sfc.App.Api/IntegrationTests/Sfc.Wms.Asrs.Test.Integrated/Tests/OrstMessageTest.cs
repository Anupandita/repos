using System;
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
        "Validate for AllocationStatus for the Task Completion"
        )]

    public class OrstMessageTest : OrstMessageFixture
    {
        [TestMethod]
        [TestInitialize]
        public void TestData()
        {
            //InitializeTestData();
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void OrstMessageTestForActionCodeAllocated()
        {
            this.Given(x => x.MsgKeyForCase1())
                .When(x => x.OrstApiIsCalledCreatedIsReturned())
                .And(x => x.ReadDataAfterApiForActionCodeAllocated())
                .Then(x => x.VerifyOrstMessageWasInsertedIntoSwmFromMheForActionCodeAllocated())
                .And(x => x.VerifyPickTicketStatusHasChangedToInPickingForActionCodeAllocated())
                .And(x => x.VerifyCartonStatusHasChangedToInPackingForActionCodeAllocated())              
             .BDDfy();
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void OrstMessageTestForActionCodeCompleted()
        {
            this.Given(x => x.TestDataForActionCodeComplete())
                .And(x => x.MsgKeyForCase2())
                .When(x => x.OrstApiIsCalledCreatedIsReturned())
                .Then(x => x.ReadDataAfterApiForActionCodeComplete())
                .And(x => x.VerifyOrstMessageWasInsertedIntoSwmFromMheForActionCodeComplete())
                .And(x => x.VerifyCartonStatusHasChangedToPickedForActionCodeComplete())
                .And(x => x.ValidateForQuantitiesInTocartonDetailTableForActionCodeComplete())
                .And(x => x.ValidateForQuantitiesInToPickTicketDetailTableForActionCodeComplete())
                .And(x => x.VerifyPickTicketStatusHasChangedToWeighedForStatusCodeComplete())
                .And(x => x.ValidateForQuantitiesInToPickLocationTableForActionCodeComplete())
                .And(x => x.ValidateForOrmtCountHasReducedForActionCodeComplete())
                .And(x => x.VerifyAllocationStatusHasChangedToCompleteForActionCodeComplete())
             .BDDfy();
        }

        //[TestMethod()]
        //[TestCategory("FUNCTIONAL")]
        //public void OrstMessageTestForActionCodeDeAllocate()
        //{
        //    this.Given(x => x.MsgKeyForCase3())
        //        .When(x => x.OrstApiIsCalledCreatedIsReturned())
        //        .Then(x => x.ReadDataAfterApiForActionCodeDeAllocate())
        //        .And(x => x.VerifyOrstMessageWasInsertedIntoSwmFromMheForActionCodeDeAllocate())
        //        .And(x => x.VerifyCartonStatusHasUpdatedToAllocatedOrWaitingForActionCodeDeAllocate())
        //        .BDDfy();
        //}

        //[TestMethod()]
        //[TestCategory("FUNCTIONAL")]
        //public void OrstMessageTestForActionCodeCancel()
        //{
        //    this.Given(x => x.TestDataForActionCodeCancel())
        //        .And(x =>x.MsgKeyForCase4())
        //        .When(x => x.OrstApiIsCalledCreatedIsReturned())
        //        .Then(x =>x.ReadDataAfterApiForActionCodeCancel())
        //        .And(x =>x.VerifyCartonStatusHasUpdatedToAllocatedOrWaitingForActionCodeCancel())
        //        .And(x => x.ValidateForQuantitiesInToPickLocationTableForActionCodeCancel())
        //        .And(x=>x.ValidateForOrmtCountHasReducedForActionCodeCancel())
        //        .BDDfy();
        //}



    }
}
