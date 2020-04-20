using System;
using System.Collections.Generic;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData;
using Sfc.Wms.Foundation.Carton.Contracts.Dtos;
using Sfc.Wms.Foundation.InboundLpn.Contracts.Dtos;
using Sfc.Wms.Foundation.Location.Contracts.Dtos;
using Sfc.Wms.Foundation.Message.Contracts.Dtos;
using Sfc.Wms.Foundation.TransitionalInventory.Contracts.Dtos;
using Sfc.Wms.Interfaces.Asrs.Dematic.Contracts.Dtos;
using Sfc.Wms.Interfaces.Asrs.Shamrock.Contracts.Dtos;
using Sfc.Wms.Interfaces.ParserAndTranslator.Contracts.Constants;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures
{
    public class CommonFunction
    {
        public OracleCommand Command;
        public List<TransitionalInventoryDto> TransInvnList = new List<TransitionalInventoryDto>();
        public OracleTransaction Transaction;
        public string Query = "";
        public OracleConnection GetOracleConnection()
        {
            return new OracleConnection(ConfigurationManager.ConnectionStrings["SfcRbacContextModel"].ConnectionString);
        }

        public WmsToEmsDto WmsToEmsData(OracleConnection db, int msgKey, string trx)
        {
            var wmsToEmsDto = new WmsToEmsDto();
            var query = CommonQueries.WmsToEms;
            Command = new OracleCommand(query, db);
            Command.Parameters.Add(new OracleParameter("transCode", trx));
            Command.Parameters.Add(new OracleParameter("msgKey", msgKey));
            var wmsToEmsReader = Command.ExecuteReader();
            if (wmsToEmsReader.Read())
            {
                wmsToEmsDto.Status = wmsToEmsReader[WmsToEms.Status].ToString();
                wmsToEmsDto.ResponseCode = Convert.ToInt16(wmsToEmsReader[WmsToEms.ReasonCode]);
                wmsToEmsDto.MessageKey = Convert.ToInt32(wmsToEmsReader[WmsToEms.MsgKey]);
                wmsToEmsDto.Transaction = wmsToEmsReader[WmsToEms.Trx].ToString();
                wmsToEmsDto.MessageText = wmsToEmsReader[WmsToEms.MsgTxt].ToString();
                wmsToEmsDto.Process = wmsToEmsReader["PRC"].ToString();
                wmsToEmsDto.ZplData = wmsToEmsReader["ZPLDATA"].ToString();
            }
            return wmsToEmsDto;
        }

        public SwmToMheDto SwmToMhe(OracleConnection db, string containerNbr, string trx, string skuId)
        {
            var swmtomhedata = new SwmToMheDto();
            var sku = $" and sku_id = '{skuId}' ";
            var ormt = $" and order_id = '{containerNbr}' ";
            var comt = $" and container_id = '{containerNbr}' ";
            var query = $"select * from SWM_TO_MHE where  source_msg_trans_code = '{trx}'";
            var orderBy = " order by source_msg_key desc ";
            switch (trx)
            {
                case TransactionCode.Ivmt:
                    query = query + comt + sku + orderBy;
                    break;
                case TransactionCode.Ormt:
                    query = query + ormt + sku + orderBy;
                    break;
                case TransactionCode.Comt:
                    query = query + comt + orderBy;
                    break;
                case TransactionCode.Skmt:
                    query = query + sku + orderBy;
                    break;
                case TransactionCode.Synr:
                    query = query + orderBy;
                    break;
                default:
                    query = query + orderBy;
                    break;
            }
            Command = new OracleCommand(query, db);
            var swmToMheReader = Command.ExecuteReader();
            if (swmToMheReader.Read())
            {
                swmtomhedata.SourceMessageKey = Convert.ToInt32(swmToMheReader[TestData.SwmToMhe.SourceMsgKey]);
                swmtomhedata.SourceMessageProcess = swmToMheReader[TestData.SwmToMhe.SourceMsgProcess].ToString();
                swmtomhedata.SourceMessageResponseCode = Convert.ToInt16(swmToMheReader[TestData.SwmToMhe.SourceMsgRsnCode]);
                swmtomhedata.SourceMessageStatus = swmToMheReader[TestData.SwmToMhe.SourceMsgStatus].ToString();
                swmtomhedata.ContainerId = swmToMheReader[TestData.SwmToMhe.ContainerId].ToString();
                swmtomhedata.ContainerType = swmToMheReader[TestData.SwmToMhe.ContainerType].ToString();
                swmtomhedata.MessageJson = swmToMheReader[TestData.SwmToMhe.MsgJson].ToString();
                swmtomhedata.LocationId = swmToMheReader[TestData.SwmToMhe.LocnId].ToString();
                swmtomhedata.SourceMessageText = swmToMheReader[TestData.SwmToMhe.SourceMsgText].ToString();
                swmtomhedata.SourceMessageTransactionCode = swmToMheReader["SOURCE_MSG_TRANS_CODE"].ToString();
                swmtomhedata.MessageStatus = Convert.ToInt32(swmToMheReader["MSG_STATUS"]);
                swmtomhedata.LocationId = swmToMheReader["LOCN_ID"].ToString();
                swmtomhedata.LotId = swmToMheReader["LOT_ID"].ToString();
                swmtomhedata.OrderId = swmToMheReader["ORDER_ID"].ToString();
                swmtomhedata.OrderLineId = ToNullableInt(swmToMheReader["ORDER_LINE_ID"]);
                swmtomhedata.PoNumber = swmToMheReader["PO_NBR"].ToString();
                swmtomhedata.Quantity = ToNullableInt(swmToMheReader["QTY"]);
                swmtomhedata.WaveNumber = swmToMheReader["WAVE_NBR"].ToString();
                swmtomhedata.ZplData = swmToMheReader["ZPL_DATA"].ToString();
            }
            return swmtomhedata;
        }

        public CaseViewDto FetchTransInvn(OracleConnection db, string skuId)
        {
            var singleSkulocal = new CaseViewDto();
            var query = CommonQueries.TransInventory;
            Command = new OracleCommand(query, db);
            Command.Parameters.Add(new OracleParameter("skuId", skuId));
            Command.Parameters.Add(new OracleParameter("transInventoryType", Constants.TransInvnType));
            var transInvnReader = Command.ExecuteReader();
            if (transInvnReader.Read())
            {
                singleSkulocal.ActualInventoryUnits = Convert.ToDecimal(transInvnReader[TransInventory.ActualInventoryUnits]);
                singleSkulocal.ActualWeight = Convert.ToDecimal(transInvnReader[TransInventory.ActlWt]);
            }
            return singleSkulocal;
        }

        public TransitionalInventoryDto FetchTransInvnentory(OracleConnection db, string skuId)
        {
            var singleSkulocal = new TransitionalInventoryDto();
            var query = CommonQueries.TransInventory;
            Command = new OracleCommand(query, db);
            Command.Parameters.Add(new OracleParameter("skuId", skuId));
            Command.Parameters.Add(new OracleParameter("transInventoryType", Constants.TransInvnType));
            var transInvnReader = Command.ExecuteReader();
            if (transInvnReader.Read())
            {
                singleSkulocal.ActualInventoryUnits = Convert.ToDecimal(transInvnReader[FieldName.ActualInventoryUnits]);
                singleSkulocal.ActualWeight = Convert.ToDecimal(transInvnReader[FieldName.ActlWt]);
            }
            return singleSkulocal;
        }

        public TransitionalInventoryDto FetchTransInvnentoryForNegative(OracleConnection db, string skuId)
        {
            var singleSkulocal = new TransitionalInventoryDto();
            var query = CommonQueries.TransInventory;
            Command = new OracleCommand(query, db);
            Command.Parameters.Add(new OracleParameter("skuId", skuId));
            Command.Parameters.Add(new OracleParameter("transInventoryType", Constants.TransInvnTypeForNegativePick));
            var transInvnReader = Command.ExecuteReader();
            if (transInvnReader.Read())
            {
                singleSkulocal.ActualInventoryUnits = Convert.ToDecimal(transInvnReader[FieldName.ActualInventoryUnits]);
                singleSkulocal.ActualWeight = Convert.ToDecimal(transInvnReader[FieldName.ActlWt]);
            }
            return singleSkulocal;
        }


        public decimal FetchUnitWeight(OracleConnection db, string skuId)
        {
            var query = CommonQueries.ItemMaster;
            Command = new OracleCommand(query, db);
            Command.Parameters.Add(new OracleParameter("skuId", skuId));
            var unitWeight = Convert.ToDecimal(Command.ExecuteScalar());
            return unitWeight;
        }
        public decimal FetchUnitVol(OracleConnection db, string skuId)
        {
            var query = $"select unit_vol from item_master where sku_id = '{skuId}'";
            Command = new OracleCommand(query, db);
            Command.Parameters.Add(new OracleParameter("skuId", skuId));
            var unitVol = Convert.ToDecimal(Command.ExecuteScalar());
            return unitVol;
        }

        public string GetTempZone(OracleConnection db, string skuId)
        {
            var query = CommonQueries.TempZone;
            Command = new OracleCommand(query, db);
            Command.Parameters.Add(new OracleParameter("skuId", skuId));
            var  itemMaster = Command.ExecuteScalar().ToString();
            return itemMaster;
        }

        public string GetLotId(OracleConnection db, string caseNbr)
        {
            var query = CommonQueries.FetchLotId;
            Command = new OracleCommand(query, db);
            Command.Parameters.Add(new OracleParameter("caseNbr", caseNbr));
            var LotId = Command.ExecuteReader();
            if (LotId.RowSize > 1)
                return "multi";
            return LotId["TRKG_DATA"].ToString();
        }

        public string GetUnitOfMeasureFromItemMaster(OracleConnection db, string skuId)
        {
            var query = $"select spl_instr_code_3 from item_master where sku_id = {skuId}";
            Command = new OracleCommand(query, db);
            var  unitOfMeasure = Command.ExecuteScalar().ToString();
            return unitOfMeasure;
        }     

        public string ItemMasterUnitOfMeasure(string unitOfMeasure)
        {
            switch (unitOfMeasure)
            {
                case "N":
                    return "Case";
                default:
                    return "Each";
            }
        }
        public CaseDetailDto FetchCaseDetailQty(OracleConnection db, string caseNbr)
        {
            var caseDtl = new CaseDetailDto();
            var query = CommonQueries.CaseDtl;
            Command = new OracleCommand(query, db);
            Command.Parameters.Add(new OracleParameter("caseNbr", caseNbr));
            var caseDtlReader = Command.ExecuteReader();
            if (caseDtlReader.Read())
            {
                caseDtl.ActualQuantity = Convert.ToDecimal(caseDtlReader["ACTL_QTY"]);
                caseDtl.TotalAllocatedQuantity = Convert.ToDecimal(caseDtlReader["TOTAL_ALLOC_QTY"]);
                caseDtl.SkuId = caseDtlReader["SKU_ID"].ToString();
                caseDtl.OriginalQuantity = Convert.ToDecimal(caseDtlReader["ORIG_QTY"]);
                caseDtl.ShippedAsnQuantity = Convert.ToDecimal(caseDtlReader["SHPD_ASN_QTY"]);
                caseDtl.CaseSequenceNumber = Convert.ToInt16(caseDtlReader["CASE_SEQ_NBR"]);
            }
            return caseDtl;
        }
        public SwmFromMheDto SwmFromMhe(OracleConnection db, long msgKey, string trx)
        {
            var swmFromMheData = new SwmFromMheDto();
            var swmFromMheView = CommonQueries.SwmFromMhe;
            Command = new OracleCommand(swmFromMheView, db);
            Command.Parameters.Add(new OracleParameter("transCode", trx));
            Command.Parameters.Add(new OracleParameter("messageKey", msgKey));
            var swmFromMheReader = Command.ExecuteReader();
            if (swmFromMheReader.Read())
            {
                swmFromMheData.SourceMessageKey = Convert.ToInt32(swmFromMheReader[TestData.SwmFromMhe.SourceMsgKey]);
                swmFromMheData.SourceMessageResponseCode = Convert.ToInt32(swmFromMheReader[TestData.SwmFromMhe.SourceMsgRsnCode]);
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
            var seqNumberGenerate = CommonQueries.SeqNbrEmsToWms;
            Command = new OracleCommand(seqNumberGenerate, db);
            var key = Convert.ToInt64(Command.ExecuteScalar().ToString());
            return key;
        }

        public long InsertEmsToWms(OracleConnection db, EmsToWmsDto emsToWmsDto)
        {
            Transaction = db.BeginTransaction();
            var msgKey = GetSeqNbrEmsToWms(db);
            var insertQuery = $"insert into emstowms values ('{emsToWmsDto.Process}','{msgKey}','{emsToWmsDto.Status}','{emsToWmsDto.Transaction}','{emsToWmsDto.MessageText}','{emsToWmsDto.ResponseCode}','TestUser',SYSDATE,SYSDATE)";
            Command = new OracleCommand(insertQuery, db);
            Command.ExecuteNonQuery();
            Transaction.Commit();
            return msgKey;
        }

        
        public PickLocationDetailsDto GetPickLocationDetails(OracleConnection db, string skuId, string locnId)
        {
            var pickLocnDtl = new PickLocationDetailsDto();
            var pickLocnView = CommonQueries.PickLocnDtl;
            Command = new OracleCommand(pickLocnView, db);
            Command.Parameters.Add(new OracleParameter("skuId", skuId));
            Command.Parameters.Add(new OracleParameter("sysCodeType", Constants.SysCodeType));
            Command.Parameters.Add(new OracleParameter("sysCodeId", Constants.SysCodeIdForActiveLocation));
            var pickLocnDtlReader = Command.ExecuteReader();
            if (pickLocnDtlReader.Read())
            {
                pickLocnDtl.ActualInventoryQuantity = Convert.ToDecimal(pickLocnDtlReader[PickLocationDetail.ActlInvnQty]);
                pickLocnDtl.ToBeFilledQty = Convert.ToDecimal(pickLocnDtlReader[PickLocationDetail.ToBeFilledQty]);
                pickLocnDtl.LocationId = pickLocnDtlReader[PickLocationDetail.LocnId].ToString();
                pickLocnDtl.ToBePickedQty = Convert.ToDecimal(pickLocnDtlReader[PickLocationDetail.ToBePickedQuantity]);
            }
            return pickLocnDtl;
        }

        public PickLocationDetailsExtenstionDto GetPickLocnDtlExt(OracleConnection db, string skuId, string locnId)
        {
            var pickLocnDtlExt = new PickLocationDetailsExtenstionDto();
            var query = CommonQueries.PickLocnDtlExt;
            Command = new OracleCommand(query, db);
            Command.Parameters.Add(new OracleParameter("skuId", skuId));
            Command.Parameters.Add(new OracleParameter("sysCodeType", Constants.SysCodeType));
            Command.Parameters.Add(new OracleParameter("sysCodeId", Constants.SysCodeIdForActiveLocation));
            var pickLocnDtlExtReader = Command.ExecuteReader();
            if (pickLocnDtlExtReader.Read())
            {
                pickLocnDtlExt.ActiveOrmtCount = Convert.ToInt32(pickLocnDtlExtReader[PickLocnDtlExt.ActiveOrmtCount]);
            }
            return pickLocnDtlExt;
        }

        public CartonHeaderDto GetStatusCodeFromCartonHdr(OracleConnection db, string cartonNbr)
        {
            var cartonHdr = new CartonHeaderDto();
            var query = CommonQueries.CartonHdr;
            Command = new OracleCommand(query, db);
            Command.Parameters.Add(new OracleParameter("cartonNumber", cartonNbr));
            var cartonHdrReader = Command.ExecuteReader();
            if (cartonHdrReader.Read())
            {
                cartonHdr.StatusCode = Convert.ToInt16(cartonHdrReader["STAT_CODE"]);
            }
            return cartonHdr;
        }
        public CaseHeaderDto CaseHeaderDetails(OracleConnection db, string caseNbr)
        {
            var caseHdr = new CaseHeaderDto();
            var query = CommonQueries.CaseHdr;
            Command = new OracleCommand(query, db);
            Command.Parameters.Add(new OracleParameter("caseNbr", caseNbr));
            var cartonHdrReader = Command.ExecuteReader();
            if (cartonHdrReader.Read())
            {
                caseHdr.StatusCode = Convert.ToInt16(cartonHdrReader["STAT_CODE"]);
                caseHdr.PreviousLocationId = cartonHdrReader["PREV_LOCN_ID"].ToString();
                caseHdr.LocationId = cartonHdrReader["LOCN_ID"].ToString();
                caseHdr.Volume = Convert.ToDecimal(cartonHdrReader["VOL"]);
                caseHdr.EstimatedWeight = Convert.ToDecimal(cartonHdrReader["EST_WT"]);
                caseHdr.ActualWeight = Convert.ToDecimal(cartonHdrReader["ACTL_WT"]);
                caseHdr.SingleSkuId = cartonHdrReader["SNGL_SKU_CASE"].ToString();
                caseHdr.SpecialInstructionCode1 = cartonHdrReader["SPL_INSTR_CODE_1"].ToString();
                caseHdr.CaseNumber = cartonHdrReader["CASE_NBR"].ToString();
            }
            return caseHdr;
        }

        public static int? ToNullableInt(object s)
        { if (s == System.DBNull.Value) return null;
            if (int.TryParse(s.ToString(), out var i))
                return i; return null;
        }

        public SwmFromMheDto SwmFromMheqtydefference(OracleConnection db, string sourceMessage)
        {
            var swmFromMheData = new SwmFromMheDto();
            var pickLocnView = $"select * from swm_from_mhe where SOURCE_MSG_TRANS_CODE='{sourceMessage}' order by CREATED_DATE_TIME desc";
            Command = new OracleCommand(pickLocnView, db);
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

        public Int64 FetchMaxIdFromPb2CorbaHdr(OracleConnection db)
        {
            var query = IvstQueries.MaxIdForPb2CorbaHdr;
            Command = new OracleCommand(query, db);
            long maxId = Convert.ToInt64(Command.ExecuteScalar());
            return maxId;
        }

        public List<Pb2CorbaHdrDtl> FetchPbHdrDetails(OracleConnection db, Int64 maxId)
        {
            var pbHdrDtlList = new List<Pb2CorbaHdrDtl>();
            var query = $"select * from pb2_corba_hdr ph inner join pb2_corba_dtl pd on ph.id=pd.id " +
                $"where func_name like '%printCaseLabelPB' and " +
                $"workstation_id = (select TRIM(SUBSTR(MISC_FLAGS, 13, 15)) " +
                $"from whse_sys_code where code_type = '309' and rec_type = 'C' and code_id = 'Dry') " +
                $"and ph.id = '{maxId}' order by parm_name asc";
            var command = new OracleCommand(query, db);
            var pbHdrDtlReader = command.ExecuteReader();
            while (pbHdrDtlReader.Read())
            {
                var set = new Pb2CorbaHdrDtl
                {
                    Status = pbHdrDtlReader["STATUS"].ToString(),
                    WorkStationId = pbHdrDtlReader["WORKSTATION_ID"].ToString(),
                    ParmName = pbHdrDtlReader["PARM_NAME"].ToString(),
                    ParmType = pbHdrDtlReader["PARM_TYPE"].ToString(),
                    ParmValue = pbHdrDtlReader["PARM_VALUE"].ToString(),
                    ChgUser = pbHdrDtlReader["CHG_USER"].ToString(),
                };
                pbHdrDtlList.Add(set);
            }
            return pbHdrDtlList;
        }

        public MessageToSortViewDto GetMsgTosvDetail(OracleConnection db, string cartonNo, string msgType)
        {
            var messageTosv = new MessageToSortViewDto();
            var query = OrstQueries.MsgToSortView;
            Command = new OracleCommand(query, db);
            Command.Parameters.Add(new OracleParameter("cartonNo", cartonNo));
            Command.Parameters.Add(new OracleParameter("msgType", msgType));
            var msgToSvReader = Command.ExecuteReader();
            if (msgToSvReader.Read())
            {
                messageTosv.Ptn = msgToSvReader[MessageToSv.Ptn].ToString();
            }
            return messageTosv;
        }    

        public ReserveLocationHeaderDto GetResrvLocnDetails(OracleConnection db, string locnId )
        {
            var rsv = new ReserveLocationHeaderDto();
            var query = $"select * from resv_locn_hdr where locn_id  = '{locnId}' order by create_date_time desc";
            Command = new OracleCommand(query,db);
            var reader = Command.ExecuteReader();
            if(reader.Read())
            {
                rsv.CurrentWeight = Convert.ToDecimal(reader["CURR_WT"]);
                rsv.CurrentVolume = Convert.ToDecimal(reader["CURR_VOL"]);
                rsv.CurrentUnitOfMeasureQuantity = Convert.ToDecimal(reader["CURR_UOM_QTY"]);
            }
            return rsv;
        }

        public int CountOfMasterPackId(OracleConnection db, string masterPackId)
        {
            var query = $"select Count(*) from msg_to_sv where message_type= 'ADD'  and PTN = '{masterPackId}' order by message_id desc";
            Command = new OracleCommand(query,db);
            var Count = Convert.ToInt16(Command.ExecuteScalar());
            return Count;
        }

        public int CalculateTheForecastCountFromCartonInfoTable(OracleConnection db)
        {
            var query = $"Select Count(*) from swm_multishuttle_carton_info sm inner join locn_grp lg on sm.cwc_zone_nbr =lg.grp_attr where sm.ptn_wave_nbr = '20200210176' and lg.whse = '008' " +
                $"and grp_type = 20 and lg.locn_id in (select lh.locn_id from locn_hdr lh inner join locn_grp lg on lg.locn_id = lh.locn_id inner join sys_code sc on sc.code_id = lg.grp_type and " +
                $"sc.code_type = '740' and sc.code_id = '18') group by cwc_zone_nbr";
            Command = new OracleCommand(query,db);
            var foreCastCount = Convert.ToInt32(Command.ExecuteScalar());
            return foreCastCount;
        }

        public int CalculateTheForeCaseCountFromMsgToCWCTable(OracleConnection db, string waveNbr)
        {
            var query = $"select count(*) from MSG_TO_CWC  inner join locn_grp lg on MSG_TO_CWC.cwc_zone_nbr =lg.grp_attr where MSG_TO_CWC.pkms_wave_nbr = '{waveNbr}'  and lg.whse = '008' and grp_type = 20 and lg.locn_id in (select lh.locn_id from locn_hdr lh inner join locn_grp lg on lg.locn_id=lh.locn_id inner join sys_code sc on sc.code_id=lg.grp_type and sc.code_type='740' and sc.code_id='18')";
            Command = new OracleCommand(query,db);
            var foreCaseCount = Convert.ToInt32(Command.ExecuteScalar());
            return foreCaseCount;
        }
    
        public SwmMultiShuttleCartonInfoDto GetCartonInfoDetails(OracleConnection db,string skuId)
        {
            var sm = new SwmMultiShuttleCartonInfoDto();
            var query = $"select Ptn_Est_Wt,IM_MHE_WT_TOL_AMNT from swm_multishuttle_carton_info  where ptn_sku_id = '{skuId}' order by create_date_time desc";
            Command = new OracleCommand(query, db);
            var smReader = Command.ExecuteReader();
            if(smReader.Read())
            {
                sm.PtnEstWt = Convert.ToDecimal(smReader["Ptn_Est_Wt"]);
                sm.ImMheWtTotalAmount = Convert.ToInt16(smReader["IM_MHE_WT_TOL_AMNT"]);
            }
            return sm;
        }

        public Data.Entities.AltSku GetAltSkuInfo(OracleConnection db, string sku)
        {
            var ats = new Data.Entities.AltSku();
            var query = $"select * from alt_sku where parent_sku_id ='{sku}' or child_sku_id = '{sku}'";
            Command = new OracleCommand(query,db);
            var atsReader = Command.ExecuteReader();
            if(atsReader.Read())
            {
                ats.ChildSkuId = atsReader["CHILD_SKU_ID"].ToString();
                ats.ParentSkuId = atsReader["PARENT_SKU_ID"].ToString();
                ats.QuantityChildPerParent = Convert.ToInt32(atsReader["QTY_CHILD_PER_PARENT"]);
            }
            return ats;
        }

        private void CalculateWeightLimit(OracleConnection db, out decimal? lowerWtLmt, out decimal? upperWtLmt)
        {
            var swmDematicCartonInfoDto = GetCartonInfoDetails(db,"sku");
            if (swmDematicCartonInfoDto.ImMheWtTotalType == DefaultPossibleValue.SplInstrCodeForParent)
            {
                lowerWtLmt = swmDematicCartonInfoDto.PtnEstWt * (1 - (decimal)swmDematicCartonInfoDto.ImMheWtTotalAmount / 100);
                upperWtLmt = swmDematicCartonInfoDto.PtnEstWt * (1 - (decimal)swmDematicCartonInfoDto.ImMheWtTotalAmount / 100);
            }
            else
            {
                lowerWtLmt = swmDematicCartonInfoDto.PtnEstWt - swmDematicCartonInfoDto.ImMheWtTotalAmount;
                upperWtLmt = swmDematicCartonInfoDto.PtnEstWt + swmDematicCartonInfoDto.ImMheWtTotalAmount;
            }

            if (upperWtLmt < 1000) return;
            lowerWtLmt = 0;
            upperWtLmt = (decimal)999.99;
        }

    }
}
