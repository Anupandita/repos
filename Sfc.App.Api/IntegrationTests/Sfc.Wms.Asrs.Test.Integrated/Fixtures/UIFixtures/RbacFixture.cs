using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using RestSharp;
using Sfc.Core.OnPrem.Result;
using Sfc.Core.OnPrem.Security.Contracts.Dtos;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData.Constant;
using System.Configuration;
using System.Data;
using System.Linq;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures.UIFixtures
{
    public class RbacFixture:BaseFixture
    {
        LoginCredentials loginCredentials;
        IRestResponse response;

        DataTable MenusDt = new DataTable();
        DataTable PermissionsDt = new DataTable();
        DataTable PreferencesDt = new DataTable();
        DataTable PrintersDt = new DataTable();

        DataTable MenusApiDt = new DataTable();
        DataTable PermissionsApiDt = new DataTable();
        DataTable PreferencesApiDt = new DataTable();
        DataTable PrintersApiDt = new DataTable();

        protected void FetchDataFromDb()
        {
            using (var db = new OracleConnection())
            {
            db.ConnectionString = ConfigurationManager.ConnectionStrings["SfcRbacContextModel"].ToString();
            db.Open();
            var sql1 = $"select sm.menu_id MenuId,menu_name MenuName,menu_url MenuUrl from swm_role_menus srm inner join swm_menus sm on sm.menu_id=srm.menu_id where ROLE_ID in (select role_id from swm_user_role where user_name = 'PSI' )ORDER BY sm.disp_order ASC";
            var command = new OracleCommand(sql1, db);
            MenusDt.Load(command.ExecuteReader());           
            

            sql1 = $"select PermissionId,sp.name,RoleId from (select  permission_id PermissionId,min(role_id) RoleId from swm_role_menus_perm where ROLE_ID in (select role_id from swm_user_role where user_name = 'PSI' )group by permission_id) inner join swm_permissions sp on sp.permission_id=PermissionId order by RoleId desc ,PermissionId asc";
            command = new OracleCommand(sql1, db);           
            PermissionsDt.Load(command.ExecuteReader());           
           

            sql1 = $"select Id,User_id UserId,setting_id SettingId,allowed_setting_value_id AllowedSettingValueId,Unconstrained_value UnconstrainedValue from swm_user_setting  where user_id = '355' ORDER BY setting_id ASC";
            command = new OracleCommand(sql1, db);
            PreferencesDt.Load(command.ExecuteReader());
          

            sql1 = $"select code_id Id,Code_desc Description, misc_flags displayName from sys_code where rec_type = 'C' and code_type = '205' order by Code_desc asc";
            command = new OracleCommand(sql1, db);
            PrintersDt.Load(command.ExecuteReader());
          
            }
}
        protected void CreateLoginDtoUsingCredentials()
        {
            loginCredentials = new LoginCredentials()
            {
                UserName = UIConstants.UserName,
                Password = UIConstants.Password
            };
        }
        protected void CallLoginApi(string url) 
        {
            var request = CallPostApi();
            request.AddJsonBody(loginCredentials);
            response = ExecuteRequest(url, request);
            VerifyOkResultAndStoreBearerToken(response);
            var payload = JsonConvert.DeserializeObject<BaseResult<UserInfoDto>>(response.Content).Payload;
            MenusApiDt = ToDataTable(payload.Menus);
            PermissionsApiDt = ToDataTable(payload.Permissions);
            PrintersApiDt = ToDataTable(payload.PrinterList.ToList());
            PreferencesApiDt = ToDataTable(payload.Preferences);
        }
        protected void VerifyMenusListAgainstDb()
        {
            VerifyApiOutputAgainstDbOutput(MenusDt, MenusApiDt);
        }
        protected void VerifyPermissionsListAgainstDb()
        {
            VerifyApiOutputAgainstDbOutput(PermissionsDt, PermissionsApiDt);
        }
        protected void VerifyPrintersListAgainstDb()
        {
            VerifyApiOutputAgainstDbOutput(PrintersDt, PrintersApiDt);
        }
        protected void VerifyPreferencesListAgainstDb()
        {
            VerifyApiOutputAgainstDbOutput(PreferencesDt, PreferencesApiDt);
        }
    }
}
