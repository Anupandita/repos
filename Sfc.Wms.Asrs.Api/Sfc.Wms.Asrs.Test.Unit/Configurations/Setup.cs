using DataGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.Asrs.Test.Unit.Fakes;
using TestStack.BDDfy.Configuration;
using TestStack.BDDfy.Reporters.Html;
using Sfc.Wms.Asrs.Test.Unit.Constants;

namespace Sfc.Wms.Asrs.Test.Unit.Configurations
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

            Configurator.BatchProcessors.Add(new HtmlReporter(new HtmlReportConfig(CustomMessages.TestFolderName,
                "Asrs Interface.html", "Sfc.Wms.Asrs.Test.Unit.Controllers", "Sfc Wms Asrs Tests",
                "Sfc Wms Asrs Interface Scenarios and their Test Results")));

            Configurator.BatchProcessors.Add(new HtmlReporter(new HtmlReportConfig(CustomMessages.TestFolderName,
                "Asrs Dematic Service.html", "Sfc.Wms.Asrs.Test.Unit.Services", "Sfc Wms Asrs Service Tests",
                "Sfc Wms Asrs Interface Service and their Test Results")));

            Configurator.BatchProcessors.Add(new HtmlReporter(new HtmlReportConfig(CustomMessages.TestFolderName,
                "Asrs Dematic Repository.html", "Sfc.Wms.Asrs.Test.Unit.Repository", "Sfc Wms Asrs Repository Tests",
                "Sfc Wms Asrs Interface Repository and their Test Results")));

            Configurator.BatchProcessors.Add(new HtmlReporter(new HtmlReportConfig(CustomMessages.TestFolderName,
                "Asrs Interface Nuget.html", "Sfc.Wms.Asrs.Test.Unit.Nuget", "Sfc Wms Asrs Nuget Tests",
                "Sfc Wms Interface Nuget and their Test Results")));
        }

        private static void ApplyFakesDataGeneratorConfiguration()
        {
            Generator.Default.Configure(c => c.Profile<SwmFromMheDtoProfile>());
            Generator.Default.Configure(c => c.Profile<SwmToMheDtoProfile>());
            Generator.Default.Configure(c => c.Profile<SwmFromMheProfile>());
            Generator.Default.Configure(c => c.Profile<EmsToWmsDtoProfile>());
            Generator.Default.Configure(c => c.Profile<WmsToEmsDtoProfile>());
            Generator.Default.Configure(c => c.Profile<EmsToWmsProfile>());
            Generator.Default.Configure(c => c.Profile<IvmtTriggerDtoMapping>());
            Generator.Default.Configure(c => c.Profile<PickLocationDtlDtoProfile>());
            Generator.Default.Configure(c => c.Profile<CostDtoMapping>());
        }
    }
}