using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures.UIFixtures;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData.Constant;
using TestStack.BDDfy;


namespace Sfc.Wms.Api.Asrs.Test.Integrated.Tests.UITests
{
    [TestClass]
    [Story(
         Title = "Lpn Api Testing",
         AsA = "Authorized User test for Lpn Page Apis",
         IWant = "To Test for all Apis Used on Lpn Page on Web UI ",
         SoThat = "I can vaidate Data that is going to be displayed on UI Grid and Pages",
         StoryUri = ""
          )]

    public class LpnApiTest: LpnFixture
    {
        [TestInitialize]
        public void AValidTestData()
        {          
            LoginToFetchToken();
            PickAnLpnTestDataFromDb();
        }

        [TestMethod()]
        [TestCategory("UI_FUNCTIONAL")]
        public void LpnSearchApi_SearchWithLpnNbr() 
        {
            this.Given(x => x.CreateUrlAndInputParamForApiUsing("LpnNumber"))
            .When(x => x.CallLpnSearchApiWithInputs(UIConstants.LpnSearchUrl))
            .And(x => x.VerifyLpnSearchOutputAgainstDbOutput())
            .BDDfy("Test Case ID : ");
        }
        [TestMethod()]
        [TestCategory("UI_FUNCTIONAL")]
        public void LpnSearchApi_SearchWithItemNumber()
        {
            this.Given(x => x.CreateUrlAndInputParamForApiUsing("ItemNumber"))
          .When(x => x.CallLpnSearchApiWithInputs(UIConstants.LpnSearchUrl))
            .And(x => x.VerifyOutputTotalReordsAgainstDbCount(UIConstants.ItemNumberCount))
            .BDDfy("Test Case ID : ");
        }
        [TestMethod()]
        [TestCategory("UI_FUNCTIONAL")]
        public void LpnSearchApi_SearchWithPalletIdNbr()
        {
            this.Given(x => x.CreateUrlAndInputParamForApiUsing("PalletIdNumber"))
            .When(x => x.CallLpnSearchApiWithInputs(UIConstants.LpnSearchUrl))
            .And(x => x.VerifyOutputTotalReordsAgainstDbCount(UIConstants.PalletIdCount))
            .BDDfy("Test Case ID : ");
        }
        [TestMethod()]
        [TestCategory("UI_FUNCTIONAL")]
        public void LpnSearchApi_SearchWithZone()
        {
            this.Given(x => x.CreateUrlAndInputParamForApiUsing("Zone"))
            .When(x => x.CallLpnSearchApiWithInputs(UIConstants.LpnSearchUrl))
            .And(x => x.VerifyDisplayLocationForZone())
            .BDDfy("Test Case ID : ");
        }
        [TestMethod()]
        [TestCategory("UI_FUNCTIONAL")]
        public void LpnSearchApi_SearchWithAisle()
        {
            this.Given(x => x.CreateUrlAndInputParamForApiUsing("Aisle"))
            .When(x => x.CallLpnSearchApiWithInputs(UIConstants.LpnSearchUrl))
            .And(x => x.VerifyDisplayLocationForAisle())
            .BDDfy("Test Case ID : ");
        }
        [TestMethod()]
        [TestCategory("UI_FUNCTIONAL")]
        public void LpnSearchApi_SearchWithSlot()
        {
            this.Given(x => x.CreateUrlAndInputParamForApiUsing("Slot"))
            .When(x => x.CallLpnSearchApiWithInputs(UIConstants.LpnSearchUrl))
            .And(x => x.VerifyDisplayLocationForSlot())
            .BDDfy("Test Case ID : ");
        }
        [TestMethod()]
        [TestCategory("UI_FUNCTIONAL")]
        public void LpnSearchApi_SearchWithLevel()
        {
            this.Given(x => x.CreateUrlAndInputParamForApiUsing("Level"))
            .When(x => x.CallLpnSearchApiWithInputs(UIConstants.LpnSearchUrl))
            .And(x => x.VerifyDisplayLocationForLevel())
            .BDDfy("Test Case ID : ");
        }
        [TestMethod()]
        [TestCategory("UI_FUNCTIONAL")]
        public void LpnSearchApi_SearchWithCreatedDate()
        {
            this.Given(x => x.CreateUrlAndInputParamForApiUsing("CreatedDate"))
            .When(x => x.CallLpnSearchApiWithInputs(UIConstants.LpnSearchUrl))
            .And(x => x.VerifyOutputTotalReordsAgainstDbCount(UIConstants.CreatedDateCount))
            .BDDfy("Test Case ID : ");
        }
        [TestMethod()]
        [TestCategory("UI_FUNCTIONAL")]
        public void LpnSearchApi_SearchWithStatus()
        {
            this.Given(x => x.CreateUrlAndInputParamForApiUsing("Status"))
            .When(x => x.CallLpnSearchApiWithInputs(UIConstants.LpnSearchUrl))
            .And(x => x.VerifyOutputTotalReordsAgainstDbCount(UIConstants.StatusCount))
            .BDDfy("Test Case ID : ");
        }
        [TestMethod()]
        [TestCategory("UI_FUNCTIONAL")]
        public void LpnCommentsApi()
        {
            this.Given(x => x.CreateUrlAndInputParamForApiUsing("Comments"))
            .When(x => x.CallLpnCommentsApiWithInputs(UIConstants.LpnCommentsUrl))
            .And(x => x.VerifyLpnCommentsOutputAgainstDbOutput())
            .BDDfy("Test Case ID : ");
        }
        [TestMethod()]
        [TestCategory("UI_FUNCTIONAL")]
        public void LpnHistoryApi()
        {
            this.Given(x => x.CreateUrlAndInputParamForApiUsing("History"))
            .When(x => x.CallLpnHistoryApiWithInputs(UIConstants.LpnHistoryUrl))
            .And(x => x.VerifyLpnHistoryOutputAgainstDbOutput())
            .BDDfy("Test Case ID : ");
        }
        [TestMethod()]
        [TestCategory("UI_FUNCTIONAL")]
        public void LpnLockUnlockApi()
        {
            this.Given(x => x.CreateUrlAndInputParamForApiUsing("LockUnlock"))
            .When(x => x.CallLpnLockUnlockApiWithInputs(UIConstants.LpnLockUnlockUrl))
            .And(x => x.VerifyLpnLockUnlockOutputAgainstDbOutput())
            .BDDfy("Test Case ID : ");
        }
        [TestMethod()]
        [TestCategory("UI_FUNCTIONAL")]
        public void ItemAttributeDrilldownApi()
        {
            this.Given(x=>x.CreateUrlAndInputParamForApiUsing("LpnComments"))
            .When(x => x.CallLpnDetailsApi(UIConstants.LpnCommentsUrl))
            .Then(x => x.VerifyOutputForActiveLocationsListInOutput())
            .And(x => x.VerifyOutputForVendorsListInOutput())
            .BDDfy("Test Case ID : ");
        }
    }
}
