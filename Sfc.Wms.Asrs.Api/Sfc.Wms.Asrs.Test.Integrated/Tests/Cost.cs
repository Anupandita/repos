using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestStack.BDDfy;
using Sfc.Wms.Asrs.Test.Integrated.Fixtures;

namespace Sfc.Wms.Asrs.Test.Integrated.Tests
{
    [TestClass]
    [Story(
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
                .And(x => x.AValidNewCostMessageRecord())                
                .When(x => x.CostApiIsCalled())
                .Then(x => x.ResultTypeCreatedIsReturned())
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
                .And(x => x.AValidNewCostMessageRecord())
                .When(x => x.CostApiIsCalled())
                .Then(x => x.ValidateResultForInvalidMessageKey())
                .BDDfy();
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void VerifyForErrorLogNoCaseFound()
        {
            this.Given(x => x.SetForInvalidCaseMsgKey())
           .And(x => x.AValidNewCostMessageRecord())
           .When(x => x.CostApiIsCalled())
           .Then(x => x.ValidateResultForInvalidCaseNumber())
           .BDDfy();
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void VerifyForErrorLogInvalidCaseStatus()
        {
            this.Given(x => x.SetForInvalidCaseStatusMsgKey())
           .And(x => x.AValidNewCostMessageRecord())
           .When(x => x.CostApiIsCalled())
           .Then(x => x.ValidateResultForInvalidCaseStatus())
           .BDDfy();
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void VerifyForErrorNotEnoughInv()
        {
            this.Given(x => x.SetForTransInvnNotExistsMsgKey())
           .And(x => x.AValidNewCostMessageRecord())
           .When(x => x.CostApiIsCalled())
           .Then(x => x.ValidateResultForTransInventoryNotExist())
           .BDDfy();
        }
    }
}
