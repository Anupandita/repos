using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.App.Api.Tests.Unit.Constants;
using Sfc.Wms.App.Api.Tests.Unit.Fixtures;

namespace Sfc.Wms.App.Api.Tests.Unit.Nuget
{
    [TestClass]
    [TestCategory(TestCategories.Unit)]
    public class ItemAttributeGatewayTest : ItemAttributeFixture
    {
        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Search_Operation_Returned_Ok_As_Response_Status()
        {
            ValidInputDataForSearch();
            AttributeSearchOperationInvoked();
            SearchReturnedOkAsResponse();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Search_Operation_Returned_BadRequest_As_Response_Status()
        {
            EmptyOrInvalidInputForSearch();
            AttributeSearchOperationInvoked();
            SearchReturnedBadRequestAsResponse();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void AttributeDrillDown_Operation_Returned_Ok_As_Response_Status()
        {
            ValidInputDataForAttributeDrillDown();
            AttributeDrillDownOperationInvoked();
            AttributeDrillDownReturnedOkAsResponse();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void AttributeDrillDown_Operation_Returned_BadRequest_As_Response_Status()
        {
            EmptyOrInvalidInputForAttributeDrillDown();
            AttributeDrillDownOperationInvoked();
            AttributeDrillDownReturnedBadRequestAsResponse();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void AttributeDrillDown_Operation_Returned_NotFound_Response_Status()
        {
            ValidInputDataForAttributeDrillDownForWhichNoRecordExists();
            AttributeDrillDownOperationInvoked();
            AttributeDrillDownReturnedNotFoundAsResponse();
        }
    }
}