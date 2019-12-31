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
    public class SkmtParentSku:SkmtMessageFixture
    {
        [TestInitialize]
        public void AValidTestData()
        {
            InitializeTestDataParent();
        }


        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void SkmtMessageTest1ForParentSkuAddScenarios()
        {
            this.Given(x => x.ValidSkuActioncodeAndSkmtUrlIs(ParentSku.SkuId, SkmtActionCode.Add,SkmtUrl))
                .When(x => x.SkmtApiIsCalledCreatedIsReturned())
                .And(x => x.GetValidDataAfterTrigger())
                .And(x => x.VerifySkmtMessageWasInsertedIntoSwmToMheForActionCode("Add", Skmt.ActionCode))
                .And(x => x.VerifySkmtMessageWasInsertedIntoWmsToEms())
                .And(x => x.VerifyForSkmtMessageSentTheSkuidWasParentSku())
                .BDDfy("Test Case Id:138273 -Dematic - SKMT - add - Call the SKMT api and validate for all functionalities when Parent sku");
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void SkmtMessageTest2ForParentSkuUpdateScenarios()
        {
            this.Given(x => x.ValidSkuActioncodeAndSkmtUrlIs(ParentSku.SkuId, SkmtActionCode.Update, SkmtUrl))
                .When(x => x.SkmtApiIsCalledCreatedIsReturned())
                .And(x => x.GetValidDataAfterTrigger())
                .And(x => x.VerifySkmtMessageWasInsertedIntoSwmToMheForActionCode("Update", Skmt.ActionCode))
                .And(x => x.VerifySkmtMessageWasInsertedIntoWmsToEms())
                .And(x => x.VerifyForSkmtMessageSentTheSkuidWasParentSku())
                .BDDfy("Test Case Id:138274-Dematic - SKMT - Update - Call the SKMT api and validate for all functionalities when Parent sku");
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void SkmtMessageTest3ForParentSkuDeleteScenarios()
        {
            this.Given(x => x.ValidSkuActioncodeAndSkmtUrlIs(ParentSku.SkuId, SkmtActionCode.Delete, SkmtUrl))
                .When(x => x.SkmtApiIsCalledCreatedIsReturned())
                .And(x => x.GetValidDataAfterTrigger())
                .And(x => x.VerifySkmtMessageWasInsertedIntoSwmToMheForActionCode("Delete", Skmt.ActionCode))
                .And(x => x.VerifySkmtMessageWasInsertedIntoWmsToEms())
                .And(x => x.VerifyForSkmtMessageSentTheSkuidWasParentSku())
                .BDDfy("Test Case Id:138275-Dematic - SKMT - Delete - Call the SKMT api and validate for all functionalities when Parent sku");
        }
    }
}
