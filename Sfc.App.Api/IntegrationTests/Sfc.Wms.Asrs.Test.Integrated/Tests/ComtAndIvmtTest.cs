using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures;
using TestStack.BDDfy;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Tests
{
    [TestClass]
    [Story(
       AsA = "Authorized User test for comt and ivmt message from wms to ems",
       IWant = "To Verify COMT and IVMT message is inserted in to SWMTOMHE table with appropriate data" +
        " And verify IVMT message is inserted in to WMSTOEMS table with appropriate data " +
        " Verify in TransInventory table for Allocation Inventory units and actual weight"+
        " Verify in Case Detail table for Quantity, CaseHeader and task detail table for status code ",
       SoThat = "I can validate for message fields in COMT and IVMT message, in Internal Table SWM_TO_MHE"+
        " and validate the quantity,weight,statuscode in the caseheader, casedetail, task header tables"
       )]
    public class ComtAndIvmtTest : ComtIvmtMessageFixture
    {     
        [TestInitialize]
        public void AValidTestData()
        {
            InitializeTestData();           
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void ComtAndIvmtMessageTestScenarios() 
        {
            this.Given(x => x.CurrentCaseNumberForSingleSku())
                .And(x => x.AValidNewComtMessageRecord())
                .When(x => x.ComtApiIsCalledCreatedIsReturned())
                .Then(x => x.GetDataFromDataBaseForSingleSkuScenarios())
                .And(x => x.VerifyComtMessageWasInsertedIntoSwmToMhe())
                .And(x => x.VerifyComtMessageWasInsertedIntoWmsToEms())
                .And(x => x.VerifyIvmtMessageWasInsertedIntoSwmToMhe())
                .And(x => x.VerifyIvmtMessageWasInsertedIntoWmsToEms())
                .And(x => x.VerifyTheQuantityIsIncreasedToTransInventory())
                .And(x => x.VerifyQuantityisReducedIntoCaseDetail())
                .And(x => x.VerifyStatusIsUpdatedIntoCaseHeader())
                .And(x => x.VerifyStatusIsUpdatedIntoTaskHeader())
                .BDDfy();
        }

        [TestMethod]
        [TestCategory("FUNCTIONAL")]
        public void ComtAndIvmtTestForMultiSkuScenarios()
        {
            this.Given(x => x.CurrentCaseNumberForMultiSku())
                .And(x => x.AValidNewComtMessageRecord())
                .When(x => x.ComtApiIsCalledCreatedIsReturned())
                .Then(x => x.GetDataAndValidateForIvmtMessageHasInsertedIntoBothTables())
                .And(x => x.VerifyComtMessageWasInsertedIntoSwmToMheForMultiSku())
                .And(x => x.VerifyComtMessageWasInsertedIntoWmsToEmsForMultiSku())
                .And(x => x.VerifyQuantityisReducedIntoCaseDetailTable())
                .And(x => x.VerifyStatusIsUpdatedIntoCaseHeaderTable())
                .BDDfy();
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void ComtIvmtMessageTestCaseForNoEnoughInventoryInCase()
        {
            this.Given(x => x.CurrentCaseNumberForNotEnoughInventoryInCase())
                .And(x => x.AValidNewComtMessageRecord())
                .When(x => x.ComtApiIsCalledForNotEnoughInventoryInCase())
                .Then(x => x.ValidateForNotEnoughInventoryInCase())  
                .BDDfy();
        }

    }
}
 