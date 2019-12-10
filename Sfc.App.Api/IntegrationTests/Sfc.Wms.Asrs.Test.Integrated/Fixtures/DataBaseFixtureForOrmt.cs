using Oracle.ManagedDataAccess.Client;
using Sfc.Wms.Interfaces.ParserAndTranslator.Contracts.Constants;
using Sfc.Wms.Interfaces.ParserAndTranslator.Contracts.Dto;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData;
using Newtonsoft.Json;
using Sfc.Wms.Foundation.Location.Contracts.Dtos;
using Sfc.Wms.Foundation.Carton.Contracts.Dtos;
using System.Collections.Generic;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.Interfaces.Asrs.Shamrock.Contracts.Dtos;
using Sfc.Wms.Interfaces.Asrs.Dematic.Contracts.Dtos;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures
{
    public class DataBaseFixtureForOrmt : CommonFunction
    {
        protected WmsToEmsDto WmsToEmsAddRelease = new WmsToEmsDto();
        protected SwmToMheDto SwmToMheAddRelease = new SwmToMheDto();
        SwmEligibleOrmtCartonDto SwmOrmtCarton = new SwmEligibleOrmtCartonDto();
        protected WmsToEmsDto WmsToEmsCancelation = new WmsToEmsDto();
        protected SwmToMheDto SwmToMheCancelation = new SwmToMheDto();
        protected WmsToEmsDto WmsToEmsEPick = new WmsToEmsDto();
        protected WmsToEmsDto WmsToEmsOnPrc =new WmsToEmsDto();
        protected SwmToMheDto SwmToMheEPick = new SwmToMheDto();
        protected SwmToMheDto SwmToMheOnProcess=new SwmToMheDto();
        protected OrmtDto Ormt = new OrmtDto();
        protected OrmtDto OrmtCancel = new OrmtDto();
        protected OrmtDto OrmtEPick = new OrmtDto();
        protected OrmtDto OrmtOnprocess =new OrmtDto();
        protected CartonView PrintCarton = new CartonView();
        protected CartonView CancelOrder = new CartonView();
        protected CartonView EPick = new CartonView();
        protected CartonView OnProCost =new CartonView();
        protected PickLocationDetailsExtenstionDto PickLcnDtlExtBeforeApi = new PickLocationDetailsExtenstionDto();
        protected PickLocationDetailsExtenstionDto PickLcnDtlExtAfterApi = new PickLocationDetailsExtenstionDto();
        protected CartonHeaderDto CartonHdr = new CartonHeaderDto();
        protected SwmEligibleOrmtCartonDto Status = new SwmEligibleOrmtCartonDto();
        protected string AsrsLocnId;
        protected CartonView ActiveOrmtCountNotFound = new CartonView();
        protected CartonView PickLocnNotFound = new CartonView();
        protected CartonView ActiveLocnNotFound = new CartonView();
        protected List<CartonView> OrderList = new List<CartonView>();
        protected List<PickLocationDetailsExtenstionDto> ActiveOrmtCountList = new List<PickLocationDetailsExtenstionDto>();
       
        public void GetDataBeforeTriggerOrmtForPrintingOfCartons()
        {
            OracleConnection db;
            using (db = GetOracleConnection())
            {
                db.Open();     
                PrintCarton = GetValidOrderDetails(db,10,0);
                UpdatePickTicketStatusCodeTo12(db, 12, PrintCarton.PickTktCtrlNbr);
                PickLcnDtlExtBeforeApi = GetPickLocnDtlExt(db, PrintCarton.SkuId, PrintCarton.LocnId);
            }
        }    

       public void GetValidDataBeforeTriggerOrmtForPrintingOfCartonsThroughWaveNumber()
       {
            OracleConnection db;
            using (db = GetOracleConnection())
            {
                db.Open();
                var waveNumber = GetWaveNumber(db);
                OrderList = GetValidOrderDetailsForWaveRelease(db, waveNumber);
                ActiveOrmtCountList = FetchActiveOrmtCount(db);
            }
       }
      
        public List<PickLocationDetailsExtenstionDto> FetchActiveOrmtCount(OracleConnection db)
        {
            var pickLocnDtlExt = new List<PickLocationDetailsExtenstionDto>();
            for (var j = 0; j < OrderList.Count; j++)
            {            
                var query = $"select * from pick_locn_dtl_ext WHERE SKU_ID='{OrderList[j].SkuId}' order by created_date_time, updated_date_time  desc ";
                Command = new OracleCommand(query, db);
                var pickLocnDtlExtReader = Command.ExecuteReader();
                if (pickLocnDtlExtReader.Read())
                {
                    var activeOrmt = new PickLocationDetailsExtenstionDto
                    {
                        ActiveOrmtCount = Convert.ToInt16(pickLocnDtlExtReader[PickLocnDtlExt.ActiveOrmtCount].ToString())
                    };
                    pickLocnDtlExt.Add(activeOrmt);
                }
            }
            return pickLocnDtlExt;
        }

        public void GetDataBeforeTriggerOrmtForCancellationOfOrders()
        {
            OracleConnection db;
            using (db = GetOracleConnection())
            {
                db.Open();
                CancelOrder = GetValidOrderDetails(db, 99, 0);
                PickLcnDtlExtBeforeApi = GetPickLocnDtlExt(db, CancelOrder.SkuId,CancelOrder.LocnId);
            }
        }
        
     
        public void GetDataBeforeCallingApiForEpickOfOrders()
        {
            OracleConnection db;
            using (db = GetOracleConnection())
            {
                db.Open();
                EPick = GetValidOrderDetails(db, 5, 1);
                PickLcnDtlExtBeforeApi = GetPickLocnDtlExt(db, EPick.SkuId, EPick.LocnId);
            }
        }

        public void GetDataBeforeCallingApiForOnProcessCostMessage()
        {
            OracleConnection db;
            using (db = GetOracleConnection())
            {
                db.Open();
                OnProCost = GetValidOnProcessCostMessage(db);
                PickLcnDtlExtBeforeApi = GetPickLocnDtlExt(db, OnProCost.SkuId, OnProCost.LocnId);
            }
        }

        public void GetDataAfterCallingOrmtApiForAddRelease()
        {
            OracleConnection db;
            using (db = GetOracleConnection())
            {
                db.Open();
                SwmToMheAddRelease = SwmToMhe(db,PrintCarton.CartonNbr, TransactionCode.Ormt, PrintCarton.SkuId);
                Ormt = JsonConvert.DeserializeObject<OrmtDto>(SwmToMheAddRelease.MessageJson);
                WmsToEmsAddRelease = WmsToEmsData(db, SwmToMheAddRelease.SourceMessageKey, TransactionCode.Ormt);
                CartonHdr = GetStatusCodeFromCartonHdr(db,PrintCarton.CartonNbr);
                PickLcnDtlExtAfterApi = GetPickLocnDtlExt(db,PrintCarton.SkuId,PrintCarton.LocnId);
                Status = GetStatusCodeFromEligibleOrmtCount(db, PrintCarton.CartonNbr);             
            }
        }
    
        public void GetDataAfterCallingOrmtApiAfterWaveRelease()
        {
            OracleConnection db;
            using (db = GetOracleConnection())
            {
                db.Open();
                for (var i = 0; i < OrderList.Count; i++)
                {
                    SwmToMheAddRelease = SwmToMhe(db, OrderList[i].CartonNbr, TransactionCode.Ormt, OrderList[i].SkuId);
                    Ormt = JsonConvert.DeserializeObject<OrmtDto>(SwmToMheAddRelease.MessageJson);
                    var printCarton = OrderList[i];
                    WmsToEmsAddRelease = WmsToEmsData(db, SwmToMheAddRelease.SourceMessageKey, TransactionCode.Ormt);
                    VerifyOrmtMessageWasInsertedInToSwmToMhe(Ormt,SwmToMheAddRelease,printCarton);
                    VerifyOrmtMessageWasInsertedInToWmsToEmsForPrintingOfOrder(SwmToMheAddRelease,WmsToEmsAddRelease);
                    CartonHdr = GetStatusCodeFromCartonHdrForWaveRelease(db, OrderList[i].CartonNbr);
                    Assert.AreEqual(12, CartonHdr.StatusCode);
                    PickLcnDtlExtAfterApi = GetPickLocnDtlExt(db, OrderList[i].SkuId,OrderList[i].LocnId);
                    Assert.AreEqual(ActiveOrmtCountList[i].ActiveOrmtCount + 1, PickLcnDtlExtAfterApi.ActiveOrmtCount);
                    Status = GetStatusCodeFromEligibleOrmtCount(db, OrderList[i].CartonNbr);
                    Assert.AreEqual(90, Convert.ToInt32(Status.Status));
                }
            }
        }
   
        public void GetDataAfterCallingApiForCancellationOfOrders()
        {
            OracleConnection db;
            using (db = GetOracleConnection())
            {
                db.Open();
                SwmToMheCancelation = SwmToMhe(db, CancelOrder.CartonNbr, TransactionCode.Ormt, CancelOrder.SkuId);
                OrmtCancel = JsonConvert.DeserializeObject<OrmtDto>(SwmToMheCancelation.MessageJson);
                WmsToEmsCancelation = WmsToEmsData(db, SwmToMheCancelation.SourceMessageKey, TransactionCode.Ormt);
                CartonHdr = GetStatusCodeFromCartonHdr(db, CancelOrder.CartonNbr);
                PickLcnDtlExtAfterApi = GetPickLocnDtlExt(db, CancelOrder.SkuId,CancelOrder.LocnId);
                Status = GetStatusCodeFromEligibleOrmtCount(db, CancelOrder.CartonNbr);
            }
        }

        public void GetDataAfterCallingApiForEPickOrders()
        {
            OracleConnection db;
            using (db = GetOracleConnection())
            {
                db.Open();
                SwmToMheEPick = SwmToMhe(db, EPick.CartonNbr, TransactionCode.Ormt, EPick.SkuId);
                OrmtEPick = JsonConvert.DeserializeObject<OrmtDto>(SwmToMheEPick.MessageJson);
                WmsToEmsEPick = WmsToEmsData(db, SwmToMheEPick.SourceMessageKey, TransactionCode.Ormt);
                CartonHdr = GetStatusCodeFromCartonHdr(db, EPick.CartonNbr);
                PickLcnDtlExtAfterApi = GetPickLocnDtlExt(db, EPick.SkuId, EPick.LocnId);
                Status = GetStatusCodeFromEligibleOrmtCount(db, EPick.CartonNbr);
            }
        }

        public void GetDataAfterCallingApiForOnProcessCost()
        {
            OracleConnection db;
            using (db = GetOracleConnection())
            {
                db.Open();
                SwmToMheOnProcess = SwmToMhe(db, OnProCost.CartonNbr, TransactionCode.Ormt, OnProCost.SkuId);
                OrmtOnprocess = JsonConvert.DeserializeObject<OrmtDto>(SwmToMheOnProcess.MessageJson);
                WmsToEmsOnPrc = WmsToEmsData(db, SwmToMheOnProcess.SourceMessageKey, TransactionCode.Ormt);
                CartonHdr = GetStatusCodeFromCartonHdr(db, OnProCost.CartonNbr);
                PickLcnDtlExtAfterApi = GetPickLocnDtlExt(db, OrmtOnprocess.Sku, null);
            }
        }

        public void GetDataForNegativeCases()
        {
            OracleConnection db;
            using (db = GetOracleConnection())
            {
                db.Open();
                ActiveOrmtCountNotFound = GetCartonNbrWhereActiveOrmtNotFound(db);
                PickLocnNotFound = GetCartonNbrWherePickLocnNotFound(db);
                ActiveLocnNotFound = GetCartonNbrWhereActiveLocationNotFound(db);
            }
        }

        public CartonView GetValidOrderDetails(OracleConnection db, int statCode, int miscNum1)
        {
            var cartonView = new CartonView();
            var query = $"select distinct ch.carton_nbr,ch.wave_nbr,ch.sku_id,pl.locn_id,ch.MISC_NUM_1, ch.total_qty, " +
                    $"ch.stat_code,pl.actl_invn_qty,ch.DEST_LOCN_ID,ph.PKT_CTRL_NBR,ph.SHIP_W_CTRL_NBR,ph.Whse," +
                    $"ph.CO,ph.DIV, im.spl_instr_code_1, im.spl_instr_code_5 from SWM_ELGBL_ORMT_CARTONS eg " +
                    $"inner join carton_hdr ch on ch.Carton_Nbr = eg.Carton_Nbr inner join PKT_HDR ph ON " +
                    $"ph.pkt_ctrl_nbr = ch.pkt_ctrl_nbr inner join Pick_Locn_dtl pl ON pl.sku_id = ch.sku_id   " +
                    $"inner join ITEM_MASTER im ON ch.sku_id = im.sku_id  inner join locn_hdr lh ON " +
                    $"pl.locn_id = lh.locn_id inner join locn_grp lg ON lg.locn_id = lh.locn_id   " +
                    $"inner join sys_code sc ON sc.code_id = lg.grp_type " +
                    $"inner join pkt_hdr ph ON ph.pkt_ctrl_nbr = ch.pkt_ctrl_nbr " +
                    $"inner join pkt_dtl pd ON pd.pkt_ctrl_nbr = ch.pkt_ctrl_nbr " +
                    $"inner join alloc_invn_dtl ai ON ai.sku_id = ch.sku_id  " +
                    $"left join pick_locn_dtl_ext ple ON pl.sku_id = ple.sku_id " +
                    $"where ch.misc_instr_code_5 is null and ch.misc_num_1 = {Constants.NumZero} and sc.code_type = '{Constants.SysCodeType}' and code_id = '{Constants.SysCodeIdForActiveLocation}' and lg.grp_attr = DECODE(im.temp_zone, 'D', 'Dry', 'Freezer') " +
                    $"and ch.misc_instr_code_5 is null and ch.stat_code = {Constants.PrintCartonStatus} and eg.status = {Constants.EgblOrmtStatus}";
            var command = new OracleCommand(query, db);
            var reader = command.ExecuteReader();
            if (reader.Read())
            {
                cartonView.PickTktCtrlNbr = reader[PickTicketHeader.PickTktCtrlNbr].ToString();
                cartonView.SkuId = reader["SKU_ID"].ToString();
                cartonView.WaveNbr = reader["WAVE_NBR"].ToString();
                cartonView.Whse = reader[PickTicketHeader.Whse].ToString();
                cartonView.Co = reader[PickTicketHeader.Co].ToString();
                cartonView.Div = reader[PickTicketHeader.Div].ToString();
                cartonView.SplInstrCode1 = reader[ItemMaster.SplInstrCode1].ToString();
                cartonView.SplInstrCode5 = reader[ItemMaster.SplInstrCode5].ToString();
                cartonView.TotalQty = reader["TOTAL_QTY"].ToString();
                cartonView.CartonNbr = reader["CARTON_NBR"].ToString();
                cartonView.DestLocnId = reader["DEST_LOCN_ID"].ToString();
                cartonView.ShipWCtrlNbr = reader["SHIP_W_CTRL_NBR"].ToString();
                cartonView.LocnId = reader["LOCN_ID"].ToString();           
            }        
           
            return cartonView;
        }

        public CartonView GetValidOnProcessCostMessage(OracleConnection db)
        {
            var onproCostView= new CartonView();
            var onQuery = $"select distinct soc.CARTON_NBR,soc.CREATED_DATE_TIME,ch.wave_nbr,ch.sku_id,pl.locn_id,ch.MISC_NUM_1," +
                $" ch.total_qty ,ch.stat_code,pl.actl_invn_qty,ch.DEST_LOCN_ID,ph.PKT_CTRL_NBR,ph.SHIP_W_CTRL_NBR,ph.Whse," +
                $"ph.CO,ph.DIV, im.spl_instr_code_1, im.spl_instr_code_5 from swm_from_mhe sfm " +
                $"inner join carton_hdr ch on sfm.SKU_ID = ch.SKU_ID " +
                $"inner join SWM_ELGBL_ORMT_CARTONS soc on soc.carton_nbr = ch.carton_nbr " +
                $"inner join PKT_HDR ph ON ph.pkt_ctrl_nbr = ch.pkt_ctrl_nbr " +
                $"inner join Pick_Locn_dtl pl ON pl.sku_id = ch.sku_id " +
                $"inner join ITEM_MASTER im ON ch.sku_id = im.sku_id " +
                $"inner join pkt_dtl pd ON pd.pkt_ctrl_nbr = ch.pkt_ctrl_nbr " +
                $"inner join pkt_hdr ph ON ph.pkt_ctrl_nbr = ch.pkt_ctrl_nbr " +
                $"inner join alloc_invn_dtl ai ON ai.sku_id = ch.sku_id " +
                $"left join pick_locn_dtl_ext ple ON pl.sku_id = ple.sku_id " +
                $"inner join locn_hdr lh ON pl.locn_id = lh.locn_id " +
                $"inner join locn_grp lg ON lg.locn_id = lh.locn_id " +
                $"inner join sys_code sc ON sc.code_id = lg.grp_type " +
                $"where ch.misc_instr_code_5 is null and misc_num_1 = {Constants.NumZero} and sc.code_type = '{Constants.SysCodeType}' and code_id = '{Constants.SysCodeIdForActiveLocation}' and " +
                $"lg.grp_attr = DECODE(im.temp_zone, 'D', 'Dry', 'Freezer') and ch.misc_instr_code_5 is null and misc_num_1 = {Constants.NumZero} " +
                $"and soc.STATUS = {Constants.OrmtNoInvnStatus} and sfm.source_msg_trans_code = '{TransactionCode.Cost}' ORDER BY soc.CARTON_NBR,soc.CREATED_DATE_TIME";
            var command = new OracleCommand(onQuery, db);
            var reader = command.ExecuteReader();
            if (reader.Read())
            {
                onproCostView.CartonNbr = reader["CARTON_NBR"].ToString();
                onproCostView.PickTktCtrlNbr = reader[PickTicketHeader.PickTktCtrlNbr].ToString();
                onproCostView.SkuId = reader["SKU_ID"].ToString();
                onproCostView.WaveNbr = reader["WAVE_NBR"].ToString();
                onproCostView.Whse = reader[PickTicketHeader.Whse].ToString();
                onproCostView.Co = reader[PickTicketHeader.Co].ToString();
                onproCostView.Div = reader[PickTicketHeader.Div].ToString();
                onproCostView.SplInstrCode1 = reader[ItemMaster.SplInstrCode1].ToString();
                onproCostView.SplInstrCode5 = reader[ItemMaster.SplInstrCode5].ToString();
                onproCostView.TotalQty = reader["TOTAL_QTY"].ToString();
                onproCostView.CartonNbr = reader["CARTON_NBR"].ToString();
                onproCostView.DestLocnId = reader["DEST_LOCN_ID"].ToString();
                onproCostView.ShipWCtrlNbr = reader["SHIP_W_CTRL_NBR"].ToString();
                onproCostView.LocnId = reader["LOCN_ID"].ToString();
            }
            return onproCostView;
        }

       
        public void UpdatePickTicketStatusCodeTo12(OracleConnection db, int pktStatCode, string pktCtrlNbr)
        {
            Transaction = db.BeginTransaction();
            var updateQuery = $"update pkt_hdr set pkt_stat_code = '{pktStatCode}' where pkt_ctrl_nbr = '{pktCtrlNbr}'";
            Command = new OracleCommand(updateQuery, db);
            Command.ExecuteNonQuery();
            Transaction.Commit();
        }

        protected string GetWaveNumber(OracleConnection db)
        {
            var query = $"select  distinct eg.wave_nbr from SWM_ELGBL_ORMT_CARTONS eg inner join carton_hdr ch on ch.Carton_Nbr = eg.Carton_Nbr inner join PKT_HDR ph ON ph.pkt_ctrl_nbr = ch.pkt_ctrl_nbr  " +
                $"inner join Pick_Locn_dtl pl ON  pl.sku_id = ch.sku_id   inner join ITEM_MASTER im ON ch.sku_id = im.sku_id  inner join locn_hdr lh ON pl.locn_id = lh.locn_id inner join locn_grp lg ON lg.locn_id = lh.locn_id   " +
                $"inner join sys_code sc ON sc.code_id = lg.grp_type inner join pkt_hdr ph ON ph.pkt_ctrl_nbr = ch.pkt_ctrl_nbr inner join pkt_dtl pd ON pd.pkt_ctrl_nbr = ch.pkt_ctrl_nbr inner join alloc_invn_dtl ai ON " +
                $"ai.sku_id = ch.sku_id  left join pick_locn_dtl_ext ple ON pl.sku_id = ple.sku_id where  ch.misc_instr_code_5 is null and misc_num_1 = {Constants.NumZero} and sc.code_type = '{Constants.SysCodeType}' and code_id = '{Constants.SysCodeIdForActiveLocation}' and " +
                $"lg.grp_attr = DECODE(im.temp_zone, 'D', 'Dry', 'Freezer') and ch.misc_instr_code_5 is null and misc_num_1 = {Constants.NumZero} and ch.stat_code = {Constants.PrintCartonStatus} and eg.status = {Constants.EgblOrmtStatus}";
            Command = new OracleCommand(query,db);
            var waveNbr = Command.ExecuteScalar().ToString();
            return waveNbr;
        }
   
        public List<CartonView> GetValidOrderDetailsForWaveRelease(OracleConnection db, string waveNbr)
        {
            var orderdtls = new List<CartonView>();
            var query = $"select  distinct eg.wave_nbr,ch.carton_nbr,ch.sku_id,ch.MISC_NUM_1,ch.total_qty, " +
                $"ch.stat_code,pl.actl_invn_qty,ch.DEST_LOCN_ID,ph.PKT_CTRL_NBR,ph.SHIP_W_CTRL_NBR,ph.Whse,ph.CO,ph.DIV, " +
                $"im.spl_instr_code_1, im.spl_instr_code_5 from SWM_ELGBL_ORMT_CARTONS eg inner join carton_hdr ch on " +
                $"ch.Carton_Nbr = eg.Carton_Nbr  inner join PKT_HDR ph ON ph.pkt_ctrl_nbr = ch.pkt_ctrl_nbr  " +
                $"inner join Pick_Locn_dtl pl ON  pl.sku_id = ch.sku_id inner join ITEM_MASTER im ON ch.sku_id = im.sku_id  " +
                $"inner join locn_hdr lh ON pl.locn_id = lh.locn_id inner join locn_grp lg ON lg.locn_id = lh.locn_id " +
                $"inner join sys_code sc ON sc.code_id = lg.grp_type inner join pkt_hdr ph ON ph.pkt_ctrl_nbr = ch.pkt_ctrl_nbr " +
                $"inner join pkt_dtl pd ON pd.pkt_ctrl_nbr = ch.pkt_ctrl_nbr " +
                $"inner join alloc_invn_dtl ai ON ai.sku_id = ch.sku_id left join pick_locn_dtl_ext ple ON pl.sku_id = ple.sku_id " +
                $"where eg.STATUS = {Constants.EgblOrmtStatus} and ch.misc_instr_code_5 is null and misc_num_1 = {Constants.NumZero} and " +
                $"sc.code_type = '{Constants.SysCodeType}' and code_id = '{Constants.SysCodeIdForActiveLocation}' " +
                $"and lg.grp_attr = DECODE(im.temp_zone, 'D', 'Dry', 'Freezer') and eg.wave_nbr = '{waveNbr}'";
            var command = new OracleCommand(query,db);
            var Reader = command.ExecuteReader();
            while (Reader.Read())
            {
                var cartonView = new CartonView();
                cartonView.PickTktCtrlNbr = Reader[TestData.PickTicketHeader.PickTktCtrlNbr].ToString();
                cartonView.SkuId = Reader["SKU_ID"].ToString();
                cartonView.WaveNbr = Reader["WAVE_NBR"].ToString();
                cartonView.Whse = Reader[TestData.PickTicketHeader.Whse].ToString();
                cartonView.Co = Reader[TestData.PickTicketHeader.Co].ToString();
                cartonView.Div = Reader[TestData.PickTicketHeader.Div].ToString();
                cartonView.SplInstrCode1 = Reader[TestData.ItemMaster.SplInstrCode1].ToString();
                cartonView.SplInstrCode5 = Reader[TestData.ItemMaster.SplInstrCode5].ToString();
                cartonView.TotalQty = Reader["TOTAL_QTY"].ToString();
                cartonView.CartonNbr = Reader["CARTON_NBR"].ToString();
                cartonView.DestLocnId = Reader["DEST_LOCN_ID"].ToString();
                cartonView.ShipWCtrlNbr = Reader["SHIP_W_CTRL_NBR"].ToString();
                orderdtls.Add(cartonView);
            }
            return orderdtls;
        }

        public CartonView GetCartonNbrWhereActiveOrmtNotFound(OracleConnection db)
        {
            var cartonView = new CartonView();
            var query = $"select ch.carton_nbr,ch.sku_id,ch.wave_nbr,ch.MISC_NUM_1, ch.total_qty, ch.stat_code,pl.actl_invn_qty,ch.DEST_LOCN_ID,ph.PKT_CTRL_NBR," +
                $"ph.SHIP_W_CTRL_NBR,ph.Whse,ph.CO,ph.DIV, im.spl_instr_code_1, im.spl_instr_code_5 from CARTON_HDR ch " +
                $"inner join PKT_HDR ph ON ph.pkt_ctrl_nbr = ch.pkt_ctrl_nbr inner join Pick_Locn_dtl pl ON  pl.locn_id = ch.pick_locn_id " +
                $"inner join ITEM_MASTER im ON pl.sku_id = im.sku_id inner join locn_hdr lh ON pl.locn_id = lh.locn_id " +
                $"inner join locn_grp lg ON lg.locn_id = lh.locn_id inner join pick_locn_dtl_ext ple ON ple.sku_id = ch.sku_id " +
                $"inner join sys_code sc ON sc.code_id = lg.grp_type where sc.code_type = '{Constants.SysCodeType}' and code_id = '{Constants.SysCodeIdForActiveLocation}' and " +
                $"lg.grp_attr = DECODE(im.temp_zone, 'D', 'Dry', 'Freezer') and pl.actl_invn_qty <= {Constants.NumZero}";
            var command = new OracleCommand(query, db);
            var reader = command.ExecuteReader();
            if (reader.Read())
            {
                cartonView.CartonNbr = reader["CARTON_NBR"].ToString();
                cartonView.SkuId = reader["SKU_ID"].ToString();
                cartonView.WaveNbr = reader["WAVE_NBR"].ToString();
            }
            return cartonView;
        }

        public CartonView GetCartonNbrWherePickLocnNotFound(OracleConnection db)
        {
            var cartonView = new CartonView();
            var query = $"select * from carton_hdr ch join pick_locn_dtl pl  ON pl.sku_id = ch.sku_id  where ch.sku_id not in " +
                $"(select sku_id from pick_locn_dtl where locn_id in (select lh.locn_id from locn_hdr lh inner join locn_grp lg on " +
                $"lg.locn_id=lh.locn_id inner join sys_code sc on sc.code_id=lg.grp_type and sc.code_type='{Constants.SysCodeType}' and sc.code_id='{Constants.SysCodeIdForActiveLocation}')) " +
                $"and stat_code = 5 and ch.misc_instr_code_5 is null and misc_num_1 = {Constants.NumZero} and pl.actl_invn_qty > {Constants.NumZero}";
            var command = new OracleCommand(query, db);
            var reader = command.ExecuteReader();
            if (reader.Read())
            {
                cartonView.CartonNbr = reader["CARTON_NBR"].ToString();
                cartonView.SkuId = reader["SKU_ID"].ToString();
                cartonView.WaveNbr = reader["WAVE_NBR"].ToString();
            }
            return cartonView;
        }

       public CartonHeaderDto GetStatusCodeFromCartonHdrForWaveRelease(OracleConnection db,string cartonNbr)
       {
           var cartonHdrView = new CartonHeaderDto();
           var query = $"Select * from carton_hdr where carton_nbr = '{cartonNbr}'";
           var command = new OracleCommand(query, db);
           var cartonHdrReader = command.ExecuteReader();
           if (cartonHdrReader.Read())
           {              
              cartonHdrView.StatusCode = Convert.ToInt16(cartonHdrReader["STAT_CODE"].ToString());
           }
            return cartonHdrView;
        }

        public SwmEligibleOrmtCartonDto GetStatusCodeFromEligibleOrmtCount(OracleConnection db, string cartonNbr)
        {
            var swmEligibleOrmtCount = new SwmEligibleOrmtCartonDto();
            var query = $"Select * from SWM_ELGBL_ORMT_CARTONS where carton_nbr = '{cartonNbr}'order by updated_date_time desc";
            var command = new OracleCommand(query,db);
            var reader = command.ExecuteReader();
            if(reader.Read())
            {
                swmEligibleOrmtCount.Status = Convert.ToInt16(reader["STATUS"].ToString());
            }
            return swmEligibleOrmtCount;
        }

        public CartonView GetCartonNbrWhereActiveLocationNotFound(OracleConnection db)
        {
            var cartonView = new CartonView();
            var query = $"select ch.carton_nbr,ch.sku_id,pl.locn_id,ch.wave_nbr from carton_hdr ch inner join " +
                $"Pick_Locn_dtl pl ON pl.sku_id = ch.sku_id where locn_id not in (select lh.locn_id from locn_hdr lh " +
                $"inner join locn_grp lg on lg.locn_id = lh.locn_id and lg.grp_attr in ('Freezer', 'Dry') " +
                $"inner join sys_code sc on sc.code_id = lg.grp_type and sc.code_type = '{Constants.SysCodeType}' and code_id = '{Constants.SysCodeIdForActiveLocation}')";
            var command = new OracleCommand(query, db);
            var reader = command.ExecuteReader();
            if (reader.Read())
            {
                cartonView.CartonNbr = reader["CARTON_NBR"].ToString();
                cartonView.SkuId = reader["SKU_ID"].ToString();
                cartonView.WaveNbr = reader["WAVE_NBR"].ToString();
            }
            return cartonView;
        }

        protected void VerifyOrmtMessageWasInsertedInToSwmToMhe(OrmtDto ormt, SwmToMheDto swmToMheAddRelease, CartonView printCarton)
        {
            Assert.AreEqual(DefaultValues.Status, swmToMheAddRelease.SourceMessageStatus);
            Assert.AreEqual(TransactionCode.Ormt, ormt.TransactionCode);
            Assert.AreEqual(MessageLength.Ormt, ormt.MessageLength);
            Assert.AreEqual(OrmtActionCode.AddRelease, ormt.ActionCode);
            Assert.AreEqual(printCarton.SkuId, ormt.Sku);
            Assert.AreEqual(printCarton.TotalQty, ormt.Quantity);
            Assert.AreEqual(UnitOfMeasures.Case, ormt.UnitOfMeasure);
            Assert.AreEqual(printCarton.CartonNbr, ormt.OrderId);
            Assert.AreEqual(DefaultPossibleValue.OrderLineId, ormt.OrderLineId);
            Assert.AreEqual(DefaultPossibleValue.OrderType, ormt.OrderType);
            Assert.AreEqual(Constants.EndOfWaveFlag, ormt.EndOfWaveFlag);
            Assert.AreEqual(printCarton.DestLocnId + "-" + printCarton.ShipWCtrlNbr, ormt.DestinationLocationId);
            Assert.AreEqual(printCarton.Whse , ormt.Owner);
            Assert.AreEqual(DefaultPossibleValue.OpRule, ormt.OpRule);
        }

        protected void VerifyOrmtMessageWasInsertedInToWmsToEmsForPrintingOfOrder(SwmToMheDto swmToMheAddRelease, WmsToEmsDto wmsToEmsAddRelease)
        {
            Assert.AreEqual(swmToMheAddRelease.SourceMessageStatus, wmsToEmsAddRelease.Status);
            Assert.AreEqual(TransactionCode.Ormt, wmsToEmsAddRelease.Transaction);
            Assert.AreEqual("MessageBuilder", wmsToEmsAddRelease.Process);
            Assert.AreEqual(swmToMheAddRelease.SourceMessageKey, wmsToEmsAddRelease.MessageKey);
            Assert.AreEqual(swmToMheAddRelease.SourceMessageTransactionCode, wmsToEmsAddRelease.Transaction);
            Assert.AreEqual(swmToMheAddRelease.SourceMessageText, wmsToEmsAddRelease.MessageText);
            Assert.AreEqual(swmToMheAddRelease.SourceMessageResponseCode, wmsToEmsAddRelease.ResponseCode);
            Assert.AreEqual(swmToMheAddRelease.ZplData, wmsToEmsAddRelease.ZplData);
        }
    }
}
