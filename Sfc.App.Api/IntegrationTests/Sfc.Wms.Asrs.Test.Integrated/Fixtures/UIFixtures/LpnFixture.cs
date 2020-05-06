using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData.Constant;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using Sfc.Core.OnPrem.Result;
using Sfc.Wms.Foundation.InboundLpn.Contracts.Dtos;
using FunctionalTestProject.SQLQueries;
using System.Collections.Generic;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures.UIFixtures
{
    public class LpnFixture:BaseFixture
    {

        protected int TotalRecords = 0;
        protected DataTable LpnSearchQueryDt=new DataTable();
        protected DataTable LpnSearchResultDt = new DataTable();
        protected DataTable LpnCommentsResultDt = new DataTable();
        protected DataTable LpnCommentsQueryDt = new DataTable();
        protected DataTable LpnHistoryQueryDt = new DataTable();
        protected DataTable LpnHistoryResultDt = new DataTable();
        protected DataTable LpnLockUnlockQueryDt = new DataTable();
        protected DataTable LpnLockUnlockResultDt = new DataTable();
        LpnDetailsDto lpnDetailsDto;
        public void PickAnLpnTestDataFromDb()
        {
            using (var db = new OracleConnection())
            {
                db.ConnectionString = ConfigurationManager.ConnectionStrings["SfcRbacContextModel"].ToString();
                db.Open();

                var _command = new OracleCommand(LpnQueries.FetchLpnNumberSql, db);
                UIConstants.LpnNumber = _command.ExecuteScalar().ToString();

                 _command = new OracleCommand(LpnQueries.FetchLpnNumberForHistory, db);
                UIConstants.LpnNumberForHistory = _command.ExecuteScalar().ToString();
                
                  _command = new OracleCommand(LpnQueries.FetchLpnNbrForLockUnlock, db);
                UIConstants.LpnNbrForLockUnlock = _command.ExecuteScalar().ToString();

                _command = new OracleCommand(LpnQueries.FetchPalletIdSql, db);
                DataTable tempDt = new DataTable();
                tempDt.Load(_command.ExecuteReader());
                UIConstants.PalletId = tempDt.Rows[0][0].ToString();
                UIConstants.PalletIdCount = tempDt.Rows[0][1].ToString();
                tempDt.Clear();
                tempDt.Columns.Clear();

                _command = new OracleCommand(LpnQueries.FetchItemNumberSql, db);
                tempDt.Load(_command.ExecuteReader());
                UIConstants.ItemNumber = tempDt.Rows[0][0].ToString();
                UIConstants.ItemNumberCount = tempDt.Rows[0][1].ToString();
                tempDt.Clear();
                tempDt.Columns.Clear();

                _command = new OracleCommand(LpnQueries.FetchSlotAisle, db);
                tempDt.Load(_command.ExecuteReader());
                UIConstants.TempZone = tempDt.Rows[0][0].ToString();
                tempDt.Clear();
                tempDt.Columns.Clear();

                _command = new OracleCommand(LpnQueries.FetchCreatedDateSql, db);
                tempDt.Load(_command.ExecuteReader());
                UIConstants.CreatedDate = tempDt.Rows[0][0].ToString();
                UIConstants.CreatedDateCount = tempDt.Rows[0][1].ToString();
                tempDt.Clear();
                tempDt.Columns.Clear();

                _command = new OracleCommand(LpnQueries.FetchGetLpnCount(), db);
                tempDt.Load(_command.ExecuteReader());
                UIConstants.StatusCount = tempDt.Rows[0][0].ToString();
                tempDt.Clear();
                tempDt.Columns.Clear();

                _command = new OracleCommand(LpnQueries.FetchLpnPageGridDtSql(), db);
                LpnSearchQueryDt.Load(_command.ExecuteReader());

                _command = new OracleCommand(LpnQueries.FetchCaseCommentsDtSql(), db);
                LpnCommentsQueryDt.Load(_command.ExecuteReader());

                _command = new OracleCommand(LpnQueries.FetchHistoryGridDtSql(), db);
                LpnHistoryQueryDt.Load(_command.ExecuteReader());

                _command = new OracleCommand(LpnQueries.FetchCaseLockDtSql(), db);
                LpnLockUnlockQueryDt.Load(_command.ExecuteReader());

                _command = new OracleCommand(LpnQueries.FetchSlotAisle, db);
                tempDt.Load(_command.ExecuteReader());
                DataRow dr = tempDt.Rows[0];
                UIConstants.Zone = dr["ZONE"].ToString();
                UIConstants.Aisle = dr["AISLE"].ToString();
                UIConstants.Slot = dr["BAY"].ToString();
                UIConstants.Level = dr["LVL"].ToString();
                tempDt.Clear();
                tempDt.Columns.Clear();

            }
        }
        public void CreateUrlAndInputParamForApiUsing(string criteria)
        {
            switch (criteria)
            {
                case "LpnNumber":
                    UIConstants.LpnSearchUrl =  UIConstants.Lpn + UIConstants.Find + UIConstants.SearchInputLpnNumber + UIConstants.LpnNumber;
                    return;
                case "ItemNumber":
                    UIConstants.LpnSearchUrl = UIConstants.Lpn + UIConstants.Find + UIConstants.SearchInputSkuId + UIConstants.ItemNumber;
                    return;
                case "PalletIdNumber":
                    UIConstants.LpnSearchUrl = UIConstants.Lpn + UIConstants.Find + UIConstants.SearchInputPalletId + UIConstants.PalletId;
                    return;
                case "Zone":
                    UIConstants.LpnSearchUrl =  UIConstants.Lpn + UIConstants.Find + UIConstants.SearchInputZone;
                    return;
                case "Aisle":
                    UIConstants.LpnSearchUrl =  UIConstants.Lpn + UIConstants.Find + UIConstants.SearchInputAisle;
                    return;
                case "Slot":
                    UIConstants.LpnSearchUrl =  UIConstants.Lpn + UIConstants.Find + UIConstants.SearchInputSlot;
                    return;
                case "Level":
                    UIConstants.LpnSearchUrl =  UIConstants.Lpn + UIConstants.Find + UIConstants.SearchInputLevel;
                    return;
                case "CreatedDate":
                    UIConstants.LpnSearchUrl =  UIConstants.Lpn + UIConstants.Find + UIConstants.SearchInputCreatedDate + UIConstants.CreatedDate;
                    return;
                case "Status":
                    UIConstants.LpnSearchUrl =  UIConstants.Lpn + UIConstants.Find + UIConstants.SearchInputFromDate + UIConstants.LpnFromStatus + UIConstants.SearchInputToDate + UIConstants.LpnToStatus;
                    return;
                case "Comments":
                    UIConstants.LpnCommentsUrl =  UIConstants.Lpn + UIConstants.LpnComments + UIConstants.LpnNumber;
                    return;
                case "History":
                    UIConstants.LpnHistoryUrl =  UIConstants.Lpn + UIConstants.LpnHistory + UIConstants.LpnNumberForHistory + UIConstants.slash+ UIConstants.Whse;
                    return;
                case "LockUnlock":
                    UIConstants.LpnLockUnlockUrl =  UIConstants.Lpn + UIConstants.LpnLockUnlock + UIConstants.LpnNbrForLockUnlock;
                    return;
            }
        }
        string displayLocation = "";
        public void CallLpnSearchApiWithInputs(string url)
        {
            var response=CallGetApi(url);
            VerifyOkResultAndStoreBearerToken(response);
            var payload = JsonConvert.DeserializeObject<BaseResult<LpnSearchResultsDto>>(response.Content).Payload;
            LpnSearchResultDt = ToDataTable(payload.LpnSearchDetails);
            TotalRecords = payload.TotalRecords;
            displayLocation = LpnSearchResultDt.Rows[0]["DisplayLocation"].ToString();
        }

        public void VerifyDisplayLocationForZone()
        {
            Assert.AreEqual(UIConstants.Zone, displayLocation.Substring(0, 2));
        }
        public void VerifyDisplayLocationForAisle()
        {
            Assert.AreEqual(UIConstants.Aisle, displayLocation.Substring(2, 2));
        }
        public void VerifyDisplayLocationForSlot()
        {
            Assert.AreEqual(UIConstants.Slot, displayLocation.Substring(4, 3));
        }
        public void VerifyDisplayLocationForLevel()
        {
            Assert.AreEqual(UIConstants.Level, displayLocation.Substring(7, 2));
        }
        public void VerifyLpnSearchOutputAgainstDbOutput() 
        {
            VerifyApiOutputAgainstDbOutput(LpnSearchQueryDt, LpnSearchResultDt);          
            
        }

        public void CallLpnCommentsApiWithInputs(string url)
        {
            var response = CallGetApi(url);
            VerifyOkResultAndStoreBearerToken(response);
            var payload = JsonConvert.DeserializeObject<BaseResult<List<CaseCommentDto>>>(response.Content).Payload;
            LpnCommentsResultDt = ToDataTable(payload);
        }

        public void VerifyLpnCommentsOutputAgainstDbOutput()
        {
            VerifyApiOutputAgainstDbOutput(LpnCommentsQueryDt, LpnCommentsResultDt);

        }
        public void CallLpnHistoryApiWithInputs(string url)
        {
            var response = CallGetApi(url);
            VerifyOkResultAndStoreBearerToken(response);
            var payload = JsonConvert.DeserializeObject<BaseResult<List<LpnHistoryDto>>>(response.Content).Payload;
            LpnHistoryResultDt = ToDataTable(payload);
        }

        public void VerifyLpnHistoryOutputAgainstDbOutput()
        {
            VerifyApiOutputAgainstDbOutput(LpnHistoryQueryDt, LpnHistoryResultDt);

        }

        public void CallLpnLockUnlockApiWithInputs(string url)
        {
            var response = CallGetApi(url);
            VerifyOkResultAndStoreBearerToken(response);
            var payload = JsonConvert.DeserializeObject<BaseResult<List<CaseLockUnlockDto>>>(response.Content).Payload;
            LpnLockUnlockResultDt = ToDataTable(payload);
        }

        public void VerifyLpnLockUnlockOutputAgainstDbOutput()
        {
            VerifyApiOutputAgainstDbOutput(LpnLockUnlockQueryDt, LpnLockUnlockResultDt);

        }

        public void VerifyOutputTotalReordsAgainstDbCount(string count)
        {
            Assert.AreEqual(count, TotalRecords.ToString());
        }

        public void  CallLpnDetailsApi(string url) 
        {
            var response = CallGetApi(url);
            VerifyOkResultAndStoreBearerToken(response);
            lpnDetailsDto = JsonConvert.DeserializeObject<BaseResult<LpnDetailsDto>>(response.Content).Payload;
           
        }
        public void VerifyOutputForActiveLocationsListInOutput() 
        {
            //foreach (DataRow dr in ActiveLocnDt.Rows)
            //    Assert.IsTrue(lpnDetailsDto.ActiveLocations.Contains(dr[0].ToString()));
        }
        public void VerifyOutputForVendorsListInOutput() 
        {
            //foreach (DataRow dr in VendorDt.Rows)
            //    Assert.IsTrue(itemAttributeDetailsDto.VendorNames.Contains(dr[0].ToString()));            
        }
    }
}
