using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures;
using TestStack.BDDfy;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Tests
{
    [TestClass]
    [Story(
       AsA = "Authorized User test for ormt message from wmstoems",
      IWant = "To Verify ORMT message is inserted in to swm_to_mhe table with appropriate data" +
       " And verify ORMT message is inserted in to wmstoems table with appropriate data " +
       " Verify in PickLocnDtlExt table for Ormt Count",
      SoThat = "I can validate for message fields in ORMT message, in Internal Table SWM_TO_MHE and in wmstoems"
       )]
    
    public class OrmtMessageTest : OrmtMessageFixture
    {
        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        [DataRow(3)]
        public void VerifyForOrmtMessageWithActionCodeAddRelease(int count)
        {
            this.Given(x => x.InitializeTestDataForPrintingOfCartons())
            .And(x => x.CartonNumberForAddRelease())
            .And(x=>x.ValidOrmtUrl())
            .When(x => x.OrmtApiIsCalledCreatedIsReturned())
            .And(x => x.ReadDataAfterApiForPrintingOfCarton())
            .Then(x => x.VerifyOrmtMessageWasInsertedInToSwmToMhe())
            .And(x => x.VerifyOrmtMessageWasInsertedInToWmsToEmsForPrintingOfOrder())
            .And(x => x.VerifyForOrmtCountInPickLocnDtlExt())
            .And(x => x.VerifyForStatusCodeinCartonHdrForAddRelease())
           .BDDfy();
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void VerifyForOrmtMessageWithActionCodeCancel()
        {
            this.Given(x => x.InitializeTestDataForCancelOfCarton())
           .And(x => x.CartonNumberForCancel())
           .And(x => x.ValidOrmtUrl())
           .When(x => x.OrmtApiIsCalledCreatedIsReturned())
           .And(x => x.ReadDataAfterApiForCancelOfCarton())
           .Then(x => x.VerifyOrmtMessageWasInsertedInToSwmToMheForCancelOrders())
           .And(x => x.VerifyOrmtMessageWasInsertedInToWmsToEmsForCancelOrder())
           .And(x => x.VerifyForOrmtCountInPickLocnDtlExt())
           .BDDfy();
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void VerifyForOrmtMessageWithActionCodeEPick()
        {
            this.Given(x=>x.InitializeTestDataForEpickOfCarton())
           .And(x => x.CartonNumberForEPick())
           .And(x => x.ValidOrmtUrl())
           .When(x => x.OrmtApiIsCalledCreatedIsReturned())
           .And(x => x.ReadDataAfterApiForEPickOfCarton())
           .Then(x => x.VerifyOrmtMessageWasInsertedInToSwmToMheForEpick())
           .And(x => x.VerifyOrmtMessageWasInsertedInToWmsToEmsForEpickOfOrder())
           .And(x => x.VerifyForOrmtCountInPickLocnDtlExt())
           .And(x => x.VerifyForStatusCodeInCartonHdrForEPick())
           .BDDfy();
        }
    }
}
