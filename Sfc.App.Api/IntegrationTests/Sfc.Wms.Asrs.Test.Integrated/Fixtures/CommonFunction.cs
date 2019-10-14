using Oracle.ManagedDataAccess.Client;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData;
using Sfc.Wms.Asrs.Dematic.Contracts.Dtos;
using Sfc.Wms.Asrs.Shamrock.Contracts.Dtos;
using Sfc.Wms.Data.Entities;
using Sfc.Wms.TransitionalInventory.Contracts.Dtos;
using Sfc.Wms.Interfaces.ParserAndTranslator.Contracts.Constants;
using Sfc.Wms.Interfaces.ParserAndTranslator.Contracts.Dto;
using System;
using System.Collections.Generic;
using System.Configuration;
using Sfc.Wms.Foundation.Location.Contracts.Dtos;
using Sfc.Wms.Foundation.Carton.Contracts.Dtos;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures
{

    public class CommonFunction
    {
        protected OracleCommand command;
        protected List<TransitionalInventoryDto> transInvnList = new List<TransitionalInventoryDto>();
        protected OracleTransaction transaction;
        public OracleConnection GetOracleConnection()
        {
            return new OracleConnection(ConfigurationManager.ConnectionStrings["SfcRbacContextModel"].ConnectionString);
        }

        protected WmsToEmsDto WmsToEmsData(OracleConnection db, int msgKey, string trx)
        {
            var WmsToEms = new WmsToEmsDto();
            var query = $"select * from WMSTOEMS where TRX = '{trx}' and MSGKEY = '{msgKey}'";
            command = new OracleCommand(query, db);
            var wmsToEmsReader = command.ExecuteReader();
            if (wmsToEmsReader.Read())
            {
                WmsToEms.Status = wmsToEmsReader[TestData.WmsToEms.Status].ToString();
                WmsToEms.ResponseCode = Convert.ToInt16(wmsToEmsReader[TestData.WmsToEms.ReasonCode].ToString());
                WmsToEms.MessageKey = Convert.ToUInt16(wmsToEmsReader[TestData.WmsToEms.MsgKey].ToString());
                WmsToEms.Transaction = wmsToEmsReader[TestData.WmsToEms.Trx].ToString();
                WmsToEms.MessageText = wmsToEmsReader[TestData.WmsToEms.MsgTxt].ToString();
            }
            return WmsToEms;
        }

        protected SwmToMheDto SwmToMhe(OracleConnection db, string caseNbr, string trx, string skuId)
        {
            var swmtomhedata = new SwmToMheDto();
            var t = $"and sku_id = '{skuId}' ";
            var query = $"select * from SWM_TO_MHE where container_id = '{caseNbr}' and source_msg_trans_code = '{trx}' ";
            var orderBy = "order by created_date_time desc";
            if (trx == TransactionCode.Ivmt)
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
                swmtomhedata.SourceMessageKey = Convert.ToInt16(swmToMheReader[TestData.SwmToMhe.SourceMsgKey].ToString());
                swmtomhedata.SourceMessageResponseCode = Convert.ToInt16(swmToMheReader[TestData.SwmToMhe.SourceMsgRsnCode].ToString());
                swmtomhedata.SourceMessageStatus = swmToMheReader[TestData.SwmToMhe.SourceMsgStatus].ToString();
                swmtomhedata.ContainerId = swmToMheReader[TestData.SwmToMhe.ContainerId].ToString();
                swmtomhedata.ContainerType = swmToMheReader[TestData.SwmToMhe.ContainerType].ToString();
                swmtomhedata.MessageJson = swmToMheReader[TestData.SwmToMhe.MsgJson].ToString();
                swmtomhedata.LocationId = swmToMheReader[TestData.SwmToMhe.LocnId].ToString();
                swmtomhedata.SourceMessageText = swmToMheReader[TestData.SwmToMhe.SourceMsgText].ToString();
                swmtomhedata.SourceMessageTransactionCode = swmToMheReader["SOURCE_MSG_TRANS_CODE"].ToString();
                swmtomhedata.MessageStatus = Convert.ToInt32(swmToMheReader["MSG_STATUS"].ToString());
                swmtomhedata.LocationId = swmToMheReader["LOCN_ID"].ToString();
                swmtomhedata.LotId = swmToMheReader["LOT_ID"].ToString();
                swmtomhedata.OrderId = swmToMheReader["ORDER_ID"].ToString();
                swmtomhedata.OrderLineId = Convert.ToInt32(swmToMheReader["ORDER_LINE_ID"].ToString());
                swmtomhedata.PoNumber = swmToMheReader["PO_NBR"].ToString();
                swmtomhedata.Quantity = Convert.ToInt32(swmToMheReader["QTY"].ToString());
                swmtomhedata.WaveNumber = swmToMheReader["WAVE_NBR"].ToString();
            }
            return swmtomhedata;
        }
        public CaseViewDto FetchTransInvn(OracleConnection db, string skuId)
        {
            var singleSkulocal = new CaseViewDto();
            var query = $"Select * from trans_invn where sku_id = '{skuId}' and  trans_invn_type = '18'";
            command = new OracleCommand(query, db);
            var transInvnReader = command.ExecuteReader();
            if (transInvnReader.Read())
            {
                singleSkulocal.ActualInventoryUnits = Convert.ToInt16(transInvnReader[TransInventory.ActualInventoryUnits].ToString());
                singleSkulocal.ActualWeight = Convert.ToDecimal(transInvnReader[TransInventory.ActlWt].ToString());
            }
            return singleSkulocal;
        }

        public TransitionalInventoryDto FetchTransInvnentory(OracleConnection db, string skuId)
        {
            var singleSkulocal = new TransitionalInventoryDto();
            var query = $"Select * from trans_invn where sku_id = '{skuId}' and  trans_invn_type = '18'";
            command = new OracleCommand(query, db);
            var transInvnReader = command.ExecuteReader();
            if (transInvnReader.Read())
            {
                singleSkulocal.ActualInventoryUnits = Convert.ToInt16(transInvnReader[TestData.FieldName.ActualInventoryUnits].ToString());
                singleSkulocal.ActualWeight = Convert.ToDecimal(transInvnReader[TestData.FieldName.ActlWt].ToString());
            }
            return singleSkulocal;
        }
        protected TransitionalInventoryDto FetchTransInvnData(OracleConnection db, string skuId)
        {
            var transInvn = new TransitionalInventoryDto();
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
        protected decimal FetchUnitWeight(OracleConnection db, string skuId)
        {
            var query = $"select unit_wt from item_master where sku_id = '{skuId}'";
            command = new OracleCommand(query, db);
            var unitWeight = Convert.ToDecimal(command.ExecuteScalar().ToString());
            return unitWeight;
        }

        public Data.Entities.ItemMaster GetTempZone(OracleConnection db, string skuId)
        {
            var itemMaster = new Data.Entities.ItemMaster();
            var query = $"select TEMP_ZONE  from item_master where sku_id= '{skuId}'";
            var command = new OracleCommand(query, db);
            var itemMasterReader = command.ExecuteReader();
            if (itemMasterReader.Read())
            {
                itemMaster.TempZone = itemMasterReader["TEMP_ZONE"].ToString();
            }
            return itemMaster;
        }

        public string TempZoneRelate(string tempZone)
        {
            if (tempZone == "D")
            {
                return "Dry";
            }
            else if (tempZone == "C")
            {
                return "";
            }

            else if (tempZone == "F")
            {
                return "Freezer";
            }

            return tempZone;
        }

        public Data.Entities.LocationGroup GetLocnId(OracleConnection db,string skuId)
        {
            var tempzone = GetTempZone(db, skuId);
            var temp = TempZoneRelate(tempzone.TempZone);
            var locnGrp = new LocationGroup();
            var query = $"select lh.locn_id,lg.grp_attr from locn_hdr lh inner join locn_grp lg on lg.locn_id = lh.locn_id and lg.grp_attr = '{temp}' inner join sys_code sc on sc.code_id = lg.grp_type and sc.code_type = '740' and code_id = '18'";
            var command = new OracleCommand(query, db);
            var locnGrpReader = command.ExecuteReader();
            if (locnGrpReader.Read())
            {
                locnGrp.LocationId = locnGrpReader["LOCN_ID"].ToString();
            }
            return locnGrp;
        }

        public SwmFromMheDto SwmFromMhe(OracleConnection db, long msgKey, string trx)
        {
            var swmFromMheData = new SwmFromMheDto();
            var swmFromMheView = $"select * from swm_from_mhe where Source_MSg_Key = {msgKey} and source_msg_trans_code = '{trx}'  order by created_date_time desc";
            command = new OracleCommand(swmFromMheView, db);
            var swmFromMheReader = command.ExecuteReader();
            if (swmFromMheReader.Read())
            {
                swmFromMheData.SourceMessageKey = Convert.ToInt16(swmFromMheReader[TestData.SwmFromMhe.SourceMsgKey].ToString());
                swmFromMheData.SourceMessageResponseCode = Convert.ToInt16(swmFromMheReader[TestData.SwmFromMhe.SourceMsgRsnCode].ToString());
                swmFromMheData.SourceMessageStatus = swmFromMheReader[TestData.SwmFromMhe.SourceMsgStatus].ToString();
                swmFromMheData.SourceMessageProcess = swmFromMheReader[TestData.SwmFromMhe.SourceMsgProcess].ToString();
                swmFromMheData.SourceMessageTransactionCode = swmFromMheReader[TestData.SwmFromMhe.SourceMsgTransCode].ToString();
                swmFromMheData.ContainerId = swmFromMheReader[TestData.SwmFromMhe.ContainerId].ToString();
                swmFromMheData.ContainerType = swmFromMheReader[TestData.SwmFromMhe.ContainerType].ToString();
                swmFromMheData.MessageJson = swmFromMheReader[TestData.SwmFromMhe.MsgJson].ToString();
                swmFromMheData.SourceMessageText = swmFromMheReader[TestData.SwmFromMhe.SourceMsgText].ToString();
                swmFromMheData.LocationId = swmFromMheReader[TestData.SwmFromMhe.LocnId].ToString();
            }
            return swmFromMheData;
        }

        public Int64 GetSeqNbrEmsToWms(OracleConnection db)
        {
            var seqNumberGenerate = $"select EMSTOWMS_MSGKEY_SEQ.nextval from dual";
            command = new OracleCommand(seqNumberGenerate, db);
            var key = Convert.ToInt64(command.ExecuteScalar().ToString());
            return key;
        }

        public long InsertEmsToWMS(OracleConnection db, EmsToWmsDto emsToWmsDto)
        {
            transaction = db.BeginTransaction();
            var MsgKey = GetSeqNbrEmsToWms(db);
            var insertQuery = $"insert into emstowms values ('{emsToWmsDto.Process}','{MsgKey}','{emsToWmsDto.Status}','{emsToWmsDto.Transaction}','{emsToWmsDto.MessageText}','{emsToWmsDto.ResponseCode}','TestUser','{DateTime.Now.ToString("dd-MMM-yy")}','{DateTime.Now.ToString("dd-MMM-yy")}')";
            command = new OracleCommand(insertQuery, db);
            command.ExecuteNonQuery();
            transaction.Commit();
            return MsgKey;
        }

        public PickLocationDetailsDto GetPickLocationDetails(OracleConnection db, string skuId,string LocnId)
        {
            var pickLocnDtl = new PickLocationDetailsDto();
            var pickLocnView = $"select * from pick_locn_dtl where sku_id = '{skuId}' and locn_id = '{LocnId}' order by mod_date_time desc";
            command = new OracleCommand(pickLocnView, db);
            var pickLocnDtlReader = command.ExecuteReader();
            if (pickLocnDtlReader.Read())
            {
                pickLocnDtl.ActualInventoryQuantity = Convert.ToDecimal(pickLocnDtlReader[TestData.PickLocationDetail.ActlInvnQty].ToString());
                pickLocnDtl.ToBeFilledQty = Convert.ToDecimal(pickLocnDtlReader[TestData.PickLocationDetail.ToBeFilledQty].ToString());
                pickLocnDtl.LocationId = pickLocnDtlReader[TestData.PickLocationDetail.LocnId].ToString();
                pickLocnDtl.ToBePickedQty = Convert.ToDecimal(pickLocnDtlReader[TestData.PickLocationDetail.ToBePickedQuantity].ToString());
            }
            return pickLocnDtl;
        }

        public PickLocationDetailsExtenstionDto GetPickLocnDtlExt(OracleConnection db, string skuId)
        {
            var pickLocnDtlExt = new PickLocationDetailsExtenstionDto();
            var query = $"select * from pick_locn_dtl_ext WHERE  SKU_ID='{skuId}'";
            command = new OracleCommand(query, db);
            var pickLocnDtlExtReader = command.ExecuteReader();
            if (pickLocnDtlExtReader.Read())
            {
                pickLocnDtlExt.ActiveOrmtCount = Convert.ToInt16(pickLocnDtlExtReader[TestData.PickLocnDtlExt.ActiveOrmtCount].ToString());
            }
            return pickLocnDtlExt;
        }

        public CartonHeaderDto GetStatusCodeFromCartonHdr(OracleConnection db,string cartonNbr)
        {
            var cartonHdr = new CartonHeaderDto();
            var query = $"Select * from carton_hdr where carton_nbr = '{cartonNbr}'";
            command = new OracleCommand(query, db);
            var cartonHdrReader = command.ExecuteReader();
            if(cartonHdrReader.Read())
            {
                cartonHdr.StatusCode = Convert.ToInt16(cartonHdrReader["STAT_CODE"].ToString());
            }
            return cartonHdr;
        }

    }
}
