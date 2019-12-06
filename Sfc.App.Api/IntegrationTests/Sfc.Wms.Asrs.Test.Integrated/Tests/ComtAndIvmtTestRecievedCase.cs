using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestStack.BDDfy;
using Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Tests
{
    [TestClass]
    [Story(
        AsA = "Authorized User test for comt message from wms to ems",
        IWant = "To Test the CaseNumbers For Received Cases From Vendors" +
                "And Verify when Message is Sent Ivmt message should be inserted into swm_to_mhe ,wmstoems table." ,
        SoThat = "I can validate for message fields in IVMT message, in Internal Table SWM_TO_MHE" +
                 " and validate the quantity,weight,statuscode in the caseheader, casedetail, task header tables"
    )]
    public class ComtAndIvmtTestRecievedCase:ComtIvmtMessageFixture
    {
        [TestInitialize]
        public void AValidTestData()
        {
            InitializeRecievedCaseTestData();
        }
      
        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void ComtAndIvmtMessageForRecievedCasesTestScenarios()
        {
            this.Given(x => x.AValidNewRecivedCaseComtMessageRecord())
                .When(x => x.ComtApiIsCalledCreatedIsReturned())
                .Then(x => x.GetDataFromDataBaseForRecivedCaseSingleSkuScenarios())
                .And(x => x.VerifyReceivedCaseComtMessageWasInsertedIntoSwmToMhe())
                .And(x => x.VerifyIvmtMessageWasInsertedIntoSwmToMhe())
                .And(x => x.VerifyIvmtMessageWasInsertedIntoWmsToEms())
                .And(x => x.VerifyTheQuantityIsIncreasedInToTransInventory())
                .And(x => x.VerifyQuantityisReducedIntoCaseDetail())
                .And(x => x.VerifyStatusIsUpdatedIntoCaseHeader())
                .And(x => x.VerifyStatusIsUpdatedIntoTaskHeader())
                .BDDfy();
        }
    }
}
