using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.App.Api.Tests.Unit.Constants;
using Sfc.Wms.App.Api.Tests.Unit.Fixtures;

namespace Sfc.Wms.App.Api.Tests.Unit.Controllers
{
    [TestClass]
    [TestCategory(TestCategories.Unit)]
    public class ItemAttributeControllerTest : ItemAttributeControllerFixture
    {

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void AttributeSearch_Operation_Returned_Ok_As_Response_Status()
        {
            ValidInputDataForAttributeSearch();
            AttributeSearchOperationInvoked();
            AttributeSearchReturnedOkAsResponse();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void AttributeSearch_Operation_Returned_BadRequest_As_Response_Status()
        {
            EmptyOrInvalidInputForAttributeSearch();
            AttributeSearchOperationInvoked();
            AttributeSearchReturnedBadRequestAsResponse();
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