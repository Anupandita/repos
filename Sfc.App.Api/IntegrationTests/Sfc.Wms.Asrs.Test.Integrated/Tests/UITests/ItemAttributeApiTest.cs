using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures.UIFixtures;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData.Constant;
using TestStack.BDDfy;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Tests.UITests
{
    [TestClass]
    [Story(
         Title = "Item Attribute Api Testing",
         AsA = "Authorized User test for Item Attribute Page Apis",
         IWant = "To Test for all Apis Used on Item Attribute Page on Web UI ",
         SoThat = "I can vaidate Data that is going to be displayed on UI Grid and Pages",
         StoryUri = ""
          )]

    public class ItemAttributeApiTest: ItemAttributeFixture
    {
        [TestInitialize]
        public void AValidTestData()
        {
            
            LoginToFetchToken();
            PickAnItemTestDataFromDbFromDB();
        }

        [TestMethod()]
        [TestCategory("UI_FUNCTIONAL")]
        public void ItemAttributeSearchApi_SearchWithItemNbr() 
        {
            this.Given(x => x.CreateUrlAndInputParamForApiUsing("Item"))
            .When(x => x.CallItemAttributeSearchApiWithInputs(ItemAttributeSearchUrl))
            .And(x => x.VerifyOutputAgainstDbOutput())
            .BDDfy("Test Case ID : ");
        }
        [TestMethod()]
        [TestCategory("UI_FUNCTIONAL")]
        public void ItemAttributeSearchApi_SearchWithItemDescription()
        {
            this.Given(x => x.CreateUrlAndInputParamForApiUsing("ItemDescription"))
            .When(x => x.CallItemAttributeSearchApiWithInputs(ItemAttributeSearchUrl))
            .And(x => x.VerifyOutputAgainstDbOutput())
            .BDDfy("Test Case ID : ");
        }
        [TestMethod()]
        [TestCategory("UI_FUNCTIONAL")]
        public void ItemAttributeSearchApi_SearchWithVendorItemNbr()
        {
            this.Given(x => x.CreateUrlAndInputParamForApiUsing("VendorItemNumber"))
            .When(x => x.CallItemAttributeSearchApiWithInputs(ItemAttributeSearchUrl))
            .And(x => x.VerifyOutputTotalReordsAgainstDbCount(UIConstants.VendorItemNumberCount))
            .BDDfy("Test Case ID : ");
        }
        [TestMethod()]
        [TestCategory("UI_FUNCTIONAL")]
        public void ItemAttributeSearchApi_SearchWithTempZone()
        {
            this.Given(x => x.CreateUrlAndInputParamForApiUsing("TempZone"))
            .When(x => x.CallItemAttributeSearchApiWithInputs(ItemAttributeSearchUrl))
            .And(x => x.VerifyOutputTotalReordsAgainstDbCount(UIConstants.TempZoneCount))
            .BDDfy("Test Case ID : ");
        }
    }
}
