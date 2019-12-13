using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures;
using TestStack.BDDfy;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Tests
{
    [TestClass]
    [Story(
     AsA = "Authorized User test for ivmt message from wms to ems",
     IWant = "To Verify IVMT message is inserted in to SWMTOMHE table with appropriate data" +
      " And verify IVMT message is inserted in to WMSTOEMS table with appropriate data " +
      " Verify in TransInventory table for Allocation Inventory units and actual weight" +
      " Verify in Case Detail table for Quantity, CaseHeader and task detail table for status code ",
     SoThat = "I can validate for message fields in IVMT message, in Internal Table SWM_TO_MHE" +
      " and validate the quantity,weight,statuscode in the caseheader, casedetail, task header tables"
     StoryUri = "http://tfsapp1:8080/tfs/ShamrockCollection/Portfolio-SOWL/WMS%20UI%20Renovate/_testManagement?planId=105523&suiteId=119426&_a=tests"
     )]
    public class Ivmt:ComtIvmtMessageFixture
    {
        [TestInitialize]
        public void AValidTestData()
        {
            InitializeTestData();
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void IvmtMessageTestScenarios()
        {
            this.Given(x => x.CurrentCaseNumberForSingleSku())
               .And(x => x.AValidNewIvmtMessageRecord())
               .When(x => x.IvmtApiIsCalledCreatedIsReturned())
               .Then(x => x.GetDataFromDataBaseForSingleSkuScenariosIvmt())
               .And(x => x.VerifyIvmtMessageWasInsertedIntoSwmToMhe())
               .And(x => x.VerifyIvmtMessageWasInsertedIntoWmsToEms())
               .And(x => x.VerifyTheQuantityIsIncreasedInToTransInventory())
               .And(x => x.VerifyQuantityisReducedIntoCaseDetail())
               .And(x => x.VerifyStatusIsUpdatedIntoCaseHeader())
               .And(x => x.VerifyStatusIsUpdatedIntoTaskHeader())
               .BDDfy("Test Case ID: 135041-Dematic - IVMT : Single Sku Case , Call the Ivmt Api and Verify all its functionalities in CaseHeader,CaseDetail,TransInventory,TaskHeader Tables.");
        }

    }
}
