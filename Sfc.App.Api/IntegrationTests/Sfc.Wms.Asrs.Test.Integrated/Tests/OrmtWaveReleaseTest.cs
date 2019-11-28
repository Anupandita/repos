using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestStack.BDDfy;
using Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Tests
{
    [TestClass]
    [Story(
        AsA = "Authorized User test for ormt message from wmstoems",
       IWant = "To Verify ORMT message is inserted in to swm_to_mhe table with appropriate data" +
        " And verify ORMT message is inserted in to wmstoems table with appropriate data " +
        " Verify in PickLocnDtlExt table for Ormt Count",
       SoThat = "I can validate for message fields in ORMT message, in Internal Table SWM_TO_MHE and in wmstoems"
        )]
    public class OrmtWaveReleaseTest:OrmtMessageFixture
    {
        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void OrmtWaveRelease()
        {
           
           this .Given(x=>x.InitializeTestDataForWaveRelease())
                .And(x=>x.ValidOrmtWaveUrl())
                .When(x=>x.OrmtApiIsCalledCreatedIsReturnedForWaveRelease())
                .And(x=>x.ReadDataAndValidateTheFieldsInInternalTables())
                .BDDfy();
        }
    }
}
