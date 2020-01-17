using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures;
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
                 " and validate the quantity,weight,statuscode in the caseheader, casedetail, task header tables",
        StoryUri = "http://tfsapp1:8080/tfs/ShamrockCollection/Portfolio-SOWL/_workitems?id=109612&_a=edit"
    )]
    public class QueueManagementTest: QueueManagementFixture
    {
        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void test()
        {
            this.Given(x => x.GetValidCartonAndWaveNumberFromSwmEligibleOrmtCarton())
                .BDDfy();
        }
    }
}
