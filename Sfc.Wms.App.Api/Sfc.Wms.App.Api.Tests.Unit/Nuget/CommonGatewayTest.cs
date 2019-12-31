using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.App.Api.Tests.Unit.Constants;
using Sfc.Wms.App.Api.Tests.Unit.Fixtures;

namespace Sfc.Wms.App.Api.Tests.Unit.Nuget
{
    [TestClass]
    public class CommonGatewayTest : CommonGatewayFixture
    {
        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void GetCodeIds_Operation_Returned_Ok_As_Response_Status()
        {
            InputParametersToGetCodeIds();
            GetCodeIdsOperationInvoked();
            TheGetCodeIdsOperationReturnedOkAsResponseStatus();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void GetCodeIds_Operation_Returned_BadRequest_As_Response_Status()
        {
            InValidInputParametersToGetCodeIds();
            GetCodeIdsOperationInvoked();
            TheGetCodeIdsOperationReturnedBadRequestAsResponseStatus();
        }

    }
}
