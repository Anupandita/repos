using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures;
using TestStack.BDDfy;

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

    public class OrmtNegativeCases :OrmtMessageFixture
    {
        [TestInitialize]
        public void AValidTestData()
        {
            InitializeTestDataForNegativeCases();
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void ValidateForMessageWhereActiveOrmtCountNotFound()
        {
            this.Given(x => x.CartonNumberForOrmtCountNotFound())
                .And(x=>x.ValidOrmtUrl())
                .When(x=>x.OrmtApiIsCalledForNotEnoughInventory())
                .And(x => x.ValidateResultForActiveOrmtNotFound())
                .BDDfy();
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void ValidateForMessageWherePickLocnNotFound()
        {
            this.Given(x => x.CartonNumberForPickLocnNotFound())
               .And(x => x.ValidOrmtUrl())
               .When(x => x.OrmtApiIsCalledForPickLocationNotFound())
               .And(x => x.ValidateResultForPickLocationNotFound())
               .BDDfy();
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void ValidateForMessageWhereActiveLocnNotFound()
        {
            this.Given(x => x.CartonNumberForActiveLocnNotFound())
               .And(x => x.ValidOrmtUrl())
               .When(x => x.OrmtApiIsCalledForActiveLocationNotFound())
               .And(x => x.ValidateResultForActiveLocationNotFound())
               .BDDfy();
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void ValidateForMessageWhereCartonNumberIsInvalid()
        {
            this.Given(x => x.CartonNumberForInvalidCartonNumber())
               .And(x => x.ValidOrmtUrl())
               .When(x => x.OrmtApiIsCalledForInvalidCartonNumber())
               .And(x => x.ValidateResultForInvalidCartonNumber())
               .BDDfy();
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void ValidateForMessageWhereActionCodeIsInvalid()
        {
            this.Given(x => x.TestForInValidActionCode())
               .And(x => x.ValidOrmtUrl())
               .When(x => x.OrmtApiIsCalledForInvalidActionCode())
               .And(x => x.ValidateResultForInvalidActionCode())
               .BDDfy();
        }

    }
}
