using DataGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.App.Api.Tests.Unit.Fakes;
using TestStack.BDDfy.Configuration;
using TestStack.BDDfy.Reporters.Html;

namespace Sfc.Wms.App.Api.Tests.Unit.Configurations
{
    [TestClass]
    public class Setup
    {
        [AssemblyInitialize]
        public static void ApplyBddFyReportConfiguration(TestContext testContext)
        {
            ApplyBddFyReportConfiguration();
            ApplyFakesDataGeneratorConfiguration();
        }

        private static void ApplyBddFyReportConfiguration()
        {
            Configurator.BatchProcessors.HtmlReport.Disable();

            Configurator.BatchProcessors.Add(new HtmlReporter(new HtmlReportConfig("UnitTesting",
                "Sfc_App_Api_Test_Unit.html", "Sfc.Wms.Api.Test.Unit.Controllers", "Sfc Wms Tests",
                "Sfc Wms Scenarios and their Test Results")));

            Configurator.BatchProcessors.Add(new HtmlReporter(new HtmlReportConfig("UnitTesting",
                "Sfc_App_Test_Unit.html", "Sfc.App.Api.Tests.Unit.App", "Sfc Wms Tests",
                "Sfc Wms Scenarios and their Test Results")));

            Configurator.BatchProcessors.Add(new HtmlReporter(new HtmlReportConfig("UnitTesting",
                "Sfc_App_Api_Nuget_Test_Unit.html", "Sfc.App.Api.Tests.Unit.Nuget", "Sfc Wms Tests",
                "Sfc Wms Scenarios and their Test Results")));
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