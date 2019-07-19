using System.IO;
using System.Linq;
using TestStack.BDDfy;
using TestStack.BDDfy.Reporters.Html;

namespace Sfc.Wms.Asrs.Dematic.Test.Unit.Configurations
{
    class MappingHtmlReportConfig : DefaultHtmlReportConfiguration
    {
        private readonly string _filename;
        private readonly string _foldername;
        private readonly string _namespace;
        private readonly string _reportDescription;
        private readonly string _reportHeader;


        public MappingHtmlReportConfig(string foldername, string filename, string nameSpace, string reportHeader,
            string reportDescription)
        {
            _foldername = foldername;
            _filename = filename;
            _namespace = nameSpace;
            _reportHeader = reportHeader;
            _reportDescription = reportDescription;
        }

        public override string ReportHeader => _reportHeader;
        public override string ReportDescription => _reportDescription;

        public override string OutputPath
        {
            get
            {
                var path = Path.Combine(@"../../Reports/", _foldername);
                Directory.CreateDirectory(path);
                return path;
            }
        }

        public override string OutputFileName => _filename;

        public override bool RunsOn(Story story)
        {
            return story.Metadata.Type.Namespace != null && story.Metadata.Type.Namespace.Contains(_namespace);
        }

    }

}
