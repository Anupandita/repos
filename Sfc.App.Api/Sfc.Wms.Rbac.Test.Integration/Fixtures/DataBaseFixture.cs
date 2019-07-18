using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Diagnostics;

namespace Sfc.Wms.UserRbac.Test.Integrated.Fixtures
{
    public class DataBaseFixture
    {
        protected int dbMenuCount;
        protected int dbPermissionCount;
        protected int dbPreferenceCount;
        protected List<string> dbMenus;
        protected List<string> dbPermissions;
        protected List<string> dbPreferences;
        public DataTable Dt = new DataTable();
      
        public void GetDataFromDatabase()
        {
            var sql1 = "";
            OracleConnection db;
            OracleCommand command;
            using (db = new OracleConnection
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["SfcRbacContextModel"].ConnectionString
            })
            {
                db.Open();
                sql1 = $"select distinct menu_id from swm_role_menus  where ROLE_ID in (select role_id from swm_user_role where user_name = 'PSI' )ORDER BY menu_id ASC";
                command = new OracleCommand(sql1, db);
                DataTable MenuIds = new DataTable();
                MenuIds.Load(command.ExecuteReader());
                dbMenus = new List<string>();
                foreach (DataRow dr in MenuIds.Rows)
                {
                    dbMenus.Add(dr[0].ToString());                   
                }
                dbMenuCount = dbMenus.Count;

                sql1 = $"select distinct permission_id from swm_role_menus_perm  where ROLE_ID in (select role_id from swm_user_role where user_name = 'PSI' )ORDER BY permission_id ASC";
                command = new OracleCommand(sql1, db);
                DataTable PermissionIds = new DataTable();
                PermissionIds.Load(command.ExecuteReader());
                dbPermissions = new List<string>();
                foreach (DataRow dr in PermissionIds.Rows)
                {
                    dbPermissions.Add(dr[0].ToString());                  
                }
                dbPermissionCount = dbPermissions.Count;

                sql1 = $"select distinct setting_id from swm_user_setting  where user_id = '355' ORDER BY setting_id ASC";
                command = new OracleCommand(sql1, db);
                DataTable SettingIds = new DataTable();
                SettingIds.Load(command.ExecuteReader());
                dbPreferences = new List<string>();
                foreach (DataRow dr in SettingIds.Rows)
                {
                    dbPreferences.Add(dr[0].ToString());                 
                }
                dbPreferenceCount = dbPreferences.Count;
            }
        }
    }
}
