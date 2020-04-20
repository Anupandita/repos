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

        protected int TotalRecords = 0;
        protected DataTable ItemAttributeSearchQueryDt=new DataTable();
        protected DataTable ItemAttributeSearchResultDt = new DataTable();
        protected DataTable VendorDt = new DataTable();
        protected DataTable ActiveLocnDt = new DataTable();
        ItemAttributeDetailsDto itemAttributeDetailsDto;
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
                ItemAttributeSearchQueryDt.Load(_command.ExecuteReader());

                _command = new OracleCommand(ItemAttributeQueries.FetchVendorDtSql(), db);
                VendorDt.Load(_command.ExecuteReader());

                _command = new OracleCommand(ItemAttributeQueries.FetchActiveLocnDtSql(), db);
                ActiveLocnDt.Load(_command.ExecuteReader());

            }
        }

        public void CreateUrlAndInputParamForApiUsing(string criteria)
        {
            switch (criteria)
            {
                case "Item":
                    UIConstants.ItemAttributeSearchUrl = ConfigurationManager.AppSettings["BaseUrl"] + UIConstants.ItemAttributes + UIConstants.Search + UIConstants.SearchInputItemId + UIConstants.ItemNumber;
                    return;
                case "ItemDescription":
                    UIConstants.ItemAttributeSearchUrl = ConfigurationManager.AppSettings["BaseUrl"] + UIConstants.ItemAttributes + UIConstants.Search + UIConstants.SearchInputItemDescription + UIConstants.ItemDescription;
                    return;
                case "VendorItemNumber":
                    UIConstants.ItemAttributeSearchUrl = ConfigurationManager.AppSettings["BaseUrl"] + UIConstants.ItemAttributes + UIConstants.Search + UIConstants.SearchInputVendorItemNumber + UIConstants.VendorItemNumber;
                    return;
                case "TempZone":
                    UIConstants.ItemAttributeSearchUrl = ConfigurationManager.AppSettings["BaseUrl"] + UIConstants.ItemAttributes + UIConstants.Search + UIConstants.SearchInputTempZone + UIConstants.TempZone;
                    return;
                case "ItemDetails":
                    UIConstants.ItemAttributeDetailsUrl = ConfigurationManager.AppSettings["BaseUrl"] + UIConstants.ItemAttributes + UIConstants.ItemNumber;
                    return;
            }
        }
        public void CallItemAttributeSearchApiWithInputs(string url)
        {
            var response=CallGetApi(url);
            VerifyOkResultAndStoreBearerToken(response);
            var payload = JsonConvert.DeserializeObject<BaseResult<ItemAttributeSearchResultDto>>(response.Content).Payload;
            ItemAttributeSearchResultDt = ToDataTable(payload.ItemAttributeDetailsDtos);
            TotalRecords = payload.TotalRecords;
        }
        public void VerifyOutputAgainstDbOutput() 
        {
            var i = -1;

            foreach (DataRow dr in ItemAttributeSearchQueryDt.Rows)
            {
                i = i+1;
                foreach (DataColumn dc in ItemAttributeSearchQueryDt.Columns)
                   
                    Assert.AreEqual(dr[dc].ToString(), ItemAttributeSearchResultDt.Rows[i][dc.ColumnName]);
            }
            
        }

        public void VerifyItemDescriptionInApiOutput()
        {
            Assert.AreEqual(UIConstants.ItemDescription,ItemAttributeSearchResultDt.Rows[0]["Description"].ToString()); ;
        }

        public void VerifyOutputTotalReordsAgainstDbCount(string count)
        {
            Assert.AreEqual(count, TotalRecords.ToString());
        }

        public void  CallItemAttributeDetailsApi(string url) 
        {
            var response = CallGetApi(url);
            VerifyOkResultAndStoreBearerToken(response);
            itemAttributeDetailsDto = JsonConvert.DeserializeObject<BaseResult<ItemAttributeDetailsDto>>(response.Content).Payload;
           
        }
        public void VerifyOutputForActiveLocationsListInOutput() 
        {
            foreach (DataRow dr in ActiveLocnDt.Rows)
                Assert.IsTrue(itemAttributeDetailsDto.ActiveLocations.Contains(dr[0].ToString()));
        }
        public void VerifyOutputForVendorsListInOutput() 
        {
            foreach (DataRow dr in VendorDt.Rows)
                Assert.IsTrue(itemAttributeDetailsDto.VendorNames.Contains(dr[0].ToString()));            
        }
    }
}
