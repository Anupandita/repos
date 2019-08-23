using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.Asrs.Test.Integrated.Fixtures;
using Sfc.Wms.ParserAndTranslator.Contracts.Interfaces;
using TestStack.BDDfy;

namespace Sfc.Wms.Asrs.Test.Integrated.Tests
{
    [TestClass]
    [Story(
       AsA = "Authorized User test for comt and ivmt message from wms to ems",
       IWant = "To Verify COMT and IVMT message is inserted in to SWMTOMHE table with appropriate data" +
        " And verify IVMT message is inserted in to WMSTOEMS table with appropriate data " +
        " Verify in TransInventory table for Allocation Inventory units and actual weight"+
        " Verify in Case Detail table for Quantity, CaseHeader and task detail table for status code ",
       SoThat = "I can validate for message fields in COMT and IVMT message, in Internal Table SWM_TO_MHE"+
        " and validate the quantity,weight,statuscode in the caseheader, casedetail, task header tables"
       )]
    public class SampleTest : ComtIvmtMessageFixture
    {
       
        [TestInitialize]
        [TestCategory("FUNCTIONAL")]
        public void AValidTestData()
        {
            GetDataBeforeCallingComtApi();           
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void ComtAndIvmtTest() 
        {
            this.Given(x => x.CurrentCaseNumberForSingleSku())
                .And(x => x.AValidNewComtMessageRecord())
                .When(x => x.ComtApiIsCalled())
                .Then(x => x.GetDataFromDataBaseForSingleSkuScenarios())
                .And(x => x.VerifyComtMessageWasInsertedIntoSwmToMhe())
                .And(x => x.VerifyComtMessageWasInsertedIntoWmsToEms())
                .And(x => x.VerifyIvmtMessageWasInsertedIntoSwmToMhe())
                .And(x => x.VerifyIvmtMessageWasInsertedIntoWmsToEms())
                .And(x => x.VerifyTheQuantityIsIncreasedToTransInventory())
                .And(x => x.VerifyQuantityisReducedIntoCaseDetail())
                .And(x => x.VerifyStatusIsUpdatedIntoCaseHeader())
                .And(x => x.VerifyStatusIsUpdatedIntoTaskHeader())
                .BDDfy();
        }

        [TestMethod]
        [TestCategory("FUNCTIONAL")]
        public void ComtAndIvmtTestForMultiSku()
        {
            this.Given(x => x.CurrentCaseNumberForMultiSku())
                .And(x => x.AValidNewComtMessageRecord())
                .When(x => x.ComtApiIsCalled())
                .Then(x => x.GetDataAndValidateForIvmtMessageHasInsertedIntoBothTables())
                .And(x => x.VerifyComtMessageWasInsertedIntoSwmToMheForMultiSku())
                .And(x => x.VerifyComtMessageWasInsertedIntoWmsToEmsForMultiSku())
                .And(x => x.VerifyQuantityisReducedIntoCaseDetailTable())
                .And(x => x.VerifyStatusIsUpdatedIntoCaseHeaderTable())
                .BDDfy();
        }      
    }
}
 