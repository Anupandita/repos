using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures;
using TestStack.BDDfy;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Tests
{
    [TestClass]
    [Story(
        AsA = "Authorized User test for cost message from emstowms",
       IWant = "To Verify COST message is inserted in to SWM_FROM_MHE table with appropriate data" +
        " And verify COST message is inserted in to swm_from_mhe table with appropriate data " +
        " Verify in TransInventory table for Allocation Inventory units and actual weight" +
        " Verify in Case Detail table for Quantity, CaseHeader and task detail table for status code ",
       SoThat = "I can validate for message fields in COST message, in Internal Table SWM_FROM_MHE" +
        " and validate the quantity,weight,statuscode in the caseheader, casedetail, task header tables"
        )]
    public class CostMessageTest : CostMessageFixture
    {        
        [TestInitialize]
        public void TestData()
        {
            TestInitialize();           
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void VerifyForValidCostMessageScenarios()
        {
            this.Given(x => x.TestInitializeForValidMessage())
                .And(x => x.AValidMsgKey())
                .When(x => x.CostApiIsCalledWithValidMsgKey())
                .And(x => x.GetValidDataAfterTrigger())
                .And(x => x.VerifyCostMessageWasInsertedIntoSwmFromMhe())
                .And(x => x.VerifyTheQuantityWasDecreasedInToTransInventory())
                .And(x => x.VerifyTheQuantityWasIncreasedIntoPickLocationTable())
                .BDDfy();              
        }
        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void VerifyForInvalidMessageKey()
        {
            this.Given(x =>x.TestInitializeForInvalidCase())
                .And(x => x.InvalidMsgKey())
                .When(x => x.CostApiIsCalledForInvalidMessageKey())
                .Then(x => x.ValidateResultForInvalidMessageKey())
                .BDDfy();
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void VerifyForErrorLogNoCaseFound()
        {
                this.Given(x => x.TestInitializeForInvalidCase())
               .And(x => x.InvalidCaseMsgKey())         
               .When(x => x.CostApiIsCalledForInvalidCaseNumber())
               .Then(x => x.ValidateResultForInvalidCaseNumber())
               .BDDfy();
        }


        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void VerifyForErrorNotEnoughInventory()
        {
                this.Given(x => x.TestInitializeForTransInvnDoesNotExist())
               .And(x => x.TransInvnNotExistsMsgKey())
               .When(x => x.CostApiIsCalledForTransInvnNotFound())
               .Then(x => x.ValidateResultForTransInventoryNotExist())
               .BDDfy();
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void VerifyForErrorPickLocationNotFound()
        {
                this.Given(x => x.TestInitializeForPickLocnDoesNotExist())
               .And(x => x.PickLocationNotExistKey())
               .When(x => x.CostApiIsCalledForPickLocnNotFound())
               .Then(x => x.ValidateResultForPickLocnNotFound())
               .BDDfy();
        }       
    }
}
