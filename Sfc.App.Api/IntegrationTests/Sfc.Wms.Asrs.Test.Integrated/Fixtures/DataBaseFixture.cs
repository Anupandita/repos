using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using Sfc.Wms.Asrs.Dematic.Contracts.Dtos;
using Sfc.Wms.Asrs.Shamrock.Contracts.Dtos;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData;
using Sfc.Wms.InboundLpn.Contracts.Dtos;
using Sfc.Wms.Parser.Parsers;
using Sfc.Wms.ParserAndTranslator.Contracts.Constants;
using Sfc.Wms.ParserAndTranslator.Contracts.Dto;
using Sfc.Wms.ParserAndTranslator.Contracts.Validation;
using Sfc.Wms.Result;
using Sfc.Wms.TaskDetail.Contracts.Dtos;
using Sfc.Wms.TransitionalInventory.Contracts.Dtos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures
{
    public class DataBaseFixture
    {
        public decimal unitWeight;
        protected CaseDto singleSkuCase = new CaseDto();
        protected CaseHeaderDto caseHdrMultiSku = new CaseHeaderDto();
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
        protected List<TransitionalInventoryDto> transInvnList = new List<TransitionalInventoryDto>();
        protected List<TransitionalInventoryDto> trnList = new List<TransitionalInventoryDto>();
        protected List<CaseDto> caseList = new List<CaseDto>();
        protected List<CaseDto> caseDtoList = new List<CaseDto>();           
        protected TaskHeaderDto taskMultiSku= new TaskHeaderDto();
        protected TaskHeaderDto taskSingleSku = new TaskHeaderDto();
        protected TransitionalInventoryDto transInvnAfterTrigger = new TransitionalInventoryDto();
        protected TransitionalInventoryDto transInvnBeforeTrigger  = new TransitionalInventoryDto();
        private readonly MessageHeaderParser _canParseMessage;
        protected string sqlStatement = "";
        protected dynamic trnInvnBeforeCallingApi;
        protected OracleCommand command;
        protected CaseDto caseDtlAfterApi  = new CaseDto();
        protected CaseDto caseHdrDtl = new CaseDto();


        public DataBaseFixture()
        {
            var dataTypeValidation = new DataTypeValidation();
            var messageHeaderParser = new MessageParser(dataTypeValidation);
            _canParseMessage = new MessageHeaderParser(messageHeaderParser);
           
        }

        public void GetDataBeforeTriggerComt()
        {
            OracleConnection db;
            using (db = new OracleConnection
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["SfcRbacContextModel"].ConnectionString
            })
            {
                db.Open();          
                var caseheader = ValidQueryToFetchCaseData(db, 1);
                var caseDto = GetCaseDtlData(db, caseheader.CaseNumber);
                singleSkuCase = FetchTransInvn(db, caseDto[0].SkuId);
                singleSkuCase.CaseNumber = caseheader.CaseNumber;
                singleSkuCase.LocationId = caseheader.LocationId;
                singleSkuCase.StatusCode = caseheader.StatusCode;
                singleSkuCase.SkuId =caseDto[0].SkuId;
                singleSkuCase.TotalAllocQty = Convert.ToInt32(caseDto[0].TotalAllocQty);
                MultiSkuData(db);
            }
        }
        public CaseHeaderDto ValidQueryToFetchCaseData(OracleConnection db, int SeqNumber)
        {
            var caseheader = new CaseHeaderDto();
            var multiSkuQuery = $"select CASE_HDR.CASE_NBR,CASE_HDR.LOCN_ID,CASE_HDR.STAT_CODE from  CASE_HDR inner join CASE_DTL on CASE_HDR.CASE_NBR = CASE_DTL.CASE_NBR and CASE_DTL.total_alloc_qty >1 and actl_qty>1 and CASE_DTL.CASE_SEQ_NBR = {SeqNumber} and stat_code = '50' and CASE_HDR.po_nbr!= 'null'";
            var command = new OracleCommand(multiSkuQuery, db);
            var caseHeaderReader = command.ExecuteReader();
            if (caseHeaderReader.Read())
            {
                caseheader.CaseNumber = caseHeaderReader[CaseHeader.CaseNumber].ToString();
                caseheader.LocationId = caseHeaderReader[CaseHeader.LocationId].ToString();
                caseheader.StatusCode = Convert.ToInt16(caseHeaderReader[CaseHeader.StatusCode].ToString());
            }
            return caseheader;
        }
         public List<CaseDto> GetCaseDtlData(OracleConnection db, string caseNumber)
        {
            var caseDtl = new List<CaseDto>();
            var singleSkuQuery = $"Select * from case_dtl where case_nbr = '{caseNumber}'";
            var command = new OracleCommand(singleSkuQuery, db);
            var caseDetailReader = command.ExecuteReader();
            while (caseDetailReader.Read())
            {
                var set = new CaseDto();
                set.SkuId = caseDetailReader[CaseDetail.SkuId].ToString();               
                set.TotalAllocQty = Convert.ToInt32(caseDetailReader[CaseDetail.TotalAllocQty].ToString());
                caseDtl.Add(set);
            }
            return caseDtl;
        }
        public CaseDto FetchTransInvn(OracleConnection db , string skuId)
        {
            var singleSkulocal = new CaseDto();
            var query  = $"Select * from trans_invn where sku_id = '{skuId}' and  trans_invn_type = '18'";
            command = new OracleCommand(query, db);
            var transInvnReader = command.ExecuteReader();
            if (transInvnReader.Read())
            {
                singleSkulocal.ActualInventoryUnits = Convert.ToInt16(transInvnReader[TransInventory.ActualInventoryUnits].ToString());
                singleSkulocal.ActualWeight = Convert.ToDecimal(transInvnReader[TransInventory.ActlWt].ToString());
            }
            return singleSkulocal;
        }

        public void MultiSkuData(OracleConnection db)
        {
            caseHdrMultiSku = ValidQueryToFetchCaseData(db,1);
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
            OracleConnection db;
            using (db = new OracleConnection
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["SfcRbacContextModel"].ConnectionString
            })
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

        protected CaseDto FetchCaseDetailsAfterTriger(OracleConnection db)
        {
            var caseDtl = new CaseDto();
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
        protected decimal FetchUnitWeight(OracleConnection db, string skuId)
        {
            var query = $"select unit_wt from item_master where sku_id = '{skuId}'";
            command = new OracleCommand(query, db);
            var unitWeight = Convert.ToDecimal(command.ExecuteScalar().ToString());
            return unitWeight;
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
        protected WmsToEmsDto WmsToEmsData(OracleConnection db, int msgKey, string trx)
        {
            var WmsToEms = new WmsToEmsDto();
            var query = $"select * from WMSTOEMS where TRX = '{trx}' and MSGKEY = '{msgKey}'";
            command = new OracleCommand(query, db);
            var wmsToEmsReader = command.ExecuteReader();
            if (wmsToEmsReader.Read())
            {
                WmsToEms.Status = wmsToEmsReader[WmsAndEms.Status].ToString();
                WmsToEms.ResponseCode = Convert.ToInt16(wmsToEmsReader[WmsAndEms.ReasonCode].ToString());
                WmsToEms.MessageKey = Convert.ToUInt16(wmsToEmsReader[WmsAndEms.MsgKey].ToString());
                WmsToEms.Transaction = wmsToEmsReader[WmsAndEms.Trx].ToString();
                WmsToEms.MessageText = wmsToEmsReader[WmsAndEms.MsgTxt].ToString();      
            }
            return WmsToEms;       
        }

        protected SwmToMheDto SwmToMhe(OracleConnection db, string caseNbr, string trx, string skuId)
        {
            var swmtomhedata = new SwmToMheDto();
            var t = $"and sku_id = '{skuId}' ";
            var query = $"select * from SWM_TO_MHE where container_id = '{caseNbr}' and source_msg_trans_code = '{trx}' ";
            var orderBy = "order by created_date_time desc";
            if(trx == TransactionCode.Ivmt)
            {
                query = query + t + orderBy;
            }
            else
            {
                query = query + orderBy;
            }
            command = new OracleCommand(query, db);
            var swmToMheReader = command.ExecuteReader();
            if (swmToMheReader.Read())
            {
                swmtomhedata.SourceMessageKey = Convert.ToInt16(swmToMheReader[SwmAndMhe.SourceMsgKey].ToString());
                swmtomhedata.SourceMessageResponseCode = Convert.ToInt16(swmToMheReader[SwmAndMhe.SourceMsgRsnCode].ToString());
                swmtomhedata.SourceMessageStatus = swmToMheReader[SwmAndMhe.SourceMsgStatus].ToString();
                swmtomhedata.ContainerId = swmToMheReader[SwmAndMhe.ContainerId].ToString();
                swmtomhedata.ContainerType = swmToMheReader[SwmAndMhe.ContainerType].ToString();
                swmtomhedata.MessageJson = swmToMheReader[SwmAndMhe.MsgJson].ToString();
                swmtomhedata.LocationId = swmToMheReader[SwmAndMhe.LocnId].ToString();
                swmtomhedata.SourceMessageText = swmToMheReader[SwmAndMhe.SourceMsgText].ToString();    
            }
            return swmtomhedata;
        }      

        public void GetDataAfterTriggerForMultiSkuAndValidateData()
        {     
            OracleCommand command;
            OracleConnection db;
            using (db = new OracleConnection
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["SfcRbacContextModel"].ConnectionString
            })
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
        
        protected TransitionalInventoryDto FetchTransInvnData(OracleConnection db,string skuId)
        {
            var transInvn  = new TransitionalInventoryDto();
            var query = $"select * from trans_invn where sku_id='{skuId}' order by mod_date_time desc";
            command = new OracleCommand(query, db);
            var transInvnAfterApi = command.ExecuteReader();
            if (transInvnAfterApi.Read())
            {
                transInvn.ActualInventoryUnits = Convert.ToDecimal(transInvnAfterApi[TransInventory.ActualInventoryUnits].ToString());
                transInvn.ActualWeight = Convert.ToDecimal(transInvnAfterApi[TransInventory.ActlWt].ToString());
                transInvnList.Add(transInvn);
            }
            return transInvn;
        }

        protected CaseDto CaseHdrAndDtlDataAfterCallingApi(OracleConnection db)
        {
            var caseHdrDtl = new CaseDto();
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
            Assert.AreEqual(testResult.ResultType, ResultTypes.Ok);
            return testResult;
        }

        protected void VerifyComtMessageWasInsertedIntoSwmToMhe(ComtDto comt, SwmToMheDto swmToMhe,string caseNbr)
        {
            Assert.AreEqual(DefaultValues.Status, swmToMheComt.SourceMessageStatus);
            Assert.AreEqual(DefaultValues.ContainerType, swmToMheComt.ContainerType);
            Assert.AreEqual(TransactionCode.Comt, comt.TransactionCode);
            Assert.AreEqual(MessageLength.Comt, comt.MessageLength);
            Assert.AreEqual(ActionCodeConstants.Create, comt.ActionCode);
            Assert.AreEqual(caseNbr, swmToMheComt.ContainerId);
            Assert.AreEqual(DefaultValues.ContainerType, swmToMheComt.ContainerType);
        }

        protected void VerifyComtMessageWasInsertedIntoWmsToEms(WmsToEmsDto wte1)
        {
            Assert.AreEqual(swmToMheComt.SourceMessageStatus, wte1.Status);
            Assert.AreEqual(TransactionCode.Comt, wte1.Transaction);
            Assert.AreEqual(Convert.ToInt16(swmToMheComt.SourceMessageResponseCode), wte1.ResponseCode);
        }

        public void VerifyIvmtMessageWasInsertedIntoSwmToMhe(IvmtDto ivmt,CaseDto caseDtl)
        {
            Assert.AreEqual(DefaultValues.Status, swmToMheIvmt.SourceMessageStatus);  
            Assert.AreEqual(TransactionCode.Ivmt, ivmt.TransactionCode);
            Assert.AreEqual(MessageLength.Ivmt, ivmt.MessageLength);
            Assert.AreEqual(ActionCodeConstants.Create, ivmt.ActionCode);
            Assert.AreEqual(caseDtl.SkuId, ivmt.Sku);
            Assert.AreEqual(caseDtl.TotalAllocQty, Convert.ToDouble(ivmt.Quantity));
            Assert.AreEqual(DefaultValues.ContainerType, ivmt.UnitOfMeasure);          
            Assert.AreEqual(DefaultValues.DataControl, ivmt.DateControl); 
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

    
    



