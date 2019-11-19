using Microsoft.VisualStudio.TestTools.UnitTesting;
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
                 " and validate the quantity,weight,statuscode in the caseheader, casedetail, task header tables"
    )]
    public class ComtAndIvmtTestRecievedCase
    {
        [TestInitialize]
        public void AValidTestData()
        {
            //InitializeTestData();
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void RecievedCasesComtAndIvmtMessageTestScenarios()
        {
            //this.Given(x => x.CurrentCaseNumberForSingleSku())
             
            //    .BDDfy();
        }
    }
}
