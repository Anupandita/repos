using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.App.Api.Tests.Unit.Constants;
using Sfc.Wms.App.Api.Tests.Unit.Fixtures;

namespace Sfc.Wms.App.Api.Tests.Unit.Nuget
{
    [TestClass]
    public class MessageMasterGatewayTest : MessageMasterGatewayFixture
    {
        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void GetMessageDetails_Operation_Returned_Ok_As_Response_Status()
        {
            InputForUiSpecificMessageDetails();
            GetMessageDetailsOperationInvoked();
            TheGetOperationReturnedOkAsResponseStatus();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void GetMessageDetails_Operation_Returned_BadRequest_As_Response_Status()
        {
            InputForWhichNoRecordsExists();
            GetMessageDetailsOperationInvoked();
            TheGetOperationReturnedBadRequestAsResponseStatus();
        }

    }
}