using System.Configuration;
using System.Diagnostics;
using System.IO;
using Sfc.Wms.Data.Context;

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
            if (ConfigurationManager.AppSettings["EnableQueryLogging"] == "true")
            {
                _fileWriter.WriteLine(message);
                _fileWriter.Flush();
            }
        }
    }
}