using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.App.Api.Tests.Unit.Constants;
using Sfc.Wms.App.Api.Tests.Unit.Fixtures;

namespace Sfc.Wms.App.Api.Tests.Unit.Nuget
{
    [TestClass]
    public class UserMasterGatewayTest : UserMasterGatewayFixture
    {

        [TestMethod, TestCategory(TestCategories.Unit)]
        public void Update_User_Preferences_Returned_Ok_Response()
        {
            ValidParametersToUpdateUserPreferences();
            UpdateUserPreferencesInvoked();
            UpdateUserPreferencesReturnedOkResponse();
        }

        [TestMethod, TestCategory(TestCategories.Unit)]
        public void Update_User_Preferences_Returned_NotFound_Response()
        {
            InvalidParametersToUpdateUserPreferences();
            UpdateUserPreferencesInvoked();
            UpdateUserPreferencesReturnedNotFoundResponse();
        }
    }
}
