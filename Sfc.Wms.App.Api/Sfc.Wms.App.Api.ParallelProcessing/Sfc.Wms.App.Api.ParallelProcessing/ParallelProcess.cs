using Oracle.ManagedDataAccess.Client;
using Sfc.Core.OnPrem.Result;
using Sfc.Wms.Interfaces.Asrs.Contracts.Interfaces;
using Sfc.Wms.Interfaces.Asrs.Dematic.Contracts.Dtos;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using System;
using System.Collections.Concurrent;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace Sfc.Wms.App.Api.ParallelProcessing
{
    public class ParallelProcess : IParallelProcess
    {
        private bool isAwake;
        private int processCount;

        private BlockingCollection<EmsToWmsDto> GetEmsToWmsData()
        {
            var emsToWmsList = new BlockingCollection<EmsToWmsDto>();
            var connectionString = ConfigurationManager.ConnectionStrings["SfcOracleDbContext"].ConnectionString;
            var connection = new OracleConnection(connectionString);
            var cmd = new OracleCommand
            {
                CommandText = "select * from  emstowms where sts='Ready'",
                Connection = connection
            };
            connection.Open();
            var dr = cmd.ExecuteReader();
            if (!dr.HasRows) return emsToWmsList;
            while (dr.Read())
            {
                var emsToWms = new EmsToWmsDto()
                {
                    MessageKey = Convert.ToInt64(dr["MSGKEY"]),
                    Process = Convert.ToString(dr["PRC"])
                };
                emsToWmsList.Add(emsToWms);
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

        private int StartParallelProcess(BlockingCollection<EmsToWmsDto> messageKeyList, Container container)
        {
            if (!int.TryParse(ConfigurationManager.AppSettings.Get("ParallelThreads"), out var maxDegreeOfParallelism))
                maxDegreeOfParallelism = 4;
            var po = new ParallelOptions { MaxDegreeOfParallelism = maxDegreeOfParallelism };
            Parallel.ForEach(messageKeyList, po, e =>
            {
                using (AsyncScopedLifestyle.BeginScope(container))
                {
                    var emsToWmsService = container.GetInstance<IEmsToWmsMessageProcessorService>();
                    var result = emsToWmsService.GetMessageAsync(e.MessageKey, e.Process).Result;
                    if (result.ResultType == ResultTypes.Created) processCount++;
                }
            });

            return CheckData(container);
        }
    }
}