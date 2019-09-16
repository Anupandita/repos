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
using Sfc.Wms.Data.Entities;

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
                return "";
            }

            return tempZone;
        }



        public Data.Entities.LocationGroup GetLocnId(OracleConnection db, string skuId, string tempZone)
        {
            var tempzone = GetTempZone(db, "");
            var temp = TempZoneRelate(tempzone.TempZone);
            var locnGrp = new LocationGroup();
            var query = $"select lh.locn_id,lg.grp_attr from locn_hdr lh inner join locn_grp lg on lg.locn_id = lh.locn_id and lg.grp_attr = '{tempZone}' inner join sys_code sc on sc.code_id = lg.grp_type and sc.code_type = '740' and code_id = '18' select TEMP_ZONE  from item_master where sku_id = '{skuId}'";
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

        public PickLocationDtlDto PickLocnData(OracleConnection db, string skuId)
        {
            var pickLocn = new PickLocationDtlDto();
            sqlStatements = $"select * from pick_locn_dtl where sku_id = '{skuId}' and locn_id = '{costData.LocnId}' order by mod_date_time desc";
            command = new OracleCommand(sqlStatements, db);
            var pickLocnReader = command.ExecuteReader();
            if (pickLocnReader.Read())
            {
                pickLocn.ActualInventoryQuantity = Convert.ToDecimal(pickLocnReader[TestData.PickLocationDetail.ActlInvnQty].ToString());
                pickLocn.ToBeFilledQty = Convert.ToDecimal(pickLocnReader[TestData.PickLocationDetail.ToBeFilledQty].ToString());
                pickLocn.LocationId = pickLocnReader[TestData.PickLocationDetail.LocnId].ToString();
            }
            return pickLocn;
        }

    }
}
