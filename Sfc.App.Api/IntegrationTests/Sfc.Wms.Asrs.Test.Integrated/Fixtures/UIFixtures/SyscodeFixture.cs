using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using Sfc.Core.OnPrem.Result;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData.Constant;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData.Queries.UIQueries;
using Sfc.Wms.Configuration.SystemCode.Contracts.Dtos;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures.UIFixtures
{
    public class SysCodeFixture : BaseFixture
    {
        DataTable SysCodeDt = new DataTable();
        DataTable SysCodeApiDt = new DataTable();

        public void CreateSysCodeApiUrl()
        {
            UIConstants.SysCodeUrl = UIConstants.SysCodeUrl + UIConstants.SysCodeInputRecType + UIConstants.RecType + "&" + UIConstants.SysCodeInputCodeType + UIConstants.CodeType + "&" + UIConstants.SysCodeInputSort + UIConstants.Sort;
        }
        public void GetSysCodeRecordsFromDbForRecTypeCodeType(string recType, string codeType)
        {
            using (var db = new OracleConnection())
            {
                db.ConnectionString = ConfigurationManager.ConnectionStrings["SfcRbacContextModel"].ToString();
                db.Open();
                var _command = new OracleCommand(UIApiQueries.SysCodeSql, db);
                SysCodeDt.Load(_command.ExecuteReader());
            }
        }

        public void CallSysCodeApi(string url)
        {
            var response = CallGetApi(url);
            VerifyOkResultAndStoreBearerToken(response);
            var payload = JsonConvert.DeserializeObject<BaseResult<List<SysCodeDto>>>(response.Content).Payload;
            SysCodeApiDt = ToDataTable(payload);
        }
        public void VerifySysCodeApiOutputAgainstDbOutput()
        {
            VerifyApiOutputAgainstDbOutput(SysCodeDt, SysCodeApiDt);
        }
    }
}
