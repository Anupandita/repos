using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures;
using TestStack.BDDfy;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Tests
{
    [TestClass]
    [Story(
          AsA = "Authorized User test for comt message from wms to ems",
          IWant = "To Verify COMT message is inserted in to SWMTOMHE table with appropriate data" +
           " Verify in TransInventory table for Allocation Inventory units and actual weight" +
           " Verify in Case Detail table for Quantity, CaseHeader and task detail table for status code ",
          SoThat = "I can validate for message fields in COMT message, in Internal Table SWM_TO_MHE" +
           " and validate the quantity,weight,statuscode in the caseheader, casedetail, task header tables",
          StoryUri = "http://tfsapp1:8080/tfs/ShamrockCollection/Portfolio-SOWL/_workitems?id=109612&_a=edit"
          )]

    public class ComtAndIvmtTestWhenTriggerIsOn : ComtIvmtMessageFixture
    {
       
        //[TestMethod()]
        //[TestCategory("FUNCTIONAL")]

        //public void TestForReceivedCasesFromVendorsWhenTriggerIsOn()
        //{
        //    this.Given(x => x.InitializeTestDataForComtWhenTriggerIsOn())
        //        .And(x => x.AValidNewComtMessageRecordWhereCaseNumberAndSkuIs(SingleSkuCase.CaseNumber, SingleSkuCase.SkuId))
        //        .Then(x => x.GetDataFromDataBaseForSingleSkuScenarios())
        //        .And(x => x.VerifyComtMessageWasInsertedIntoSwmToMhe())
        //        .And(x => x.VerifyComtMessageWasInsertedIntoWmsToEms())
        //        .And(x => x.VerifyIvmtMessageWasInsertedIntoSwmToMhe(SingleSkuCase.TotalAllocQty))
        //        .And(x => x.VerifyIvmtMessageWasInsertedIntoWmsToEms())
        //        .And(x => x.VerifyTheQuantityIsIncreasedInToTransInventory())
        //        .And(x => x.VerifyQuantityisReducedIntoCaseDetail())
        //        .And(x => x.VerifyStatusIsUpdatedIntoCaseHeader())
        //        .And(x => x.VerifyStatusIsUpdatedIntoTaskHeader())
        //        .BDDfy("Test Case ID : 120959 -Dematic - COMT,IVMT : Single Sku Case-Case received from Vendor, Call the Comt Api and Verify all its functionalities");
        //}

        //[TestMethod()]
        //[TestCategory("FUNCTIONAL")]
        //public void TestForReceivedCasesFromReturnsWhenTriggerIsOn()
        //{
        //    this.Given(x =>x.InitializeTestDataForStatCode30WhenTriggerIsOn())
        //        .And(x =>x.AValidNewComtMessageRecordWhereCaseNumberAndSkuIs(SingleSkuCase.CaseNumber, SingleSkuCase.SkuId))
        //        .Then(x => x.GetDataFromDataBaseForSingleSkuScenarios())             
        //        .And(x => x.VerifyIvmtMessageWasInsertedIntoSwmToMhe(SingleSkuCase.ActlQty))
        //        .And(x => x.VerifyIvmtMessageWasInsertedIntoWmsToEms())
        //        .And(x => x.VerifyTheQuantityIsIncreasedInToTransInventory())
        //        .And(x => x.VerifyQuantityisReducedIntoCaseDetail())
        //        .And(x => x.VerifyStatusIsUpdatedIntoCaseHeader())
        //        .And(x => x.VerifyStatusIsUpdatedIntoTaskHeader())
        //        .BDDfy("Test Case ID : 120959 -Dematic - COMT,IVMT : Single Sku Case-Case received from Vendor, Call the Comt Api and Verify all its functionalities");
        //}
    }
}
