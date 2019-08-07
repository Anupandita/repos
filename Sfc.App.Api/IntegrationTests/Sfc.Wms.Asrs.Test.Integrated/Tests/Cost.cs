using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestStack.BDDfy;
using Sfc.Wms.Asrs.Test.Integrated.Fixtures;

namespace Sfc.Wms.Asrs.Test.Integrated.Tests
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
    public class Cost : CostMessageFixture
    {        
        [TestInitialize]
        [TestCategory("FUNCTIONAL")]
        public void AValidTestData()
        {
            GetValidDataBeforeTrigger();           
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void VerifyForValidCostMessageScenarios()
        {
            this.Given(x => x.SetCurrentMsgKey())
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
            this.Given(x => x.SetInvalidMsgKey())
                .When(x => x.CostApiIsCalled())
                .Then(x => x.ValidateResultForInvalidMessageKey())
                .BDDfy();
        }

        //[TestMethod()]
        //[TestCategory("FUNCTIONAL")]
        //public void VerifyForErrorLogNoCaseFound()
        //{
        //    this.Given(x => x.SetForInvalidCaseMsgKey())
        //   .And(x => x.AValidNewCostMessageRecord())
        //   .When(x => x.CostApiIsCalled())
        //   .Then(x => x.ValidateResultForInvalidCaseNumber())
        //   .BDDfy();
        //}

        //[TestMethod()]
        //[TestCategory("FUNCTIONAL")]
        //public void VerifyForErrorLogInvalidCaseStatus()
        //{
        //    this.Given(x => x.SetForInvalidCaseStatusMsgKey())
        //   .And(x => x.AValidNewCostMessageRecord())
        //   .When(x => x.CostApiIsCalled())
        //   .Then(x => x.ValidateResultForInvalidCaseStatus())
        //   .BDDfy();
        //}

        //[TestMethod()]
        //[TestCategory("FUNCTIONAL")]
        //public void VerifyForErrorNotEnoughInv()
        //{
        //    this.Given(x => x.SetForTransInvnNotExistsMsgKey())
        //   .And(x => x.AValidNewCostMessageRecord())
        //   .When(x => x.CostApiIsCalled())
        //   .Then(x => x.ValidateResultForTransInventoryNotExist())
        //   .BDDfy();
        //}
    }
}
