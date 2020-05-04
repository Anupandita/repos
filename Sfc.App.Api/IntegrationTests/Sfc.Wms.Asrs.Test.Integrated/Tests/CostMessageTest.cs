using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData;
using Sfc.Wms.Interfaces.ParserAndTranslator.Contracts.Constants;
using TestStack.BDDfy;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Tests
{
    [TestClass]
    [Story(
        AsA = "Authorized User test for cost message from emstowms",
       IWant = "To Verify COST message is inserted in to SWM_FROM_MHE table with appropriate data" +
        " And verify COST message is inserted in to swm_from_mhe table with appropriate data " +
        " Verify in TransInventory table for Allocation Inventory units and actual weight" +
        " Verify in Case Detail table for Quantity, CaseHeader and task detail table for status code ",
       SoThat = "I can validate for message fields in COST message, in Internal Table SWM_FROM_MHE" +
        " and validate the quantity,weight,statuscode in the caseheader, casedetail, task header tables",
       StoryUri = "http://tfsapp1:8080/tfs/ShamrockCollection/Portfolio-SOWL/_workitems?id=129461&_a=edit"
        )]
    public class CostMessageTest : CostMessageFixture
    {        
        [TestInitialize]
        public void TestData()
        {
            TestInitialize();           
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void VerifyForValidCostMessageScenarios()
        {
            this.Given(x => x.TestInitializeForValidMessage())              
                //.And(x => x.ValidCostUrlMsgKeyAndProcessorIs(CostUrl,CostData.MsgKey, DefaultPossibleValue.MessageProcessor))
                //.When(x => x.CostApiIsCalledWithValidMsgKey())
                //.And(x => x.GetValidDataAfterTrigger())
                //.And(x => x.VerifyCostMessageWasInsertedIntoSwmFromMhe())
                //.And(x => x.VerifyTheQuantityWasDecreasedInToTransInventory())
                //.And(x => x.VerifyTheQuantityWasIncreasedIntoPickLocationTable())
                .BDDfy("Test Case ID :122681 - Dematic - COST  - Call the Cost api and verify all its funtionalities in EmsToWms, Swm_from_Mhe, Trans_Invn ,Pick_Location tables");              
        }
        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void VerifyForInvalidMessageKey()
        {
            this.Given(x =>x.TestInitializeForInvalidCase())               
                .And(x => x.ValidCostUrlMsgKeyAndProcessorIs(CostUrl,Constants.InvalidMsgKey, DefaultPossibleValue.MessageProcessor))
                .When(x => x.CostApiIsCalledForInvalidMessageKey())
                .Then(x => x.ValidateResultForInvalidMessageKey())
                .BDDfy("Test Case ID:122693- Dematic - COST - Negative test case1: Pass Invalid message key in the api call");
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void VerifyForErrorLogNoCaseFound()
        {
                this.Given(x => x.TestInitializeForInvalidCase())            
               .And(x => x.ValidCostUrlMsgKeyAndProcessorIs(CostUrl,CostData.InvalidKey,DefaultPossibleValue.MessageProcessor))
               .When(x => x.CostApiIsCalledForInvalidCaseNumber())
               .Then(x => x.ValidateResultForInvalidCaseNumber())
               .BDDfy("Test Case ID :122697 - Dematic - COST - Negative test case 3: If case number not in CaseHeader");
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void VerifyForErrorNotEnoughInventory()
        {
                this.Given(x => x.TestInitializeForTransInvnDoesNotExist())             
               .And(x => x.ValidCostUrlMsgKeyAndProcessorIs(CostUrl,CostDataForTransInvnNotExist.MsgKey,DefaultPossibleValue.MessageProcessor))
               .When(x => x.CostApiIsCalledForTransInvnNotFound())
               .Then(x => x.ValidateResultForTransInventoryNotExist())
               .BDDfy("Test Case ID :122697 - Dematic - COST - Negative test case 5 :If  transit inventory  does not exists");
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void VerifyForErrorPickLocationNotFound()
        {
                this.Given(x => x.TestInitializeForPickLocnDoesNotExist())            
               .And(x => x.ValidCostUrlMsgKeyAndProcessorIs(CostUrl,CostDataForPickLocnNotExist.MsgKey,DefaultPossibleValue.MessageProcessor))
               .When(x => x.CostApiIsCalledForPickLocnNotFound())
               .Then(x => x.ValidateResultForPickLocnNotFound())
               .BDDfy("Test Case ID : Dematic - COST - Negative test case 6 :If  pick location does not exists ");
        }       
    }
}
