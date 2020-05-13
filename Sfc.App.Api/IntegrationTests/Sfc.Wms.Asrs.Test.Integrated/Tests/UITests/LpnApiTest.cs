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
        protected void AValidTestData()
        {          
            LoginToFetchToken();
            PickAnLpnTestDataFromDb();
        }

        [TestMethod()]
        [TestCategory("UI_FUNCTIONAL")]
        protected void LpnSearchApi_SearchWithLpnNbr() 
        {
            this.Given(x => x.CreateUrlAndInputParamForApiUsing("LpnNumber"))
            .When(x => x.CallLpnSearchApiWithInputs(UIConstants.LpnSearchUrl))
            .And(x => x.VerifyLpnSearchOutputAgainstDbOutput())
            .BDDfy("Test Case ID : ");
        }
        [TestMethod()]
        [TestCategory("UI_FUNCTIONAL")]
        protected void LpnSearchApi_SearchWithItemNumber()
        {
            this.Given(x => x.CreateUrlAndInputParamForApiUsing("ItemNumber"))
          .When(x => x.CallLpnSearchApiWithInputs(UIConstants.LpnSearchUrl))
            .And(x => x.VerifyOutputTotalReordsAgainstDbCount(UIConstants.ItemNumberCount))
            .BDDfy("Test Case ID : ");
        }
        [TestMethod()]
        [TestCategory("UI_FUNCTIONAL")]
        protected void LpnSearchApi_SearchWithPalletIdNbr()
        {
            this.Given(x => x.CreateUrlAndInputParamForApiUsing("PalletIdNumber"))
            .When(x => x.CallLpnSearchApiWithInputs(UIConstants.LpnSearchUrl))
            .And(x => x.VerifyOutputTotalReordsAgainstDbCount(UIConstants.PalletIdCount))
            .BDDfy("Test Case ID : ");
        }
        [TestMethod()]
        [TestCategory("UI_FUNCTIONAL")]
        protected void LpnSearchApi_SearchWithZone()
        {
            this.Given(x => x.CreateUrlAndInputParamForApiUsing("Zone"))
            .When(x => x.CallLpnSearchApiWithInputs(UIConstants.LpnSearchUrl))
            .And(x => x.VerifyDisplayLocationForZone())
            .BDDfy("Test Case ID : ");
        }
        [TestMethod()]
        [TestCategory("UI_FUNCTIONAL")]
        protected void LpnSearchApi_SearchWithAisle()
        {
            this.Given(x => x.CreateUrlAndInputParamForApiUsing("Aisle"))
            .When(x => x.CallLpnSearchApiWithInputs(UIConstants.LpnSearchUrl))
            .And(x => x.VerifyDisplayLocationForAisle())
            .BDDfy("Test Case ID : ");
        }
        [TestMethod()]
        [TestCategory("UI_FUNCTIONAL")]
        protected void LpnSearchApi_SearchWithSlot()
        {
            this.Given(x => x.CreateUrlAndInputParamForApiUsing("Slot"))
            .When(x => x.CallLpnSearchApiWithInputs(UIConstants.LpnSearchUrl))
            .And(x => x.VerifyDisplayLocationForSlot())
            .BDDfy("Test Case ID : ");
        }
        [TestMethod()]
        [TestCategory("UI_FUNCTIONAL")]
        protected void LpnSearchApi_SearchWithLevel()
        {
            this.Given(x => x.CreateUrlAndInputParamForApiUsing("Level"))
            .When(x => x.CallLpnSearchApiWithInputs(UIConstants.LpnSearchUrl))
            .And(x => x.VerifyDisplayLocationForLevel())
            .BDDfy("Test Case ID : ");
        }
        [TestMethod()]
        [TestCategory("UI_FUNCTIONAL")]
        protected void LpnSearchApi_SearchWithCreatedDate()
        {
            this.Given(x => x.CreateUrlAndInputParamForApiUsing("CreatedDate"))
            .When(x => x.CallLpnSearchApiWithInputs(UIConstants.LpnSearchUrl))
            .And(x => x.VerifyOutputTotalReordsAgainstDbCount(UIConstants.CreatedDateCount))
            .BDDfy("Test Case ID : ");
        }
        [TestMethod()]
        [TestCategory("UI_FUNCTIONAL")]
        protected void LpnSearchApi_SearchWithStatus()
        {
            this.Given(x => x.CreateUrlAndInputParamForApiUsing("Status"))
            .When(x => x.CallLpnSearchApiWithInputs(UIConstants.LpnSearchUrl))
            .And(x => x.VerifyOutputTotalReordsAgainstDbCount(UIConstants.StatusCount))
            .BDDfy("Test Case ID : ");
        }
        [TestMethod()]
        [TestCategory("UI_FUNCTIONAL")]
        protected void LpnCommentsApi()
        {
            this.Given(x => x.CreateUrlAndInputParamForApiUsing("Comments"))
            .When(x => x.CallLpnCommentsApiWithInputs(UIConstants.LpnCommentsUrl))
            .And(x => x.VerifyLpnCommentsOutputAgainstDbOutput())
            .BDDfy("Test Case ID : ");
        }
        [TestMethod()]
        [TestCategory("UI_FUNCTIONAL")]
        protected void LpnHistoryApi()
        {
            this.Given(x => x.CreateUrlAndInputParamForApiUsing("History"))
            .When(x => x.CallLpnHistoryApiWithInputs(UIConstants.LpnHistoryUrl))
            .And(x => x.VerifyLpnHistoryOutputAgainstDbOutput())
            .BDDfy("Test Case ID : ");
        }
        [TestMethod()]
        [TestCategory("UI_FUNCTIONAL")]
        protected void LpnLockUnlockApi()
        {
            this.Given(x => x.CreateUrlAndInputParamForApiUsing("LockUnlock"))
            .When(x => x.CallLpnLockUnlockApiWithInputs(UIConstants.LpnLockUnlockUrl))
            .And(x => x.VerifyLpnLockUnlockOutputAgainstDbOutput())
            .BDDfy("Test Case ID : ");
        }
        [TestMethod()]
        [TestCategory("UI_FUNCTIONAL")]
        protected void LpnCaseUnlockApi()
        {
            this.Given(x => x.CreateUrlAndInputParamForApiUsing("CaseUnlock"))
            .When(x => x.CallLpnCaseUnlockApiWithInputs(UIConstants.LpnCaseUnlockUrl))
            .And(x => x.VerifyLpnCaseUnlockOutputAgainstDbOutput())
            .BDDfy("Test Case ID : ");
        }
        [TestMethod()]
        [TestCategory("UI_FUNCTIONAL")]
        protected void LpnDetailsApi()
        {
            this.Given(x => x.CreateUrlAndInputParamForApiUsing("Details"))
            .When(x => x.CallLpnDetailsApi(UIConstants.LpnDetailsUrl))
            .And(x => x.VerifyLpnDetailsOutputAgainstDbOutput())
            .BDDfy("Test Case ID : ");
        }
        [TestMethod()]
        [TestCategory("UI_FUNCTIONAL")]
        protected void LpnAddCommentsApi()
        {
            this.Given(x => x.CreateUrlAndInputParamForApiUsing("AddComments"))
            .And(x => x.CreateInputDtoForAddCommentsApi())
            .When(x => x.CallLpnAddCommentsApi(UIConstants.LpnAddCommentsUrl))
            .And(x => x.VerifyCommentsIsInsertedinDb())
            .BDDfy("Test Case ID : ");
        }
        [TestMethod()]
        [TestCategory("UI_FUNCTIONAL")]
        protected void LpnEditCommentsApi()
        {
            this.Given(x => x.CreateUrlAndInputParamForApiUsing("EditComments"))
            .And(x => x.CreateInputDtoForEditCommentApi())
            .When(x => x.CallLpnEditCommentsApi(UIConstants.LpnEditCommentsUrl))
            .And(x => x.VerifyCommentsIsUpdatedInDb())
            .BDDfy("Test Case ID : ");
        }
        [TestMethod()]
        [TestCategory("UI_FUNCTIONAL")]
        protected void LpnDeleteCommentsApi()
        {
            this.Given(x => x.CreateUrlAndInputParamForApiUsing("DeleteComments"))
            .When(x => x.CallLpnDeleteCommentsApi(UIConstants.LpnDeleteCommentsUrl))
            .And(x => x.VerifyCommentsIsDeletedInDb())
            .BDDfy("Test Case ID : ");
        }
        [TestMethod()]
        [TestCategory("UI_FUNCTIONAL")]
        protected void LpnUpdateApi()
        {
            this.Given(x => x.CreateUrlAndInputParamForApiUsing("Update"))
            .And(x => x.CreateInputDtoForLpnUpdateApi())
            .When(x => x.CallLpnUpdateApi(UIConstants.LpnUpdateUrl))
            .And(x => x.VerifyUpdateFieldsAreUpdatedInDb())
            .BDDfy("Test Case ID : ");
        }
        [TestMethod()]
        [TestCategory("UI_FUNCTIONAL")]
        protected void LpnEditItemsApi()
        {
            this.Given(x => x.CreateUrlAndInputParamForApiUsing("Items"))
            .And(x => x.CreateInputDtoForEditItemApi())
            .When(x => x.CallLpnItemsApi(UIConstants.LpnItemsUrl))
            .And(x => x.VerifyItemFieldsAreUpdatedInDb())
            .BDDfy("Test Case ID : ");
        }
        [TestMethod()]
        [TestCategory("UI_FUNCTIONAL")]
        protected void LpnMultiUnlockApi()
        {
            this.Given(x => x.CreateUrlAndInputParamForApiUsing("MultiUnlock"))
            .And(x => x.CreateInputDtoForMultiUnlockApi())
            .When(x => x.CallLpnMultiUnlockApi(UIConstants.LpnMultiUnlockUrl))
            .And(x => x.VerifyLocksAreDeletedinDb())
            .Then(x => x.VerifyCorbaResultFromDbFor("MultiUnlock"))
            .BDDfy("Test Case ID : ");
        }
        [TestMethod()]
        [TestCategory("UI_FUNCTIONAL")]
        protected void LpnMultiLockApi()
        {
            this.Given(x => x.CreateUrlAndInputParamForApiUsing("MultiLock"))
            .And(x => x.CreateInputDtoForMultiLockApi())
            .When(x => x.CallLpnMultiLockApi(UIConstants.LpnMultiLockUrl))
            .And(x => x.VerifyLocksAreAddedInDb())
            .Then(x => x.VerifyCorbaResultFromDbFor("MultiLock"))
            .BDDfy("Test Case ID : ");
        }
        [TestMethod()]
        [TestCategory("UI_FUNCTIONAL")]
        protected void LpnMultiCommentsApi()
        {
            this.Given(x => x.CreateUrlAndInputParamForApiUsing("MultiComments"))
            .And(x => x.CreateInputDtoForMultiCommentsApi())
            .When(x => x.CallLpnMultiCommentsApi(UIConstants.LpnMultiCommentsUrl))
            .And(x => x.VerifyMultiCommentsAreAddedInDb())
            .BDDfy("Test Case ID : ");
        }
        [TestMethod()]
        [TestCategory("UI_FUNCTIONAL")]
        protected void LpnMultiEditApi()
        {
            this.Given(x => x.CreateUrlAndInputParamForApiUsing("MultiEdit"))
            .And(x => x.CreateInputDtoForMultiEditApi())
            .When(x => x.CallLpnMultiEditApi(UIConstants.LpnMultiEditUrl))
            .And(x => x.VerifyMultiEditAreAddedInDb())
            .BDDfy("Test Case ID : ");
        }
    }
}
