using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using Sfc.Wms.Interfaces.Asrs.Dematic.Contracts.Dtos;

namespace DataGenerator.Fixtures
{
    public class WmsToEmsFixture
    {
        public IEnumerable<WmsToEmsDto> GetList(string where)
        {
            var wmsToEmsDtos = new List<WmsToEmsDto>();
            var query = $"select * from wmstoems where {where}";
            using (var dbConnection = GetOracleConnection())
            {
                dbConnection.Open();
                var command = new OracleCommand(query, dbConnection);

                var wmsToEmsReader = command.ExecuteReader();
                while (wmsToEmsReader.Read())
                {
                    var wmsToEmsDto = new WmsToEmsDto
                    {
                        Status = wmsToEmsReader["STS"].ToString(),
                        Process = wmsToEmsReader["PRC"].ToString(),
                        MessageKey = Convert.ToInt32(wmsToEmsReader["MSGKEY"]),
                        Transaction = wmsToEmsReader["TRX"].ToString(),
                        MessageText = wmsToEmsReader["MSGTEXT"].ToString(),
                        ResponseCode = Convert.ToInt32(wmsToEmsReader["RSNRCODE"]),
                        ZplData = wmsToEmsReader["ZPLDATA"].ToString(),
                    };
                    wmsToEmsDtos.Add(wmsToEmsDto);
                }
            }
            return wmsToEmsDtos;
        }

        public void UpdateStatusToProcessed(WmsToEmsDto wmsToEmsDto)
        {
            var status = "Processed";
            var query =
                $"UPDATE WMSTOEMS SET STS='{status}' where PRC='{wmsToEmsDto.Process}' and MSGKEY='{wmsToEmsDto.MessageKey}'";
            using (var dbConnection = GetOracleConnection())
            {
                dbConnection.Open();
                var command = new OracleCommand(query, dbConnection);
                var commandStatus = command.ExecuteNonQuery();
                var commandStatus2 = commandStatus;
            }
        }

        //method not tested
        public string BuildWhereStringFromExpression<T>(Expression<Func<T, bool>> exp)
        {
            string expBody = ((LambdaExpression)exp).Body.ToString();

            var paramName = exp.Parameters[0].Name;
            var paramTypeName = exp.Parameters[0].Type.Name;

            expBody = expBody.Replace(paramName + ".", paramTypeName + ".")
                .Replace("AndAlso", "&&");
            return expBody;
        }

        public OracleConnection GetOracleConnection()
        {
            return new OracleConnection(ConfigurationManager.ConnectionStrings["SfcRbacContextModel"].ConnectionString);
        }
    }
}