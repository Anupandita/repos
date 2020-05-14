using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData;
using Sfc.Wms.Interfaces.ParserAndTranslator.Contracts.Constants;
using TestStack.BDDfy;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Tests
{
    [TestClass]
    [Story(
       AsA = "Authorized User test for InvildSkuid",
       SoThat = "The result should be Notfound from ItemMaster",
       StoryUri = "http://tfsapp1:8080/tfs/ShamrockCollection/Portfolio-SOWL/_workitems?id=128771&_a=edit")
        ]
    public class SkmtNeagtiveCase:SkmtMessageFixture
    {
        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        [Priority(1)]
        protected void SkmtMessageTestNegativeScenarios()
        {
            this.Given(x => x.ValidSkuActioncodeAndSkmtUrlIs(DefaultValues.InvalidSku, SkmtActionCode.Add,SkmtUrl))
                .When(x => x.SkmtApiIsCalledCreatedForNegativeCases())
                .And(x => x.SkmtApiIsCalledForInvalidSkuId())
                .And(x => x.ValidateResultForInvalidSkuId())
                .BDDfy();
        }
    }
}
