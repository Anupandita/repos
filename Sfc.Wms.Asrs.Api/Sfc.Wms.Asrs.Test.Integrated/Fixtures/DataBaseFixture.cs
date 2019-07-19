using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;
using Newtonsoft.Json;
using Sfc.Wms.DematicMessage.Contracts.Dto;
using Sfc.Wms.Asrs.Shamrock.Contracts.Dtos;
using Sfc.Wms.Amh.Dematic.Contracts.Dtos;
using Sfc.Wms.Asrs.Dematic.Contracts.Dtos;
using Sfc.Wms.TransitionalInventory.Contracts.Dtos;
using System.Diagnostics;
using System.Collections.Generic;
using Sfc.Wms.DematicMessage.Contracts.Constants;

namespace Sfc.Wms.Asrs.Test.Integrated.Fixtures
{
    public class DataBaseFixture
    {     
        public decimal unitWeight;
        public string caseNumberSingleSku;
        public string caseNumberMultiSku;
        protected CaseHeaderDto caseHeader = new CaseHeaderDto();
        protected SwmToMheDto swmToMhe = new SwmToMheDto();
        protected ComtDto comt = new ComtDto();
        protected IvmtDto ivmt = new IvmtDto();       
        protected CaseDetailDto cd = new CaseDetailDto();
        protected WmsToEmsDto w = new WmsToEmsDto();
        protected WmsToEmsDto wte = new WmsToEmsDto();
        protected SwmToMheDto stm = new SwmToMheDto();
        protected TransitionalInventoryDto trn = new TransitionalInventoryDto();
        protected TaskDetailDto task = new TaskDetailDto();
        protected TransitionalInventoryDto transInvn = new TransitionalInventoryDto();
        protected CaseHeaderDto caseHdr = new CaseHeaderDto();
        protected CaseDetailDto caseDtl = new CaseDetailDto();     
        protected CaseHeaderDto caseHeader1 = new CaseHeaderDto();
        protected List<CaseDetailDto> CaseDetailList = new List<CaseDetailDto>();
        protected List<TransitionalInventoryDto> transInvnList = new List<TransitionalInventoryDto>();
        protected List<CaseDetailDto> caseList = new List<CaseDetailDto>();
        protected SwmToMheDto swmToMhe1 = new SwmToMheDto();
        protected ComtDto comt1 = new ComtDto();
        protected WmsToEmsDto wmsToEms1 = new WmsToEmsDto();
        protected TaskHeaderDto task1 = new TaskHeaderDto();
        protected SwmToMheDto stm1 = new SwmToMheDto();
        protected IvmtDto ivmt1 = new IvmtDto();
        protected TransitionalInventoryDto trn1 = new TransitionalInventoryDto();
        protected WmsToEmsDto wte1 = new WmsToEmsDto();
        protected TransitionalInventoryDto transInvn1 = new TransitionalInventoryDto();
        protected string sql1 = "";
        
        public void GetDataBeforeBeforeTriggerComt() 
        {                    
            OracleCommand command;
            OracleConnection db;
            using (db = new OracleConnection
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["SfcRbacContextModel"].ConnectionString
            })
            {
                db.Open();
                var sql1 = $"select CASE_NBR from CASE_HDR where STAT_CODE = 50 and CASE_NBR in (select CASE_NBR from CASE_DTL where ACTL_QTY >1 and CASE_SEQ_NBR = 1)";
                command = new OracleCommand(sql1, db);
                caseNumberSingleSku = command.ExecuteScalar().ToString();               

                var sql2 = $"select CASE_HDR.CASE_NBR from  CASE_HDR inner join CASE_DTL on CASE_HDR.CASE_NBR = CASE_DTL.CASE_NBR and CASE_DTL.ACTL_QTY > 1 and CASE_DTL.CASE_SEQ_NBR = 2";
                command = new OracleCommand(sql2, db);
                caseNumberMultiSku = command.ExecuteScalar().ToString();               
                             
                var sql3 = $"select case_nbr,locn_id,actl_wt,mfg_date,ship_by_date,xpire_date,po_nbr,stat_code,prev_locn_id,mod_date_time,user_id from case_hdr where case_nbr = '{caseNumberSingleSku}'";
                command = new OracleCommand(sql3, db);
                var dr21 = command.ExecuteReader();
                if (dr21.Read())
                {
                    caseHeader.CaseNumber = dr21["CASE_NBR"].ToString();
                    //caseHeader.ActlWt = Convert.ToInt16(dr1["ACTL_WT"].ToString());
                    caseHeader.LocationId = dr21["LOCN_ID"].ToString();
                    caseHeader.StatCode = Convert.ToInt16(dr21["STAT_CODE"].ToString());
                }

                var sql4 = $"select case_nbr,sku_id,actl_qty,batch_nbr,total_alloc_qty,orig_qty from case_dtl where case_nbr = '{caseNumberSingleSku}'";
                command = new OracleCommand(sql4, db);
                var dr2 = command.ExecuteReader();
                if (dr2.Read())
                {
                    cd.CaseNumber = dr2["CASE_NBR"].ToString();
                    cd.SkuId = dr2["SKU_ID"].ToString();
                    cd.ActlQty = Convert.ToInt32(dr2["ACTL_QTY"].ToString());
                    cd.TotalAllocQty = Convert.ToInt32(dr2["TOTAL_ALLOC_QTY"].ToString());
                }

                try
                {
                   var sql5 = $"select sku_id,locn_id,actl_invn_units,actl_wt,trans_invn_type,user_id,mod_date_time from trans_invn where sku_id = '{cd.SkuId}' and trans_invn_type = '18' order by mod_date_time desc";
                    command = new OracleCommand(sql5, db);
                    var tr = command.ExecuteReader();
                    if (tr.Read())
                    {
                        transInvn.SkuId = tr["SKU_ID"].ToString();
                        transInvn.ActualInventoryUnits = Convert.ToInt16(tr["ACTL_INVN_UNITS"].ToString());
                        transInvn.ActualWeight = Convert.ToInt16(tr["ACTL_WT"].ToString());
                        transInvn.TransitionInventoryType = Convert.ToInt16(tr["TRANS_INVN_TYPE"].ToString());
                    }
                }
                catch
                {
                    Debug.Print("There are no records");
                }

                var sql6 = $"select CASE_NBR from CASE_HDR where STAT_CODE = 50 and CASE_NBR in (select CASE_NBR from CASE_DTL where ACTL_QTY >1 and CASE_SEQ_NBR = 1)";
                command = new OracleCommand(sql6, db);
                var dr0 = command.ExecuteReader();
                if (dr0.Read())
                {
                    caseNumberMultiSku = dr0["CASE_NBR"].ToString();
                }

                var sql7 = $"select case_nbr,locn_id,actl_wt,mfg_date,ship_by_date,xpire_date,po_nbr,stat_code,prev_locn_id,mod_date_time,user_id from case_hdr where case_nbr = '{caseNumberMultiSku}'";
                command = new OracleCommand(sql7, db);
                var dr19 = command.ExecuteReader();
                if (dr19.Read())
                {
                    caseHeader1.CaseNumber = dr19["CASE_NBR"].ToString();
                    //caseHeader.ActlWt = Convert.ToInt16(dr1["ACTL_WT"].ToString());
                    caseHeader1.LocationId = dr19["LOCN_ID"].ToString();
                    caseHeader1.StatCode = Convert.ToInt16(dr19["STAT_CODE"].ToString());
                }

                var sql8 = $"select case_nbr,sku_id,actl_qty,batch_nbr,total_alloc_qty,orig_qty from case_dtl where case_nbr = '{caseNumberMultiSku}'";
                command = new OracleCommand(sql8, db);
                var dr20 = command.ExecuteReader();
                while (dr2.Read())
                {
                    var cd = new CaseDetailDto();
                    cd.CaseNumber = dr20["CASE_NBR"].ToString();
                    cd.SkuId = dr20["SKU_ID"].ToString();
                    cd.ActlQty = Convert.ToInt32(dr20["ACTL_QTY"].ToString());
                    cd.TotalAllocQty = Convert.ToInt32(dr20["TOTAL_ALLOC_QTY"].ToString());
                    CaseDetailList.Add(cd);
                }

                for (var j = 0; j < CaseDetailList.Count; j++)
                {
                    var sql9 = $"select sku_id,locn_id,trans_invn_type,actl_invn_units,actl_wt,user_id,mod_date_time from trans_invn where sku_id='{CaseDetailList[j].SkuId}' order by mod_date_time desc";
                    command = new OracleCommand(sql9, db);
                    var dr13 = command.ExecuteReader();
                    if (dr13.Read())
                    {
                        trn1.ActualInventoryUnits = Convert.ToDecimal(dr13["ACTL_INVN_UNITS"].ToString());
                        trn1.ActualWeight = Convert.ToDecimal(dr13["ACTL_WT"].ToString());
                        transInvnList.Add(trn1);
                    }
                }
            }
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
                var sql10 = $"select source_msg_key,source_msg_process,source_msg_status,source_msg_trans_code,source_msg_rsn_code,error_msg,error_code,msg_status,msg_json,container_id,container_type,locn_id,lot_id,po_nbr,sku_id,staging_locn,qty,created_date_time from SWM_TO_MHE where container_id = '{caseNumberSingleSku}' and source_msg_trans_code = 'COMT'";
                command = new OracleCommand(sql10, db);
                var dr15 = command.ExecuteReader();
                if (dr15.Read())
                {
                    swmToMhe.SourceMessageKey = Convert.ToInt16(dr15["SOURCE_MSG_KEY"].ToString());
                    swmToMhe.SourceMessageResponseCode = Convert.ToInt16(dr15["SOURCE_MSG_RSN_CODE"].ToString());
                    swmToMhe.SourceMessageStatus = dr15["SOURCE_MSG_STATUS"].ToString();
                    swmToMhe.CreatedDateTime = Convert.ToDateTime(dr15["CREATED_DATE_TIME"].ToString());
                    swmToMhe.SourceMessageProcess = dr15["SOURCE_MSG_PROCESS"].ToString();
                    swmToMhe.ContainerId = dr15["CONTAINER_ID"].ToString();
                    swmToMhe.ContainerType = dr15["CONTAINER_TYPE"].ToString();
                    swmToMhe.MessageJson = dr15["MSG_JSON"].ToString();
                    swmToMhe.LocationId = dr15["LOCN_ID"].ToString();
                    var msgDto = JsonConvert.DeserializeObject<ComtDto>(swmToMhe.MessageJson);
                    comt.Weight = msgDto.Weight;
                    comt.Fullness = msgDto.Fullness;
                    comt.TransactionCode = msgDto.TransactionCode;
                    comt.MessageLength = msgDto.MessageLength;
                    comt.ActionCode = msgDto.ActionCode;
                    comt.CurrentLocationId = msgDto.CurrentLocationId;
                    comt.ParentContainerId = msgDto.PhysicalContainerId;
                }

                var sql11 = $"select * from WMSTOEMS where TRX = 'COMT' order by MSGKEY desc";
                command = new OracleCommand(sql11, db);
                var d10 = command.ExecuteReader();
                if (d10.Read())
                {
                    w.Status = d10["STS"].ToString();
                    w.ResponseCode = Convert.ToInt16(d10["RSNRCODE"].ToString());
                    w.AddWho = d10["ADDWHO"].ToString();
                    w.MessageKey = Convert.ToUInt16(d10["MSGKEY"].ToString());
                    w.Transaction = d10["TRX"].ToString();
                }

                var sql12 = $"select source_msg_key,source_msg_process,source_msg_status,source_msg_trans_code,source_msg_rsn_code,error_msg,error_code,msg_status,msg_json,container_id,container_type,locn_id,lot_id,po_nbr,sku_id,staging_locn,qty from SWM_TO_MHE where container_id = '{caseNumberSingleSku}' and source_msg_trans_code = 'IVMT' and sku_id='{cd.SkuId}'";
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
            
                var sql13 = $"select * from WMSTOEMS where TRX = 'IVMT' order by MSGKEY desc";
                command = new OracleCommand(sql13, db);
                var d14 = command.ExecuteReader();
                if (d14.Read())
                {
                    wte.Status = d14["STS"].ToString();
                    wte.ResponseCode = Convert.ToInt16(d14["RSNRCODE"].ToString());
                    wte.AddWho = d14["ADDWHO"].ToString();
                    wte.MessageKey = Convert.ToUInt16(d14["MSGKEY"].ToString());
                    wte.Transaction = d14["TRX"].ToString();
                }

                var sql14 = $"select unit_wt from item_master where sku_id = '{cd.SkuId}'";
                command = new OracleCommand(sql14, db);
                var dr12 = command.ExecuteReader();
                if (dr12.Read())
                {
                   unitWeight = Convert.ToDecimal(dr12["UNIT_WT"].ToString());
                }

                var sql15 = $"select sku_id,locn_id,trans_invn_type,actl_invn_units,actl_wt,user_id,mod_date_time from trans_invn where sku_id='{cd.SkuId}' order by mod_date_time desc";
                command = new OracleCommand(sql15, db);
                var dr13 = command.ExecuteReader();
                if (dr13.Read())
                {
                    trn.ActualInventoryUnits = Convert.ToDecimal(dr13["ACTL_INVN_UNITS"].ToString());
                    trn.ActualWeight = Convert.ToDecimal(dr13["ACTL_WT"].ToString());
                }

                var sql16 = $"select case_nbr,locn_id,actl_wt,mfg_date,ship_by_date,xpire_date,po_nbr,stat_code,prev_locn_id,mod_date_time,user_id from case_hdr where case_nbr = '{caseNumberSingleSku}'";
                command = new OracleCommand(sql16, db);
                var dr1 = command.ExecuteReader();
                if (dr1.Read())
                {                  
                    caseHdr.StatCode = Convert.ToInt16(dr1["STAT_CODE"].ToString());
                }

                var sql17 = $"select case_nbr,sku_id,actl_qty,batch_nbr,total_alloc_qty,orig_qty from case_dtl where case_nbr = '{caseNumberSingleSku}'";
                command = new OracleCommand(sql17, db);
                var dr2 = command.ExecuteReader();
                if (dr2.Read())
                {
                    caseDtl.ActlQty = Convert.ToInt32(dr2["ACTL_QTY"].ToString());
                    caseDtl.TotalAllocQty = Convert.ToInt32(dr2["TOTAL_ALLOC_QTY"].ToString());
                }

                var sql18 = $"select stat_code from task_hdr where sku_id = '{cd.SkuId}' order by mod_date_time desc";
                command = new OracleCommand(sql18, db);
                var dr10 = command.ExecuteReader();
                if (dr10.Read())
                {
                    task.StatusCode = Convert.ToByte(dr10["STAT_CODE"].ToString());
                }
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
                var sql19 = $"select source_msg_key,source_msg_status,source_msg_trans_code,source_msg_rsn_code,error_msg,error_code,msg_status,msg_json,container_id,container_type,locn_id,lot_id,po_nbr,sku_id,staging_locn,qty,created_date_time from SWM_TO_MHE where container_id = '{caseNumberMultiSku}' and source_msg_trans_code = 'COMT'";
                command = new OracleCommand(sql19, db);
                var dr15 = command.ExecuteReader();
                if (dr15.Read())
                {
                    swmToMhe1.SourceMessageKey = Convert.ToInt16(dr15["SOURCE_MSG_KEY"].ToString());
                    swmToMhe1.SourceMessageResponseCode = Convert.ToInt16(dr15["SOURCE_MSG_RSN_CODE"].ToString());
                    swmToMhe1.SourceMessageStatus = dr15["SOURCE_MSG_STATUS"].ToString();
                    swmToMhe1.CreatedDateTime = Convert.ToDateTime(dr15["CREATED_DATE_TIME"].ToString());
                    swmToMhe1.ContainerId = dr15["CONTAINER_ID"].ToString();
                    swmToMhe1.ContainerType = dr15["CONTAINER_TYPE"].ToString();
                    swmToMhe1.MessageJson = dr15["MSG_JSON"].ToString();
                    swmToMhe1.LocationId = dr15["LOCN_ID"].ToString();
                    var msgDto = JsonConvert.DeserializeObject<ComtDto>(swmToMhe1.MessageJson);
                    comt1.Weight = msgDto.Weight;
                    comt1.Fullness = msgDto.Fullness;
                    comt1.TransactionCode = msgDto.TransactionCode;
                    comt1.MessageLength = msgDto.MessageLength;
                    comt1.ActionCode = msgDto.ActionCode;
                    comt1.CurrentLocationId = msgDto.CurrentLocationId;
                    comt1.ParentContainerId = msgDto.PhysicalContainerId;
                    VerifyComtMessageWasInsertedIntoSwmToMhe(comt1, swmToMhe1);
                }

                var sql20 = $"select msgkey,sts,trx,msgtext,rsnrcode,addwho from wmstoems where msgkey = {swmToMhe1.SourceMessageKey}";
                command = new OracleCommand(sql20, db);
                var d10 = command.ExecuteReader();
                if (d10.Read())
                {
                    wmsToEms1.Status = d10["STS"].ToString();
                    wmsToEms1.ResponseCode = Convert.ToInt16(d10["RSNRCODE"].ToString());
                    wmsToEms1.AddWho = d10["ADDWHO"].ToString();
                    wmsToEms1.MessageKey = Convert.ToUInt16(d10["MSGKEY"].ToString());
                    wmsToEms1.Transaction = d10["TRX"].ToString();
                    VerifyComtMessageWasInsertedIntoWmsToEms(wmsToEms1);
                }

                for (var i = 0; i < CaseDetailList.Count; i++)
                {
                    var sql21 = $"select source_msg_key,source_msg_status,source_msg_trans_code,source_msg_rsn_code,error_msg,error_code,msg_status,msg_json,container_id,container_type,locn_id,lot_id,po_nbr,sku_id,staging_locn,qty from SWM_TO_MHE where container_id = '{caseNumberMultiSku}' and source_msg_trans_code = 'IVMT' and sku_id='{CaseDetailList[i].SkuId}'";
                    command = new OracleCommand(sql21, db);
                    var dr9 = command.ExecuteReader();
                    if (dr9.Read())
                    {
                        stm1.SourceMessageKey = Convert.ToInt16(dr9["SOURCE_MSG_KEY"].ToString());
                        stm1.SourceMessageResponseCode = Convert.ToInt16(dr9["SOURCE_MSG_RSN_CODE"].ToString());
                        stm1.SourceMessageStatus = dr9["SOURCE_MSG_STATUS"].ToString();
                        stm1.ContainerId = dr9["CONTAINER_ID"].ToString();
                        stm1.ContainerType = dr9["CONTAINER_TYPE"].ToString();
                        stm1.SkuId = dr9["SKU_ID"].ToString();
                        stm1.Quantity = Convert.ToInt16(dr9["QTY"].ToString());
                        stm1.MessageJson = dr9["MSG_JSON"].ToString();
                        var msgDto = JsonConvert.DeserializeObject<IvmtDto>(stm1.MessageJson);
                        ivmt1.TransactionCode = msgDto.TransactionCode;
                        ivmt1.MessageLength = msgDto.MessageLength;
                        ivmt1.ActionCode = msgDto.ActionCode;
                        ivmt1.ContainerId = msgDto.ContainerId;
                        ivmt1.Sku = msgDto.Sku;
                        ivmt1.Quantity = msgDto.Quantity;
                        ivmt1.UnitOfMeasure = msgDto.UnitOfMeasure;
                        ivmt1.DateControl = msgDto.DateControl;
                        ivmt1.QuantityToInduct = msgDto.QuantityToInduct;
                        ivmt1.Owner = msgDto.Owner;
                        var caseDetail = CaseDetailList[i];
                        VerifyIvmtMessageWasInsertedIntoSwmToMhe(caseDetail, ivmt1);
                    }

                    var sql22 = $"select msgkey,sts,trx,msgtext,rsnrcode,addwho from wmstoems where msgkey = {stm1.SourceMessageKey}";
                    command = new OracleCommand(sql22, db);
                    var d14 = command.ExecuteReader();
                    if (d14.Read())
                    {
                        wte1.Status = d14["STS"].ToString();
                        wte1.ResponseCode = Convert.ToInt16(d14["RSNRCODE"].ToString());
                        wte1.AddWho = d14["ADDWHO"].ToString();
                        wte1.MessageKey = Convert.ToUInt16(d14["MSGKEY"].ToString());
                        wte1.Transaction = d14["TRX"].ToString();
                        VerifyIvmtMessageWasInsertedIntoWmsToEms(wte1);
                    }

                    var sql23 = $"select unit_wt from item_master where sku_id = '{CaseDetailList[i].SkuId}'";
                    command = new OracleCommand(sql23, db);
                    var dr12 = command.ExecuteReader();
                    if (dr12.Read())
                    {
                        unitWeight = Convert.ToDecimal(dr12["UNIT_WT"].ToString());
                    }

                    var sql24 = $"select sku_id,locn_id,trans_invn_type,actl_invn_units,actl_wt,user_id,mod_date_time from trans_invn where sku_id='{CaseDetailList[i].SkuId}' order by mod_date_time desc";
                    command = new OracleCommand(sql24, db);
                    var dr13 = command.ExecuteReader();
                    if (dr13.Read())
                    {
                        trn1.ActualInventoryUnits = Convert.ToDecimal(dr13["ACTL_INVN_UNITS"].ToString());
                        trn1.ActualWeight = Convert.ToDecimal(dr13["ACTL_WT"].ToString());
                        var trnIn = transInvnList[i];
                        VerifyQuantityIsIncreasedIntoTransInvn(trnIn, trn1);
                    }

                    var sql25 = $"select stat_code from task_hdr where sku_id = '{CaseDetailList[i].SkuId}' order by mod_date_time desc";
                    command = new OracleCommand(sql25, db);
                    var dr10 = command.ExecuteReader();
                    if (dr10.Read())
                    {
                        task1.StatusCode = Convert.ToByte(dr10["STAT_CODE"].ToString());
                        VerifyStatusIsUpdatedIntoTaskHeader(task1);
                    }

                }

                    sql1 = $"select case_nbr,locn_id,actl_wt,mfg_date,ship_by_date,xpire_date,po_nbr,stat_code,prev_locn_id,mod_date_time,user_id from case_hdr where case_nbr = '{caseNumberMultiSku}'";
                    command = new OracleCommand(sql1, db);
                    var dr29 = command.ExecuteReader();
                    if (dr29.Read())
                    {
                        caseHeader1.StatCode = Convert.ToInt16(dr29["STAT_CODE"].ToString());
                    }

                    sql1 = $"select case_nbr,sku_id,actl_qty,batch_nbr,total_alloc_qty,orig_qty from case_dtl where case_nbr = '{caseNumberMultiSku}'";
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
        protected void VerifyComtMessageWasInsertedIntoSwmToMhe(ComtDto comt, SwmToMheDto swmToMhe)
        {
            Assert.AreEqual("Ready", swmToMhe.SourceMessageStatus);
            Assert.AreEqual("Case", swmToMhe.ContainerType);
            Assert.AreEqual(0, caseHeader1.ActlWt);
            Assert.AreEqual(TransactionCode.Comt, comt.TransactionCode);
            Assert.AreEqual(MessageLength.Comt, comt.MessageLength);
            Assert.AreEqual(ActionCodeConstants.create, comt.ActionCode);
            Assert.AreEqual(caseHeader1.CaseNumber, swmToMhe.ContainerId);
            Assert.AreEqual("Case", swmToMhe.ContainerType);
        }
        protected void VerifyComtMessageWasInsertedIntoWmsToEms(WmsToEmsDto wmsToEms)
        {
            Assert.AreEqual(swmToMhe1.SourceMessageStatus, wmsToEms.Status);
            Assert.AreEqual(TransactionCode.Comt, wmsToEms.Transaction);
            Assert.AreEqual(Convert.ToInt16(swmToMhe1.SourceMessageResponseCode), wmsToEms.ResponseCode);
        }

        public void VerifyIvmtMessageWasInsertedIntoSwmToMhe(CaseDetailDto caseDetail, IvmtDto ivmt)
        {
            Assert.AreEqual(caseDetail.SkuId, ivmt.Sku);
            Assert.AreEqual(caseDetail.ActlQty, Convert.ToDouble(ivmt.Quantity));
            Assert.AreEqual("Ready", stm1.SourceMessageStatus);
            Assert.AreEqual("Warehouse", ivmt.Owner);
            Assert.AreEqual("IVMT", ivmt.TransactionCode);
            Assert.AreEqual("00217", ivmt.MessageLength);
            Assert.AreEqual("Create", ivmt.ActionCode);
            Assert.AreEqual("Case", ivmt.UnitOfMeasure);
            Assert.AreEqual(caseHeader1.ManufacturingDate, ivmt.ManufactureDate);
            Assert.AreEqual(caseHeader1.ShipByDate, ivmt.FifoDate);
            Assert.AreEqual(caseHeader1.XpireDate, ivmt.ExpirationDate);
            Assert.AreEqual("F", ivmt.DateControl);
        }
        protected void VerifyIvmtMessageWasInsertedIntoWmsToEms(WmsToEmsDto wte)
        {
            Assert.AreEqual("Ready", wte.Status);
            Assert.AreEqual("IVMT", wte.Transaction);
            Assert.AreEqual(Convert.ToInt16(stm1.SourceMessageResponseCode), wte.ResponseCode);
        }
        protected void VerifyQuantityIsIncreasedIntoTransInvn(TransitionalInventoryDto trn, TransitionalInventoryDto trnIn)
        {
            Assert.AreEqual(trnIn.ActualInventoryUnits + Convert.ToDecimal(ivmt1.Quantity), trn.ActualInventoryUnits);
            Assert.AreEqual((unitWeight * Convert.ToDecimal(ivmt1.Quantity)) + Convert.ToInt16(trnIn.ActualWeight), Convert.ToDecimal(trn.ActualWeight));
        }
        protected void VerifyQuantityisReducedIntoCaseDetailForMultiSku()
        {
            for (var j = 0; j < caseList.Count; j++)
            {
                Assert.AreEqual(0, caseList[j].ActlQty);
            }
        }
        protected void VerifyStatusIsUpdatedIntoCaseHeaderForMultiSku()
        {
            Assert.AreEqual(96, caseHeader1.StatCode);
        }
        protected void VerifyStatusIsUpdatedIntoTaskHeader(TaskHeaderDto task1)
        {
            Assert.AreEqual(90, task1.StatusCode);
        }
    }
}

    
    



