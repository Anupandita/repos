using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData;
using Sfc.Wms.Interfaces.ParserAndTranslator.Contracts.Constants;
using TestStack.BDDfy;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Tests
{
    [TestClass]
    [Story(
        AsA = "Authorized User test for ormt message from wmstoems",
        IWant = "To Test for Ormt Negative Cases ."+
         "To Verify ORMT message is inserted in to swm_to_mhe table with appropriate data" +
         " And verify ORMT message is inserted in to wmstoems table with appropriate data " +
         " Verify in PickLocnDtlExt table for Ormt Count",
        SoThat = "I can validate for validation messages per messages.",
        StoryUri = "http://tfsapp1:8080/tfs/ShamrockCollection/Portfolio-SOWL/_workitems?id=129455&_a=edit"
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
            this.Given(x=>x.ValidOrmtUrlCartonNumberAndActioncodeIs(OrmtUrl, ActiveOrmtCountNotFound.CartonNbr, OrmtActionCode.AddRelease))
                .When(x=>x.OrmtApiIsCalledForNotEnoughInventory())
                .And(x => x.ValidateResultForActiveOrmtNotFound())
                .BDDfy("Test Case Id:146378 - ORMT: Validate for message where active ormt count not found.");
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void ValidateForMessageWherePickLocnNotFound()
        {           
           this.Given(x => x.ValidOrmtUrlCartonNumberAndActioncodeIs(OrmtUrl, PickLocnNotFound.CartonNbr, OrmtActionCode.AddRelease))
               .When(x => x.OrmtApiIsCalledForPickLocationNotFound())
               .And(x => x.ValidateResultForPickLocationNotFound())
               .BDDfy("Test Case Id:146379 -ORMT: Validate for message where pick location not found.");
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void ValidateForMessageWhereActiveLocnNotFound()
        {         
           this.Given(x => x.ValidOrmtUrlCartonNumberAndActioncodeIs(OrmtUrl, ActiveLocnNotFound.CartonNbr, OrmtActionCode.AddRelease))
               .When(x => x.OrmtApiIsCalledForActiveLocationNotFound())
               .And(x => x.ValidateResultForActiveLocationNotFound())
               .BDDfy("Test Case Id:146380 -ORMT: Validate for message where active location not found.");
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void ValidateForMessageWhereCartonNumberIsInvalid()
        {
           this.Given(x => x.ValidOrmtUrlCartonNumberAndActioncodeIs(OrmtUrl, CancelOrder.CartonNbr,OrmtActionCode.AddRelease))
               .When(x => x.OrmtApiIsCalledForInvalidCartonNumber())
               .And(x => x.ValidateResultForInvalidCartonNumber())
               .BDDfy("Test Case Id:146381 -ORMT: Validate for message where carton number is invalid.");
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void ValidateForMessageWhereActionCodeIsInvalid()
        {          
           this.Given(x => x.ValidOrmtUrlCartonNumberAndActioncodeIs(OrmtUrl, PrintCarton.CartonNbr, Constants.InvalidOrmtActionCode))
               .When(x => x.OrmtApiIsCalledForInvalidActionCode())
               .And(x => x.ValidateResultForInvalidActionCode())
               .BDDfy("Test Case Id:146382 -ORMT: Validate for message where action code is invalid.");
        }
    }
}
