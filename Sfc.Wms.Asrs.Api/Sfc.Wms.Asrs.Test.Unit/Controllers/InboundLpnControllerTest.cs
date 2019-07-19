using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.Asrs.Test.Unit.Fixtures;
using TestStack.BDDfy;

namespace Sfc.Wms.Asrs.Test.Unit.Controllers
{
    [TestClass]
    [Story(
        AsA = "End user",
        IWant = "To perform update operation on CaseDetail table",
        SoThat = "update operation is done on CaseDetail table"
    )]
    public class InboundLpnControllerTest : InboundLpnFixture
    {
        [TestMethod]
        [TestCategory("UNIT")]
        public void Update_Quantity_For_Valid_Ivmt_Message()
        {
            this.Given(el=>el.ValidData())
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
