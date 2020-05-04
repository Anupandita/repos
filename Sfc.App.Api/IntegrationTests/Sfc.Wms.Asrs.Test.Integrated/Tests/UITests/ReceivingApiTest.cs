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
        public void AValidTestData()
        {
            LoginToFetchToken();
            PickAnIReceivingTestDataFromDb();
        }

        [TestMethod()]
        [TestCategory("UI_FUNCTIONAL")]
        public void ReceivingSearchApi_SearchWithPoNbr()
        {
            this.Given(x => x.CreateUrlAndInputParamForApiUsing("PoNumber"))
            .When(x => x.CallReceivingSearchApiWithInputs(UIConstants.ReceivingSearchUrl))
            .And(x => x.VerifyReceivingSearchOutputAgainstDbOutput())
            .BDDfy("Test Case ID : ");
        }
        [TestMethod()]
        [TestCategory("UI_FUNCTIONAL")]
        public void ReceivingSearchApi_SearchWithIShipmentNbr()
        {
            this.Given(x => x.CreateUrlAndInputParamForApiUsing("ShipmentNumber"))
            .When(x => x.CallReceivingSearchApiWithInputs(UIConstants.ReceivingSearchUrl))
            .And(x => x.VerifyOutputTotalReordsAgainstDbCount(UIConstants.ShipmentNbrCount))
            .BDDfy("Test Case ID : ");
        }
        [TestMethod()]
        [TestCategory("UI_FUNCTIONAL")]
        public void ReceivingSearchApi_SearchWithVendorName()
        {
            this.Given(x => x.CreateUrlAndInputParamForApiUsing("VendorName"))
            .When(x => x.CallReceivingSearchApiWithInputs(UIConstants.ReceivingSearchUrl))
            .And(x => x.VerifyOutputTotalReordsAgainstDbCount(UIConstants.VendorVendorNumberCount))
            .BDDfy("Test Case ID : ");
        }
        [TestMethod()]
        [TestCategory("UI_FUNCTIONAL")]
        public void ReceivingSearchApi_SearchWithStatusRange()
        {
            this.Given(x => x.CreateUrlAndInputParamForApiUsing("StatusRange"))
            .When(x => x.CallReceivingSearchApiWithInputs(UIConstants.ReceivingSearchUrl))
            .And(x => x.VerifyOutputTotalReordsAgainstDbCount(UIConstants.ReceivingStatusCount))
            .BDDfy("Test Case ID : ");
        }
        [TestMethod()]
        [TestCategory("UI_FUNCTIONAL")]
        public void ReceivingSearchApi_SearchWithVerifiedDateRange()
        {
            this.Given(x => x.CreateUrlAndInputParamForApiUsing("VerifiedDateRange"))
            .When(x => x.CallReceivingSearchApiWithInputs(UIConstants.ReceivingSearchUrl))
            .And(x => x.VerifyOutputTotalReordsAgainstDbCount(UIConstants.VerifiedDateRangeCount))
            .BDDfy("Test Case ID : ");
        }

        [TestMethod()]
        [TestCategory("UI_FUNCTIONAL")]
        public void ReceivingDrilldownApi()
        {
            this.Given(x => x.CreateUrlAndInputParamForApiUsing("Drilldown"))
            .When(x => x.CallReceivingDrilldownApi(UIConstants.ReceivingDrilldownUrl))
            .Then(x => x.VerifyOutputForReceivingDrilldownInApiOutput())
            .BDDfy("Test Case ID : ");
        }
        [TestMethod()]
        [TestCategory("UI_FUNCTIONAL")]
        public void ReceivingDetailsApi()
        {
            this.Given(x => x.CreateUrlAndInputParamForApiUsing("Details"))
            .When(x => x.CallReceivingDetailsApi(UIConstants.ReceivingDetailsUrl))
            .And(x => x.VerifyOutputForReceivingDetailsInApiOutput())
            .BDDfy("Test Case ID : ");
        }
        [TestMethod()]
        [TestCategory("UI_FUNCTIONAL")]
        public void ReceivingDetailsDrilldownApi()
        {
            this.Given(x => x.CreateUrlAndInputParamForApiUsing("DetailsDrilldown"))
            .When(x => x.CallReceivingDetailsDrilldownApi(UIConstants.ReceivingDetailsDrilldownUrl))
            .And(x => x.VerifyOutputForReceivingDetailsDrilldownInApiOutput())
            .BDDfy("Test Case ID : ");
        }
        [TestMethod()]
        [TestCategory("UI_FUNCTIONAL")]
        public void ReceivingQvDetailsApi()
        {
            this.Given(x => x.CreateUrlAndInputParamForApiUsing("QvDetails"))
            .When(x => x.CallReceivingQvDetailsApi(UIConstants.ReceivingQvDetailsUrl))
            .And(x => x.VerifyOutputForQvDetailsInApiOutput())
            .BDDfy("Test Case ID : ");
        }
        [TestMethod()]
        [TestCategory("UI_FUNCTIONAL")]
        public void ReceivingQvDetailsUpdateApi()
        {
            this.Given(x => x.CreateUrlAndInputParamForApiUsing("UpdateQvDetails"))
            .When(x => x.CallReceivingUpdateQvDetailsApi(UIConstants.ReceivingQvDetailsUrl))
            .And(x => x.VerifyQVDetailsUpdatedInDb())
            .BDDfy("Test Case ID : ");
        }
    }
}
