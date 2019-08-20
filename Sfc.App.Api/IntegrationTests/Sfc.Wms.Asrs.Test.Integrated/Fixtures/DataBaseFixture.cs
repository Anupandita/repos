using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using Sfc.Wms.Asrs.Dematic.Contracts.Dtos;
using Sfc.Wms.Asrs.Shamrock.Contracts.Dtos;
using Sfc.Wms.Asrs.Test.Integrated.TestData;
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

namespace Sfc.Wms.Asrs.Test.Integrated.Fixtures
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
        protected List<CaseDetailDto> caseList = new List<CaseDetailDto>();
        protected List<CaseDto> caseDtoList = new List<CaseDto>();           
        protected TaskHeaderDto taskMultiSku= new TaskHeaderDto();
        protected TaskDetailDto taskSingleSku = new TaskDetailDto();
        protected TransitionalInventoryDto transInvnAfterTrigger = new TransitionalInventoryDto();
        protected TransitionalInventoryDto transInvnBeforeTrigger  = new TransitionalInventoryDto();
        private readonly MessageHeaderParser _canParseMessage;
        protected string sqlStatement = "";
        protected dynamic trnInvnBeforeCallingApi;
        protected OracleCommand command;


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
            var dr19 = command.ExecuteReader();
            if (dr19.Read())
            {
                caseheader.CaseNumber = dr19["CASE_NBR"].ToString();
                caseheader.LocationId = dr19["LOCN_ID"].ToString();
                caseheader.StatusCode = Convert.ToInt16(dr19["STAT_CODE"].ToString());
            }
            return caseheader;
        }
         public List<CaseDto> GetCaseDtlData(OracleConnection db, string caseNumber)
        {
            var caseDtl = new List<CaseDto>();
            var singleSkuQuery = $"Select * from case_dtl where case_nbr = '{caseNumber}'";
            var command = new OracleCommand(singleSkuQuery, db);
            var dr1 = command.ExecuteReader();
            while (dr1.Read())
            {
                var set = new CaseDto();
                set.SkuId = dr1["SKU_ID"].ToString();               
                set.TotalAllocQty = Convert.ToInt32(dr1["TOTAL_ALLOC_QTY"].ToString());
                caseDtl.Add(set);
            }
            return caseDtl;
        }
        public CaseDto FetchTransInvn(OracleConnection db , string skuId)
        {
            var singleSkulocal = new CaseDto();
            var sql2 = $"Select * from trans_invn where sku_id = '{skuId}' and  trans_invn_type = '18'";
            command = new OracleCommand(sql2, db);
            var dr2 = command.ExecuteReader();
            if (dr2.Read())
            {
                singleSkulocal.ActualInventoryUnits = Convert.ToInt16(dr2["ACTL_INVN_UNITS"].ToString());
                singleSkulocal.ActualWeight = Convert.ToDecimal(dr2["ACTL_WT"].ToString());
            }
            return singleSkulocal;
        }

        public void MultiSkuData(OracleConnection db)
        {
            caseHdrMultiSku = ValidQueryToFetchCaseData(db,1);
            caseDtoList = GetCaseDtlData(db, caseHdrMultiSku.CaseNumber);
            FetchTransInvnDataForMultiSku(db);
        }

        public void FetchTransInvnDataForMultiSku(OracleConnection db)
        {
            for (var j = 0; j < caseDtoList.Count; j++)
            {
                var sql9 = $"select sku_id,locn_id,trans_invn_type,actl_invn_units,actl_wt,user_id,mod_date_time from trans_invn where sku_id='{caseDtoList[j].SkuId}'  and trans_invn_type = 18";
                command = new OracleCommand(sql9, db);
                var dr13 = command.ExecuteReader();
                if (dr13.Read())
                {
                    transInvnBeforeTrigger.ActualInventoryUnits = Convert.ToDecimal(dr13["ACTL_INVN_UNITS"].ToString());
                    transInvnBeforeTrigger.ActualWeight = Convert.ToDecimal(dr13["ACTL_WT"].ToString());
                    trnList.Add(transInvnBeforeTrigger);
                }
            }
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
                ParserTestforMsgText(TransactionCode.Comt, swmToMheComt.SourceMessageText);
                WmsToEmsForComt(db, swmToMheComt.SourceMessageKey,TransactionCode.Comt);
                swmToMheIvmt = SwmToMhe(db, singleSkuCase.CaseNumber, TransactionCode.Ivmt, singleSkuCase.SkuId);
                ivmt = JsonConvert.DeserializeObject<IvmtDto>(swmToMheIvmt.MessageJson);
                ParserTestforMsgText(TransactionCode.Ivmt, swmToMheIvmt.SourceMessageText);
                WmsToEmsForIvmt(db, swmToMheIvmt.SourceMessageKey, TransactionCode.Ivmt);
                FetchCaseDetailsAfterTriger(db);
                unitWeight = FetchUnitWeight(db, singleSkuCase.SkuId);
                FetchTaskDetails(db, singleSkuCase.SkuId);
            }
        }

        protected void FetchCaseDetailsAfterTriger(OracleConnection db)
        {
            var sql15 = $"select TRANS_INVN.MOD_DATE_TIME, CASE_HDR.STAT_CODE,CASE_DTL.ACTL_QTY,CASE_DTL.TOTAL_ALLOC_QTY,TRANS_INVN.ACTL_INVN_UNITS,TRANS_INVN.ACTL_WT from CASE_HDR inner join CASE_DTL on CASE_HDR.CASE_NBR = CASE_DTL.CASE_NBR  inner join TRANS_INVN on CASE_DTL.SKU_ID = TRANS_INVN.SKU_ID  and TRANS_INVN.TRANS_INVN_TYPE = '18'  and CASE_HDR.CASE_NBR ='{singleSkuCase.CaseNumber}' order by TRANS_INVN.MOD_DATE_TIME desc";
            command = new OracleCommand(sql15, db);
            var dr13 = command.ExecuteReader();
            if (dr13.Read())
            {
                caseHdr.StatusCode = Convert.ToInt16(dr13["STAT_CODE"].ToString());
                trn.ActualInventoryUnits = Convert.ToDecimal(dr13["ACTL_INVN_UNITS"].ToString());
                trn.ActualWeight = Convert.ToDecimal(dr13["ACTL_WT"].ToString());
                caseDtlAfterTrigger.ActualQuantity = Convert.ToInt32(dr13["ACTL_QTY"].ToString());
                caseDtlAfterTrigger.TotalAllocatedQuantity = Convert.ToInt32(dr13["TOTAL_ALLOC_QTY"].ToString());
            }
        }
        protected decimal FetchUnitWeight(OracleConnection db, string skuId)
        {
            var sql14 = $"select unit_wt from item_master where sku_id = '{skuId}'";
            command = new OracleCommand(sql14, db);
            var unitWeight = Convert.ToDecimal(command.ExecuteScalar().ToString());
            return unitWeight;
        }

        protected void FetchTaskDetails(OracleConnection db,string skuId)
        {
            var sql18 = $"select * from task_hdr where sku_id = '{skuId}' order by mod_date_time desc";
            command = new OracleCommand(sql18, db);
            var dr10 = command.ExecuteReader();
            if (dr10.Read())
            {
                taskSingleSku.StatusCode = Convert.ToByte(dr10["STAT_CODE"].ToString());
            }
        }
        protected WmsToEmsDto WmsToEmsData(OracleConnection db, int msgKey, string trx)
        {
            var WmsToEms = new WmsToEmsDto();
            var sql11 = $"select * from WMSTOEMS where TRX = '{trx}' and MSGKEY = '{msgKey}'";
            command = new OracleCommand(sql11, db);
            var d10 = command.ExecuteReader();
            if (d10.Read())
            {
                WmsToEms.Status = d10["STS"].ToString();
                WmsToEms.ResponseCode = Convert.ToInt16(d10["RSNRCODE"].ToString());
                WmsToEms.MessageKey = Convert.ToUInt16(d10["MSGKEY"].ToString());
                WmsToEms.Transaction = d10["TRX"].ToString();
                WmsToEms.MessageText = d10["MSGTEXT"].ToString();      
            }
            return WmsToEms;       
        }


        protected void WmsToEmsForComt(OracleConnection db, int msgKey, string trx)
        {
            wmsToEmsComt = WmsToEmsData(db, msgKey, trx);
        }
        protected void WmsToEmsForIvmt(OracleConnection db, int msgKey, string trx)
        {
            wmsToEmsIvmt = WmsToEmsData(db, msgKey, trx);
        }
        protected SwmToMheDto SwmToMhe(OracleConnection db, string caseNbr, string trx, string skuId)
        {
            var swmtomhedata = new SwmToMheDto();
            var t = $"and sku_id = '{skuId}' ";
            var sql19 = $"select * from SWM_TO_MHE where container_id = '{caseNbr}' and source_msg_trans_code = '{trx}' ";
            var orderBy = "order by created_date_time desc";
            if(trx == TransactionCode.Ivmt)
            {
                sql19 = sql19 + t + orderBy;
            }
            else
            {
                sql19 = sql19 + orderBy;
            }
            command = new OracleCommand(sql19, db);
            var dr15 = command.ExecuteReader();
            if (dr15.Read())
            {
                swmtomhedata.SourceMessageKey = Convert.ToInt16(dr15["SOURCE_MSG_KEY"].ToString());
                swmtomhedata.SourceMessageResponseCode = Convert.ToInt16(dr15["SOURCE_MSG_RSN_CODE"].ToString());
                swmtomhedata.SourceMessageStatus = dr15["SOURCE_MSG_STATUS"].ToString();
                swmtomhedata.ContainerId = dr15["CONTAINER_ID"].ToString();
                swmtomhedata.ContainerType = dr15["CONTAINER_TYPE"].ToString();
                swmtomhedata.MessageJson = dr15["MSG_JSON"].ToString();
                swmtomhedata.LocationId = dr15["LOCN_ID"].ToString();
                swmtomhedata.SourceMessageText = dr15["SOURCE_MSG_TEXT"].ToString();    
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
                ParserTestforMsgText(TransactionCode.Comt, swmToMheComt.SourceMessageText);
                WmsToEmsForComt(db, swmToMheComt.SourceMessageKey, TransactionCode.Comt);               
                for (var i = 0; i < caseDtoList.Count; i++)
                {
                    swmToMheIvmt = SwmToMhe(db, caseHdrMultiSku.CaseNumber, TransactionCode.Ivmt, caseDtoList[i].SkuId);
                    ivmt = JsonConvert.DeserializeObject<IvmtDto>(swmToMheIvmt.MessageJson);
                    ParserTestforMsgText(TransactionCode.Ivmt, swmToMheIvmt.SourceMessageText);
                    var caseDtl = caseDtoList[i];
                    VerifyIvmtMessageWasInsertedIntoSwmToMhe(ivmt,caseDtl);
                    WmsToEmsForIvmt(db, swmToMheIvmt.SourceMessageKey, TransactionCode.Ivmt);
                    VerifyIvmtMessageWasInsertedIntoWmsToEms(wmsToEmsIvmt);
                    unitWeight = FetchUnitWeight(db, caseDtoList[i].SkuId);
                    FetchTransInvnData(db, caseDtoList[i].SkuId);
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
                    FetchTaskDetails(db, caseDtl.SkuId);                
                    VerifyStatusIsUpdatedIntoTaskHeader(taskMultiSku.StatusCode);
                }

                CaseHdrAndDtlDataAfterCallingApi(db);
            }
        }
        
        protected void FetchTransInvnData(OracleConnection db,string skuId)
        {
            var sql24 = $"select * from trans_invn where sku_id='{skuId}' order by mod_date_time desc";
            command = new OracleCommand(sql24, db);
            var dr13 = command.ExecuteReader();
            if (dr13.Read())
            {
                transInvnAfterTrigger.ActualInventoryUnits = Convert.ToDecimal(dr13["ACTL_INVN_UNITS"].ToString());
                transInvnAfterTrigger.ActualWeight = Convert.ToDecimal(dr13["ACTL_WT"].ToString());
                transInvnList.Add(transInvnAfterTrigger);
            }
        }

        protected void CaseHdrAndDtlDataAfterCallingApi(OracleConnection db)
        {
            var sql29 = $"select * from case_hdr where case_nbr ='{caseHdrMultiSku.CaseNumber}'";
            command = new OracleCommand(sql29, db);
            var dr29 = command.ExecuteReader();
            if (dr29.Read())
            {
                caseHeaderAfterTrigger.StatusCode = Convert.ToInt16(dr29["STAT_CODE"].ToString());
            }
            sqlStatement = $"select * from case_dtl where case_nbr = '{caseHdrMultiSku.CaseNumber}'";
            command = new OracleCommand(sqlStatement, db);
            var dr2 = command.ExecuteReader();
            while (dr2.Read())
            {
                var cd = new CaseDetailDto();
                cd.ActualQuantity = Convert.ToInt32(dr2["ACTL_QTY"].ToString());
                cd.TotalAllocatedQuantity = Convert.ToInt32(dr2["TOTAL_ALLOC_QTY"].ToString());
                caseList.Add(cd);
            }
        }
        
        protected void ParserTestforMsgText(string transactionCode,string sourceTextMsg)
        {         
            var testResult = _canParseMessage.ParseMessage(transactionCode, sourceTextMsg);
            Assert.AreEqual(testResult.ResultType, ResultTypes.Ok);               
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
                Assert.AreEqual(0, caseList[j].TotalAllocatedQuantity);
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

    
    



