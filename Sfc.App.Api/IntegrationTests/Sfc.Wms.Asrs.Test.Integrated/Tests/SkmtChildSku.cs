﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures;
using TestStack.BDDfy;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Tests
{
    [TestClass]
    [Story(
       AsA = "Authorized User test for skmt message from wmstoems",
      IWant = "To Verify SKMT message is inserted in to swm_to_mhe table with appropriate data" +
       "And verify SKMT message is inserted in to wmstoems table with appropriate data ",
      SoThat = "I can validate for message fields in SKMT message, in Internal Table SWM_TO_MHE and in wmstoems"
       )]
    public class SkmtChildSku : SkmtMessageFixture
    {
        [TestInitialize]
        public void AValidTestData()
        {
            InitializeTestDataChild();
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void SkmtMessageTestFor1ChildSkuAddScenarios()
        {
            this.Given(x => x.CurrentSkuIdForChildSkuItemmaster())
                .And(x => x.CurrentActioncodeAdd())
                .And(x => x.ValidSkmtUrl())
               .When(x => x.SkmtApiIsCalledCreatedIsReturned())
                .And(x => x.GetValidDataAfterTrigger())
              .And(x => x.VerifySkmtMessageWasInsertedForIntoSwmToMhe("Add", skmt.ActionCode))
              .And(x => x.VerifySkmtMessageWasInsertedIntoWmsToEms())
              .And(x => x.VerifySkmtMessageWasChildSku())

            .BDDfy();

        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void SkmtMessageTestFor2ChildSkuUpdateScenarios()
        {
            this.Given(x => x.CurrentSkuIdForChildSkuItemmaster())
               .And(x => x.CurrentActioncodeUpdate())
               .And(x => x.ValidSkmtUrl())
              .When(x => x.SkmtApiIsCalledCreatedIsReturned())
               .And(x => x.GetValidDataAfterTrigger())
              .And(x => x.VerifySkmtMessageWasInsertedForIntoSwmToMhe("Update", skmt.ActionCode))
              .And(x => x.VerifySkmtMessageWasInsertedIntoWmsToEms())
              .And(x => x.VerifySkmtMessageWasChildSku())
              .BDDfy();

        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void SkmtMessageTestFor3ChildSkuDeleteScenarios()
        {
            this.Given(x => x.CurrentSkuIdForChildSkuItemmaster())
               .And(x => x.CurrentActioncodeDelete())
               .And(x => x.ValidSkmtUrl())
              .When(x => x.SkmtApiIsCalledCreatedIsReturned())
               .And(x => x.GetValidDataAfterTrigger())
              .And(x => x.VerifySkmtMessageWasInsertedForIntoSwmToMhe("Delete", skmt.ActionCode))
              .And(x => x.VerifySkmtMessageWasInsertedIntoWmsToEms())
              .And(x => x.VerifySkmtMessageWasChildSku())
            .BDDfy();

        }


    }
}