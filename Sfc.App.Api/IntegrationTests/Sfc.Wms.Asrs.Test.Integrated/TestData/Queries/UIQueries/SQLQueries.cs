using System.Data;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData.Constant;
using Oracle.ManagedDataAccess.Client;
namespace FunctionalTestProject.SQLQueries
{
    public class SqlQueries
    {
        private OracleCommand _command;
        public string GetStringFromDb(string sql, OracleConnection db)
        {
            _command = new OracleCommand(sql, db);
            var temp = _command.ExecuteScalar();
            if (temp != null)               
                return temp.ToString();
            else return "";
        }
        public DataTable GetDataTableFromDb(string sql, OracleConnection db)
        {
            _command = new OracleCommand(sql, db);
            var tempDt = new DataTable();
            tempDt.Load(_command.ExecuteReader());
            return UIConstants.UpdateTable(tempDt, false);
        }
        public DataTable GetDataTableFromDbForGrid(string sql, OracleConnection db)
        {
            _command = new OracleCommand(sql, db);
            var tempDt = new DataTable();
            tempDt.Load(_command.ExecuteReader());
            return UIConstants.UpdateTable(tempDt, true);
        }
    }
}
