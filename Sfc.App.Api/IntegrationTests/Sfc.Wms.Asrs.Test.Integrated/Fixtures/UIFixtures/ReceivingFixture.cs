using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Sfc.Wms.Configuration.ItemMasters.Contracts.Dtos;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData.Constant;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using Sfc.Core.OnPrem.Result;
using FunctionalTestProject.SQLQueries;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures.UIFixtures
{
    public class ReceivingFixture:BaseFixture
    {

        protected int TotalRecords = 0;
        protected DataTable ReceivingSearchQueryDt=new DataTable();
        protected DataTable ReceivingSearchResultDt = new DataTable();


        protected DataTable ReceivingDetailsQueryDt = new DataTable();
        protected DataTable ReceivingDetailsResultDt = new DataTable();
        protected DataTable ReceivingDetailsDrilldownQueryDt = new DataTable();
        protected DataTable ReceivingDetailsDrilldownResultDt = new DataTable();
        protected DataTable ReceivingQVDetailsQueryDt = new DataTable();
        protected DataTable ReceivingQVDetailsResultDt = new DataTable();
        protected DataTable VendorDt = new DataTable();
        protected DataTable ActiveLocnDt = new DataTable();
        public void PickAnIReceivingTestDataFromDb()
        {
            using (var db = new OracleConnection())
            {
                db.ConnectionString = ConfigurationManager.ConnectionStrings["SfcRbacContextModel"].ToString();
                db.Open();

                var _command = new OracleCommand(ReceiptInquiryQueries.FetchPoNbrSql, db);
                UIConstants.PoNumber = _command.ExecuteScalar().ToString();

                 _command = new OracleCommand(ReceiptInquiryQueries.FetchAsnNbrSql, db);
                DataTable tempDt = new DataTable();
                tempDt.Load(_command.ExecuteReader());
                UIConstants.ShipmentNbr = tempDt.Rows[0][0].ToString();
                UIConstants.ShipmentNbrCount = tempDt.Rows[0][1].ToString();
                tempDt.Clear();
                tempDt.Columns.Clear();

                _command = new OracleCommand(ReceiptInquiryQueries.FetchMainPageGridDtSql(), db);
                ReceivingSearchQueryDt.Load(_command.ExecuteReader());

                _command = new OracleCommand(ReceiptInquiryQueries.FetchDetailsGridDtSql(), db);
                ReceivingDetailsQueryDt.Load(_command.ExecuteReader());
               
                _command = new OracleCommand(ReceiptInquiryQueries.FetchDetailsDrilldownGridDataSql(), db);
                ReceivingDetailsDrilldownQueryDt.Load(_command.ExecuteReader());

                _command = new OracleCommand(ReceiptInquiryQueries.FetchQvGridDataSql(), db);
                ReceivingQVDetailsQueryDt.Load(_command.ExecuteReader());


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

        public void CallReceivingSearchApiWithInputs(string url)
        {
            var response=CallGetApi(url);
            VerifyOkResultAndStoreBearerToken(response);
            var payload = JsonConvert.DeserializeObject<BaseResult<ItemAttributeSearchResultDto>>(response.Content).Payload;
            ReceivingSearchResultDt = ToDataTable(payload.ItemAttributeDetailsDtos);
            TotalRecords = payload.TotalRecords;
        }
        public void VerifyReceivingSearchOutputAgainstDbOutput() 
        {
            VerifyApiOutputAgainstDbOutput(ReceivingSearchQueryDt, ReceivingSearchResultDt);          
            
        }

        public void VerifyItemDescriptionInApiOutput()
        {
            Assert.AreEqual(UIConstants.ItemDescription,ReceivingSearchResultDt.Rows[0]["Description"].ToString()); ;
        }

        public void VerifyOutputTotalReordsAgainstDbCount(string count)
        {
            Assert.AreEqual(count, TotalRecords.ToString());
        }

        public void CallReceivingDrilldownApi(string url) 
        {
            var response = CallGetApi(url);
            VerifyOkResultAndStoreBearerToken(response);
          //  itemAttributeDetailsDto = JsonConvert.DeserializeObject<BaseResult<ItemAttributeDetailsDto>>(response.Content).Payload;
           
        }
        public void CallReceivingDetailsApi(string url)
        {
            var response = CallGetApi(url);
            VerifyOkResultAndStoreBearerToken(response);
            //itemAttributeDetailsDto = JsonConvert.DeserializeObject<BaseResult<ItemAttributeDetailsDto>>(response.Content).Payload;

        }
        public void CallReceivingDetailsDrilldownApi(string url)
        {
            var response = CallGetApi(url);
            VerifyOkResultAndStoreBearerToken(response);
          //  itemAttributeDetailsDto = JsonConvert.DeserializeObject<BaseResult<ItemAttributeDetailsDto>>(response.Content).Payload;

        }
        public void VerifyOutputForReceivingDrilldownInApiOutput() 
        {
            VerifyApiOutputAgainstDbOutput(ReceivingDetailsDrilldownQueryDt, ReceivingDetailsDrilldownResultDt);
        }
        public void VerifyOutputForReceivingDetailsInApiOutput() 
        {
            VerifyApiOutputAgainstDbOutput(ReceivingDetailsQueryDt, ReceivingDetailsResultDt);
        }
        public void VerifyOutputForReceivingDetailsDrilldownInApiOutput()
        {
            VerifyApiOutputAgainstDbOutput(ReceivingDetailsDrilldownQueryDt, ReceivingDetailsDrilldownResultDt);
               
        }
        public void CallReceivingUpdateQvDetailsApi(string url)
        {
            var response = CallGetApi(url);
            VerifyOkResultAndStoreBearerToken(response);
           // itemAttributeDetailsDto = JsonConvert.DeserializeObject<BaseResult<ItemAttributeDetailsDto>>(response.Content).Payload;

        }
        public void CallReceivingQvDetailsApi(string url)
        {
            var response = CallGetApi(url);
            VerifyOkResultAndStoreBearerToken(response);
            //itemAttributeDetailsDto = JsonConvert.DeserializeObject<BaseResult<ItemAttributeDetailsDto>>(response.Content).Payload;

        }
        public void VerifyOutputForQvDetailsInApiOutput()
        {
            VerifyApiOutputAgainstDbOutput(ReceivingQVDetailsQueryDt, ReceivingQVDetailsResultDt);
        }
        public void VerifyQVDetailsUpdatedInDb()
        {
            //foreach (DataRow dr in VendorDt.Rows)
               // Assert.IsTrue(itemAttributeDetailsDto.VendorNames.Contains(dr[0].ToString()));
        }
    }
}
