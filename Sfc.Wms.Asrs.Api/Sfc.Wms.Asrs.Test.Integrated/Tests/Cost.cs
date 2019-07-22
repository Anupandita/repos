﻿using System;
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
            NegativeCases();
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void ComtAndIvmtTest()
        {
            this.Given(x => x.SetCurrentMsgKey())
                .And(x => x.AValidNewCostMessageRecord())                
                .When(x => x.CostApiIsCalled())
                .Then(x => x.ResultTypeCreatedIsReturned())
                .Then(x => x.GetValidDataAfterTrigger())
                .And(x => x.VerifyCostMessageWasInsertedIntoSwmFromMhe())
                .And(x => x.VerifyTheQuantityIsDecreasedInToTransInventory())
                .And(x => x.VerifyTheQuantityIsIncreasedIntoPickLocationTable())
                .BDDfy();              
        }
        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void TestForInvalidMessageKey()
        {
            this.Given(x => x.SetInvalidMsgKey())
                .And(x => x.AValidNewCostMessageRecord())
                .When(x => x.CostApiIsCalled())
                .Then(x => x.ResultForInvalidMessageKey())
                .BDDfy();
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void TestForErrorLogNoCaseFound()
        {
            this.Given(x => x.SetForInvalidCaseMsgKey())
           .And(x => x.AValidNewCostMessageRecord())
           .When(x => x.CostApiIsCalled())
           .Then(x => x.ResultForInvalidCaseNumber())
           .BDDfy();
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void VerifyForErrorLogInvalidCaseStatus()
        {
            this.Given(x => x.SetForInvalidCaseStatusMsgKey())
           .And(x => x.AValidNewCostMessageRecord())
           .When(x => x.CostApiIsCalled())
           .Then(x => x.ResultForInvalidCaseStatus())
           .BDDfy();
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void VerifyForErrorNotEnoughInv()
        {
            this.Given(x => x.SetForTransInvnNotExistsMsgKey())
           .And(x => x.AValidNewCostMessageRecord())
           .When(x => x.CostApiIsCalled())
           .Then(x => x.ResultForTransInventoryNotExist())
           .BDDfy();
        }
    }
}
