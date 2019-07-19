using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.Asrs.Test.Unit.Fixtures.Nuget;
using TestStack.BDDfy;

namespace Sfc.Wms.Asrs.Test.Unit.Nuget
{
    [TestClass]
    [Story(
        AsA = "End user",
        IWant = "To perform update operation on CaseDetail table",
        SoThat = "update operation is done on CaseDetail table"
    )]
    public class InboundLpnGatewayTest : InboundLpnGatewayFixture
    {
        [TestMethod]
        [TestCategory("UNIT")]
        public void Update_Quantity_For_Valid_Ivmt_Message()
        {
            this.Given(el => el.ValidData())
                .When(el => el.UpdateQuantityInvoked())
                .Then(el => el.QuantityShouldBeUpdated()).BDDfy();
        }

        [TestMethod]
        [TestCategory("UNIT")]
        public void Update_Quantity_For_Invalid_Ivmt_Message()
        {
            this.Given(el => el.InvalidData())
                .When(el => el.UpdateQuantityInvoked())
                .Then(el => el.QuantityShouldNotBeUpdated()).BDDfy();
        }
    }
}