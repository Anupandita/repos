using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestStack.BDDfy.Configuration;
using TestStack.BDDfy.Reporters.Html;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Configurations
{
    [TestClass]
    public class Setup
    {
        [AssemblyInitialize]
        public static void ApplyBddFyReportConfiguration(TestContext testContext)
        {
            ApplyBddFyReportConfiguration();
        }

        private static void ApplyBddFyReportConfiguration()
        {
            Configurator.BatchProcessors.HtmlReport.Disable();
            //Configure your test namespace to check the BDDFY report.
            Configurator.BatchProcessors.Add(new HtmlReporter(new HtmlReportConfig("IntegrationTesting",
                "Sfc_Wms_Asrs_Test_Integration.html", "Sfc.Wms.Api.Asrs.Test.Integrated.Tests", "Sfc WMS Tests",
                "Sfc WMS Scenarios and their Test Results")));

        }
    }
}