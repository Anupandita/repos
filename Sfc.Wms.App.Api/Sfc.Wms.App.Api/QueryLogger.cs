using System.Configuration;
using System.IO;

namespace Sfc.Wms.App.Api
{
    public class QueryLogger
    {
        private readonly StreamWriter _fileWriter;

        public QueryLogger(StreamWriter fileWriter)
        {
            _fileWriter = fileWriter;
        }

        public void Write(string message)
        {
            if (ConfigurationManager.AppSettings["EnableQueryLogging"] != "true") return;
            _fileWriter.WriteLine(message);
            _fileWriter.Flush();
        }
    }
}