using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using Sfc.Wms.Amh.Dematic.Contracts.Dtos;
using Sfc.Wms.Asrs.Dematic.Contracts.Dtos;
using Sfc.Wms.Asrs.Shamrock.Contracts.Dtos;
using Sfc.Wms.DematicMessage.Contracts.Dto;
using Sfc.Wms.TransitionalInventory.Contracts.Dtos;

namespace Sfc.Wms.Asrs.Test.Integrated.Fixtures
{
    [TestClass]
    public class DataBaseFixtureForMultiSku 
    {
        public decimal unitWeight;
        protected CaseHeaderDto cn = new CaseHeaderDto();
        protected CaseHeaderDto caseHeader = new CaseHeaderDto();
        protected List<CaseDetailDto> CaseDetailList = new List<CaseDetailDto>();
        protected List<CaseDetailDto> caseList = new List<CaseDetailDto>();
        protected SwmToMheDto swmToMhe = new SwmToMheDto();
        protected ComtDto comt = new ComtDto();
        protected WmsToEmsDto wmsToEms = new WmsToEmsDto();
        protected TaskHeaderDto task  = new TaskHeaderDto();
        protected SwmToMheDto stm = new SwmToMheDto();
        protected IvmtDto ivmt = new IvmtDto();
        protected TransitionalInventoryDto trn = new TransitionalInventoryDto();
        protected WmsToEmsDto wte = new WmsToEmsDto();
        protected string sql1 = "";

        public void GetDataBeforeTriggerForComt()
        {            
            OracleCommand command;
            OracleConnection db;
            using (db = new OracleConnection
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["SfcRbacContextModel"].ConnectionString
            })
            {
                db.Open();
                sql1 = $"select CASE_NBR from CASE_HDR where CASE_NBR in (select CASE_NBR from CASE_DTL where ACTL_QTY >1 and CASE_SEQ_NBR = 2)";
                command = new OracleCommand(sql1, db);
                var dr = command.ExecuteReader();
                if (dr.Read())
                {
                    cn.CaseNumber = dr["CASE_NBR"].ToString();
                }

                sql1 = $"select case_nbr,locn_id,actl_wt,mfg_date,ship_by_date,xpire_date,po_nbr,stat_code,prev_locn_id,mod_date_time,user_id from case_hdr where case_nbr = '{cn.CaseNumber}'";
                command = new OracleCommand(sql1, db);
                var dr1 = command.ExecuteReader();
                if (dr1.Read())
                {
                    caseHeader.CaseNumber = dr1["CASE_NBR"].ToString();
                    //caseHeader.ActlWt = Convert.ToInt16(dr1["ACTL_WT"].ToString());
                    caseHeader.LocationId = dr1["LOCN_ID"].ToString();
                    caseHeader.StatCode = Convert.ToInt16(dr1["STAT_CODE"].ToString());
                }

                sql1 = $"select case_nbr,sku_id,actl_qty,batch_nbr,total_alloc_qty,orig_qty from case_dtl where case_nbr = '{cn.CaseNumber}'";
                command = new OracleCommand(sql1, db);
                var dr2 = command.ExecuteReader();
                while (dr2.Read())
                {
                    var cd = new CaseDetailDto();
                    cd.CaseNumber = dr2["CASE_NBR"].ToString();
                    cd.SkuId = dr2["SKU_ID"].ToString();
                    cd.ActlQty = Convert.ToInt32(dr2["ACTL_QTY"].ToString());
                    cd.TotalAllocQty = Convert.ToInt32(dr2["TOTAL_ALLOC_QTY"].ToString());
                    CaseDetailList.Add(cd);
                }

            }
        }

        public void GetDataAndValidateForComtAndIvmt() 
        {
            OracleCommand command;
            OracleConnection db;

            using (db = new OracleConnection
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["SfcRbacContextModel"].ConnectionString
            })
            {
                db.Open();
                sql1 = $"select source_msg_key,source_msg_status,source_msg_trans_code,source_msg_rsn_code,error_msg,error_code,msg_status,msg_json,container_id,container_type,locn_id,lot_id,po_nbr,sku_id,staging_locn,qty,created_date_time from SWM_TO_MHE where container_id = '{cn.CaseNumber}' and source_msg_trans_code = 'COMT'";
                command = new OracleCommand(sql1, db);
                var dr15 = command.ExecuteReader();
                if (dr15.Read())
                {
                    swmToMhe.SourceMessageKey = Convert.ToInt16(dr15["SOURCE_MSG_KEY"].ToString());
                    swmToMhe.SourceMessageResponseCode = Convert.ToInt16(dr15["SOURCE_MSG_RSN_CODE"].ToString());
                    swmToMhe.SourceMessageStatus = dr15["SOURCE_MSG_STATUS"].ToString();
                    swmToMhe.CreatedDateTime = Convert.ToDateTime(dr15["CREATED_DATE_TIME"].ToString());
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

                sql1 = $"select msgkey,sts,trx,msgtext,rsnrcode,addwho from wmstoems where msgkey = {swmToMhe.SourceMessageKey}";
                command = new OracleCommand(sql1, db);
                var d10 = command.ExecuteReader();
                if (d10.Read())
                {
                    wmsToEms.Status = d10["STS"].ToString();
                    wmsToEms.ResponseCode = Convert.ToInt16(d10["RSNRCODE"].ToString());
                    wmsToEms.AddWho = d10["ADDWHO"].ToString();
                    wmsToEms.MessageKey = Convert.ToUInt16(d10["MSGKEY"].ToString());
                    wmsToEms.Transaction = d10["TRX"].ToString();
                }

                for (var i = 0; i < CaseDetailList.Count;i++ )
                {
                    sql1 = $"select source_msg_key,source_msg_status,source_msg_trans_code,source_msg_rsn_code,error_msg,error_code,msg_status,msg_json,container_id,container_type,locn_id,lot_id,po_nbr,sku_id,staging_locn,qty from SWM_TO_MHE where container_id = '{cn.CaseNumber}' and source_msg_trans_code = 'IVMT' and sku_id='{CaseDetailList[i].SkuId}'";
                    command = new OracleCommand(sql1, db);
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
                        var caseDetail = CaseDetailList[i];
                        CheckMessageIsInsertedInSwmToMheForIvmtMessage(caseDetail, ivmt);
                    }
                   
                    sql1 = $"select msgkey,sts,trx,msgtext,rsnrcode,addwho from wmstoems where msgkey = {stm.SourceMessageKey}";
                    command = new OracleCommand(sql1, db);
                    var d14 = command.ExecuteReader();
                    if (d14.Read())
                    {
                        wte.Status = d14["STS"].ToString();
                        wte.ResponseCode = Convert.ToInt16(d14["RSNRCODE"].ToString());
                        wte.AddWho = d14["ADDWHO"].ToString();
                        wte.MessageKey = Convert.ToUInt16(d14["MSGKEY"].ToString());
                        wte.Transaction = d14["TRX"].ToString();

                    }

                    sql1 = $"select unit_wt from item_master where sku_id = '{CaseDetailList[i].SkuId}'";
                    command = new OracleCommand(sql1, db);
                    var dr12 = command.ExecuteReader();
                    if (dr12.Read())
                    {
                        unitWeight = Convert.ToDecimal(dr12["UNIT_WT"].ToString());
                    }

                    sql1 = $"select sku_id,locn_id,trans_invn_type,actl_invn_units,actl_wt,user_id,mod_date_time from trans_invn where sku_id='{CaseDetailList[i].SkuId}' order by mod_date_time desc";
                    command = new OracleCommand(sql1, db);
                    var dr13 = command.ExecuteReader();
                    if (dr13.Read())
                    {
                        trn.ActualInventoryUnits = Convert.ToDecimal(dr13["ACTL_INVN_UNITS"].ToString());
                        trn.ActualWeight = Convert.ToDecimal(dr13["ACTL_WT"].ToString());
                        Assert.AreEqual(Convert.ToDecimal(ivmt.Quantity), trn.ActualInventoryUnits);
                        Assert.AreEqual((unitWeight * Convert.ToDecimal(ivmt.Quantity)), Convert.ToDecimal(trn.ActualWeight));
                    }

                    sql1 = $"select stat_code from task_hdr where sku_id = '{CaseDetailList[i].SkuId}' order by mod_date_time desc";
                    command = new OracleCommand(sql1, db);
                    var dr10 = command.ExecuteReader();
                    if (dr10.Read())
                    {
                        task.StatusCode = Convert.ToByte(dr10["STAT_CODE"].ToString());
                        CheckInTaskHeaderForStatusCode(task);
                    }

                }

                sql1 = $"select case_nbr,locn_id,actl_wt,mfg_date,ship_by_date,xpire_date,po_nbr,stat_code,prev_locn_id,mod_date_time,user_id from case_hdr where case_nbr = '{cn.CaseNumber}'";
                command = new OracleCommand(sql1, db);
                var dr1 = command.ExecuteReader();
                if (dr1.Read())
                {
                    caseHeader.StatCode = Convert.ToInt16(dr1["STAT_CODE"].ToString());
                }

                sql1 = $"select case_nbr,sku_id,actl_qty,batch_nbr,total_alloc_qty,orig_qty from case_dtl where case_nbr = '{cn.CaseNumber}'";
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
        
        public void CheckMessageIsInsertedInSwmToMheForIvmtMessage(CaseDetailDto caseDetail, IvmtDto ivmt)
        {
            Assert.AreEqual(caseDetail.SkuId, ivmt.Sku);
            Assert.AreEqual(caseDetail.ActlQty, Convert.ToDouble(ivmt.Quantity));
            Assert.AreEqual("Ready", stm.SourceMessageStatus);
            Assert.AreEqual("Warehouse", ivmt.Owner);
            Assert.AreEqual("IVMT", ivmt.TransactionCode);
            Assert.AreEqual("00217", ivmt.MessageLength);
            Assert.AreEqual("Create", ivmt.ActionCode);
            Assert.AreEqual("Case", ivmt.UnitOfMeasure);
            Assert.AreEqual(caseHeader.ManufacturingDate, ivmt.ManufactureDate);
            Assert.AreEqual(caseHeader.ShipByDate, ivmt.FifoDate);
            Assert.AreEqual(caseHeader.XpireDate, ivmt.ExpirationDate);
            Assert.AreEqual("F", ivmt.DateControl);
        }

        /*  protected void CheckMessageIsInsertedInWmsToEmsForIvmtMessage(WmsToEmsDto wte)
          {
              Assert.AreEqual(stm.SourceMessageStatus, wte.Status);
              Assert.AreEqual("IVMT", wte.Transaction);
              Assert.AreEqual(stm.SourceMessageProcess, wte.AddWho);
              Assert.AreEqual(Convert.ToInt16(stm.SourceMessageResponseCode), wte.ResponseCode);
          }*/


        protected void CheckInTaskHeaderForStatusCode(TaskHeaderDto task)
        {
            Assert.AreEqual(90, task.StatusCode);
        }

        protected void CheckInCaseDetailForQuantity()
        {
            for (var j = 0; j < caseList.Count; j++)
            {
                Assert.AreEqual(0, caseList[j].ActlQty);
                Assert.AreEqual(0, caseList[j].TotalAllocQty);
            }
        }

        protected void CheckInCaseHdrForStatusCode()
        {
            Assert.AreEqual(96, caseHeader.StatCode);
        }
    }
}
