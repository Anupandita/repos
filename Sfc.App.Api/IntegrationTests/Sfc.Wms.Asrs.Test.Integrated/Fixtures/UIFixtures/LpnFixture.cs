using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData.Constant;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using Sfc.Core.OnPrem.Result;
using Sfc.Wms.Foundation.InboundLpn.Contracts.Dtos;
using FunctionalTestProject.SQLQueries;
using System.Collections.Generic;
using System;
using System.Globalization;
using SFC.Core.Extensions;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures.UIFixtures
{
    public class LpnFixture : BaseFixture
    {

        protected int TotalRecords = 0;
        protected DataTable LpnSearchQueryDt = new DataTable();
        protected DataTable LpnSearchResultDt = new DataTable();
        protected DataTable LpnCommentsResultDt = new DataTable();
        protected DataTable LpnCommentsQueryDt = new DataTable();
        protected DataTable LpnHistoryQueryDt = new DataTable();
        protected DataTable LpnHistoryResultDt = new DataTable();
        protected DataTable LpnLockUnlockQueryDt = new DataTable();
        protected DataTable LpnLockUnlockResultDt = new DataTable();
        protected DataTable LpnCaseUnlockResultDt = new DataTable();
        protected DataTable LpnItemskResultDt = new DataTable();
        protected DataTable LpnDetailsQueryDt = new DataTable();
        protected DataTable LpnDetailsResultDt = new DataTable();
        protected DataTable VendorQueryDt = new DataTable();
        protected DataTable VendorResultDt = new DataTable();
        CaseCommentDto caseCommentDto, caseCommentDto2;
        LpnMultipleUnlockDto lpnMultipleUnlockDto1, lpnMultipleUnlockDto2;
        CaseLockCommentDto caseLockCommentDto;
        LpnBatchUpdateDto lpnBatchUpdateDto;
        LpnHeaderUpdateDto lpnUpdate;
        LpnDetailsUpdateDto lpnCaseDetailsUpdate;
      
      

        public void PickAnLpnTestDataFromDb()
        {
            using (var db = new OracleConnection())
            {
                db.ConnectionString = ConfigurationManager.ConnectionStrings["SfcRbacContextModel"].ToString();
                db.Open();
                DataTable tempDt = new DataTable();
                var _command = new OracleCommand(LpnQueries.FetchLpnNumberSql, db);
                tempDt.Load(_command.ExecuteReader());
                UIConstants.LpnNumber = tempDt.Rows[0][0].ToString();
                UIConstants.SkuId = tempDt.Rows[0][1].ToString();
                tempDt.Clear();
                tempDt.Columns.Clear();

                _command = new OracleCommand(LpnQueries.FetchLpnNumberForHistory, db);
                UIConstants.LpnNumberForHistory = _command.ExecuteScalar().ToString();

                _command = new OracleCommand(LpnQueries.FetchLpnNbrForLockUnlock, db);
                tempDt.Load(_command.ExecuteReader());
                UIConstants.LpnNbrForLockUnlock = tempDt.Rows[0][0].ToString();
                UIConstants.LpnNbrForLockUnlock1 = tempDt.Rows[1][0].ToString();
                UIConstants.LockCount = tempDt.Rows[0][1].ToString();
                UIConstants.LockCount1 = tempDt.Rows[1][1].ToString();
                tempDt.Clear();
                tempDt.Columns.Clear();

                _command = new OracleCommand(LpnQueries.FetchPalletIdSql, db);
                tempDt.Load(_command.ExecuteReader());
                UIConstants.PalletId = tempDt.Rows[0][0].ToString();
                UIConstants.PalletIdCount = tempDt.Rows[0][1].ToString();
                tempDt.Clear();
                tempDt.Columns.Clear();

                _command = new OracleCommand(LpnQueries.FetchVendorListSql, db);
                VendorQueryDt.Load(_command.ExecuteReader());               
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
                UIConstants.ExpireDate = DateTime.ParseExact("12/31/2021","MM/dd/yyyy",CultureInfo.InvariantCulture);
                UIConstants.ManufacturingDate = DateTime.ParseExact("12/31/2020", "MM/dd/yyyy", CultureInfo.InvariantCulture);
                UIConstants.ConsumePriorityDate = DateTime.ParseExact("12/01/2020", "MM/dd/yyyy", CultureInfo.InvariantCulture);
                UIConstants.ConsumePriority = "1";
                UIConstants.ConsumeSequence = "1";
                UIConstants.EstWt = Convert.ToDecimal(LpnSearchQueryDt.Rows[0]["estimateWeight"].ToString()) + 1;
                UIConstants.ActlWt = Convert.ToDecimal(LpnSearchQueryDt.Rows[0]["actualWeight"].ToString()) + 1;
                UIConstants.Volume = Convert.ToDecimal(LpnSearchQueryDt.Rows[0]["volume"].ToString()) + 1;
                UIConstants.SpclInstCode1 ="1";
                UIConstants.SpclInstCode2 ="1";
                UIConstants.SpclInstCode3 ="1";
                UIConstants.SpclInstCode4 ="1";
                UIConstants.SpclInstCode5 ="1";

                _command = new OracleCommand(LpnQueries.FetchLpnNbrFromShpmtNbrUpdated(), db);
                tempDt.Load(_command.ExecuteReader());
                UIConstants.ShipmentNbr = tempDt.Rows[0]["orig_shpmt_nbr"].ToString() ;
                UIConstants.DcOrderNbr = tempDt.Rows[0]["dc_ord_nbr"].ToString() ;
                UIConstants.PoNumber = tempDt.Rows[0]["po_nbr"].ToString() ;
                tempDt.Clear();
                tempDt.Columns.Clear();

                _command = new OracleCommand(LpnQueries.FetchCaseCommentsDtSql(), db);
                LpnCommentsQueryDt.Load(_command.ExecuteReader());
                UIConstants.CommentSequenceNumber = Convert.ToInt16(LpnCommentsQueryDt.Rows[0]["CommentSequenceNumber"].ToString());
                UIConstants.CommentCode = LpnCommentsQueryDt.Rows[0]["CommentCode"].ToString();
                UIConstants.CommentType = LpnCommentsQueryDt.Rows[0]["CommentType"].ToString();
                UIConstants.SystemCodeCommentCode=LpnCommentsQueryDt.Rows[0]["SystemCodeCommentCode"].ToString();
                UIConstants.SystemCodeCommentType = LpnCommentsQueryDt.Rows[0]["SystemCodeCommentType"].ToString();

                _command = new OracleCommand(LpnQueries.FetchHistoryGridDtSql(), db);
                LpnHistoryQueryDt.Load(_command.ExecuteReader());
               

                _command = new OracleCommand(LpnQueries.FetchCaseLockDtSql(), db);
                LpnLockUnlockQueryDt.Load(_command.ExecuteReader());

                _command = new OracleCommand(LpnQueries.FetchDetailsDtSql(), db);
                LpnDetailsQueryDt.Load(_command.ExecuteReader());

                _command = new OracleCommand(LpnQueries.FetchItemsDtSql(), db);
                tempDt.Load(_command.ExecuteReader());
                UIConstants.CutNumber = tempDt.Rows[0]["cutNumber"].ToString() + "1";
                UIConstants.AssortNumber = tempDt.Rows[0]["assortmentNumber"].ToString() + "1";
                tempDt.Clear();
                tempDt.Columns.Clear();

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
                    UIConstants.LpnSearchUrl = UIConstants.Lpn + UIConstants.Find + UIConstants.SearchInputLpnNumber + UIConstants.LpnNumber;
                    return;
                case "ItemNumber":
                    UIConstants.LpnSearchUrl = UIConstants.Lpn + UIConstants.Find + UIConstants.SearchInputSkuId + UIConstants.ItemNumber;
                    return;
                case "PalletIdNumber":
                    UIConstants.LpnSearchUrl = UIConstants.Lpn + UIConstants.Find + UIConstants.SearchInputPalletId + UIConstants.PalletId;
                    return;
                case "Zone":
                    UIConstants.LpnSearchUrl = UIConstants.Lpn + UIConstants.Find + UIConstants.SearchInputZone + UIConstants.Zone;
                    return;
                case "Aisle":
                    UIConstants.LpnSearchUrl = UIConstants.Lpn + UIConstants.Find + UIConstants.SearchInputAisle + UIConstants.Aisle;
                    return;
                case "Slot":
                    UIConstants.LpnSearchUrl = UIConstants.Lpn + UIConstants.Find + UIConstants.SearchInputSlot + UIConstants.Slot;
                    return;
                case "Level":
                    UIConstants.LpnSearchUrl = UIConstants.Lpn + UIConstants.Find + UIConstants.SearchInputLevel + UIConstants.Level;
                    return;
                case "CreatedDate":
                    UIConstants.LpnSearchUrl = UIConstants.Lpn + UIConstants.Find + UIConstants.SearchInputCreatedDate + UIConstants.CreatedDate;
                    return;
                case "Status":
                    UIConstants.LpnSearchUrl = UIConstants.Lpn + UIConstants.Find + UIConstants.SearchInputFromDate + UIConstants.LpnFromStatus + UIConstants.SearchInputToDate + UIConstants.LpnToStatus;
                    return;
                case "Comments":
                    UIConstants.LpnCommentsUrl = UIConstants.Lpn + UIConstants.LpnComments + UIConstants.slash + UIConstants.LpnNumber;
                    return;
                case "History":
                    UIConstants.LpnHistoryUrl = UIConstants.Lpn + UIConstants.LpnHistory + UIConstants.LpnNumberForHistory + UIConstants.slash + UIConstants.Whse;
                    return;
                case "LockUnlock":
                    UIConstants.LpnLockUnlockUrl = UIConstants.Lpn + UIConstants.LpnLockUnlock + UIConstants.LpnNbrForLockUnlock;
                    return;
                case "CaseUnlock":
                    UIConstants.LpnCaseUnlockUrl = UIConstants.Lpn + UIConstants.LpnCaseUnlock + UIConstants.LpnIds + UIConstants.LpnNbrForLockUnlock;
                    return;
                case "Details":
                    UIConstants.LpnDetailsUrl = UIConstants.Lpn + UIConstants.LpnDetails + UIConstants.slash + UIConstants.LpnNumber;
                    return;
                case "AddComments":
                    UIConstants.LpnAddCommentsUrl = UIConstants.Lpn + UIConstants.LpnComments;
                    return;
                case "EditComments":
                    UIConstants.LpnEditCommentsUrl = UIConstants.Lpn + UIConstants.LpnComments;
                    return;
                case "DeleteComments":
                    UIConstants.LpnDeleteCommentsUrl = UIConstants.Lpn + UIConstants.LpnComments+UIConstants.slash+UIConstants.LpnNumber+UIConstants.slash+UIConstants.CommentSequenceNumber;
                    return;
                case "Update":
                    UIConstants.LpnUpdateUrl = UIConstants.Lpn + UIConstants.LpnDetails;
                    return;
                case "Items":
                    UIConstants.LpnItemsUrl = UIConstants.Lpn + UIConstants.LpnCaseDetails;
                    return;
                case "MultiUnlock":
                    UIConstants.LpnMultiUnlockUrl = UIConstants.Lpn + UIConstants.LpnMultiUnlock;
                    return;
                case "MultiLock":
                    UIConstants.LpnMultiLockUrl = UIConstants.Lpn + UIConstants.LpnMultiLock;
                    return;
                case "MultiComments":
                    UIConstants.LpnMultiCommentsUrl = UIConstants.Lpn + UIConstants.LpnMultiComments;
                    return;
                case "MultiEdit":
                    UIConstants.LpnMultiEditUrl = UIConstants.Lpn + UIConstants.LpnMultiEdit;
                    return;
            }
        }
        string displayLocation = "";
        public void CallLpnSearchApiWithInputs(string url)
        {
            var response = CallGetApi(url);
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

        public void CallLpnCaseUnlockApiWithInputs(string url)
        {
            var response = CallGetApi(url);
            VerifyOkResultAndStoreBearerToken(response);
            var payload = JsonConvert.DeserializeObject<BaseResult<List<CaseLockDto>>>(response.Content).Payload;
            LpnCaseUnlockResultDt = ToDataTable(payload);

        }

        public void VerifyLpnCaseUnlockOutputAgainstDbOutput()
        {
            Assert.AreEqual(LpnLockUnlockQueryDt.Rows.Count, LpnCaseUnlockResultDt.Rows.Count, "Api and Db count donot match");
            var i = -1;

            foreach (DataRow dr in LpnLockUnlockQueryDt.Rows)
            {
                i = i + 1;
                Assert.AreEqual(dr["InventoryLockId"].ToString(), LpnCaseUnlockResultDt.Rows[i]["InventoryLockCode"].ToString(), "InventoryLockCode : Values are not equal");
            }
        }

        public void CreateInputDtoForAddCommentsApi()
        {
            UIConstants.Message = Guid.NewGuid().ToString("n").Substring(0, 8);
            caseCommentDto = new CaseCommentDto()
            {
                CaseNumber = UIConstants.LpnNumber,
                CommentCode = UIConstants.CommentCode,
                CommentText = UIConstants.Message,
                CommentType = UIConstants.CommentCode,
                SystemCodeCommentCode = UIConstants.SystemCodeCommentCode,
                SystemCodeCommentType = UIConstants.SystemCodeCommentType
            };

        }
        public void CallLpnDetailsApi(string url)
        {
            var response = CallGetApi(url);
            VerifyOkResultAndStoreBearerToken(response);
            var payload = JsonConvert.DeserializeObject<BaseResult<LpnDetailsDto>>(response.Content).Payload;
            LpnDetailsResultDt = ToDataTable(payload.CaseDetailDtos);
            VendorResultDt = ToDataTable(payload.VendorDtos);
        }
        public void CreateInputDtoForMultiUnlockApi()
        {
            lpnMultipleUnlockDto1 = new LpnMultipleUnlockDto()
            {
                LpnNumber = UIConstants.LpnNbrForLockUnlock,
                WorkStationId = UIConstants.WorkStation,
                LockCount = Convert.ToInt32(UIConstants.LockCount)

            };

            lpnMultipleUnlockDto2 = new LpnMultipleUnlockDto()
            {
                LpnNumber = UIConstants.LpnNbrForLockUnlock1,
                WorkStationId = UIConstants.WorkStation,
                LockCount = Convert.ToInt32(UIConstants.LockCount1)
            };
        }
        public void CreateInputDtoForMultiLockApi()
        {
            UIConstants.Message = Guid.NewGuid().ToString("n").Substring(0, 8);
            caseLockCommentDto = new CaseLockCommentDto()
            {
                WorkStationId = UIConstants.WorkStation,
                LpnIds = new List<string>() { UIConstants.LpnNbrForLockUnlock, UIConstants.LpnNbrForLockUnlock1 },
                Comments = new CaseCommentDto()
                {
                    CommentCode = UIConstants.CommentCode,
                    CommentText = UIConstants.Message,
                    CommentType = UIConstants.CommentCode,
                    SystemCodeCommentCode = UIConstants.SystemCodeCommentCode,
                    SystemCodeCommentType = UIConstants.SystemCodeCommentType
                }

            };

        }
        public void CreateInputDtoForMultiCommentsApi()
        {
            UIConstants.Message = Guid.NewGuid().ToString("n").Substring(0, 8);
            caseCommentDto = new CaseCommentDto()
            {
                CaseNumber = UIConstants.LpnNbrForLockUnlock,
                CommentCode = UIConstants.CommentCode,
                CommentText = UIConstants.Message,
                CommentType = UIConstants.CommentCode,
                SystemCodeCommentCode = UIConstants.SystemCodeCommentCode,
                SystemCodeCommentType = UIConstants.SystemCodeCommentType
            };
            caseCommentDto2 = new CaseCommentDto()
            {
                CaseNumber = UIConstants.LpnNbrForLockUnlock1,
                CommentCode = UIConstants.CommentCode,
                CommentText = UIConstants.Message,
                CommentType = UIConstants.CommentCode,
                SystemCodeCommentCode = UIConstants.SystemCodeCommentCode,
                SystemCodeCommentType = UIConstants.SystemCodeCommentType
            };
        }
        public void CallLpnAddCommentsApi(string url)
        {
            var request = CallPostApi();
            request.AddJsonBody(caseCommentDto);
            var response = ExecuteRequest(url, request);
            VerifyCreatedResultAndStoreBearerToken(response);
        }
        public void CallLpnMultiUnlockApi(string url)
        {
            var request = CallPostApi();
            request.AddJsonBody(new List<LpnMultipleUnlockDto> { lpnMultipleUnlockDto1, lpnMultipleUnlockDto2 });
            var response = ExecuteRequest(url, request);
            VerifyOkResultAndStoreBearerToken(response);
        }
        public void CallLpnMultiLockApi(string url)
        {
            var request = CallPostApi();
            request.AddJsonBody(caseLockCommentDto);
            var response = ExecuteRequest(url, request);
            VerifyOkResultAndStoreBearerToken(response);
        }
        public void CallLpnMultiCommentsApi(string url)
        {
            var request = CallPostApi();
            request.AddJsonBody(new List<CaseCommentDto>() { caseCommentDto, caseCommentDto2 });
            var response = ExecuteRequest(url, request);
            VerifyCreatedResultAndStoreBearerToken(response);
        }
        public void VerifyCommentsIsInsertedinDb()
        {
            using (var db = new OracleConnection())
            {
                db.ConnectionString = ConfigurationManager.ConnectionStrings["SfcRbacContextModel"].ToString();
                db.Open();

                var _command = new OracleCommand(LpnQueries.FetchEditedCommentSql(), db);
                Assert.IsNotNull(_command.ExecuteScalar());

            }
        }



        public void CallLpnEditCommentsApi(string url)
        {
            var request = CallPutApi();
            request.AddJsonBody(caseCommentDto);
            var response = ExecuteRequest(url, request);
            VerifyOkResultAndStoreBearerToken(response);
        }

        public void VerifyCommentsIsUpdatedInDb()
        {
            using (var db = new OracleConnection())
            {
                db.ConnectionString = ConfigurationManager.ConnectionStrings["SfcRbacContextModel"].ToString();
                db.Open();

                var _command = new OracleCommand(LpnQueries.FetchEditedCommentSql(), db);
                Assert.IsNotNull(_command.ExecuteScalar());

            }
        }

        public void CallLpnDeleteCommentsApi(string url)
        {
            var request = CallDeleteApi();
            request.AddJsonBody(caseCommentDto);
            var response = ExecuteRequest(url, request);
            VerifyOkResultAndStoreBearerToken(response);
        }

        public void VerifyCommentsIsDeletedInDb()
        {
            using (var db = new OracleConnection())
            {
                db.ConnectionString = ConfigurationManager.ConnectionStrings["SfcRbacContextModel"].ToString();
                db.Open();

                var _command = new OracleCommand(LpnQueries.FetchDeletedCommentSql(), db);
                Assert.IsNull(_command.ExecuteScalar());

            }
        }
        public void CallLpnUpdateApi(string url)
        {
            var request = CallPutApi();
            request.AddJsonBody(lpnUpdate);
            var response = ExecuteRequest(url, request);
            VerifyOkResultAndStoreBearerToken(response);
        }

        public void VerifyUpdateFieldsAreUpdatedInDb()
        {
            using (var db = new OracleConnection())
            {
                db.ConnectionString = ConfigurationManager.ConnectionStrings["SfcRbacContextModel"].ToString();
                db.Open();

                var _command = new OracleCommand(LpnQueries.FetchUpdateDtSql(), db);
                var tempdt = new DataTable();
                tempdt.Load(_command.ExecuteReader());
               // Assert.AreEqual(UIConstants.PoNumber, tempdt.Rows[0][0]);
               // Assert.AreEqual(UIConstants.ShipmentNbr, tempdt.Rows[0][1]);
               // Assert.AreEqual(UIConstants.DcOrderNbr, tempdt.Rows[0][2]);
                Assert.AreEqual(UIConstants.ConsumePriority, tempdt.Rows[0][3]);
                Assert.AreEqual(UIConstants.ConsumePriorityDate, tempdt.Rows[0][4]);
                Assert.AreEqual(UIConstants.ConsumeSequence, tempdt.Rows[0][5]);
                Assert.AreEqual(UIConstants.ManufacturingDate, tempdt.Rows[0][6]);
                Assert.AreEqual(UIConstants.ExpireDate, tempdt.Rows[0][7]);
                Assert.AreEqual(UIConstants.Volume.ToString(), tempdt.Rows[0][8].ToString());
                Assert.AreEqual(UIConstants.EstWt.ToString(), tempdt.Rows[0][9].ToString());
                Assert.AreEqual(UIConstants.ActlWt.ToString(), tempdt.Rows[0][10].ToString());
                Assert.AreEqual(UIConstants.SpclInstCode1, tempdt.Rows[0][11]);
                Assert.AreEqual(UIConstants.SpclInstCode2, tempdt.Rows[0][12]);
                Assert.AreEqual(UIConstants.SpclInstCode3, tempdt.Rows[0][13]);
                Assert.AreEqual(UIConstants.SpclInstCode4, tempdt.Rows[0][14]);
                Assert.AreEqual(UIConstants.SpclInstCode5, tempdt.Rows[0][15]);

            }
        }
        public void CallLpnItemsApi(string url)
        {
            var request = CallPutApi();
            request.AddJsonBody(lpnCaseDetailsUpdate);
            var response = ExecuteRequest(url, request);
            VerifyOkResultAndStoreBearerToken(response);
        }

        public void VerifyItemFieldsAreUpdatedInDb()
        {
            using (var db = new OracleConnection())
            {
                db.ConnectionString = ConfigurationManager.ConnectionStrings["SfcRbacContextModel"].ToString();
                db.Open();

                var _command = new OracleCommand(LpnQueries.FetchItemsDtSql(), db);
                var tempdt = new DataTable();
                tempdt.Load(_command.ExecuteReader());
                Assert.AreEqual(UIConstants.CutNumber, tempdt.Rows[0][0]);
                Assert.AreEqual(UIConstants.AssortNumber, tempdt.Rows[0][1]);
            }
        }


        public void VerifyLocksAreDeletedinDb()
        {
            using (var db = new OracleConnection())
            {
                db.ConnectionString = ConfigurationManager.ConnectionStrings["SfcRbacContextModel"].ToString();
                db.Open();
                var _command = new OracleCommand(LpnQueries.FetchMultiLockDtSql(), db);
                Assert.IsNull(_command.ExecuteScalar());
            }
        }


        public void VerifyLocksAreAddedInDb()
        {
        }

        public void VerifyCorbaResultFromDbFor(string btnName)
        {
            string sql = "";
            switch (btnName)
            {
                case "MultiUnlock":
                    {
                        sql = "select * from pb2_corba_dtl where id in (select max(ph.id) from pb2_corba_hdr ph inner join pb2_corba_dtl pd on ph.id= pd.id where func_name like '%" + FuncName.MultiUnLock + "' and parm_name='caseNbr' and parm_value='" + UIConstants.LpnNbrForLockUnlock + "' and crt_date like sysdate) and parm_name='return'";
                        break;
                    }
                case "MultiLock":
                    {
                        sql = "select * from pb2_corba_dtl where id in (select max(ph.id) from pb2_corba_hdr ph inner join pb2_corba_dtl pd on ph.id= pd.id where func_name like '%" + FuncName.MultiLock + "' and parm_name='caseNbr' and parm_value='" + UIConstants.LpnNbrForLockUnlock + "' and crt_date like sysdate) and parm_name='return'";
                        break;
                    }
               
            }
        }

            public void VerifyMultiCommentsAreAddedInDb()
        {
            using (var db = new OracleConnection())
            {
                db.ConnectionString = ConfigurationManager.ConnectionStrings["SfcRbacContextModel"].ToString();
                db.Open();

                var _command = new OracleCommand(LpnQueries.FetchMultiCaseCommentsDtSql(), db);
                var tempdt = new DataTable();
                tempdt.Load(_command.ExecuteReader());
                Assert.AreEqual(UIConstants.CommentCode, tempdt.Rows[0][1]);
                Assert.AreEqual(UIConstants.SystemCodeCommentType, tempdt.Rows[0][2]);
                Assert.AreEqual(UIConstants.CommentCode, tempdt.Rows[0][3]);
                Assert.AreEqual(UIConstants.SystemCodeCommentCode, tempdt.Rows[0][4]);
                Assert.AreEqual(UIConstants.Message, tempdt.Rows[0][5]);
                Assert.AreEqual(UIConstants.CommentCode, tempdt.Rows[1][1]);
                Assert.AreEqual(UIConstants.SystemCodeCommentType, tempdt.Rows[1][2]);
                Assert.AreEqual(UIConstants.CommentCode, tempdt.Rows[1][3]);
                Assert.AreEqual(UIConstants.SystemCodeCommentCode, tempdt.Rows[1][4]);
                Assert.AreEqual(UIConstants.Message, tempdt.Rows[1][5]);
            }

        }
        public void CallLpnMultiEditApi(string url)
        {
            var request = CallPutApi();
            request.AddJsonBody(lpnBatchUpdateDto);
            var response = ExecuteRequest(url, request);
            VerifyOkResultAndStoreBearerToken(response);
        }

        public void VerifyMultiEditAreAddedInDb()
        {
            using (var db = new OracleConnection())
            {
                db.ConnectionString = ConfigurationManager.ConnectionStrings["SfcRbacContextModel"].ToString();
                db.Open();

                var _command = new OracleCommand(LpnQueries.FetchMultiEditDtSql(), db);
                var tempdt = new DataTable();
                tempdt.Load(_command.ExecuteReader());
                Assert.AreEqual(UIConstants.ConsumePriority, tempdt.Rows[0]["consumeCasePriority"]);
                Assert.AreEqual(UIConstants.ManufacturingDate, tempdt.Rows[0]["manufacturingOn"]);
                Assert.AreEqual(UIConstants.ExpireDate, tempdt.Rows[0]["expiryDate"]);
                Assert.AreEqual(UIConstants.ConsumePriority, tempdt.Rows[1]["consumeCasePriority"]);
                Assert.AreEqual(UIConstants.ManufacturingDate, tempdt.Rows[1]["manufacturingOn"]);
                Assert.AreEqual(UIConstants.ExpireDate, tempdt.Rows[1]["expiryDate"]);

            }
        }

        public void VerifyOutputTotalReordsAgainstDbCount(string count)
        {
            VerifyOutputTotalReordsAgainstDbCount(count, TotalRecords.ToString());
        }
        public void CreateInputDtoForEditCommentApi()
        {
            UIConstants.Message = Guid.NewGuid().ToString("n").Substring(0, 10);
            caseCommentDto = new CaseCommentDto()
            {
                CaseNumber = UIConstants.LpnNumber,
                CommentCode = UIConstants.CommentCode,
                CommentText = UIConstants.Message,
                CommentType = UIConstants.CommentCode,
                SystemCodeCommentCode = UIConstants.SystemCodeCommentCode,
                SystemCodeCommentType = UIConstants.SystemCodeCommentType,
                CommentSequenceNumber = UIConstants.CommentSequenceNumber
            };
        }
        public void CreateInputDtoForEditItemApi()
        {
            lpnCaseDetailsUpdate = new LpnDetailsUpdateDto()
            {
                CaseNumber = UIConstants.LpnNumber,
                CutNumber = UIConstants.CutNumber,
                AssortmentNumber = UIConstants.AssortNumber,
                SkuId = UIConstants.SkuId
            };
        }
        public void CreateInputDtoForMultiEditApi()
        {
            lpnBatchUpdateDto = new LpnBatchUpdateDto()
            {
               CaseNumbers = new List<string>(){UIConstants.LpnNbrForLockUnlock, UIConstants.LpnNbrForLockUnlock1},
               ManufacturingDate = UIConstants.ManufacturingDate.SetAzureDbDateTimeKindToUtc(),
               ExpireDate = UIConstants.ExpireDate.SetAzureDbDateTimeKindToUtc(),
                ConsumePriority = UIConstants.ConsumePriority
            };
        }
        public void CreateInputDtoForLpnUpdateApi()
        {
            lpnUpdate = new LpnHeaderUpdateDto()
            {
               // ReceivedShipmentNumber = UIConstants.ShipmentNbr,
                ExpireDate = UIConstants.ExpireDate.SetAzureDbDateTimeKindToUtc(),
                ManufacturingDate = UIConstants.ManufacturingDate.SetAzureDbDateTimeKindToUtc(),
                ConsumeCasePriority = UIConstants.ConsumePriority,
                ConsumePriorityDate = UIConstants.ConsumePriorityDate.SetAzureDbDateTimeKindToUtc(),
                ConsumeSequence = UIConstants.ConsumeSequence,
                CaseNumber = UIConstants.LpnNumber,
                EstimatedWeight = UIConstants.EstWt,
                ActualWeight = UIConstants.ActlWt,
                // DistributionCenterOrderNumber = UIConstants.DcOrderNbr,
                // PoNumber = UIConstants.PoNumber,
                Volume = UIConstants.Volume,
                VendorId = UIConstants.VendorId,
                SpecialInstructionCode1 = UIConstants.SpclInstCode1,
                SpecialInstructionCode2 = UIConstants.SpclInstCode2,
                SpecialInstructionCode3 = UIConstants.SpclInstCode3,
                SpecialInstructionCode4 = UIConstants.SpclInstCode4,
                SpecialInstructionCode5 = UIConstants.SpclInstCode5,
                ValidShipmentNumber = false,
               // AisleValue = ""

            };
        }

        public void VerifyLpnDetailsOutputAgainstDbOutput()
        {
            VerifyApiOutputAgainstDbOutput(LpnDetailsQueryDt, LpnDetailsResultDt);
            VerifyApiOutputAgainstDbOutput(VendorQueryDt, VendorResultDt);
        }
        
    }
}
