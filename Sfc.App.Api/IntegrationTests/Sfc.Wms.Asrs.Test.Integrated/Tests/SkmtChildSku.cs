using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures;
using Sfc.Wms.Interfaces.ParserAndTranslator.Contracts.Constants;
using TestStack.BDDfy;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Tests
{
    [TestClass]
    [Story(
       AsA = "Authorized User test for skmt message from wmstoems",
      IWant = "To Verify SKMT message is inserted in to swm_to_mhe table with appropriate data" +
       "And verify SKMT message is inserted in to wmstoems table with appropriate data ",
      SoThat = "I can validate for message fields in SKMT message, in Internal Table SWM_TO_MHE and in wmstoems",
      StoryUri = "http://tfsapp1:8080/tfs/ShamrockCollection/Portfolio-SOWL/_workitems?id=128771&_a=edit"
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
            this.Given(x => x.ValidSkuActioncodeAndSkmtUrlIs(ChildSku.SkuId, SkmtActionCode.Add,SkmtUrl))
                .When(x => x.SkmtApiIsCalledCreatedIsReturned())
                .And(x => x.GetValidDataAfterTrigger())
                .And(x => x.VerifySkmtMessageWasInsertedIntoSwmToMheForActionCode("Add", Skmt.ActionCode))
                .And(x => x.VerifySkmtMessageWasInsertedIntoWmsToEms())
                .And(x => x.VerifyForSkmtMessageSentTheSkuidWasChildSku())
                .BDDfy("Test Case Id:138475-Dematic - SKMT - add - Call the SKMT api and validate for all functionalities when Child sku");
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void SkmtMessageTestFor2ChildSkuUpdateScenarios()
        {
            this.Given(x => x.ValidSkuActioncodeAndSkmtUrlIs(ChildSku.SkuId, SkmtActionCode.Update, SkmtUrl))
                .When(x => x.SkmtApiIsCalledCreatedIsReturned())
                .And(x => x.GetValidDataAfterTrigger())
                .And(x => x.VerifySkmtMessageWasInsertedIntoSwmToMheForActionCode("Update", Skmt.ActionCode))
                .And(x => x.VerifySkmtMessageWasInsertedIntoWmsToEms())
                .And(x => x.VerifyForSkmtMessageSentTheSkuidWasChildSku())
                .BDDfy("Test Case Id:138476-Dematic - SKMT - Update - Call the SKMT api and validate for all functionalities when Child sku");
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void SkmtMessageTestFor3ChildSkuDeleteScenarios()
        {
            this.Given(x => x.ValidSkuActioncodeAndSkmtUrlIs(ChildSku.SkuId, SkmtActionCode.Delete, SkmtUrl))
                .When(x => x.SkmtApiIsCalledCreatedIsReturned())
                .And(x => x.GetValidDataAfterTrigger())
                .And(x => x.VerifySkmtMessageWasInsertedIntoSwmToMheForActionCode("Delete", Skmt.ActionCode))
                .And(x => x.VerifySkmtMessageWasInsertedIntoWmsToEms())
                .And(x => x.VerifyForSkmtMessageSentTheSkuidWasChildSku())
                .BDDfy("Test Case Id:138477-Dematic - SKMT - Delete - Call the SKMT api and validate for all functionalities when Child sku");
        }
    }
}
