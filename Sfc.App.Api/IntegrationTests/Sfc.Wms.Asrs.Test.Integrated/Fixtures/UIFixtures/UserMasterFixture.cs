using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using Sfc.Core.OnPrem.Result;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData.Queries.UIQueries;
using System.Collections.Generic;
using Sfc.Core.OnPrem.Security.Contracts.Dtos;
using System.Configuration;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData.Constant;
using RestSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Sfc.Core.OnPrem.Pagination.Extensions;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures.UIFixtures
{
    public class UserMasterFixture:BaseFixture
    {
        PreferencesDto preferencesDto;
        IRestResponse response;
        public void FetchUserPreferenceIdIfAnyFromDb()
        {
            using (var db = new OracleConnection())
            {
                db.ConnectionString = ConfigurationManager.ConnectionStrings["SfcRbacContextModel"].ToString();
                db.Open();
                var _command = new OracleCommand(UIApiQueries.FetchUserPrefIdSql, db);
                var Id= _command.ExecuteScalar();
                if (Id.IsNullOrDefault())
                    Id = 0;
                UIConstants.PreferenceId = Convert.ToInt32(Id);

            }
         }
        public void CreateUserMasterDto()
        {
            preferencesDto = new PreferencesDto()
            {
                Id = UIConstants.PreferenceId,
                UserId = 355,
                SettingId = 4,
                AllowedSettingValueId = null,
                UnconstrainedValue = UIConstants.UnconstrainedValue
            };
        }
        public void CallUserMasterApi(string url)
        {
            var request = CallPostApi();
            request.AddJsonBody(new List<PreferencesDto> { preferencesDto });
            response = ExecuteRequest(url, request);
            VerifyOkResultAndStoreBearerToken(response);
        }
       public void  VerifyUserMasterApiOutputAgainstDbOutput(int Id)
        {
            var payload = JsonConvert.DeserializeObject<BaseResult<List<PreferencesDto>>>(response.Content).Payload;
            var Dt = ToDataTable(payload);          
            Assert.IsTrue(Dt.Rows.Count == 1);
            UIConstants.PreferenceId = Convert.ToInt32(Dt.Rows[0][0].ToString());
            Assert.AreEqual(Id,UIConstants.PreferenceId);
        }
    }
}
