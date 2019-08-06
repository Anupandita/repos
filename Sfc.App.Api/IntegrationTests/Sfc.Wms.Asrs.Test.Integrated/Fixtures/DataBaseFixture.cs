using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using Sfc.Wms.Amh.Dematic.Contracts.Dtos;
using Sfc.Wms.Asrs.Dematic.Contracts.Dtos;
using Sfc.Wms.Asrs.Shamrock.Contracts.Dtos;
using Sfc.Wms.Asrs.Test.Integrated.TestData;
using Sfc.Wms.DematicMessage.Contracts.Constants;
using Sfc.Wms.Parser.Parsers;
using Sfc.Wms.ParserAndTranslator.Contracts.Dto;
using Sfc.Wms.ParserAndTranslator.Contracts.Interfaces;
using Sfc.Wms.ParserAndTranslator.Contracts.Validation;
using Sfc.Wms.Result;
using Sfc.Wms.TransitionalInventory.Contracts.Dtos;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace Sfc.Wms.Asrs.Test.Integrated.Fixtures
{
    public class DataBaseFixture
    {
        public decimal unitWeight;
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
        protected List<CaseDetailDto> caseList = new List<CaseDetailDto>();
        protected List<IVMTDto> ivmtList = new List<IVMTDto>();
        protected SwmToMheDto swmToMhe1 = new SwmToMheDto();
        protected ComtDto comt1 = new ComtDto();
        protected WmsToEmsDto wmsToEms1 = new WmsToEmsDto();
        protected TaskHeaderDto task1 = new TaskHeaderDto();
        protected SwmToMheDto stm1 = new SwmToMheDto();
        protected IvmtDto ivmt1 = new IvmtDto();
        protected TransitionalInventoryDto trn1 = new TransitionalInventoryDto();
        protected TransitionalInventoryDto trn2  = new TransitionalInventoryDto();
        protected WmsToEmsDto wte1 = new WmsToEmsDto();
        protected TransitionalInventoryDto transInvn1 = new TransitionalInventoryDto();
        private readonly IHaveDataTypeValidation _dataTypeValidation;
        protected string sql1 = "";
        
      
        public DataBaseFixture()
        {
            _dataTypeValidation = new DataTypeValidation();
        }

        public void GetDataBeforeTriggerComt() 
        {                    
            OracleCommand command;
            OracleConnection db;
            using (db = new OracleConnection
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["SfcRbacContextModel"].ConnectionString
            })
            {
                db.Open();
                var singleSkuQuery = $"select ch.case_nbr,ch.po_nbr,ch.stat_code,ch.locn_id,cd.sku_id,cd.actl_qty,cd.total_alloc_qty from  CASE_HDR ch INNER JOIN CASE_DTL cd  on cd.CASE_NBR = ch.CASE_NBR  and cd.actl_qty >10 and cd.case_seq_nbr = 1 and  ch.stat_code = 50 and ch.po_nbr!= 'null'";
                command = new OracleCommand(singleSkuQuery, db);
                var dr1 = command.ExecuteReader();
                if (dr1.Read())
                {
                    caseSingleSku.CaseNumber = dr1["CASE_NBR"].ToString();
                    caseSingleSku.LocationId = dr1["LOCN_ID"].ToString();
                    caseSingleSku.StatCode = Convert.ToInt16(dr1["STAT_CODE"].ToString());
                    cd.SkuId = dr1["SKU_ID"].ToString();
                    cd.ActlQty = Convert.ToInt32(dr1["ACTL_QTY"].ToString());
                    cd.TotalAllocQty = Convert.ToInt32(dr1["TOTAL_ALLOC_QTY"].ToString());                  
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
            var multiSkuQuery = $"select CASE_HDR.CASE_NBR,CASE_HDR.LOCN_ID,CASE_HDR.STAT_CODE from  CASE_HDR inner join CASE_DTL on CASE_HDR.CASE_NBR = CASE_DTL.CASE_NBR and CASE_DTL.ACTL_QTY >1 and CASE_DTL.CASE_SEQ_NBR = 2 and stat_code = '50' and CASE_HDR.po_nbr!= 'null'";
            var command = new OracleCommand(multiSkuQuery, db);
            var dr19 = command.ExecuteReader();
            if (dr19.Read())
            {
                caseHdrMultiSku.CaseNumber = dr19["CASE_NBR"].ToString();
                caseHdrMultiSku.LocationId = dr19["LOCN_ID"].ToString();
                caseHdrMultiSku.StatCode = Convert.ToInt16(dr19["STAT_CODE"].ToString());
            }          
            ivmtList = GetCaseData(db, caseHdrMultiSku.CaseNumber);

            for (var j = 0; j < ivmtList.Count; j++)
            {
                var sql9 = $"select sku_id,locn_id,trans_invn_type,actl_invn_units,actl_wt,user_id,mod_date_time from trans_invn where sku_id='{ivmtList[j].SkuId}'  and trans_invn_type = 18 order by mod_date_time desc";
                command = new OracleCommand(sql9, db);
                var dr13 = command.ExecuteReader();
                if (dr13.Read())
                {
                    trn2.ActualInventoryUnits = Convert.ToDecimal(dr13["ACTL_INVN_UNITS"].ToString());
                    trn2.ActualWeight = Convert.ToDecimal(dr13["ACTL_WT"].ToString());
                    transInvnList.Add(trn1);
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
            OracleCommand command;
            OracleConnection db;
            using (db = new OracleConnection
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["SfcRbacContextModel"].ConnectionString
            })
            {
                db.Open();
                command = new OracleCommand();
                SwmToMheData(command, db, caseSingleSku.CaseNumber, TransactionCode.Comt);
                WmsToEms(command, db, swmToMhe1.SourceMessageKey,TransactionCode.Comt);
                SwmToMheForIvmt(command, db, caseSingleSku.CaseNumber, cd.SkuId);
                WmsToEmsForIvmt(command, db, stm.SourceMessageKey, TransactionCode.Ivmt);

                var sql14 = $"select unit_wt from item_master where sku_id = '{cd.SkuId}'";
                command = new OracleCommand(sql14, db);
                var dr12 = command.ExecuteReader();
                if (dr12.Read())
                {
                   unitWeight = Convert.ToDecimal(dr12["UNIT_WT"].ToString());
                }

                var sql15 = $"select TRANS_INVN.MOD_DATE_TIME, CASE_HDR.STAT_CODE,CASE_DTL.ACTL_QTY,CASE_DTL.TOTAL_ALLOC_QTY,TRANS_INVN.ACTL_INVN_UNITS,TRANS_INVN.ACTL_WT from CASE_HDR inner join CASE_DTL on CASE_HDR.CASE_NBR = CASE_DTL.CASE_NBR  inner join TRANS_INVN on CASE_DTL.SKU_ID = TRANS_INVN.SKU_ID  and TRANS_INVN.TRANS_INVN_TYPE = '18'  and CASE_HDR.CASE_NBR ='{caseSingleSku.CaseNumber}' order by TRANS_INVN.MOD_DATE_TIME desc";
                command = new OracleCommand(sql15, db);
                var dr13 = command.ExecuteReader();
                if (dr13.Read())
                {
                    caseHdr.StatCode = Convert.ToInt16(dr13["STAT_CODE"].ToString());
                    trn.ActualInventoryUnits = Convert.ToDecimal(dr13["ACTL_INVN_UNITS"].ToString());
                    trn.ActualWeight = Convert.ToDecimal(dr13["ACTL_WT"].ToString());
                    caseDtl.ActlQty = Convert.ToInt32(dr13["ACTL_QTY"].ToString());
                    caseDtl.TotalAllocQty = Convert.ToInt32(dr13["TOTAL_ALLOC_QTY"].ToString());
                }

                var sql18 = $"select * from task_hdr where sku_id = '{cd.SkuId}' order by mod_date_time desc";
                command = new OracleCommand(sql18, db);
                var dr10 = command.ExecuteReader();
                if (dr10.Read())
                {
                    task.StatusCode = Convert.ToByte(dr10["STAT_CODE"].ToString());
                }
            }
        }

        protected void WmsToEms(OracleCommand command,OracleConnection db, int msgKey, string trx)
        {
            var sql11 = $"select * from WMSTOEMS where TRX = '{trx}' and MSGKEY = '{msgKey}'";
            command = new OracleCommand(sql11, db);
            var d10 = command.ExecuteReader();
            if (d10.Read())
            {
                wmsToEms.Status = d10["STS"].ToString();
                wmsToEms.ResponseCode = Convert.ToInt16(d10["RSNRCODE"].ToString());
                wmsToEms.MessageKey = Convert.ToUInt16(d10["MSGKEY"].ToString());
                wmsToEms.Transaction = d10["TRX"].ToString();
            }
        }

        protected void WmsToEmsForIvmt(OracleCommand command, OracleConnection db, int msgKey, string trx)
        {
            var sql11 = $"select * from WMSTOEMS where TRX = '{trx}' and MSGKEY = '{msgKey}'";
            command = new OracleCommand(sql11, db);
            var d10 = command.ExecuteReader();
            if (d10.Read())
            {
                wte1.Status = d10["STS"].ToString();
                wte1.ResponseCode = Convert.ToInt16(d10["RSNRCODE"].ToString());
                wte1.MessageKey = Convert.ToUInt16(d10["MSGKEY"].ToString());
                wte1.Transaction = d10["TRX"].ToString();
            }
        }

        protected void SwmToMheForIvmt(OracleCommand command,OracleConnection db,string caseNbr,string skuId)
        {
            var sql12 = $"select * from SWM_TO_MHE where container_id = '{caseNbr}' and source_msg_trans_code = 'IVMT' and sku_id='{skuId}'";
            command = new OracleCommand(sql12, db);
            var dr9 = command.ExecuteReader();
            if (dr9.Read())
            {
                stm.SourceMessageKey = Convert.ToInt16(dr9["SOURCE_MSG_KEY"].ToString());
                stm.SourceMessageResponseCode = Convert.ToInt16(dr9["SOURCE_MSG_RSN_CODE"].ToString());
                stm.SourceMessageStatus = dr9["SOURCE_MSG_STATUS"].ToString();
                stm.ContainerId = dr9["CONTAINER_ID"].ToString();
                stm.ContainerType = dr9["CONTAINER_TYPE"].ToString();
                stm.SkuId = dr9["SKU_ID"].ToString();
                stm.Quantity = Convert.ToInt16(dr9["QTY"].ToString());
                stm.MessageJson = dr9["MSG_JSON"].ToString();
                stm.SourceMessageProcess = dr9["SOURCE_MSG_PROCESS"].ToString();
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
                SwmToMheData(command, db, caseHdrMultiSku.CaseNumber, TransactionCode.Comt);
                VerifyComtMessageWasInsertedIntoSwmToMhe(comt1, swmToMhe1, caseHdrMultiSku.CaseNumber);
                WmsToEms(command, db, swmToMhe1.SourceMessageKey, TransactionCode.Comt);
                VerifyComtMessageWasInsertedIntoWmsToEms(wmsToEms);
                for (var i = 0; i < ivmtList.Count; i++)
                {
                    SwmToMheForIvmt(command, db, caseHdrMultiSku.CaseNumber, ivmtList[i].SkuId);
                    var caseDtl = ivmtList[i];
                    VerifyIvmtMessageWasInsertedIntoSwmToMhe(ivmt, cd, caseDtl.SkuId, caseDtl.ActlQty);
                    WmsToEmsForIvmt(command, db, stm.SourceMessageKey, TransactionCode.Ivmt);
                    VerifyIvmtMessageWasInsertedIntoWmsToEms(wte1);
                    var sql23 = $"select * from item_master where sku_id = '{ivmtList[i].SkuId}'";
                    command = new OracleCommand(sql23, db);
                    var dr12 = command.ExecuteReader();
                    if (dr12.Read())
                    {
                        unitWeight = Convert.ToDecimal(dr12["UNIT_WT"].ToString());
                    }

                    var sql24 = $"select * from trans_invn where sku_id='{ivmtList[i].SkuId}' order by mod_date_time desc";
                    command = new OracleCommand(sql24, db);
                    var dr13 = command.ExecuteReader();
                    if (dr13.Read())
                    {
                        trn1.ActualInventoryUnits = Convert.ToDecimal(dr13["ACTL_INVN_UNITS"].ToString());
                        trn1.ActualWeight = Convert.ToDecimal(dr13["ACTL_WT"].ToString());
                        transInvnList.Add(trn1);
                        var tr = transInvnList[i];
                        VerifyQuantityIsIncreasedIntoTransInvn(trn1,trn2);
                    }

                    var sql25 = $"select * from task_hdr where sku_id = '{ivmtList[i].SkuId}' order by mod_date_time desc";
                    command = new OracleCommand(sql25, db);
                    var dr10 = command.ExecuteReader();
                    if (dr10.Read())
                    {
                        task1.StatusCode = Convert.ToByte(dr10["STAT_CODE"].ToString());
                        VerifyStatusIsUpdatedIntoTaskHeader(task1.StatusCode);
                    }
                }

                    var sql9 = $"select * from case_hdr where case_nbr = '{caseHdrMultiSku.CaseNumber}'";
                    command = new OracleCommand(sql9, db);
                    var dr29 = command.ExecuteReader();
                    if (dr29.Read())
                    {
                      caseHeader.StatCode = Convert.ToInt16(dr29["STAT_CODE"].ToString());
                    }
                    sql1 = $"select * from case_dtl where case_nbr = '{caseHdrMultiSku.CaseNumber}'";
                    command = new OracleCommand(sql1, db);
                    var dr2 = command.ExecuteReader();
                    while (dr2.Read())
                    {
                        var cd = new CaseDetailDto();
                        cd.ActlQty = Convert.ToInt32(dr2["ACTL_QTY"].ToString());
                        cd.TotalAllocQty = Convert.ToInt32(dr2["TOTAL_ALLOC_QTY"].ToString());
                        caseList.Add(cd);
                    }       
            }
        }

        protected void SwmToMheData(OracleCommand command, OracleConnection db, string caseNbr, string trx)
        {
            var sql19 = $"select * from SWM_TO_MHE where container_id = '{caseNbr}' and source_msg_trans_code = '{trx}'";
            command = new OracleCommand(sql19, db);
            var dr15 = command.ExecuteReader();
            if (dr15.Read())
            {
                swmToMhe1.SourceMessageKey = Convert.ToInt16(dr15["SOURCE_MSG_KEY"].ToString());
                swmToMhe1.SourceMessageResponseCode = Convert.ToInt16(dr15["SOURCE_MSG_RSN_CODE"].ToString());
                swmToMhe1.SourceMessageStatus = dr15["SOURCE_MSG_STATUS"].ToString();
                swmToMhe1.ContainerId = dr15["CONTAINER_ID"].ToString();
                swmToMhe1.ContainerType = dr15["CONTAINER_TYPE"].ToString();
                swmToMhe1.MessageJson = dr15["MSG_JSON"].ToString();
                swmToMhe1.LocationId = dr15["LOCN_ID"].ToString();
                swmToMhe1.SourceMessageText = dr15["SOURCE_MSG_TEXT"].ToString();
                var msgDto = JsonConvert.DeserializeObject<ComtDto>(swmToMhe1.MessageJson);
                comt1.Weight = msgDto.Weight;
                comt1.Fullness = msgDto.Fullness;
                comt1.TransactionCode = msgDto.TransactionCode;
                comt1.MessageLength = msgDto.MessageLength;
                comt1.ActionCode = msgDto.ActionCode;
                comt1.CurrentLocationId = msgDto.CurrentLocationId;
                comt1.ParentContainerId = msgDto.PhysicalContainerId;
            }
        }

        protected void ParserTestForComtMsg(string sourceTextMsg)
        {
            var result = new BaseResult<DematicMessageBaseDto>
            {
                Payload = new DematicMessageBaseDto
                {
                    MessageLength = MessageLength.Comt,
                    ReasonCode = ReasonCode.Success,
                    TransactionCode = TransactionCode.Comt
                }
            };
            MessageParser mp = new MessageParser(_dataTypeValidation);
            var testResult = mp.ParseMessage<ComtDto, ComtValidationRule>(sourceTextMsg, result);
            Assert.AreEqual(testResult.ResultType, ResultTypes.Ok);
            Assert.IsNotNull(testResult.Payload);
        }

        protected void ParserTestForIvmtMsg(string sourceTextMsg)
        {
            var result = new BaseResult<DematicMessageBaseDto>
            {
                Payload = new DematicMessageBaseDto
                {
                    MessageLength = MessageLength.Ivmt,
                    ReasonCode = ReasonCode.Success,
                    TransactionCode = TransactionCode.Ivmt
                }
            };
            MessageParser mp = new MessageParser(_dataTypeValidation);
            var testResult = mp.ParseMessage<IvmtDto, IvmtValidationRule>(sourceTextMsg, result);
            Assert.AreEqual(testResult.ResultType, ResultTypes.Ok);
            Assert.IsNotNull(testResult.Payload);
        }

        protected void VerifyComtMessageWasInsertedIntoSwmToMhe(ComtDto comt, SwmToMheDto swmToMhe,string caseNbr)
        {
            Assert.AreEqual(DefaultValues.Status, swmToMhe1.SourceMessageStatus);
            Assert.AreEqual(DefaultValues.ContainerType, swmToMhe1.ContainerType);
            Assert.AreEqual(TransactionCode.Comt, comt1.TransactionCode);
            Assert.AreEqual(MessageLength.Comt, comt1.MessageLength);
            Assert.AreEqual(ActionCodeConstants.create, comt1.ActionCode);
            Assert.AreEqual(caseNbr, swmToMhe1.ContainerId);
            Assert.AreEqual(DefaultValues.ContainerType, swmToMhe1.ContainerType);
        }

        protected void VerifyComtMessageWasInsertedIntoWmsToEms(WmsToEmsDto wmsToEms)
        {
            Assert.AreEqual(swmToMhe1.SourceMessageStatus, wmsToEms.Status);
            Assert.AreEqual(TransactionCode.Comt, wmsToEms.Transaction);
            Assert.AreEqual(Convert.ToInt16(swmToMhe1.SourceMessageResponseCode), wmsToEms.ResponseCode);
        }

        public void VerifyIvmtMessageWasInsertedIntoSwmToMhe(IvmtDto ivmt,CaseDetailDto cd,string sku,double qty)
        {
            Assert.AreEqual(DefaultValues.Status, stm.SourceMessageStatus);  
            Assert.AreEqual(TransactionCode.Ivmt, ivmt.TransactionCode);
            Assert.AreEqual(MessageLength.Ivmt, ivmt.MessageLength);
            Assert.AreEqual(ActionCodeConstants.create, ivmt.ActionCode);
            Assert.AreEqual( sku, ivmt.Sku);
            Assert.AreEqual( qty, Convert.ToDouble(ivmt.Quantity));
            Assert.AreEqual(DefaultValues.ContainerType, ivmt.UnitOfMeasure);          
            Assert.AreEqual(DefaultValues.DataControl, ivmt.DateControl); 
        }
        protected void VerifyIvmtMessageWasInsertedIntoWmsToEms(WmsToEmsDto wte1)
        {
            Assert.AreEqual(stm.SourceMessageStatus, wte1.Status);
            Assert.AreEqual(TransactionCode.Ivmt, wte1.Transaction);
            Assert.AreEqual(Convert.ToInt16(stm.SourceMessageResponseCode), wte1.ResponseCode);
        }
        protected void VerifyQuantityIsIncreasedIntoTransInvn(TransitionalInventoryDto trn1,TransitionalInventoryDto trn2)
        {
            Assert.AreEqual(trn2.ActualInventoryUnits + Convert.ToDecimal(ivmt.Quantity), trn1.ActualInventoryUnits);
            Assert.AreEqual(Math.Round((unitWeight * Convert.ToDecimal(ivmt.Quantity)) + Convert.ToInt16(trn2.ActualWeight)), Math.Round(Convert.ToDecimal(trn1.ActualWeight)));
        }
        protected void VerifyQuantityisReducedIntoCaseDetailForMultiSku()
        {
            for (var j = 0; j < caseList.Count; j++)
            {
                Assert.AreEqual(0, caseList[j].ActlQty);
            }
        }
        protected void VerifyStatusIsUpdatedIntoCaseHeader(int statcode)
        {
            Assert.AreEqual(DefaultValues.CaseHdrStatCode,  statcode);
        }
        protected void VerifyStatusIsUpdatedIntoTaskHeader(int statcode)
        {
            Assert.AreEqual(DefaultValues.TaskStatusCode, statcode);
        }
    }
}

    
    



