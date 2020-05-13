using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures;
using TestStack.BDDfy;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Tests
{
    [TestClass]
    [Story(
       AsA = "Authorized User test for comt message from wms to ems",
       IWant = "To Verify COMT message is inserted in to SWMTOMHE table with appropriate data" +
        " Verify in TransInventory table for Allocation Inventory units and actual weight"+
        " Verify in Case Detail table for Quantity, CaseHeader and task detail table for status code ",
       SoThat = "I can validate for message fields in COMT message, in Internal Table SWM_TO_MHE"+
        " and validate the quantity,weight,statuscode in the caseheader, casedetail, task header tables",
       StoryUri = "http://tfsapp1:8080/tfs/ShamrockCollection/Portfolio-SOWL/_workitems?id=109612&_a=edit"
       )]
    public class ComtAndIvmtTest : ComtIvmtMessageFixture
    {     
        [TestInitialize]
        protected void AValidTestData()
        {
            InitializeTestData();           
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]

        protected void ComtAndIvmtMessageTestScenarios() 
        {
            this.Given(x => x.AValidNewComtMessageRecordWhereCaseNumberAndSkuIs(SingleSkuCase.CaseNumber, SingleSkuCase.SkuId))
                .When(x => x.ComtApiIsCalledCreatedIsReturnedWithValidUrlIs(ComtUrl))
                .Then(x => x.GetDataFromDataBaseForSingleSkuScenarios())
                .And(x => x.VerifyComtMessageWasInsertedIntoSwmToMhe())
                .And(x => x.VerifyComtMessageWasInsertedIntoWmsToEms())
                .And(x => x.VerifyIvmtMessageWasInsertedIntoSwmToMhe(SingleSkuCase.TotalAllocQty))
                .And(x => x.VerifyIvmtMessageWasInsertedIntoWmsToEms())
                .And(x => x.VerifyTheQuantityIsIncreasedInToTransInventory())
                .And(x => x.VerifyQuantityisReducedIntoCaseDetail())
                .And(x => x.VerifyStatusIsUpdatedIntoCaseHeader())
                .And(x => x.VerifyStatusIsUpdatedIntoTaskHeader())   
                .And(x=>x.VerifyWeightAndVolumeIsReducedInResrvLocnHdrTable())
                .BDDfy("Test Case ID : 120959 -Dematic - COMT,IVMT : Single Sku Case-Case received from Vendor, Call the Comt Api and Verify all its functionalities");
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        protected void ComtAndIvmtTestForMultiSkuScenarios()
        {
            this.Given(x => x.AValidNewComtMessageRecordWhereCaseNumberAndSkuIs(CaseHdrMultiSku.CaseNumber, CaseHdrMultiSku.SingleSkuId))
              .When(x => x.ComtApiIsCalledCreatedIsReturnedWithValidUrlIs(ComtUrl))
              .Then(x => x.GetDataAndValidateForIvmtMessageHasInsertedIntoBothTables())
              .And(x => x.VerifyComtMessageWasInsertedIntoSwmToMheForMultiSku())
              .And(x => x.VerifyComtMessageWasInsertedIntoWmsToEmsForMultiSku())
              .And(x => x.VerifyQuantityisReducedIntoCaseDetailTable())
              .And(x => x.VerifyStatusIsUpdatedIntoCaseHeaderTable())
              .BDDfy("Test Case ID : 133225 -Dematic - COMT,IVMT : Multi Sku Case-Case received from Vendor ,Call the comt api Verify all its functionalities");
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        protected void ComtIvmtMessageTestCaseForNoEnoughInventoryInCase()
        {
            this.Given(x => x.AValidNewComtMessageRecordWhereCaseNumberAndSkuIs(NotEnoughInvCase.CaseNumber, null))
                .When(x => x.ComtApiIsCalledForNotEnoughInventoryInCaseAndUrlIs(ComtUrl))
                .Then(x => x.ValidateForNotEnoughInventoryInCase())  
                .BDDfy("Test Case ID: 133226 -Dematic - COMT,IVMT : Validate for NotEnough Inventory in Case and validate for error messages");
        }

    }
}
 