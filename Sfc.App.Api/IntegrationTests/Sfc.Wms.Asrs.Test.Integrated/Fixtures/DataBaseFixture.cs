using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using Sfc.Wms.Interfaces.Asrs.Dematic.Contracts.Dtos;
using Sfc.Wms.Interfaces.Asrs.Shamrock.Contracts.Dtos;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData;
using Sfc.Wms.Foundation.InboundLpn.Contracts.Dtos;
using Sfc.Wms.Interfaces.ParserAndTranslator.Contracts.Constants;
using Sfc.Wms.Interfaces.ParserAndTranslator.Contracts.Dto;
using Sfc.Core.OnPrem.Result;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Sfc.Core.OnPrem.ParserAndTranslator.Constants;
using Sfc.Wms.Interfaces.Parser.Parsers;
using Sfc.Wms.Foundation.Tasks.Contracts.Dtos;
using Sfc.Wms.Foundation.TransitionalInventory.Contracts.Dtos;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures
{
    public class DataBaseFixture : CommonFunction
    {
        public decimal UnitWeight;
        protected CaseViewDto SingleSkuCase = new CaseViewDto();
        protected CaseViewDto CaseHdrMultiSku = new CaseViewDto();
        protected SwmToMheDto SwmToMheComt = new SwmToMheDto();
        protected SwmToMheDto SwmToMheIvmt = new SwmToMheDto();
        protected IvmtDto Ivmt = new IvmtDto();
        protected ComtDto Comt = new ComtDto();
        protected WmsToEmsDto WmsToEmsIvmt = new WmsToEmsDto();
        protected WmsToEmsDto WmsToEmsComt = new WmsToEmsDto();
        protected List<TransitionalInventoryDto> TrnList = new List<TransitionalInventoryDto>();
        protected List<CaseViewDto> CaseList = new List<CaseViewDto>();
        protected List<CaseViewDto> CaseDtoList = new List<CaseViewDto>();           
        protected TaskHeaderDto TaskMultiSku= new TaskHeaderDto();
        protected TaskHeaderDto TaskSingleSku = new TaskHeaderDto();
        protected TransitionalInventoryDto TransInvnAfterTrigger = new TransitionalInventoryDto();
        protected TransitionalInventoryDto TransInvnBeforeTrigger  = new TransitionalInventoryDto();
        private readonly MessageHeaderParser _canParseMessage;
        protected string LocnId;
        protected dynamic TrnInvnBeforeCallingApi;
        protected CaseViewDto CaseDtlAfterApi  = new CaseViewDto();
        protected CaseViewDto CaseHdrDtl = new CaseViewDto();
        protected CaseHeaderDto NotEnoughInvCase = new CaseHeaderDto();


        public DataBaseFixture()
        {
            _canParseMessage = new MessageHeaderParser();
        }

        public void GetDataBeforeTriggerComt()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                var caseheader = TriggerOnCaseHeader(db, 1);
                var caseDto = GetCaseDtlData(db, caseheader.CaseNumber);
                SingleSkuCase = FetchTransInvn(db, caseDto[0].SkuId);
                SingleSkuCase.CaseNumber = caseheader.CaseNumber;
                SingleSkuCase.LocationId = caseheader.LocationId;
                SingleSkuCase.StatusCode = caseheader.StatusCode;
                SingleSkuCase.SkuId = caseDto[0].SkuId;
                SingleSkuCase.TotalAllocQty = Convert.ToInt32(caseDto[0].TotalAllocQty);
                MultiSkuData(db);
                NotEnoughInvCase = QueryForNotEnoughInventoryInCase(db, 1);
            }
        }     

        public CaseViewDto TriggerOnCaseHeader(OracleConnection db, int seqNumber)
        {
            var caseheader = new CaseViewDto();
            var countQuery = $"select COUNT(*) from  CASE_HDR inner join CASE_DTL on CASE_HDR.CASE_NBR = CASE_DTL.CASE_NBR inner join ITEM_MASTER im ON CASE_DTL.sku_id = im.sku_id inner join locn_hdr lh ON CASE_HDR.locn_id = lh.locn_id inner join locn_grp lg ON lg.locn_id=lh.locn_id inner join sys_code sc ON sc.code_id=lg.grp_type where CASE_DTL.total_alloc_qty >1 and CASE_DTL.actl_qty>1 and CASE_DTL.CASE_SEQ_NBR = 1 and CASE_HDR.stat_code = '50'and CASE_HDR.po_nbr!= 'null'and sc.code_type='740' and code_id ='19' and im.temp_zone in ('D', 'F') and case_hdr.actl_wt < 1000";
            var countCommand = new OracleCommand(countQuery, db);
            var rowSize = Convert.ToInt32(countCommand.ExecuteScalar());
            if (rowSize == 0)
            {
                caseheader = ValidQueryToFetchCaseData(db, seqNumber);
                var currentLocnId = FetchLocnId(db, caseheader.TempZone);
                UpdateDropZoneLocation(db,caseheader.CaseNumber, currentLocnId);
            }
            else
            {
                var q = $"select CASE_HDR.CASE_NBR,CASE_HDR.LOCN_ID,CASE_HDR.STAT_CODE from  CASE_HDR inner join CASE_DTL on CASE_HDR.CASE_NBR = CASE_DTL.CASE_NBR inner join ITEM_MASTER im ON CASE_DTL.sku_id = im.sku_id inner join locn_hdr lh ON CASE_HDR.locn_id = lh.locn_id inner join locn_grp lg ON lg.locn_id=lh.locn_id inner join sys_code sc ON sc.code_id=lg.grp_type where CASE_DTL.total_alloc_qty >1 and CASE_DTL.actl_qty>1 and CASE_DTL.CASE_SEQ_NBR = 1 and CASE_HDR.stat_code = '50'and CASE_HDR.po_nbr!= 'null'and sc.code_type='740' and code_id ='19' and im.temp_zone in ('D', 'F') and case_hdr.actl_wt < 1000";
                var command = new OracleCommand(q, db);

                var caseHeaderReader = command.ExecuteReader();
                if (caseHeaderReader.Read())
                {
                    caseheader.CaseNumber = caseHeaderReader[CaseHeader.CaseNumber].ToString();
                    caseheader.LocationId = caseHeaderReader[CaseHeader.LocationId].ToString();
                    caseheader.StatusCode = Convert.ToInt16(caseHeaderReader[CaseHeader.StatusCode].ToString());
                }
            }
            return caseheader;
        }

        public CaseViewDto ValidQueryToFetchCaseData(OracleConnection db, int seqNumber)
        {
            var caseheader = new CaseViewDto();
            var query = $"select CASE_HDR.CASE_NBR,CASE_HDR.LOCN_ID,CASE_HDR.STAT_CODE,im.temp_zone from  CASE_HDR inner join CASE_DTL on CASE_HDR.CASE_NBR = CASE_DTL.CASE_NBR inner join ITEM_MASTER im ON CASE_DTL.sku_id = im.sku_id where CASE_DTL.total_alloc_qty > 1 and CASE_DTL.actl_qty > 1 and CASE_DTL.CASE_SEQ_NBR = 1 and CASE_HDR.stat_code = '50' and CASE_HDR.po_nbr != 'null' and im.temp_zone in ('D', 'F') and case_hdr.actl_wt < 1000";
            var command = new OracleCommand(query, db);
            var caseHeaderReader = command.ExecuteReader();
            if (caseHeaderReader.Read())
            {
                caseheader.CaseNumber = caseHeaderReader[CaseHeader.CaseNumber].ToString();
                caseheader.LocationId = caseHeaderReader[CaseHeader.LocationId].ToString();
                caseheader.StatusCode = Convert.ToInt16(caseHeaderReader[CaseHeader.StatusCode].ToString());
                caseheader.TempZone = caseHeaderReader["TEMP_ZONE"].ToString();

            }
            return caseheader;
        }

        public string FetchLocnId(OracleConnection db,string tempZone)
        {
            var query = $"select lh.locn_id,lg.grp_attr from locn_hdr lh inner join locn_grp lg on lg.locn_id = lh.locn_id inner join sys_code sc on sc.code_id = lg.grp_type and sc.code_type = '740' and sc.code_id = '19' and lg.grp_attr = DECODE('{tempZone}', 'D', 'Dry', 'Freezer')";
            var command = new OracleCommand(query, db);
            LocnId = command.ExecuteScalar().ToString();
            return LocnId;
        }


        public void UpdateDropZoneLocation(OracleConnection db, string caseNbr, string locnId)
        {
            Transaction = db.BeginTransaction();
            var updateQuery = $"update case_hdr set locn_id = '{locnId}' where case_nbr = '{caseNbr}'";
            Command = new OracleCommand(updateQuery, db);
            Command.ExecuteNonQuery();
            Transaction.Commit();
        }

        public CaseHeaderDto QueryForNotEnoughInventoryInCase(OracleConnection db, int seqNumber)
        {
            var caseheader = new CaseHeaderDto();
            var query = $"select CASE_HDR.CASE_NBR,CASE_HDR.LOCN_ID,CASE_HDR.STAT_CODE from  CASE_HDR inner join CASE_DTL on CASE_HDR.CASE_NBR = CASE_DTL.CASE_NBR and CASE_DTL.total_alloc_qty <=0  and CASE_DTL.CASE_SEQ_NBR = 1 and stat_code = '50' and CASE_HDR.po_nbr!= 'null'";
            var command = new OracleCommand(query, db);
            var caseHeaderReader = command.ExecuteReader();
            if (caseHeaderReader.Read())
            {
                caseheader.CaseNumber = caseHeaderReader[CaseHeader.CaseNumber].ToString();
            }
            return caseheader;
        }
        public List<CaseViewDto> GetCaseDtlData(OracleConnection db, string caseNumber)
        {
            var caseDtl = new List<CaseViewDto>();
            var singleSkuQuery = $"Select * from case_dtl where case_nbr = '{caseNumber}'";
            var command = new OracleCommand(singleSkuQuery, db);
            var caseDetailReader = command.ExecuteReader();
            while (caseDetailReader.Read())
            {
                var set = new CaseViewDto
                {
                    SkuId = caseDetailReader[CaseDetail.SkuId].ToString(),
                    TotalAllocQty = Convert.ToInt32(caseDetailReader[CaseDetail.TotalAllocQty].ToString())
                };
                caseDtl.Add(set);
            }
            return caseDtl;
        }
       
        public void MultiSkuData(OracleConnection db)
        {
            CaseHdrMultiSku = TriggerOnCaseHeader(db,1);
            CaseDtoList = GetCaseDtlData(db, CaseHdrMultiSku.CaseNumber);
            FetchTransInvnDataForMultiSku(db);
        }

        public List<TransitionalInventoryDto> FetchTransInvnDataForMultiSku(OracleConnection db)
        {
            for (var j = 0; j < CaseDtoList.Count; j++)
            {
                var query = $"select sku_id,locn_id,trans_invn_type,actl_invn_units,actl_wt,user_id,mod_date_time from trans_invn where sku_id='{CaseDtoList[j].SkuId}'  and trans_invn_type = 18";
                Command = new OracleCommand(query, db);
                var transInvnMultiSku = Command.ExecuteReader();
                if (transInvnMultiSku.Read())
                {
                    TransInvnBeforeTrigger.ActualInventoryUnits = Convert.ToDecimal(transInvnMultiSku[TransInventory.ActualInventoryUnits].ToString());
                    TransInvnBeforeTrigger.ActualWeight = Convert.ToDecimal(transInvnMultiSku[TransInventory.ActlWt].ToString());
                    TrnList.Add(TransInvnBeforeTrigger);
                }        
            }
            return TrnList;
        }
       
        public void GetDataAfterTriggerOfComtForSingleSku()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                Command = new OracleCommand();
                SwmToMheComt = SwmToMhe(db, SingleSkuCase.CaseNumber, TransactionCode.Comt, null);
                Comt = JsonConvert.DeserializeObject<ComtDto>(SwmToMheComt.MessageJson);
                ParserTestforMsgText(TransactionCode.Comt, SwmToMheComt.SourceMessageText);
                WmsToEmsComt = WmsToEmsData(db, SwmToMheComt.SourceMessageKey,TransactionCode.Comt);
                SwmToMheIvmt = SwmToMhe(db, SingleSkuCase.CaseNumber, TransactionCode.Ivmt, SingleSkuCase.SkuId);
                Ivmt = JsonConvert.DeserializeObject<IvmtDto>(SwmToMheIvmt.MessageJson);
                ParserTestforMsgText(TransactionCode.Ivmt, SwmToMheIvmt.SourceMessageText);
                WmsToEmsIvmt = WmsToEmsData(db, SwmToMheIvmt.SourceMessageKey, TransactionCode.Ivmt);
                CaseDtlAfterApi =FetchCaseDetailsAfterTriger(db);
                UnitWeight = FetchUnitWeight(db, SingleSkuCase.SkuId);
                TaskSingleSku = FetchTaskDetails(db, SingleSkuCase.SkuId);
            }
        }

        public void GetDataAfterTriggerOfIvmtForSingleSku()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                Command = new OracleCommand();
                SwmToMheIvmt = SwmToMhe(db, SingleSkuCase.CaseNumber, TransactionCode.Ivmt, SingleSkuCase.SkuId);
                Ivmt = JsonConvert.DeserializeObject<IvmtDto>(SwmToMheIvmt.MessageJson);
                ParserTestforMsgText(TransactionCode.Ivmt, SwmToMheIvmt.SourceMessageText);
                WmsToEmsIvmt = WmsToEmsData(db, SwmToMheIvmt.SourceMessageKey, TransactionCode.Ivmt);
                CaseDtlAfterApi = FetchCaseDetailsAfterTriger(db);
                UnitWeight = FetchUnitWeight(db, SingleSkuCase.SkuId);
                TaskSingleSku = FetchTaskDetails(db, SingleSkuCase.SkuId);
            }
        }

        protected CaseViewDto FetchCaseDetailsAfterTriger(OracleConnection db)
        {
            var caseDtl = new CaseViewDto();
            var query = $"select TRANS_INVN.MOD_DATE_TIME, CASE_HDR.STAT_CODE,CASE_DTL.ACTL_QTY,CASE_DTL.TOTAL_ALLOC_QTY,TRANS_INVN.ACTL_INVN_UNITS,TRANS_INVN.ACTL_WT from CASE_HDR inner join CASE_DTL on CASE_HDR.CASE_NBR = CASE_DTL.CASE_NBR  inner join TRANS_INVN on CASE_DTL.SKU_ID = TRANS_INVN.SKU_ID  and TRANS_INVN.TRANS_INVN_TYPE = '18'  and CASE_HDR.CASE_NBR ='{SingleSkuCase.CaseNumber}' order by TRANS_INVN.MOD_DATE_TIME desc";
            Command = new OracleCommand(query, db);
            var caseHdrDtlTrnReader = Command.ExecuteReader();
            if (caseHdrDtlTrnReader.Read())
            {
                caseDtl.StatusCode = Convert.ToInt16(caseHdrDtlTrnReader[CaseHeader.StatusCode].ToString());
                caseDtl.ActualInventoryUnits = Convert.ToDecimal(caseHdrDtlTrnReader[TransInventory.ActualInventoryUnits].ToString());
                caseDtl.ActualWeight = Convert.ToDecimal(caseHdrDtlTrnReader[TransInventory.ActlWt].ToString());
                caseDtl.TotalAllocQty = Convert.ToInt32(caseHdrDtlTrnReader[CaseDetail.TotalAllocQty].ToString());
            }
            return caseDtl;
        }
        
        protected TaskHeaderDto FetchTaskDetails(OracleConnection db,string skuId)
        {
            var task = new TaskHeaderDto();
            var query = $"select * from task_hdr where sku_id = '{skuId}' order by mod_date_time desc";
            Command = new OracleCommand(query, db);
            var taskHdr = Command.ExecuteReader();
            if (taskHdr.Read())
            {
                task.StatusCode = Convert.ToByte(taskHdr[TaskHeader.StatCode].ToString());
            }
            return task;
        }

        public void GetDataAfterTriggerForMultiSkuAndValidateData()
        {     
            using (var db = GetOracleConnection())
            {
                db.Open();
                SwmToMheComt = SwmToMhe(db, CaseHdrMultiSku.CaseNumber, TransactionCode.Comt, null);
                Comt = JsonConvert.DeserializeObject<ComtDto>(SwmToMheComt.MessageJson);
                ParserTestforMsgText(TransactionCode.Comt, SwmToMheComt.SourceMessageText);
                WmsToEmsComt = WmsToEmsData(db, SwmToMheComt.SourceMessageKey, TransactionCode.Comt);               
                for (var i = 0; i < CaseDtoList.Count; i++)
                {
                    SwmToMheIvmt = SwmToMhe(db, CaseHdrMultiSku.CaseNumber, TransactionCode.Ivmt, CaseDtoList[i].SkuId);
                    Ivmt = JsonConvert.DeserializeObject<IvmtDto>(SwmToMheIvmt.MessageJson);
                    ParserTestforMsgText(TransactionCode.Ivmt, SwmToMheIvmt.SourceMessageText);
                    var caseDtl = CaseDtoList[i];
                    VerifyIvmtMessageWasInsertedIntoSwmToMhe(Ivmt,caseDtl);
                    WmsToEmsIvmt = WmsToEmsData(db, SwmToMheIvmt.SourceMessageKey, TransactionCode.Ivmt);
                    VerifyIvmtMessageWasInsertedIntoWmsToEms(WmsToEmsIvmt);
                    UnitWeight = FetchUnitWeight(db, CaseDtoList[i].SkuId);
                    TransInvnAfterTrigger =FetchTransInvnData(db, CaseDtoList[i].SkuId);
                    var trnInvnAfterApi = TransInvnList[i];
                    try
                    {
                        TrnInvnBeforeCallingApi = TrnList[i];
                    }
                    catch
                    {
                        Debug.Print("No TransInvn Record");
                    }
                    
                    VerifyQuantityIsIncreasedIntoTransInvn(trnInvnAfterApi, TrnInvnBeforeCallingApi);
                    TaskMultiSku = FetchTaskDetails(db, caseDtl.SkuId);                
                    VerifyStatusIsUpdatedIntoTaskHeader(TaskMultiSku.StatusCode);
                }
                CaseHdrDtl = CaseHdrAndDtlDataAfterCallingApi(db);
            }
        }     

        protected CaseViewDto CaseHdrAndDtlDataAfterCallingApi(OracleConnection db)
        {
            var caseHdrDtl = new CaseViewDto();
            var query = $"select case_hdr.stat_code,case_dtl.total_alloc_qty from case_hdr inner join case_dtl on case_hdr.case_nbr = case_dtl.case_nbr and case_hdr.case_nbr ='{CaseHdrMultiSku.CaseNumber}'";
            Command = new OracleCommand(query, db);
            var caseReader = Command.ExecuteReader();
            if (caseReader.Read())
            {
                caseHdrDtl.StatusCode = Convert.ToInt16(caseReader[CaseHeader.StatusCode].ToString());
                caseHdrDtl.TotalAllocQty = Convert.ToInt32(caseReader[CaseDetail.TotalAllocQty].ToString());
                CaseList.Add(caseHdrDtl);
            }
            return caseHdrDtl;
        }
        
        protected BaseResult ParserTestforMsgText(string transactionCode,string sourceTextMsg)
        {
            var testResult = _canParseMessage.ParseMessage(transactionCode, sourceTextMsg);
            return testResult;
        }

        protected void VerifyComtMessageWasInsertedIntoSwmToMhe(ComtDto comt, SwmToMheDto swmToMhe,string caseNbr)
        {
            Assert.AreEqual(DefaultValues.Status, SwmToMheComt.SourceMessageStatus);
            Assert.AreEqual(0, SwmToMheComt.SourceMessageResponseCode);
            Assert.AreEqual(DefaultValues.ContainerType, SwmToMheComt.ContainerType);
            Assert.AreEqual(TransactionCode.Comt, comt.TransactionCode);
            Assert.AreEqual(MessageLength.Comt, comt.MessageLength);
            Assert.AreEqual(ActionCodeConstants.Create, comt.ActionCode);
            Assert.AreEqual(caseNbr, SwmToMheComt.ContainerId);
            Assert.AreEqual(DefaultValues.ContainerType, SwmToMheComt.ContainerType);
            Assert.AreEqual(1, SwmToMheComt.MessageStatus);
        }

        protected void VerifyComtMessageWasInsertedIntoWmsToEms(WmsToEmsDto wte1)
        {
            Assert.AreEqual(SwmToMheComt.SourceMessageProcess, wte1.Process);
            Assert.AreEqual(SwmToMheComt.SourceMessageKey,wte1.MessageKey);
            Assert.AreEqual(SwmToMheComt.SourceMessageTransactionCode, wte1.Transaction);
            Assert.AreEqual(SwmToMheComt.SourceMessageText,wte1.MessageText);
            Assert.AreEqual(SwmToMheComt.SourceMessageStatus, wte1.Status);
            Assert.AreEqual(SwmToMheComt.SourceMessageResponseCode,wte1.ResponseCode);
            Assert.AreEqual(TransactionCode.Comt, wte1.Transaction);
            Assert.AreEqual(Convert.ToInt16(SwmToMheComt.SourceMessageResponseCode), wte1.ResponseCode);
        }

        public void VerifyIvmtMessageWasInsertedIntoSwmToMhe(IvmtDto ivmt,CaseViewDto caseDtl)
        {
            Assert.AreEqual(DefaultValues.Status, SwmToMheIvmt.SourceMessageStatus);  
            Assert.AreEqual(TransactionCode.Ivmt, ivmt.TransactionCode);
            Assert.AreEqual(MessageLength.Ivmt, ivmt.MessageLength);
            Assert.AreEqual(ActionCodeConstants.Create, ivmt.ActionCode);
            Assert.AreEqual(caseDtl.SkuId, ivmt.Sku);
            Assert.AreEqual(caseDtl.TotalAllocQty, Convert.ToDouble(ivmt.Quantity));
            Assert.AreEqual(DefaultValues.ContainerType, ivmt.UnitOfMeasure);          
            Assert.AreEqual(DefaultValues.DataControl, ivmt.DateControl);
            Assert.AreEqual(DefaultValues.InBoundPallet,ivmt.InboundPallet);
        }

        protected void VerifyIvmtMessageWasInsertedIntoWmsToEms(WmsToEmsDto wmsToEms)
        {
            Assert.AreEqual(SwmToMheIvmt.SourceMessageStatus, wmsToEms.Status);
            Assert.AreEqual(TransactionCode.Ivmt, wmsToEms.Transaction);
            Assert.AreEqual(Convert.ToInt16(SwmToMheIvmt.SourceMessageResponseCode), wmsToEms.ResponseCode);
        }
        protected void VerifyQuantityIsIncreasedIntoTransInvn(TransitionalInventoryDto trnInvnAfterApi, TransitionalInventoryDto trnInvnBeforeApi)
        {
            Assert.AreEqual(TransInvnBeforeTrigger.ActualInventoryUnits + Convert.ToDecimal(Ivmt.Quantity), TransInvnAfterTrigger.ActualInventoryUnits);
            //* the bellow line of code is tested after new build release.
            //Assert.AreEqual((unitWeight * Convert.ToDecimal(ivmt.Quantity)) + Convert.ToInt16(transInvnBeforeApi.ActualWeight), Math.Round(Convert.ToDecimal(transInvnAfterApi.ActualWeight)));
        }
        protected void VerifyQuantityisReducedIntoCaseDetailForMultiSku()
        {
            for (var j = 0; j < CaseList.Count; j++)
            {
                Assert.AreEqual(0, CaseList[j].TotalAllocQty);
            }
        }
        protected void VerifyStatusIsUpdatedIntoCaseHeader(int caseHdrstatcode)
        {
            Assert.AreEqual(DefaultValues.CaseHdrStatCode, caseHdrstatcode);
        }
        protected void VerifyStatusIsUpdatedIntoTaskHeader(int statcode)
        {
            try
            {
                Assert.AreEqual(DefaultValues.TaskStatusCode, statcode);
            }
            catch
            {
                Debug.Print("No Task Found");
            }
        }
    }
}

    
    



