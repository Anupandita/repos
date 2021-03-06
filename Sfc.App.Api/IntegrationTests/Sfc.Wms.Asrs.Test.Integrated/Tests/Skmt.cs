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
         " And verify SKMT message is inserted in to wmstoems table with appropriate data ",
        SoThat = "I can validate for message fields in SKMT message, in Internal Table SWM_TO_MHE and in wmstoems",
        StoryUri = "http://tfsapp1:8080/tfs/ShamrockCollection/Portfolio-SOWL/_workitems?id=128771&_a=edit"
         )]
    public class Skmt : SkmtMessageFixture
    {
        [TestInitialize]
        protected void AValidTestData()
        {
            InitializeTestData();
        }
        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        [Priority(1)]
        protected void SkmtMessageTest1SkuIdIsNullForAddScenarios()
        {
            this.Given(x => x.ValidSkuActioncodeAndSkmtUrlIs(Normal.SkuId, SkmtActionCode.Add,SkmtUrl))
                .When(x => x.SkmtApiIsCalledCreatedIsReturned())
                .And(x => x.GetValidDataAfterTrigger())
                .And(x => x.VerifySkmtMessageWasInsertedIntoSwmToMheForActionCode("Add", Skmt.ActionCode))
                .And(x => x.VerifySkmtMessageWasInsertedIntoWmsToEms())
                .And(x => x.VerifyForSkmtMessageSentTheSkuidWasNormalSku())
            .BDDfy("Test Case Id: 137882 - Dematic - SKMT - Add - Call the SKMT api and validate for all functionalities in wmstoems,swm_to_mhe tables");
        }
        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        [Priority(2)]
        protected void SkmtMessageTest2SkuIdIsNullForupdateScenarios()
        {
           this.Given(x => x.ValidSkuActioncodeAndSkmtUrlIs(Normal.SkuId, SkmtActionCode.Update,SkmtUrl))
                .When(x => x.SkmtApiIsCalledCreatedIsReturned())
                .And(x => x.GetValidDataAfterTrigger())
                .And(x => x.VerifySkmtMessageWasInsertedIntoSwmToMheForActionCode("Update", Skmt.ActionCode))
                .And(x => x.VerifySkmtMessageWasInsertedIntoWmsToEms())
                .And(x => x.VerifyForSkmtMessageSentTheSkuidWasNormalSku())
            .BDDfy("Test Case Id:137883 -Dematic - SKMT - UPDATE - Call the SKMT api and validate for all functionalities in wmstoems,swm_to_mhe tables");
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        [Priority(3)]
        protected void SkmtMessageTest3SkuIdIsNullDeleteForScenarios()
        {
            this.Given(x => x.ValidSkuActioncodeAndSkmtUrlIs(Normal.SkuId, SkmtActionCode.Delete,SkmtUrl))
                .When(x => x.SkmtApiIsCalledCreatedIsReturned())
                .And(x => x.GetValidDataAfterTrigger())
                .And(x => x.VerifySkmtMessageWasInsertedIntoSwmToMheForActionCode("Delete", Skmt.ActionCode))
                .And(x => x.VerifySkmtMessageWasInsertedIntoWmsToEms())
                .And(x => x.VerifyForSkmtMessageSentTheSkuidWasNormalSku())
            .BDDfy("Test Case Id:138122 -Dematic - SKMT - DELETE - Call the SKMT api and validate for all functionalities in wmstoems,swm_to_mhe tables");
        }
    }
}
