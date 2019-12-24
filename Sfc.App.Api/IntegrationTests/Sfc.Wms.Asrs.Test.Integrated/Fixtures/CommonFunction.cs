using Oracle.ManagedDataAccess.Client;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData;
using Sfc.Wms.Data.Entities;
using Sfc.Wms.Foundation.Carton.Contracts.Dtos;
using Sfc.Wms.Foundation.Location.Contracts.Dtos;
using Sfc.Wms.Foundation.TransitionalInventory.Contracts.Dtos;
using Sfc.Wms.Interfaces.Asrs.Dematic.Contracts.Dtos;
using Sfc.Wms.Interfaces.Asrs.Shamrock.Contracts.Dtos;
using Sfc.Wms.Interfaces.ParserAndTranslator.Contracts.Constants;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures
{
    public class CommonFunction
    {
        protected OracleCommand Command;
        protected List<TransitionalInventoryDto> TransInvnList = new List<TransitionalInventoryDto>();
        protected OracleTransaction Transaction;
        public OracleConnection GetOracleConnection()
        {
            return new OracleConnection(ConfigurationManager.ConnectionStrings["SfcRbacContextModel"].ConnectionString);
        }

        protected WmsToEmsDto WmsToEmsData(OracleConnection db, int msgKey, string trx)
        {
            var wmsToEmsDto = new WmsToEmsDto();
            var query = $"{CommonQueries.WmsToEms}";
            Command = new OracleCommand(query, db);            
            Command.Parameters.Add(new OracleParameter("transCode", trx));
            Command.Parameters.Add(new OracleParameter("msgKey", msgKey));
            var wmsToEmsReader = Command.ExecuteReader();
            if (wmsToEmsReader.Read())
            {
                wmsToEmsDto.Status = wmsToEmsReader[TestData.WmsToEms.Status].ToString();
                wmsToEmsDto.ResponseCode = Convert.ToInt16(wmsToEmsReader[TestData.WmsToEms.ReasonCode].ToString());
                wmsToEmsDto.MessageKey = Convert.ToUInt16(wmsToEmsReader[TestData.WmsToEms.MsgKey].ToString());
                wmsToEmsDto.Transaction = wmsToEmsReader[TestData.WmsToEms.Trx].ToString();
                wmsToEmsDto.MessageText = wmsToEmsReader[TestData.WmsToEms.MsgTxt].ToString();
                wmsToEmsDto.Process = wmsToEmsReader["PRC"].ToString();
                wmsToEmsDto.ZplData = wmsToEmsReader["ZPLDATA"].ToString();
            }
            return wmsToEmsDto;
        }

        protected SwmToMheDto SwmToMhe(OracleConnection db, string containerNbr, string trx, string skuId)
        {
            var swmtomhedata = new SwmToMheDto();
            var sku = $" and sku_id = '{skuId}' ";
            var ormt = $" and order_id = '{containerNbr}' ";
            var comt = $" and container_id = '{containerNbr}' ";
            var query = $"select * from SWM_TO_MHE where  source_msg_trans_code = '{trx}'";
            var orderBy = " order by source_msg_key desc ";
            if (trx == TransactionCode.Ivmt)
            {
                query = query + comt + sku + orderBy;
            }
            else if (trx == TransactionCode.Ormt)
            {
                query = query + ormt + sku + orderBy;
            }
            else if(trx == TransactionCode.Comt)
            {
                query = query + comt + orderBy;
            }
            else
            {
                query = query + orderBy;
            }
            Command = new OracleCommand(query, db);
            var swmToMheReader = Command.ExecuteReader();
            if (swmToMheReader.Read())
            {
                swmtomhedata.SourceMessageKey = Convert.ToInt16(swmToMheReader[TestData.SwmToMhe.SourceMsgKey].ToString());
                swmtomhedata.SourceMessageProcess = swmToMheReader[TestData.SwmToMhe.SourceMsgProcess].ToString();
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
                swmtomhedata.ZplData = swmToMheReader["ZPL_DATA"].ToString();
            }
            return swmtomhedata;
        }

        public CaseViewDto FetchTransInvn(OracleConnection db, string skuId)
        {
            var singleSkulocal = new CaseViewDto();
            var query = $"{CommonQueries.TransInventory}";
            Command = new OracleCommand(query, db);            
            Command.Parameters.Add(new OracleParameter("skuId", skuId));
            Command.Parameters.Add(new OracleParameter("transInventoryType", Constants.TransInvnType));
            var transInvnReader = Command.ExecuteReader();
            if (transInvnReader.Read())
            {
                singleSkulocal.ActualInventoryUnits = Convert.ToDecimal(transInvnReader[TransInventory.ActualInventoryUnits].ToString());
                singleSkulocal.ActualWeight = Convert.ToDecimal(transInvnReader[TransInventory.ActlWt].ToString());
            }
            return singleSkulocal;
        }

        public TransitionalInventoryDto FetchTransInvnentory(OracleConnection db, string skuId)
        {
            var singleSkulocal = new TransitionalInventoryDto();
            var query = $"{CommonQueries.TransInventory}";
            Command = new OracleCommand(query, db);            
            Command.Parameters.Add(new OracleParameter("skuId", skuId));
            Command.Parameters.Add(new OracleParameter("transInventoryType", Constants.TransInvnType));
            var transInvnReader = Command.ExecuteReader();
            if (transInvnReader.Read())
            {
                singleSkulocal.ActualInventoryUnits = Convert.ToDecimal(transInvnReader[FieldName.ActualInventoryUnits].ToString());
                singleSkulocal.ActualWeight = Convert.ToDecimal(transInvnReader[FieldName.ActlWt].ToString());
            }
            return singleSkulocal;
        }
        
        protected decimal FetchUnitWeight(OracleConnection db, string skuId)
        {
            var query = $"{CommonQueries.ItemMaster}";
            Command = new OracleCommand(query, db);           
            Command.Parameters.Add(new OracleParameter("skuId", skuId));
            var unitWeight = Convert.ToDecimal(Command.ExecuteScalar().ToString());
            return unitWeight;
        }

        public string GetTempZone(OracleConnection db, string skuId)
        {          
            var query = $"{CommonQueries.TempZone}";
            Command = new OracleCommand(query, db);           
            Command.Parameters.Add(new OracleParameter("skuId", skuId));
            string itemMaster = Command.ExecuteReader().ToString();            
            return itemMaster;
        }

        public SwmFromMheDto SwmFromMhe(OracleConnection db, long msgKey, string trx)
        {
            var swmFromMheData = new SwmFromMheDto();
            var swmFromMheView = $"{CommonQueries.SwmFromMhe}";
            Command = new OracleCommand(swmFromMheView, db);
            Command.Parameters.Add(new OracleParameter("transCode", trx));
            Command.Parameters.Add(new OracleParameter("messageKey", msgKey));

            var swmFromMheReader = Command.ExecuteReader();
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
            var seqNumberGenerate = $"{CommonQueries.SeqNbrEmsToWms}";
            Command = new OracleCommand(seqNumberGenerate, db);
            var key = Convert.ToInt64(Command.ExecuteScalar().ToString());
            return key;
        }

        public long InsertEmsToWms(OracleConnection db, EmsToWmsDto emsToWmsDto)
        {
            Transaction = db.BeginTransaction();
            var msgKey = GetSeqNbrEmsToWms(db);
            var insertQuery = $"insert into emstowms values ('{emsToWmsDto.Process}','{msgKey}','{emsToWmsDto.Status}','{emsToWmsDto.Transaction}','{emsToWmsDto.MessageText}','{emsToWmsDto.ResponseCode}','TestUser','{DateTime.Now.ToString("dd-MMM-yy")}','{DateTime.Now.ToString("dd-MMM-yy")}')";
            Command = new OracleCommand(insertQuery, db);
            Command.ExecuteNonQuery();
            Transaction.Commit();
            return msgKey;
        }

        public PickLocationDetailsDto GetPickLocationDetails(OracleConnection db, string skuId, string locnId)
        {
            var pickLocnDtl = new PickLocationDetailsDto();
            var pickLocnView = $"{CommonQueries.PickLocnDtl}";
            Command = new OracleCommand(pickLocnView, db);           
            Command.Parameters.Add(new OracleParameter("skuId", skuId));
            Command.Parameters.Add(new OracleParameter("sysCodeType", Constants.SysCodeType));
            Command.Parameters.Add(new OracleParameter("sysCodeId", Constants.SysCodeIdForActiveLocation));
            var pickLocnDtlReader = Command.ExecuteReader();
            if (pickLocnDtlReader.Read())
            {
                pickLocnDtl.ActualInventoryQuantity = Convert.ToDecimal(pickLocnDtlReader[TestData.PickLocationDetail.ActlInvnQty].ToString());
                pickLocnDtl.ToBeFilledQty = Convert.ToDecimal(pickLocnDtlReader[TestData.PickLocationDetail.ToBeFilledQty].ToString());
                pickLocnDtl.LocationId = pickLocnDtlReader[TestData.PickLocationDetail.LocnId].ToString();
                pickLocnDtl.ToBePickedQty = Convert.ToDecimal(pickLocnDtlReader[TestData.PickLocationDetail.ToBePickedQuantity].ToString());
            }
            return pickLocnDtl;
        }

        public PickLocationDetailsExtenstionDto GetPickLocnDtlExt(OracleConnection db, string skuId, string locnId)
        {
            var pickLocnDtlExt = new PickLocationDetailsExtenstionDto();
            var query = $"{CommonQueries.PickLocnDtlExt}";
            Command = new OracleCommand(query, db);           
            Command.Parameters.Add(new OracleParameter("skuId", skuId));
            Command.Parameters.Add(new OracleParameter("sysCodeType", Constants.SysCodeType));
            Command.Parameters.Add(new OracleParameter("sysCodeId", Constants.SysCodeIdForActiveLocation));
            var pickLocnDtlExtReader = Command.ExecuteReader();
            if (pickLocnDtlExtReader.Read())
            {
                pickLocnDtlExt.ActiveOrmtCount = Convert.ToInt16(pickLocnDtlExtReader[PickLocnDtlExt.ActiveOrmtCount].ToString());
            }
            return pickLocnDtlExt;
        }

        public CartonHeaderDto GetStatusCodeFromCartonHdr(OracleConnection db, string cartonNbr)
        {
            var cartonHdr = new CartonHeaderDto();
            var query = $"{CommonQueries.CartonHdr}";
            Command = new OracleCommand(query, db);            
            Command.Parameters.Add(new OracleParameter("cartonNumber", cartonNbr));
            var cartonHdrReader = Command.ExecuteReader();
            if (cartonHdrReader.Read())
            {
                cartonHdr.StatusCode = Convert.ToInt16(cartonHdrReader["STAT_CODE"].ToString());
            }
            return cartonHdr;
        }
    }
}
