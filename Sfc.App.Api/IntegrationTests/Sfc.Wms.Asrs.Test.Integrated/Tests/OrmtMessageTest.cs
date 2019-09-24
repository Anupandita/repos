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
        [TestMethod]
        [TestInitialize]
        public void TestData()
        {
             InitializeTestData();
        }
        
        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void VerifyForOrmtMessageWithActionCodeAddRelease()
        {
            this.Given(x => x.CartonNumberForAddRelease())
          //  .And(x => x.AValidNewOrmtMessageRecord())
            .When(x => x.OrmtApiIsCalledCreatedIsReturned())
            .And(x => x.ReadDataAfterApiForPrintingOfCarton())
            .Then(x => x.VerifyOrmtMessageWasInsertedInToSwmToMhe())
            .And(x => x.VerifyOrmtMessageWasInsertedInToWmsToEms())
            .And(x => x.VerifyForOrmtCountInPickLocnDtlExt())
            .And(x=>x.VerifyForStatusCodeinCartonHdrForAddRelease())
           .BDDfy();
        }

        //[TestMethod()]
        //[TestCategory("FUNCTIONAL")]
        //public void VerifyForOrmtMessageWithActionCodeCancel()
        //{
        //    this.Given(x => x.CartonNumberForCancel())
        //   .And(x=>x.AValidNewOrmtMessageRecord())
        //   //.When(x => x.OrmtApiIsCalledCreatedIsReturned())
        //   //.Then(x => x.VerifyOrmtMessageWasInsertedInToSwmToMheForCancelOrders())
        //   //.And(x=>x.VerifyOrmtMessageWasInsertedInToWmsToEms())
        //   .BDDfy();
        //}

        //[TestMethod()]
        //[TestCategory("FUNCTIONAL")]
        //public void VerifyForOrmtMessageWithActionCodeEPick()
        //{
        //    this.Given(x => x.CartonNumberForEPick())
        //   .And(x=>x.AValidNewOrmtMessageRecord())
        //   //.When(x => x.OrmtApiIsCalledCreatedIsReturned())
        //   //.Then(x => x.VerifyOrmtMessageWasInsertedInToSwmToMheForEpick())
        //   //.And(x => x.VerifyOrmtMessageWasInsertedInToWmsToEms())
        //   .BDDfy();
        //}


    }
}
