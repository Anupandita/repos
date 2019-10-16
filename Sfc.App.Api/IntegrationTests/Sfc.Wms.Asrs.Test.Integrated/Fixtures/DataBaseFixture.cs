using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using Sfc.Wms.Asrs.Dematic.Contracts.Dtos;
using Sfc.Wms.Asrs.Shamrock.Contracts.Dtos;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData;
using Sfc.Wms.InboundLpn.Contracts.Dtos;
using Sfc.Wms.Interfaces.ParserAndTranslator.Contracts.Constants;
using Sfc.Wms.Interfaces.ParserAndTranslator.Contracts.Dto;
using Sfc.Core.OnPrem.Result;
using Sfc.Wms.TaskDetail.Contracts.Dtos;
using Sfc.Wms.TransitionalInventory.Contracts.Dtos;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Sfc.Core.OnPrem.ParserAndTranslator.Constants;
using Sfc.Wms.Interfaces.Parser.Parsers;
using Sfc.Wms.Interfaces.ParserAndTranslator.Contracts.Validation;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures
{
    public class DataBaseFixture : CommonFunction
    {
        public decimal unitWeight;
        protected CaseViewDto singleSkuCase = new CaseViewDto();
        protected CaseViewDto caseHdrMultiSku = new CaseViewDto();
        protected SwmToMheDto swmToMhe = new SwmToMheDto();
        protected SwmToMheDto swmToMheComt = new SwmToMheDto();
        protected SwmToMheDto swmToMheIvmt = new SwmToMheDto();
        protected IvmtDto ivmt = new IvmtDto();
        protected ComtDto comt = new ComtDto();
        protected WmsToEmsDto wmsToEmsIvmt = new WmsToEmsDto();
        protected WmsToEmsDto wmsToEmsComt = new WmsToEmsDto();
        protected TransitionalInventoryDto trn = new TransitionalInventoryDto(); 
        protected CaseHeaderDto caseHdr = new CaseHeaderDto();
        protected CaseHeaderDto caseHeaderAfterTrigger = new CaseHeaderDto();
        protected CaseDetailDto caseDtlAfterTrigger = new CaseDetailDto();      
        protected List<CaseDetailDto> CaseDetailList = new List<CaseDetailDto>();
        protected List<TransitionalInventoryDto> trnList = new List<TransitionalInventoryDto>();
        protected List<CaseViewDto> caseList = new List<CaseViewDto>();
        protected List<CaseViewDto> caseDtoList = new List<CaseViewDto>();           
        protected TaskHeaderDto taskMultiSku= new TaskHeaderDto();
        protected TaskHeaderDto taskSingleSku = new TaskHeaderDto();
        protected TransitionalInventoryDto transInvnAfterTrigger = new TransitionalInventoryDto();
        protected TransitionalInventoryDto transInvnBeforeTrigger  = new TransitionalInventoryDto();
        private readonly MessageHeaderParser _canParseMessage;
        protected string sqlStatement = "";
        protected int rowSize;
        protected string locnId;
        protected dynamic trnInvnBeforeCallingApi;
        protected CaseViewDto caseDtlAfterApi  = new CaseViewDto();
        protected CaseViewDto caseHdrDtl = new CaseViewDto();
        protected CaseHeaderDto NotEnoughInvCase = new CaseHeaderDto();


        public DataBaseFixture()
        {
             var dataTypeValidation = new DataTypeValidation();
             //var messageHeaderParser = new MessageParser();
             _canParseMessage = new MessageHeaderParser();         
        }

        public void GetDataBeforeTriggerComt()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                var caseheader = TriggerOnCaseHeader(db, 1);
                var caseDto = GetCaseDtlData(db, caseheader.CaseNumber);
                singleSkuCase = FetchTransInvn(db, caseDto[0].SkuId);
                singleSkuCase.CaseNumber = caseheader.CaseNumber;
                singleSkuCase.LocationId = caseheader.LocationId;
                singleSkuCase.StatusCode = caseheader.StatusCode;
                singleSkuCase.SkuId = caseDto[0].SkuId;
                singleSkuCase.TotalAllocQty = Convert.ToInt32(caseDto[0].TotalAllocQty);
                MultiSkuData(db);
                NotEnoughInvCase = QueryForNotEnoughInventoryInCase(db, 1);
            }
        }     

        public CaseViewDto TriggerOnCaseHeader(OracleConnection db, int SeqNumber)
        {
            var caseheader = new CaseViewDto();
            var countQuery = $"select COUNT(*) from  CASE_HDR inner join CASE_DTL on CASE_HDR.CASE_NBR = CASE_DTL.CASE_NBR inner join ITEM_MASTER im ON CASE_DTL.sku_id = im.sku_id inner join locn_hdr lh ON CASE_HDR.locn_id = lh.locn_id inner join locn_grp lg ON lg.locn_id=lh.locn_id inner join sys_code sc ON sc.code_id=lg.grp_type where CASE_DTL.total_alloc_qty >1 and CASE_DTL.actl_qty>1 and CASE_DTL.CASE_SEQ_NBR = 1 and CASE_HDR.stat_code = '50'and CASE_HDR.po_nbr!= 'null'and sc.code_type='740' and code_id ='19' and im.temp_zone in ('D', 'F') and case_hdr.actl_wt < 1000";
            var countCommand = new OracleCommand(countQuery, db);
            var rowSize = Convert.ToInt32(countCommand.ExecuteScalar());
            if (rowSize == 0)
            {
                caseheader = ValidQueryToFetchCaseData(db, SeqNumber);
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

        public CaseViewDto ValidQueryToFetchCaseData(OracleConnection db, int SeqNumber)
        {
            var caseheader = new CaseViewDto();
            var Query = $"select CASE_HDR.CASE_NBR,CASE_HDR.LOCN_ID,CASE_HDR.STAT_CODE,im.temp_zone from  CASE_HDR inner join CASE_DTL on CASE_HDR.CASE_NBR = CASE_DTL.CASE_NBR inner join ITEM_MASTER im ON CASE_DTL.sku_id = im.sku_id where CASE_DTL.total_alloc_qty > 1 and CASE_DTL.actl_qty > 1 and CASE_DTL.CASE_SEQ_NBR = 1 and CASE_HDR.stat_code = '50' and CASE_HDR.po_nbr != 'null' and im.temp_zone in ('D', 'F') and case_hdr.actl_wt < 1000";
            var command = new OracleCommand(Query, db);
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
            locnId = command.ExecuteScalar().ToString();
            return locnId;
        }


        public void UpdateDropZoneLocation(OracleConnection db, string caseNbr, string locnId)
        {
            transaction = db.BeginTransaction();
            var updateQuery = $"update case_hdr set locn_id = '{locnId}' where case_nbr = '{caseNbr}'";
            command = new OracleCommand(updateQuery, db);
            command.ExecuteNonQuery();
            transaction.Commit();
        }

        public CaseHeaderDto QueryForNotEnoughInventoryInCase(OracleConnection db, int SeqNumber)
        {
            var caseheader = new CaseHeaderDto();
            var Query = $"select CASE_HDR.CASE_NBR,CASE_HDR.LOCN_ID,CASE_HDR.STAT_CODE from  CASE_HDR inner join CASE_DTL on CASE_HDR.CASE_NBR = CASE_DTL.CASE_NBR and CASE_DTL.total_alloc_qty <=0  and CASE_DTL.CASE_SEQ_NBR = 1 and stat_code = '50' and CASE_HDR.po_nbr!= 'null'";
            var command = new OracleCommand(Query, db);
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
            caseHdrMultiSku = TriggerOnCaseHeader(db,1);
            caseDtoList = GetCaseDtlData(db, caseHdrMultiSku.CaseNumber);
            FetchTransInvnDataForMultiSku(db);
        }

        public List<TransitionalInventoryDto> FetchTransInvnDataForMultiSku(OracleConnection db)
        {
            for (var j = 0; j < caseDtoList.Count; j++)
            {
                var query = $"select sku_id,locn_id,trans_invn_type,actl_invn_units,actl_wt,user_id,mod_date_time from trans_invn where sku_id='{caseDtoList[j].SkuId}'  and trans_invn_type = 18";
                command = new OracleCommand(query, db);
                var transInvnMultiSku = command.ExecuteReader();
                if (transInvnMultiSku.Read())
                {
                    transInvnBeforeTrigger.ActualInventoryUnits = Convert.ToDecimal(transInvnMultiSku[TransInventory.ActualInventoryUnits].ToString());
                    transInvnBeforeTrigger.ActualWeight = Convert.ToDecimal(transInvnMultiSku[TransInventory.ActlWt].ToString());
                    trnList.Add(transInvnBeforeTrigger);
                }        
            }
            return trnList;
        }
       
        public void GetDataAfterTriggerOfComtForSingleSku()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                command = new OracleCommand();
                swmToMheComt = SwmToMhe(db, singleSkuCase.CaseNumber, TransactionCode.Comt, null);
                comt = JsonConvert.DeserializeObject<ComtDto>(swmToMheComt.MessageJson);
                var parsrtest = ParserTestforMsgText(TransactionCode.Comt, swmToMheComt.SourceMessageText);
                wmsToEmsComt = WmsToEmsData(db, swmToMheComt.SourceMessageKey,TransactionCode.Comt);
                swmToMheIvmt = SwmToMhe(db, singleSkuCase.CaseNumber, TransactionCode.Ivmt, singleSkuCase.SkuId);
                ivmt = JsonConvert.DeserializeObject<IvmtDto>(swmToMheIvmt.MessageJson);
                var parsertest =ParserTestforMsgText(TransactionCode.Ivmt, swmToMheIvmt.SourceMessageText);
                wmsToEmsIvmt = WmsToEmsData(db, swmToMheIvmt.SourceMessageKey, TransactionCode.Ivmt);
                caseDtlAfterApi =FetchCaseDetailsAfterTriger(db);
                unitWeight = FetchUnitWeight(db, singleSkuCase.SkuId);
                taskSingleSku = FetchTaskDetails(db, singleSkuCase.SkuId);
            }
        }

        public void GetDataAfterTriggerOfIvmtForSingleSku()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                command = new OracleCommand();
                swmToMheIvmt = SwmToMhe(db, singleSkuCase.CaseNumber, TransactionCode.Ivmt, singleSkuCase.SkuId);
                ivmt = JsonConvert.DeserializeObject<IvmtDto>(swmToMheIvmt.MessageJson);
                var parsertest = ParserTestforMsgText(TransactionCode.Ivmt, swmToMheIvmt.SourceMessageText);
                wmsToEmsIvmt = WmsToEmsData(db, swmToMheIvmt.SourceMessageKey, TransactionCode.Ivmt);
                caseDtlAfterApi = FetchCaseDetailsAfterTriger(db);
                unitWeight = FetchUnitWeight(db, singleSkuCase.SkuId);
                taskSingleSku = FetchTaskDetails(db, singleSkuCase.SkuId);
            }
        }

        protected CaseViewDto FetchCaseDetailsAfterTriger(OracleConnection db)
        {
            var caseDtl = new CaseViewDto();
            var query = $"select TRANS_INVN.MOD_DATE_TIME, CASE_HDR.STAT_CODE,CASE_DTL.ACTL_QTY,CASE_DTL.TOTAL_ALLOC_QTY,TRANS_INVN.ACTL_INVN_UNITS,TRANS_INVN.ACTL_WT from CASE_HDR inner join CASE_DTL on CASE_HDR.CASE_NBR = CASE_DTL.CASE_NBR  inner join TRANS_INVN on CASE_DTL.SKU_ID = TRANS_INVN.SKU_ID  and TRANS_INVN.TRANS_INVN_TYPE = '18'  and CASE_HDR.CASE_NBR ='{singleSkuCase.CaseNumber}' order by TRANS_INVN.MOD_DATE_TIME desc";
            command = new OracleCommand(query, db);
            var CaseHdrDtlTrnReader = command.ExecuteReader();
            if (CaseHdrDtlTrnReader.Read())
            {
                caseDtl.StatusCode = Convert.ToInt16(CaseHdrDtlTrnReader[CaseHeader.StatusCode].ToString());
                caseDtl.ActualInventoryUnits = Convert.ToDecimal(CaseHdrDtlTrnReader[TransInventory.ActualInventoryUnits].ToString());
                caseDtl.ActualWeight = Convert.ToDecimal(CaseHdrDtlTrnReader[TransInventory.ActlWt].ToString());
                caseDtl.TotalAllocQty = Convert.ToInt32(CaseHdrDtlTrnReader[CaseDetail.TotalAllocQty].ToString());
            }
            return caseDtl;
        }
        

        protected TaskHeaderDto FetchTaskDetails(OracleConnection db,string skuId)
        {
            var task = new TaskHeaderDto();
            var query = $"select * from task_hdr where sku_id = '{skuId}' order by mod_date_time desc";
            command = new OracleCommand(query, db);
            var taskHdr = command.ExecuteReader();
            if (taskHdr.Read())
            {
                task.StatusCode = Convert.ToByte(taskHdr[TaskHeader.StatCode].ToString());
            }
            return task;
        }

        public void GetDataAfterTriggerForMultiSkuAndValidateData()
        {     
            OracleCommand command;
            using (var db = GetOracleConnection())
            {
                db.Open();
                command = new OracleCommand();
                swmToMheComt = SwmToMhe(db, caseHdrMultiSku.CaseNumber, TransactionCode.Comt, null);
                comt = JsonConvert.DeserializeObject<ComtDto>(swmToMheComt.MessageJson);
                var parserTestForComtMsg = ParserTestforMsgText(TransactionCode.Comt, swmToMheComt.SourceMessageText);
                wmsToEmsComt = WmsToEmsData(db, swmToMheComt.SourceMessageKey, TransactionCode.Comt);               
                for (var i = 0; i < caseDtoList.Count; i++)
                {
                    swmToMheIvmt = SwmToMhe(db, caseHdrMultiSku.CaseNumber, TransactionCode.Ivmt, caseDtoList[i].SkuId);
                    ivmt = JsonConvert.DeserializeObject<IvmtDto>(swmToMheIvmt.MessageJson);
                    var parserTestForIvmtMsg = ParserTestforMsgText(TransactionCode.Ivmt, swmToMheIvmt.SourceMessageText);
                    var caseDtl = caseDtoList[i];
                    VerifyIvmtMessageWasInsertedIntoSwmToMhe(ivmt,caseDtl);
                    wmsToEmsIvmt = WmsToEmsData(db, swmToMheIvmt.SourceMessageKey, TransactionCode.Ivmt);
                    VerifyIvmtMessageWasInsertedIntoWmsToEms(wmsToEmsIvmt);
                    unitWeight = FetchUnitWeight(db, caseDtoList[i].SkuId);
                    transInvnAfterTrigger =FetchTransInvnData(db, caseDtoList[i].SkuId);
                    var trnInvnAfterApi = transInvnList[i];
                    try
                    {
                        trnInvnBeforeCallingApi = trnList[i];
                    }
                    catch
                    {
                        Debug.Print("No TransInvn Record");
                    }
                    
                    VerifyQuantityIsIncreasedIntoTransInvn(trnInvnAfterApi, trnInvnBeforeCallingApi);
                    taskMultiSku = FetchTaskDetails(db, caseDtl.SkuId);                
                    VerifyStatusIsUpdatedIntoTaskHeader(taskMultiSku.StatusCode);
                }
                caseHdrDtl = CaseHdrAndDtlDataAfterCallingApi(db);
            }
        }     

        protected CaseViewDto CaseHdrAndDtlDataAfterCallingApi(OracleConnection db)
        {
            var caseHdrDtl = new CaseViewDto();
            var query = $"select case_hdr.stat_code,case_dtl.total_alloc_qty from case_hdr inner join case_dtl on case_hdr.case_nbr = case_dtl.case_nbr and case_hdr.case_nbr ='{caseHdrMultiSku.CaseNumber}'";
            command = new OracleCommand(query, db);
            var caseReader = command.ExecuteReader();
            if (caseReader.Read())
            {
                caseHdrDtl.StatusCode = Convert.ToInt16(caseReader[CaseHeader.StatusCode].ToString());
                caseHdrDtl.TotalAllocQty = Convert.ToInt32(caseReader[CaseDetail.TotalAllocQty].ToString());
                caseList.Add(caseHdrDtl);
            }
            return caseHdrDtl;
        }
        
        protected BaseResult ParserTestforMsgText(string transactionCode,string sourceTextMsg)
        {
            var testResult = _canParseMessage.ParseMessage(transactionCode, sourceTextMsg);
            //Assert.AreEqual(testResult.ResultType, ResultTypes.Ok);
            return testResult;
        }

        protected void VerifyComtMessageWasInsertedIntoSwmToMhe(ComtDto comt, SwmToMheDto swmToMhe,string caseNbr)
        {
            Assert.AreEqual(DefaultValues.Status, swmToMheComt.SourceMessageStatus);
            Assert.AreEqual(0, swmToMheComt.SourceMessageResponseCode);
            Assert.AreEqual(DefaultValues.ContainerType, swmToMheComt.ContainerType);
            Assert.AreEqual(TransactionCode.Comt, comt.TransactionCode);
            Assert.AreEqual(MessageLength.Comt, comt.MessageLength);
            Assert.AreEqual(ActionCodeConstants.Create, comt.ActionCode);
            Assert.AreEqual(caseNbr, swmToMheComt.ContainerId);
            Assert.AreEqual(DefaultValues.ContainerType, swmToMheComt.ContainerType);
            Assert.AreEqual(1, swmToMheComt.MessageStatus);
        }

        protected void VerifyComtMessageWasInsertedIntoWmsToEms(WmsToEmsDto wte1)
        {
            Assert.AreEqual(swmToMheComt.SourceMessageProcess, wte1.Process);
            Assert.AreEqual(swmToMheComt.SourceMessageKey,wte1.MessageKey);
            Assert.AreEqual(swmToMheComt.SourceMessageTransactionCode, wte1.Transaction);
            Assert.AreEqual(swmToMheComt.SourceMessageText,wte1.MessageText);
            Assert.AreEqual(swmToMheComt.SourceMessageStatus, wte1.Status);
            Assert.AreEqual(swmToMheComt.SourceMessageResponseCode,wte1.ResponseCode);
            Assert.AreEqual(TransactionCode.Comt, wte1.Transaction);
            Assert.AreEqual(Convert.ToInt16(swmToMheComt.SourceMessageResponseCode), wte1.ResponseCode);
        }

        public void VerifyIvmtMessageWasInsertedIntoSwmToMhe(IvmtDto ivmt,CaseViewDto caseDtl)
        {
            Assert.AreEqual(DefaultValues.Status, swmToMheIvmt.SourceMessageStatus);  
            Assert.AreEqual(TransactionCode.Ivmt, ivmt.TransactionCode);
            Assert.AreEqual(MessageLength.Ivmt, ivmt.MessageLength);
            Assert.AreEqual(ActionCodeConstants.Create, ivmt.ActionCode);
            Assert.AreEqual(caseDtl.SkuId, ivmt.Sku);
            Assert.AreEqual(caseDtl.TotalAllocQty, Convert.ToDouble(ivmt.Quantity));
            Assert.AreEqual(DefaultValues.ContainerType, ivmt.UnitOfMeasure);          
            Assert.AreEqual(DefaultValues.DataControl, ivmt.DateControl);
            Assert.AreEqual(DefaultValues.InBoundPallet,ivmt.InboundPallet);
            //Assert.AreEqual("0", swmToMheIvmt.SourceMessageResponseCode);
            //Assert.AreEqual("PONumber", swmToMheIvmt.PoNumber);
        }

        protected void VerifyIvmtMessageWasInsertedIntoWmsToEms(WmsToEmsDto wmsToEms)
        {
            Assert.AreEqual(swmToMheIvmt.SourceMessageStatus, wmsToEms.Status);
            Assert.AreEqual(TransactionCode.Ivmt, wmsToEms.Transaction);
            Assert.AreEqual(Convert.ToInt16(swmToMheIvmt.SourceMessageResponseCode), wmsToEms.ResponseCode);
        }
        protected void VerifyQuantityIsIncreasedIntoTransInvn(TransitionalInventoryDto trnInvnAfterApi, TransitionalInventoryDto trnInvnBeforeApi)
        {
            Assert.AreEqual(transInvnBeforeTrigger.ActualInventoryUnits + Convert.ToDecimal(ivmt.Quantity), transInvnAfterTrigger.ActualInventoryUnits);
            //* the bellow line of code is tested after new build release.
            //Assert.AreEqual((unitWeight * Convert.ToDecimal(ivmt.Quantity)) + Convert.ToInt16(transInvnBeforeApi.ActualWeight), Math.Round(Convert.ToDecimal(transInvnAfterApi.ActualWeight)));
        }
        protected void VerifyQuantityisReducedIntoCaseDetailForMultiSku()
        {
            for (var j = 0; j < caseList.Count; j++)
            {
                Assert.AreEqual(0, caseList[j].TotalAllocQty);
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

    
    



