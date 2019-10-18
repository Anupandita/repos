using DataGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.App.Api.Tests.Unit.Fakes;

namespace Sfc.Wms.App.Api.Tests.Unit.Configurations
{
    [TestClass]
    public class Setup
    {
        [AssemblyInitialize]
        public static void ApplyBddFyReportConfiguration(TestContext testContext)
        {
            ApplyFakesDataGeneratorConfiguration();
        }

        private static void ApplyFakesDataGeneratorConfiguration()
        {
            Generator.Default.Configure(c => c.Profile<LoginCredentialsDataGeneratorProfile>());
            Generator.Default.Configure(c => c.Profile<RolesDataGeneratorProfile>());
            Generator.Default.Configure(c => c.Profile<MenusDtoDataGeneratorProfile>());
            Generator.Default.Configure(c => c.Profile<PreferencesDataGeneratorProfile>());
            Generator.Default.Configure(c => c.Profile<PermissionDataGeneratorProfile>());
            Generator.Default.Configure(c => c.Profile<UserInfoDtoDataGeneratorProfile>());
            Generator.Default.Configure(c => c.Profile<MenuDetailsDtoDataGeneratorProfile>());
        }
    }
}