using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures;
using Sfc.Wms.Interfaces.ParserAndTranslator.Contracts.Constants;
using TestStack.BDDfy;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Tests
{
    [TestClass]
    [Story(
      AsA = "Authorized User test for ormt message from wmstoems",
      IWant = "To Test for Ormt messages on printing of carton,On Cancellation of carton,OnEpick and On Processing COST message" +
        "To Verify ORMT message is inserted in to swm_to_mhe table with appropriate data" +
       " Verify in PickLocnDtlExt table for Ormt Count and status in SWM_ELGBL_ORMT_CARTONS table",
      SoThat = "I can validate for message fields in ORMT message, in Internal Table SWM_TO_MHE and in wmstoems",
      StoryUri = "http://tfsapp1:8080/tfs/ShamrockCollection/Portfolio-SOWL/_workitems?id=129455&_a=edit"
       )]
    public class OrmtTestWhenTriggerIsOn : OrmtMessageFixture
    {
        //[TestMethod()]
        //[TestCategory("FUNCTIONAL")]
        //[DataRow(1)]
        //public void AOrmtTest1VerifyForOrmtMessageWithActionCodeAddRelease(int count)
        //{
        ////    this.Given(x => x.InitializeTestDataForPrintingOfCartons())
        ////    .And(x => x.ValidOrmtUrlCartonNumberAndActioncodeIs(Url, PrintCarton.CartonNbr, OrmtActionCode.AddRelease)) 
        ////    .And(x => x.ReadDataAfterApiForPrintingOfCarton())
        ////    .Then(x => x.VerifyOrmtMessageWasInsertedInToSwmToMhe())
        ////    .And(x => x.VerifyOrmtMessageWasInsertedInToWmsToEmsForPrintingOfOrder())
        ////    .And(x => x.VerifyForOrmtCountInPickLocnDtlExt())
        ////    .And(x => x.VerifyForStatusCodeinCartonHdrForAddRelease())
        ////    .And(x => x.VerifyForStatusInSwmEligibleOrmtCartons())
        ////    .BDDfy("Test Case Id:132756 -Dematic - ORMT - AddRelease - Call the ormt api and validate for all functionalities in  wmstoems,swm_to_mhe,carton_hdr,pick_locn_dtl_ext  tables");
        ////}
        //[TestMethod()]
        //[TestCategory("FUNCTIONAL")]
        //public void BOrmtTest2VerifyForOrmtMessageWithActionCodeCancel()
        //{
        //    this.Given(x => x.InitializeTestDataForCancelOfCarton())
        //   .And(x => x.ValidOrmtUrlCartonNumberAndActioncodeIs(OrmtUrl, CancelOrder.CartonNbr, OrmtActionCode.Cancel))          
        //   .And(x => x.ReadDataAfterApiForCancelOfCarton())
        //   .Then(x => x.VerifyOrmtMessageWasInsertedInToSwmToMheForCancelOrders())
        //   .And(x => x.VerifyOrmtMessageWasInsertedInToWmsToEmsForCancelOrder())
        //   .And(x => x.VerifyForOrmtCountInPickLocnDtlExt())
        //   .BDDfy("Test Case Id:132757 -Dematic - ORMT - Cancel - Call the ormt api and validate for all functionalities in  wmstoems,swm_to_mhe,carton_hdr,pick_locn_dtl_ext  tables");
        //}

        //[TestMethod()]
        //[TestCategory("FUNCTIONAL")]
        //public void COrmtTest3VerifyForOrmtMessageWithActionCodeEPick()
        //{
        //    this.Given(x => x.InitializeTestDataForEpickOfCarton())
        //   .And(x => x.ValidOrmtUrlCartonNumberAndActioncodeIs(OrmtUrl, EPick.CartonNbr, OrmtActionCode.AddRelease))        
        //   .And(x => x.ReadDataAfterApiForEPickOfCarton())
        //   .Then(x => x.VerifyOrmtMessageWasInsertedInToSwmToMheForEpick())
        //   .And(x => x.VerifyOrmtMessageWasInsertedInToWmsToEmsForEpickOfOrder())
        //   .And(x => x.VerifyForOrmtCountInPickLocnDtlExt())
        //   .And(x => x.VerifyForStatusCodeInCartonHdrForEPick())
        //   .BDDfy("Test Case Id:132758 -Dematic - ORMT - EPick -Call the ormt api and validate for all functionalities in  wmstoems,swm_to_mhe,carton_hdr,pick_locn_dtl_ext  tables");
        //}

    }
}
