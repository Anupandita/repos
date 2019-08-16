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
        public decimal unitWeight2;
        protected CaseHeaderDto caseSingleSku = new CaseHeaderDto();
        protected SwmToMheDto swmToMhe = new SwmToMheDto();
        protected ComtDto comt = new ComtDto();
        protected IvmtDto ivmt = new IvmtDto();       
        protected CaseDetailDto cd = new CaseDetailDto();
        protected WmsToEmsDto wmsToEms = new WmsToEmsDto();
        protected SwmToMheDto stm = new SwmToMheDto();
        protected TransitionalInventoryDto trn = new TransitionalInventoryDto();
        protected TaskDetailDto task = new TaskDetailDto();
        protected TransitionalInventoryDto transInvn = new TransitionalInventoryDto();
        protected CaseHeaderDto caseHdr = new CaseHeaderDto();
        protected CaseDetailDto caseDtl = new CaseDetailDto();     
        protected CaseHeaderDto caseHdrMultiSku = new CaseHeaderDto();
        protected CaseHeaderDto caseHeader = new CaseHeaderDto();
        protected List<CaseDetailDto> CaseDetailList = new List<CaseDetailDto>();
        protected List<TransitionalInventoryDto> transInvnList = new List<TransitionalInventoryDto>();
        protected List<TransitionalInventoryDto> trnList = new List<TransitionalInventoryDto>();
        protected List<CaseDetailDto> caseList = new List<CaseDetailDto>();
        protected List<IVMTDto> ivmtList = new List<IVMTDto>();       
        protected SwmToMheDto swmToMhe1 = new SwmToMheDto();
        protected SwmToMheDto stm2 = new SwmToMheDto();
        protected ComtDto comt1 = new ComtDto();
        protected WmsToEmsDto wmsToEms1 = new WmsToEmsDto();
        protected TaskHeaderDto task1 = new TaskHeaderDto();
        protected SwmToMheDto stm1 = new SwmToMheDto();
        protected IvmtDto ivmt1 = new IvmtDto();
        protected TransitionalInventoryDto trn1 = new TransitionalInventoryDto();
        protected TransitionalInventoryDto trn2  = new TransitionalInventoryDto();
        protected WmsToEmsDto wte1 = new WmsToEmsDto();
        protected TransitionalInventoryDto transInvn1 = new TransitionalInventoryDto();
        private readonly MessageHeaderParser _canParseMessage;
        protected OracleCommand command;
        protected string sql1 = "";
        protected dynamic trnInvnBeforeApi;


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
                var singleSkuQuery = $"select ch.case_nbr,ch.po_nbr,ch.stat_code,ch.locn_id,cd.sku_id,cd.actl_qty,cd.total_alloc_qty " +
                    $"from  CASE_HDR ch INNER JOIN CASE_DTL cd  on cd.CASE_NBR = ch.CASE_NBR  and cd.total_alloc_qty >1 and cd.case_seq_nbr = 1 " +
                    $"and  ch.stat_code = 50 and ch.po_nbr!= 'null'";
                command = new OracleCommand(singleSkuQuery, db);
                var dr1 = command.ExecuteReader();
                if (dr1.Read())
                {
                    caseSingleSku.CaseNumber = dr1["CASE_NBR"].ToString();
                    caseSingleSku.LocationId = dr1["LOCN_ID"].ToString();
                    caseSingleSku.StatusCode = Convert.ToInt16(dr1["STAT_CODE"].ToString());
                    cd.SkuId = dr1["SKU_ID"].ToString();
                    cd.ActualQuantity = Convert.ToInt32(dr1["ACTL_QTY"].ToString());
                    cd.TotalAllocatedQuantity = Convert.ToInt32(dr1["TOTAL_ALLOC_QTY"].ToString());                  
                }
                var sql2 = $"Select * from trans_invn where sku_id = '{cd.SkuId}' and  trans_invn_type = '18'";
                command = new OracleCommand(sql2, db);
                var dr2 = command.ExecuteReader();
                if(dr2.Read())
                {
                    transInvn.ActualInventoryUnits = Convert.ToInt16(dr2["ACTL_INVN_UNITS"].ToString());
                    transInvn.ActualWeight = Convert.ToDecimal(dr2["ACTL_WT"].ToString());
                }

                MultiSkuData(db);
            }
        }

        public void MultiSkuData(OracleConnection db)
        {
            var multiSkuQuery = $"select CASE_HDR.CASE_NBR,CASE_HDR.LOCN_ID,CASE_HDR.STAT_CODE from  CASE_HDR inner join CASE_DTL on CASE_HDR.CASE_NBR = CASE_DTL.CASE_NBR and CASE_DTL.total_alloc_qty >1 and actl_qty>1 and CASE_DTL.CASE_SEQ_NBR = 2 and stat_code = '50' and CASE_HDR.po_nbr!= 'null'";
            var command = new OracleCommand(multiSkuQuery, db);
            var dr19 = command.ExecuteReader();
            if (dr19.Read())
            {
                caseHdrMultiSku.CaseNumber = dr19["CASE_NBR"].ToString();
                caseHdrMultiSku.LocationId = dr19["LOCN_ID"].ToString();
                caseHdrMultiSku.StatusCode = Convert.ToInt16(dr19["STAT_CODE"].ToString());
            }          
            ivmtList = GetCaseData(db, caseHdrMultiSku.CaseNumber);
            FetchTransInvnDataForMultiSku(db);
        }

        public void FetchTransInvnDataForMultiSku(OracleConnection db)
        {
            for (var j = 0; j < ivmtList.Count; j++)
            {
                var sql9 = $"select sku_id,locn_id,trans_invn_type,actl_invn_units,actl_wt,user_id,mod_date_time from trans_invn where sku_id='{ivmtList[j].SkuId}'  and trans_invn_type = 18";
                command = new OracleCommand(sql9, db);
                var dr13 = command.ExecuteReader();
                if (dr13.Read())
                {
                    trn2.ActualInventoryUnits = Convert.ToDecimal(dr13["ACTL_INVN_UNITS"].ToString());
                    trn2.ActualWeight = Convert.ToDecimal(dr13["ACTL_WT"].ToString());
                    trnList.Add(trn2);
                }
            }
        }
       
        public List<IVMTDto> GetCaseData(OracleConnection db, string caseNumber)
        {
            var t = new List<IVMTDto>();
            var singleSkuQuery = $"select ch.case_nbr,ch.stat_code,ch.locn_id,cd.sku_id,cd.actl_qty,cd.total_alloc_qty,tn.actl_invn_units,tn.actl_wt from  CASE_HDR ch INNER JOIN CASE_DTL cd  on cd.CASE_NBR = ch.CASE_NBR  LEFT JOIN TRANS_INVN tn on cd.SKU_ID = tn.SKU_ID and cd.actl_qty >1 and  ch.stat_code = 50 and tn.trans_invn_type = '18' where  ch.case_nbr = '{caseNumber}'";
            var command = new OracleCommand(singleSkuQuery, db);
            var dr1 = command.ExecuteReader();
            while (dr1.Read())
            {
                var set = new IVMTDto();
                set.CaseNumber = dr1["CASE_NBR"].ToString();
                set.LocationId = dr1["LOCN_ID"].ToString();
                set.StatCode = Convert.ToInt16(dr1["STAT_CODE"].ToString());
                set.SkuId = dr1["SKU_ID"].ToString();
                set.ActlQty = Convert.ToInt32(dr1["ACTL_QTY"].ToString());
                set.TotalAllocQty = Convert.ToInt32(dr1["TOTAL_ALLOC_QTY"].ToString());
                t.Add(set);
            }
            return t;
        }

        public void GetDataAfterTriggerForComtForSingleSku()
        {
            OracleConnection db;
            using (db = new OracleConnection
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["SfcRbacContextModel"].ConnectionString
            })
            {
                db.Open();
                command = new OracleCommand();
                SwmToMheForComtData(db, caseSingleSku.CaseNumber, TransactionCode.Comt,null);
                WmsToEmsForComt(db, swmToMhe1.SourceMessageKey,TransactionCode.Comt);
                SwmToMheForIvmtData(db, caseSingleSku.CaseNumber, TransactionCode.Ivmt, cd.SkuId);
                WmsToEmsForIvmt(db, stm.SourceMessageKey, TransactionCode.Ivmt);
                FetchCaseDetailsAfterTriger(db);
                unitWeight = FetchUnitWeight(db, cd.SkuId);
                FetchTaskDetails(db, cd.SkuId);
            }
        }

        protected void FetchCaseDetailsAfterTriger(OracleConnection db)
        {
            var sql15 = $"select TRANS_INVN.MOD_DATE_TIME, CASE_HDR.STAT_CODE,CASE_DTL.ACTL_QTY,CASE_DTL.TOTAL_ALLOC_QTY,TRANS_INVN.ACTL_INVN_UNITS,TRANS_INVN.ACTL_WT from CASE_HDR inner join CASE_DTL on CASE_HDR.CASE_NBR = CASE_DTL.CASE_NBR  inner join TRANS_INVN on CASE_DTL.SKU_ID = TRANS_INVN.SKU_ID  and TRANS_INVN.TRANS_INVN_TYPE = '18'  and CASE_HDR.CASE_NBR ='{caseSingleSku.CaseNumber}' order by TRANS_INVN.MOD_DATE_TIME desc";
            command = new OracleCommand(sql15, db);
            var dr13 = command.ExecuteReader();
            if (dr13.Read())
            {
                caseHdr.StatusCode = Convert.ToInt16(dr13["STAT_CODE"].ToString());
                trn.ActualInventoryUnits = Convert.ToDecimal(dr13["ACTL_INVN_UNITS"].ToString());
                trn.ActualWeight = Convert.ToDecimal(dr13["ACTL_WT"].ToString());
                caseDtl.ActualQuantity = Convert.ToInt32(dr13["ACTL_QTY"].ToString());
                caseDtl.TotalAllocatedQuantity = Convert.ToInt32(dr13["TOTAL_ALLOC_QTY"].ToString());
            }

        }
        protected decimal FetchUnitWeight(OracleConnection db, string skuId)
        {
            
            var sql14 = $"select unit_wt from item_master where sku_id = '{skuId}'";
            command = new OracleCommand(sql14, db);
            var dr12 = command.ExecuteReader();
            if (dr12.Read())
            {
               unitWeight2 = Convert.ToDecimal(dr12["UNIT_WT"].ToString());
            }
            return unitWeight2;
        }

        protected void FetchTaskDetails(OracleConnection db,string skuId)
        {
            var sql18 = $"select * from task_hdr where sku_id = '{skuId}' order by mod_date_time desc";
            command = new OracleCommand(sql18, db);
            var dr10 = command.ExecuteReader();
            if (dr10.Read())
            {
                task.StatusCode = Convert.ToByte(dr10["STAT_CODE"].ToString());
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
            wte1 = WmsToEmsData(db, msgKey, trx);
        }
        protected void WmsToEmsForIvmt(OracleConnection db, int msgKey, string trx)
        {
            wmsToEms = WmsToEmsData(db, msgKey, trx);
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
        protected void SwmToMheForComtData(OracleConnection db, string caseNbr, string trx,string skuId)
        {
            swmToMhe1 = SwmToMhe(db, caseNbr, trx, skuId);
            var msgDto = JsonConvert.DeserializeObject<ComtDto>(swmToMhe1.MessageJson);
            comt1.Weight = msgDto.Weight;
            comt1.Fullness = msgDto.Fullness;
            comt1.TransactionCode = msgDto.TransactionCode;
            comt1.MessageLength = msgDto.MessageLength;
            comt1.ActionCode = msgDto.ActionCode;
            comt1.CurrentLocationId = msgDto.CurrentLocationId;
            comt1.ParentContainerId = msgDto.PhysicalContainerId;
        }
        protected void SwmToMheForIvmtData(OracleConnection db, string caseNbr, string trx, string skuId)
        {
            
            stm = SwmToMhe(db, caseNbr, trx, skuId);
            var msgDto = JsonConvert.DeserializeObject<IvmtDto>(stm.MessageJson);
            ivmt.TransactionCode = msgDto.TransactionCode;
            ivmt.MessageLength = msgDto.MessageLength;
            ivmt.ActionCode = msgDto.ActionCode;
            ivmt.ContainerId = msgDto.ContainerId;
            ivmt.Sku = msgDto.Sku;
            ivmt.Quantity = msgDto.Quantity;
            ivmt.UnitOfMeasure = msgDto.UnitOfMeasure;
            ivmt.DateControl = msgDto.DateControl;
            ivmt.QuantityToInduct = msgDto.QuantityToInduct;
            ivmt.Owner = msgDto.Owner;
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
                SwmToMheForComtData(db, caseHdrMultiSku.CaseNumber, TransactionCode.Comt,null);
                WmsToEmsForComt(db, swmToMhe1.SourceMessageKey, TransactionCode.Comt);               
                for (var i = 0; i < ivmtList.Count; i++)
                {
                    SwmToMheForIvmtData(db, caseHdrMultiSku.CaseNumber,TransactionCode.Ivmt ,ivmtList[i].SkuId);
                    var caseDtl = ivmtList[i];
                    VerifyIvmtMessageWasInsertedIntoSwmToMhe(ivmt, cd, caseDtl);
                    WmsToEmsForIvmt(db, stm.SourceMessageKey, TransactionCode.Ivmt);
                    VerifyIvmtMessageWasInsertedIntoWmsToEms(wmsToEms);
                    unitWeight = FetchUnitWeight(db, ivmtList[i].SkuId);
                    FetchTransInvnData(db, ivmtList[i].SkuId);
                    var trnInvnAfterApi = transInvnList[i];
                    try
                    {
                        trnInvnBeforeApi = trnList[i];
                    }
                    catch
                    {
                        Debug.Print("No TransInvn Record");
                    }
                    
                    VerifyQuantityIsIncreasedIntoTransInvn(trnInvnAfterApi, trnInvnBeforeApi);
                    FetchTaskDetails(db, caseDtl.SkuId);                
                    VerifyStatusIsUpdatedIntoTaskHeader(task1.StatusCode);
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
                trn1.ActualInventoryUnits = Convert.ToDecimal(dr13["ACTL_INVN_UNITS"].ToString());
                trn1.ActualWeight = Convert.ToDecimal(dr13["ACTL_WT"].ToString());
                transInvnList.Add(trn1);
            }
        }

        protected void CaseHdrAndDtlDataAfterCallingApi(OracleConnection db)
        {
            var sql29 = $"select * from case_hdr where case_nbr ='{caseHdrMultiSku.CaseNumber}'";
            command = new OracleCommand(sql29, db);
            var dr29 = command.ExecuteReader();
            if (dr29.Read())
            {
                caseHeader.StatusCode = Convert.ToInt16(dr29["STAT_CODE"].ToString());
            }
            sql1 = $"select * from case_dtl where case_nbr = '{caseHdrMultiSku.CaseNumber}'";
            command = new OracleCommand(sql1, db);
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
            Assert.AreEqual(DefaultValues.Status, swmToMhe1.SourceMessageStatus);
            Assert.AreEqual(DefaultValues.ContainerType, swmToMhe1.ContainerType);
            Assert.AreEqual(TransactionCode.Comt, comt1.TransactionCode);
            Assert.AreEqual(MessageLength.Comt, comt1.MessageLength);
            Assert.AreEqual(ActionCodeConstants.Create, comt1.ActionCode);
            Assert.AreEqual(caseNbr, swmToMhe1.ContainerId);
            Assert.AreEqual(DefaultValues.ContainerType, swmToMhe1.ContainerType);
        }

        protected void VerifyComtMessageWasInsertedIntoWmsToEms(WmsToEmsDto wte1)
        {
            Assert.AreEqual(swmToMhe1.SourceMessageStatus, wte1.Status);
            Assert.AreEqual(TransactionCode.Comt, wte1.Transaction);
            Assert.AreEqual(Convert.ToInt16(swmToMhe1.SourceMessageResponseCode), wte1.ResponseCode);
        }

        public void VerifyIvmtMessageWasInsertedIntoSwmToMhe(IvmtDto ivmt,CaseDetailDto cd,IVMTDto caseDtl)
        {
            Assert.AreEqual(DefaultValues.Status, stm.SourceMessageStatus);  
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
            Assert.AreEqual(stm.SourceMessageStatus, wmsToEms.Status);
            Assert.AreEqual(TransactionCode.Ivmt, wmsToEms.Transaction);
            Assert.AreEqual(Convert.ToInt16(stm.SourceMessageResponseCode), wmsToEms.ResponseCode);
        }
        protected void VerifyQuantityIsIncreasedIntoTransInvn(TransitionalInventoryDto trnInvnAfterApi, TransitionalInventoryDto trnInvnBeforeApi)
        {
            Assert.AreEqual(trn2.ActualInventoryUnits + Convert.ToDecimal(ivmt.Quantity), trn1.ActualInventoryUnits);
            Assert.AreEqual((unitWeight * Convert.ToDecimal(ivmt.Quantity)) + Convert.ToInt16(trn2.ActualWeight), Math.Round(Convert.ToDecimal(trn1.ActualWeight)));
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

    
    



