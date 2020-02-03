using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.App.Api.Tests.Unit.Constants;
using Sfc.Wms.App.Api.Tests.Unit.Fixtures;

namespace Sfc.Wms.App.Api.Tests.Unit.Controllers
{
    [TestClass]
    public class UserMasterControllerTest : UserMasterControllerFixture
    {
        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Update_User_Preferences_Returned_Ok_Response()
        {
            UpdateUserPreferencesExist();
            UpdateUserPreferenceInvoked();
            UpdateUserPreferencesReturnedOkResponse();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Update_User_Preferences_Returned_NotFound_Response()
        {
            UpdateUserPreferencesNotExist();
            UpdateUserPreferenceInvoked();
            UpdateUserPreferencesReturnedNotFoundResponse();
        }
    }
}
