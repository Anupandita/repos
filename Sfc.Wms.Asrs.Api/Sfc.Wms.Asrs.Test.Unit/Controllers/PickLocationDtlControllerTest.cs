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
        IWant = "To update detail on PickLocationDetail table",
        SoThat = "created API to perform the operation "
    )]
    public class PickLocationDtlControllerTest : PickLocationDetailFixture
    {
        [TestMethod]
        [TestCategory("UNIT")]
        public void Update_PickLocationDetail_Is_Called_With_Valid_Cost_Message()
        {
            this.Given(el => el.ValidData())
                .When(el => el.UpdatePickLocationDetailInvoked())
                .Then(el => el.PickLocationDetailShouldBeUpdated()).BDDfy();
        }

        [TestMethod]
        [TestCategory("UNIT")]
        public void Update_PickLocationDetail_Is_Called_With_Invalid_Cost_Message()
        {
            this.Given(el => el.InvalidData())
                .When(el => el.UpdatePickLocationDetailInvoked())
                .Then(el => el.PickLocationDetailShouldNotBeUpdated()).BDDfy();
        }
    }
}