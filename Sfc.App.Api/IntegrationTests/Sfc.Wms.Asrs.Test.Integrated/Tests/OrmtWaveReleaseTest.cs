using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestStack.BDDfy;
using Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Tests
{
    [TestClass]
    [Story(
       AsA = "Authorized User test for ormt message from wmstoems",
       IWant = "To Test for printing of carton through Wave number."+
        "To Verify ORMT messages is inserted in to swm_to_mhe table for " +
        " And verify ORMT message is inserted in to wmstoems table with appropriate data " +
        " Verify in PickLocnDtlExt table for Ormt Count",
       SoThat = "I can validate for message fields in ORMT message, in Internal Table SWM_TO_MHE and in wmstoems",
       StoryUri = "http://tfsapp1:8080/tfs/ShamrockCollection/Portfolio-SOWL/_workitems?id=129455&_a=edit"
        )]
    public class OrmtWaveReleaseTest:OrmtMessageFixture
    {
        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        protected void OrmtWaveRelease()
        {
            this.Given(x => x.InitializeTestDataForWaveRelease())
                 .And(x => x.ValidOrmtWaveUrlAndWaveNumberIs(OrmtUrl, OrderList[0].WaveNbr))
                 .When(x => x.OrmtApiIsCalledCreatedIsReturnedForWaveRelease())
                 .And(x => x.ReadDataAndValidateTheFieldsInInternalTables())
                 .BDDfy("Test Case Id:146383-ORMT Wave release : test ormt message by passing wave number in the api call and validate all the functionalities");
        }
    }
}
