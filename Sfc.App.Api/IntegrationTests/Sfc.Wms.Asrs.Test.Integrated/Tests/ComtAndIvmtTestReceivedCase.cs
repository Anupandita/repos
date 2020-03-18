using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestStack.BDDfy;
using Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Tests
{
    [TestClass]
    [Story(
        AsA = "Authorized User test for comt message from wms to ems",
        IWant = "To Test the CaseNumbers For Received Cases From Returns" +
                "And Verify when Message is Sent, Ivmt message should be inserted into swm_to_mhe ,wmstoems table." ,
        SoThat = "I can validate for message fields in IVMT message, in Internal Table SWM_TO_MHE" +
                 " and validate the quantity,weight,statuscode in the caseheader, casedetail, task header tables",
        StoryUri = "http://tfsapp1:8080/tfs/ShamrockCollection/Portfolio-SOWL/_workitems?id=129462&_a=edit"
    )]
    public class ComtAndIvmtTestReceivedCase:ComtIvmtMessageFixture
    {
        [TestInitialize]
        public void AValidTestData()
        {
            InitializeReceivedCaseTestData();
        }
      
        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void ComtAndIvmtMessageForReceivedCasesFromReturnsTestScenarios()
        {
            this.Given(x => x.AValidNewCaseReturnedRecordWhereCaseNumberAndSkuIdIs(SingleSkuCase.CaseNumber,SingleSkuCase.SkuId))
                .When(x => x.ComtApiIsCalledCreatedIsReturnedWithValidUrlIs(ComtUrl))
                .Then(x => x.GetDataFromDataBaseAfterApiIsCalled())     
                .And(x => x.VerifyIvmtMessageWasInsertedIntoSwmToMhe(SingleSkuCase.ActlQty))
                .And(x => x.VerifyIvmtMessageWasInsertedIntoWmsToEms())
                .And(x => x.VerifyTheQuantityIsIncreasedInToTransInventory())
                .And(x => x.VerifyActualQuantityIsReducedInToCaseDtl())
                .And(x => x.VerifyStatusIsUpdatedIntoCaseHeader())
                .And(x => x.VerifyStatusIsUpdatedIntoTaskHeader())
                .And(x =>x.VerifyQtyShouldBeIncreasedInPickLocnTableForToBeFilledQtyField())
                .And(x =>x.VerifyWeightAndVolumeIsReducedInResrvLocnHdrTable())
                .BDDfy("Test Case ID: 142101 -Dematic - IVMT :  For received cases from returns, Call the Comt Api and Verify all its functionalities");
        }
    }
}
