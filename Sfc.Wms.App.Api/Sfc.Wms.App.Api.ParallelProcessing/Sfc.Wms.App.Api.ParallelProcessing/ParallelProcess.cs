using System;
using System.Collections.Concurrent;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using Sfc.Core.OnPrem.Result;
using Sfc.Wms.Interfaces.Asrs.Contracts.Interfaces;
using Sfc.Wms.Interfaces.Asrs.Dematic.Contracts.EnumsAndConstants.Enums;
using Sfc.Wms.Interfaces.Asrs.Dematic.Contracts.Interfaces;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace Sfc.Wms.App.Api.ParallelProcessing
{
    public class ParallelProcess : IParallelProcess
    {
        private bool isAwake;
        private int processCount=0;

        private BlockingCollection<long> GetEmsToWmsData()
        {
            var emsToWmsList = new BlockingCollection<long>();
            var connectionString = ConfigurationManager.ConnectionStrings["SfcOracleDbContext"].ConnectionString;
            var connection = new OracleConnection(connectionString);
            var cmd = new OracleCommand
            {
                CommandText = "select * from  emstowms where sts='Ready' and msgkey in (16631,16646,11437,16641,17635,12426,17630,26626,21417,26621,12417,17626,17621)",
                Connection = connection
            };
            connection.Open();
            var dr = cmd.ExecuteReader();
            if (!dr.HasRows) return emsToWmsList;
            while (dr.Read())
            {
                emsToWmsList.Add(Convert.ToInt64(dr["MSGKEY"]));
            }
            connection.Close();
            return emsToWmsList;
        }

        public int WakeUp(Container container)
        {
            if (isAwake) return processCount;
            isAwake = true;
            return CheckData(container);
        }

        private int CheckData(Container container)
        {
            var messageKeyList = GetEmsToWmsData();
            if (messageKeyList.Any()) 
                return StartParallelProcess(messageKeyList, container);
            isAwake = false;
            return processCount;

        }

        private int StartParallelProcess(BlockingCollection<long> messageKeyList, Container container)
        {
            if (!int.TryParse(ConfigurationManager.AppSettings.Get("ParallelThreads"), out var maxDegreeOfParallelism))
                maxDegreeOfParallelism = 4;
            var po = new ParallelOptions { MaxDegreeOfParallelism = maxDegreeOfParallelism };
            Parallel.ForEach(messageKeyList, po, i =>
            {
                using (AsyncScopedLifestyle.BeginScope(container))
                {
                    var emsToWmsService = container.GetInstance<IEmsToWmsMessageProcessorService>();
                    var result = emsToWmsService.GetMessageAsync(i).Result;
                    if (result.ResultType == ResultTypes.Created) processCount++;
                }
            });

           return CheckData(container);
        }
    }
}