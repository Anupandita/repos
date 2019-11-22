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
               .And(x => x.VerifyTheQuantityIsIncreasedToTransInventory())
               .And(x => x.VerifyQuantityisReducedIntoCaseDetail())
               .And(x => x.VerifyStatusIsUpdatedIntoCaseHeader())
               .And(x => x.VerifyStatusIsUpdatedIntoTaskHeader())
               .BDDfy();
        }

    }
}
