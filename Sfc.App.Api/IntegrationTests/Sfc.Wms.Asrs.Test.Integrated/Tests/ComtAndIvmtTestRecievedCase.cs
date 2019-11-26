using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestStack.BDDfy;
using Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Tests
{
    [TestClass]
    [Story(
        AsA = "Authorized User test for comt message from wms to ems",
        IWant = "To Verify COMT message is inserted in to SWMTOMHE table with appropriate data" +
                " Verify in TransInventory table for Allocation Inventory units and actual weight" +
                " Verify in Case Detail table for Quantity, CaseHeader and task detail table for status code ",
        SoThat = "I can validate for message fields in COMT message, in Internal Table SWM_TO_MHE" +
                 " and validate the quantity,weight,statuscode in the caseheader, casedetail, task header tables"
    )]
    public class ComtAndIvmtTestRecievedCase:ComtIvmtMessageFixture
    {
        [TestInitialize]
        public void AValidTestData()
        {
            InitializeRecievedCaseTestData();
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void ComtAndIvmtMessageForRecievedCasesTestScenarios()
        {
            this.Given(x => x.CurrentCaseNumberForRecivedCasesfromReturn())
                .And(x => x.AValidNewRecivedCaseComtMessageRecord())
                .When(x => x.ComtApiIsCalledCreatedIsReturned())
                .Then(x => x.GetDataFromDataBaseForRecivedCaseSingleSkuScenarios())
                .And(x => x.VerifyRecivedCaseComtMessageWasInsertedIntoSwmToMhe())
                .And(x => x.VerifyComtMessageWasInsertedIntoWmsToEms())
                .And(x => x.VerifyIvmtMessageWasInsertedIntoSwmToMhe())
                .And(x => x.VerifyIvmtMessageWasInsertedIntoWmsToEms())
                .And(x => x.VerifyTheQuantityIsIncreasedToTransInventory())
                .And(x => x.VerifyQuantityisReducedIntoCaseDetail())
                .And(x =>x.VerifyTheQuantityCasedetail())
                .And(x => x.VerifyStatusIsUpdatedIntoCaseHeader())
                .And(x => x.VerifyStatusIsUpdatedIntoTaskHeader())

                .BDDfy();
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void ComtAndIvmtTestForReciveCaseMultiSkuScenarios()
        {
            this.Given(x => x.CurrentRecivedCaseNumberForMultisku())
                .And(x => x.AValidNewRecivedCaseComtMessageRecord())
                .When(x => x.ComtApiIsCalledCreatedIsReturned())
                .Then(x => x.GetDataAndValidateForCaseRecievedIvmtMessageInsertedIntoBothTable())
                .And(x => x.VerifyComtMessageWasInsertedIntoSwmToMheForRecivedMultiSku())
                .And(x => x.VerifyComtMessageWasInsertedIntoWmsToEmsForMultiSku())
                .And(x => x.VerifyQuantityisReducedIntoCaseDetailTable())
                .And(x => x.VerifyStatusIsUpdatedIntoCaseHeaderTable())
                .BDDfy();
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void ComtIvmtMessageTestCaseForNoEnoughInventoryInCase()
        {
            this.Given(x => x.CurrentCaseNumberForNotEnoughInventoryInCaseForRecivedCase())
                .And(x => x.AValidNewRecivedCaseComtMessageRecord())
                .When(x => x.ComtApiIsCalledForNotEnoughInventoryInCase())
                .Then(x => x.ValidateForNotEnoughInventoryInCase())
                .BDDfy();
        }


    }
}
