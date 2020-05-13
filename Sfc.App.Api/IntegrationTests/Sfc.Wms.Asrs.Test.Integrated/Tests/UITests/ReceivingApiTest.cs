using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures.UIFixtures;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData.Constant;
using TestStack.BDDfy;


namespace Sfc.Wms.Api.Asrs.Test.Integrated.Tests.UITests
{
    [TestClass]
    [Story(
         Title = "Receiving Api Testing",
         AsA = "Authorized User test for Receiving Page Apis",
         IWant = "To Test for all Apis Used on Receiving Page on Web UI ",
         SoThat = "I can vaidate Data that is going to be displayed on UI Grid and Pages",
         StoryUri = ""
          )]

    public class ReceivingApiTest: ReceivingFixture
    {
        [TestInitialize]
        protected void AValidTestData()
        {
            LoginToFetchToken();
            PickAnIReceivingTestDataFromDb();
        }

        [TestMethod()]
        [TestCategory("UI_FUNCTIONAL")]
        protected void ReceivingSearchApi_SearchWithPoNbr()
        {
            this.Given(x => x.CreateUrlAndInputParamForApiUsing("PoNumber"))
            .When(x => x.CallReceivingSearchApiWithInputs(UIConstants.ReceivingSearchUrl))
            .Then(x => x.VerifyReceivingSearchOutputAgainstDbOutput())
            .BDDfy("Test Case ID : ");
        }
        [TestMethod()]
        [TestCategory("UI_FUNCTIONAL")]
        protected void ReceivingSearchApi_SearchWithIShipmentNbr()
        {
            this.Given(x => x.CreateUrlAndInputParamForApiUsing("ShipmentNumber"))
            .When(x => x.CallReceivingSearchApiWithInputs(UIConstants.ReceivingSearchUrl))
            .Then(x => x.VerifyOutputTotalReordsAgainstDbCount(UIConstants.ShipmentNbrCount))
            .BDDfy("Test Case ID : ");
        }
        [TestMethod()]
        [TestCategory("UI_FUNCTIONAL")]
        protected void ReceivingSearchApi_SearchWithVendorName()
        {
            this.Given(x => x.CreateUrlAndInputParamForApiUsing("VendorName"))
            .When(x => x.CallReceivingSearchApiWithInputs(UIConstants.ReceivingSearchUrl))
            .Then(x => x.VerifyOutputTotalReordsAgainstDbCount(UIConstants.VendorNameCount))
            .BDDfy("Test Case ID : ");
        }
        [TestMethod()]
        [TestCategory("UI_FUNCTIONAL")]
        protected void ReceivingSearchApi_SearchWithStatusRange()
        {
            this.Given(x => x.CreateUrlAndInputParamForApiUsing("StatusRange"))
            .When(x => x.CallReceivingSearchApiWithInputs(UIConstants.ReceivingSearchUrl))
            .Then(x => x.VerifyOutputTotalReordsAgainstDbCount(UIConstants.ReceivingStatusCount))
            .BDDfy("Test Case ID : ");
        }
        [TestMethod()]
        [TestCategory("UI_FUNCTIONAL")]
        protected void ReceivingSearchApi_SearchWithVerifiedDateRange()
        {
            this.Given(x => x.CreateUrlAndInputParamForApiUsing("VerifiedDateRange"))
            .When(x => x.CallReceivingSearchApiWithInputs(UIConstants.ReceivingSearchUrl))
            .Then(x => x.VerifyOutputTotalReordsAgainstDbCount(UIConstants.VerifiedDateRangeCount))
            .BDDfy("Test Case ID : ");
        }

        [TestMethod()]
        [TestCategory("UI_FUNCTIONAL")]
        protected void ReceivingDetailsApi()
        {
            this.Given(x => x.CreateUrlAndInputParamForApiUsing("Details"))
            .When(x => x.CallReceivingDetailsApi(UIConstants.ReceivingDetailsUrl))
            .Then(x => x.VerifyOutputForReceivingDetailsInApiOutput())
            .BDDfy("Test Case ID : ");
        }
        [TestMethod()]
        [TestCategory("UI_FUNCTIONAL")]
        protected void ReceivingDetailsDrilldownApi()
        {
            this.Given(x => x.CreateUrlAndInputParamForApiUsing("DetailsDrilldown"))
            .When(x => x.CallReceivingDetailsDrilldownApi(UIConstants.ReceivingDetailsDrilldownUrl))
            .Then(x => x.VerifyOutputForReceivingDetailsDrilldownInApiOutput())
            .BDDfy("Test Case ID : ");
        }
        [TestMethod()]
        [TestCategory("UI_FUNCTIONAL")]
        protected void ReceivingQvDetailsApi()
        {
            this.Given(x => x.CreateUrlAndInputParamForApiUsing("QvDetails"))
            .When(x => x.CallReceivingQvDetailsApi(UIConstants.ReceivingQvDetailsUrl))
            .Then(x => x.VerifyOutputForQvDetailsInApiOutput())
            .BDDfy("Test Case ID : ");
        }
        [TestMethod()]
        [TestCategory("UI_FUNCTIONAL")]
        protected void ReceivingQvDetailsUpdateApi()
        {
            this.Given(x => x.CreateUrlAndInputParamForApiUsing("UpdateQvDetails"))
            .And(x=>x.CreateAnswerInput())
            .When(x => x.CallReceivingUpdateQvDetailsApi(UIConstants.ReceivingQvDetailsUrl))
            .Then(x => x.VerifyQVDetailsUpdatedInDb())
            .BDDfy("Test Case ID : ");
        }

        [TestMethod()]
        [TestCategory("UI_FUNCTIONAL")]
        protected void VerifyReceiptButtonCall()
        {
            this.Given(x => x.CreateUrlAndInputParamForApiUsing("Verify Receipt"))
            .And(x => x.CreateInputDtoFor("Verify Receipt"))
            .When(x => x.CallReceivingCorbaApi(UIConstants.ReceivingDetailsUrl))
            .Then(x => x.VerifyCorbaResultFromDbFor("Verify Receipt"))
            .BDDfy("Test Case ID : ");
        }
        [TestMethod()]
        [TestCategory("UI_FUNCTIONAL")]
        protected void MultiItemButtonCall()
        {
            this.Given(x => x.CreateUrlAndInputParamForApiUsing("Details"))
            .And(x => x.CreateInputDtoFor("Multi Item"))
            .When(x => x.CallReceivingCorbaApi(UIConstants.ReceivingDetailsUrl))
            .Then(x => x.VerifyCorbaResultFromDbFor("Multi Item"))
            .BDDfy("Test Case ID : ");
        }
        [TestMethod()]
        [TestCategory("UI_FUNCTIONAL")]
        protected void NllButtonCall()
        {
            this.Given(x => x.CreateUrlAndInputParamForApiUsing("Details"))
            .And(x => x.CreateInputDtoFor("NLL"))
            .When(x => x.CallReceivingCorbaApi(UIConstants.ReceivingDetailsUrl))
            .Then(x => x.VerifyCorbaResultFromDbFor("NLL"))
            .BDDfy("Test Case ID : ");
        }
    }
}
