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
        LpnDetailsDto lpnDetailsDto;
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
                    UIConstants.LpnDeleteCommentsUrl = UIConstants.Lpn + UIConstants.LpnComments;
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
            Assert.AreEqual(LpnLockUnlockResultDt.Rows.Count, LpnCaseUnlockResultDt.Rows.Count, "Api and Db count donot match");
            var i = -1;

            foreach (DataRow dr in LpnLockUnlockResultDt.Rows)
            {
                i = i + 1;
                Assert.AreEqual(dr["InventoryLockCode"].ToString(), LpnCaseUnlockResultDt.Rows[i]["InventoryLockCode"].ToString(), "InventoryLockCode : Values are not equal");
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
            lpnDetailsDto = JsonConvert.DeserializeObject<BaseResult<LpnDetailsDto>>(response.Content).Payload;
        }
        public void CreateInputDtoForMultiUnlockApi()
        {
            lpnMultipleUnlockDto1 = new LpnMultipleUnlockDto()
            {
                LpnNumber = UIConstants.LpnNumber,
                WorkStationId = UIConstants.WorkStation,
                LockCount=Convert.ToInt32(UIConstants.LockCount)
                
            };

            lpnMultipleUnlockDto2= new LpnMultipleUnlockDto()
            {
                LpnNumber = UIConstants.LpnNumber2,
                WorkStationId = UIConstants.WorkStation,
                LockCount = Convert.ToInt32(UIConstants.LockCount)
            };
        }
        public void CreateInputDtoForMultiLockApi()
        {
            UIConstants.Message = Guid.NewGuid().ToString("n").Substring(0, 8);
            caseLockCommentDto = new CaseLockCommentDto()
            {
                WorkStationId = UIConstants.WorkStation,
                LpnIds = new List<string>() { UIConstants.LpnNumber, UIConstants.LpnNumber2 },
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
            caseCommentDto = new CaseCommentDto()
            {
                CaseNumber = UIConstants.LpnNumber,
                CommentCode = UIConstants.CommentCode,
                CommentText = UIConstants.Message,
                CommentType = UIConstants.CommentCode,
                SystemCodeCommentCode = UIConstants.SystemCodeCommentCode,
                SystemCodeCommentType = UIConstants.SystemCodeCommentType
            };
            caseCommentDto2 = new CaseCommentDto()
            {
                CaseNumber = UIConstants.LpnNumber2,
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
            VerifyOkResultAndStoreBearerToken(response);
        }
        public void VerifyCommentsIsInsertedinDb()
        {
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
        }

        public void CallLpnDeleteCommentsApi(string url)
        {
        }

        public void VerifyCommentsIsDeletedInDb()
        {
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
        }
       

        public void VerifyLocksAreDeletedinDb()
        {
        }
     

        public void VerifyLocksAreAddedInDb()
        {
        }
        

        public void VerifyMultiCommentsAreAddedInDb()
        {
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
                SystemCodeCommentType = UIConstants.SystemCodeCommentType
            };
        }
        public void CreateInputDtoForEditItemApi() 
        {
            lpnCaseDetailsUpdate = new LpnDetailsUpdateDto()
            {
                CaseNumber = UIConstants.LpnNumber,
                CutNumber = UIConstants.CutNumber,
                AssortmentNumber = UIConstants.AssortNumber,
                SkuId=UIConstants.ItemNumber
            };
        }
        public void CreateInputDtoForMultiEditApi()
        {
            lpnBatchUpdateDto = new LpnBatchUpdateDto()
            {
                ManufacturingDate = UIConstants.ManufacturingDate,
                ExpireDate = UIConstants.ExpireDate,
                ConsumePriority = UIConstants.ConsumePriority
            };
        }
        public void CreateInputDtoForLpnUpdateApi() 
        {
            lpnUpdate = new LpnHeaderUpdateDto()
            {
                ReceivedShipmentNumber=UIConstants.ShipmentNbr,
                ExpireDate = UIConstants.ExpireDate,
                ManufacturingDate = UIConstants.ManufacturingDate,
                ConsumeCasePriority = UIConstants.ConsumePriority,
                ConsumePriorityDate = UIConstants.ConsumePriorityDate,
                ConsumeSequence = UIConstants.ConsumeSequence,
                CaseNumber = UIConstants.LpnNumber,
                EstimatedWeight=UIConstants.EstWt,
                ActualWeight=UIConstants.ActlWt,
                DistributionCenterOrderNumber=UIConstants.DcOrderNbr,
                PoNumber=UIConstants.PoNumber,
                Volume=UIConstants.Volume,
                VendorId=UIConstants.VendorId,
                SpecialInstructionCode1=UIConstants.SpclInstCode1,
                SpecialInstructionCode2 = UIConstants.SpclInstCode2,
                SpecialInstructionCode3 = UIConstants.SpclInstCode3,
                SpecialInstructionCode4 = UIConstants.SpclInstCode4,
                SpecialInstructionCode5 = UIConstants.SpclInstCode5,
                ValidShipmentNumber=true

            };
        }

        public void VerifyLpnDetailsOutputAgainstDbOutput()
        {
        }       
    }
}