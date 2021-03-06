using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.App.Api.Tests.Unit.Constants;
using Sfc.Wms.App.Api.Tests.Unit.Fixtures;

namespace Sfc.Wms.App.Api.Tests.Unit.Controllers
{

    [TestClass]
    public class CommonControllerTest : CommonControllerFixture
    {
        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void GetSystemCodes_Operation_Returned_Ok_As_Response_Status()
        {
            InputParametersToGetSystemCodes();
            GetSystemCodesOperationInvoked();
            TheGetSystemCodesOperationReturnedOkResponse();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void GetSystemCodes_Operation_Returned_BadRequest_As_Response_Status()
        {
            InValidInputParametersToGetSystemCodes();
            GetSystemCodesOperationInvoked();
            TheGetSystemCodesOperationReturnedBadRequestResponse();
        }

    }
}
