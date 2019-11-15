using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures;
using TestStack.BDDfy;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Tests
{
    [TestClass]
    [Story(
       AsA = "Authorized User test for InvildSkuid",
       SoThat = "The result should be Notfound from ItemMaster")]
    public class SkmtNeagtiveCase:SkmtMessageFixture
    {

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        [Priority(1)]
        public void SkmtMessageTestNegativeScenarios()
        {
            this.Given(x => x.InitializeInvalidTestData())
                .And(x => x.CurrentActioncodeAdd())
                .And(x => x.ValidSkmtUrl())
                .When(x => x.SkmtApiIsCalledCreatedForNegativeCases())
                .And(x => x.SkmtApiIsCalledForInvalidSkuId())
                .And(x => x.ValidateResultForInvalidSkuId())
                .BDDfy();

        }


    }
}
