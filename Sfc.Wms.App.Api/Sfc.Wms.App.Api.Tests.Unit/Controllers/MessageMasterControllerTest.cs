using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.App.Api.Tests.Unit.Constants;
using Sfc.Wms.App.Api.Tests.Unit.Fixtures;

namespace Sfc.Wms.App.Api.Tests.Unit.Controllers
{
    [TestClass, TestCategory(TestCategories.Unit)]
    public class MessageMasterControllerTest : MessageMasterControllerFixture
    {
        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Get_Ui_Specific_Message_Details_Invocation_Returned_Ok_As_Response_Status()
        {
            InputParametersForMessageDetailsRetrieval();
            GetOperationInvoked();
            TheReturnedOkResponse();
        }

    }
}