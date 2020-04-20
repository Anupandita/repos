using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData;
using Sfc.Wms.Configuration.ItemMasters.Contracts.Dtos;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData.Constant;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using Sfc.Core.OnPrem.Result;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures.UIFixtures
{
    public class ItemAttributeFixture:BaseFixture
    {

        protected string ItemAttributeSearchUrl;
        protected int TotalRecords = 0;
        protected DataTable itemAttributeSearchQueryDt=new DataTable();
        protected DataTable itemAttributeSearchResultDt = new DataTable();
        public void PickAnItemTestDataFromDbFromDB()
        {
            using (var db = new OracleConnection())
            {
                db.ConnectionString = ConfigurationManager.ConnectionStrings["SfcRbacContextModel"].ToString();
                db.Open();
                var _command = new OracleCommand(ItemAttributeQueries.FetchItemNumberInItemAttributeSql, db);
                UIConstants.ItemNumber = _command.ExecuteScalar().ToString();
                 _command = new OracleCommand(ItemAttributeQueries.FetchItemDescriptionSql, db);
                UIConstants.ItemDescription = _command.ExecuteScalar().ToString();
                _command = new OracleCommand(ItemAttributeQueries.FetchVendorItemNumberSql, db);
                DataTable tempDt = new DataTable();
                tempDt.Load(_command.ExecuteReader());
                UIConstants.VendorItemNumber = tempDt.Rows[0][0].ToString();
                UIConstants.VendorItemNumberCount= tempDt.Rows[0][1].ToString();
                tempDt.Clear();
                tempDt.Columns.Clear();

                _command = new OracleCommand(ItemAttributeQueries.FetchTempZoneSql, db);
                tempDt.Load(_command.ExecuteReader());
                UIConstants.TempZone = tempDt.Rows[0][0].ToString();
                UIConstants.TempZoneCount = tempDt.Rows[0][1].ToString();

                _command = new OracleCommand(ItemAttributeQueries.FetchItemPageGridDtSql(), db);
                itemAttributeSearchQueryDt.Load(_command.ExecuteReader());           
             
            }
        }

        public void CreateUrlAndInputParamForApiUsing(string criteria)
        {
            switch (criteria)
            {
                case "Item":
                    ItemAttributeSearchUrl = ConfigurationManager.AppSettings["BaseUrl"] + UIConstants.ItemAttributes + UIConstants.Search + UIConstants.SearchInputItemId + UIConstants.ItemNumber;
                    return;
                case "ItemDescription":
                    ItemAttributeSearchUrl = ConfigurationManager.AppSettings["BaseUrl"] + UIConstants.ItemAttributes + UIConstants.Search + UIConstants.SearchInputItemDescription + UIConstants.ItemDescription;
                    return;
                case "VendorItemNumber":
                    ItemAttributeSearchUrl = ConfigurationManager.AppSettings["BaseUrl"] + UIConstants.ItemAttributes + UIConstants.Search + UIConstants.SearchInputVendorItemNumber + UIConstants.VendorItemNumber;
                    return;
                case "TempZone":
                    ItemAttributeSearchUrl = ConfigurationManager.AppSettings["BaseUrl"] + UIConstants.ItemAttributes + UIConstants.Search + UIConstants.SearchInputTempZone + UIConstants.TempZone;
                    return;
            }
        }
        public void CallItemAttributeSearchApiWithInputs(string url)
        {
            var response=CallGetApi(url);
            VerifyOkResultAndStoreBearerToken(response);
            var payload = JsonConvert.DeserializeObject<BaseResult<ItemAttributeSearchResultDto>>(response.Content).Payload;
            itemAttributeSearchResultDt = ToDataTable(payload.ItemAttributeDetailsDtos);
            TotalRecords = payload.TotalRecords;
        }
        public void VerifyOutputAgainstDbOutput() 
        {
            var i = -1;

            foreach (DataRow dr in itemAttributeSearchQueryDt.Rows)
            {
                i = i+1;
                foreach (DataColumn dc in itemAttributeSearchQueryDt.Columns)
                   
                    Assert.AreEqual(dr[dc].ToString(), itemAttributeSearchResultDt.Rows[i][dc.ColumnName]);
            }
            
        }

        public void VerifyOutputTotalReordsAgainstDbCount(string count)
        {
            Assert.AreEqual(count, TotalRecords.ToString());
        }
    }
}
