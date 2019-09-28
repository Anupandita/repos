using Sfc.Wms.Asrs.Dematic.Contracts.Dtos;
using Sfc.Wms.Asrs.Shamrock.Contracts.Dtos;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;
using Sfc.Wms.Interface.ParserAndTranslator.Contracts.Constants;
using Sfc.Wms.Interface.ParserAndTranslator.Contracts.Dto;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData;
using Sfc.Wms.Data.Entities;
using Newtonsoft.Json;
using System.Diagnostics;
using Sfc.Wms.Foundation.Location.Contracts.Dtos;
using Sfc.Wms.Foundation.Carton.Contracts.Dtos;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures
{
    public class DataBaseFixtureForOrmt : CommonFunction
    {
        protected WmsToEmsDto wmsToEmsAddRelease = new WmsToEmsDto();
        protected SwmToMheDto swmToMheAddRelease = new SwmToMheDto();
        protected WmsToEmsDto wmsToEmsCancelation = new WmsToEmsDto();
        protected SwmToMheDto swmToMheCancelation = new SwmToMheDto();
        protected WmsToEmsDto wmsToEmsEPick = new WmsToEmsDto();
        protected SwmToMheDto swmToMheEPick = new SwmToMheDto();
        protected OrmtDto ormt = new OrmtDto();
        protected OrmtDto ormtCancel = new OrmtDto();
        protected OrmtDto ormtEPick = new OrmtDto();
        protected CartonView printCarton = new CartonView();
        protected CartonView cancelOrder = new CartonView();
        protected CartonView ePick = new CartonView();
        protected PickLocationDetailsExtenstionDto pickLcnDtlExtBeforeApi = new PickLocationDetailsExtenstionDto();
        protected PickLocationDetailsExtenstionDto pickLcnDtlExtAfterApi = new PickLocationDetailsExtenstionDto();
        protected CartonHeaderDto cartonHdr = new CartonHeaderDto();
        protected string asrsLocnId;
        protected CartonView activeOrmtCountNotFound = new CartonView();
        protected CartonView pickLocnNotFound = new CartonView();
        protected CartonView ActiveLocnNotFound = new CartonView();


        public void GetDataBeforeTriggerOrmtForPrintingOfCartons()
        {
            OracleConnection db;
            using (db = GetOracleConnection())
            {
                db.Open();     
                printCarton = GetValidOrderDetails(db,5,0);
                pickLcnDtlExtBeforeApi = GetPickLocnDtlExt(db, printCarton.SkuId);
            }
        }

        public void GetDataBeforeTriggerOrmtForCancellationOfOrders()
        {
            OracleConnection db;
            using (db = GetOracleConnection())
            {
                db.Open();
                cancelOrder = GetValidOrderDetails(db, 99, 0);
                pickLcnDtlExtBeforeApi = GetPickLocnDtlExt(db, printCarton.SkuId);
            }
        }
        
        public void GetDataBeforeCallingApiForEpickOfOrders()
        {
            OracleConnection db;
            using (db = GetOracleConnection())
            {
                db.Open();
                ePick = GetValidOrderDetails(db, 5, 1);
                pickLcnDtlExtBeforeApi = GetPickLocnDtlExt(db, printCarton.SkuId);
            }
        }
        public void GetDataAfterCallingOrmtApiForAddRelease()
        {
            OracleConnection db;
            using (db = GetOracleConnection())
            {
                db.Open();
                swmToMheAddRelease = SwmToMhe(db, TransactionCode.Ormt, printCarton.SkuId);
                ormt = JsonConvert.DeserializeObject<OrmtDto>(swmToMheAddRelease.MessageJson);
                wmsToEmsAddRelease = WmsToEmsData(db, swmToMheAddRelease.SourceMessageKey, TransactionCode.Ormt);
                cartonHdr = GetStatusCodeFromCartonHdr(db,printCarton.CartonNbr);
                pickLcnDtlExtAfterApi = GetPickLocnDtlExt(db,printCarton.SkuId);
            }
        }

        public void GetDataAfterCallingApiForCancellationOfOrders()
        {
            OracleConnection db;
            using (db = GetOracleConnection())
            {
                db.Open();
                swmToMheCancelation = SwmToMhe(db, TransactionCode.Ormt, cancelOrder.SkuId);
                ormtCancel = JsonConvert.DeserializeObject<OrmtDto>(swmToMheCancelation.MessageJson);
                wmsToEmsCancelation = WmsToEmsData(db, swmToMheCancelation.SourceMessageKey, TransactionCode.Ormt);
                cartonHdr = GetStatusCodeFromCartonHdr(db, cancelOrder.CartonNbr);
                pickLcnDtlExtAfterApi = GetPickLocnDtlExt(db, cancelOrder.SkuId);
            }
        }

        public void GetDataAfterCallingApiForEPickOrders()
        {
            OracleConnection db;
            using (db = GetOracleConnection())
            {
                db.Open();
                swmToMheEPick = SwmToMhe(db,TransactionCode.Ormt, ePick.SkuId);
                ormtEPick = JsonConvert.DeserializeObject<OrmtDto>(swmToMheEPick.MessageJson);
                wmsToEmsEPick = WmsToEmsData(db, swmToMheEPick.SourceMessageKey, TransactionCode.Ormt);
                cartonHdr = GetStatusCodeFromCartonHdr(db, ePick.CartonNbr);
                pickLcnDtlExtAfterApi = GetPickLocnDtlExt(db, ePick.SkuId);
            }
        }

        public void GetDataForNegativeCases()
        {
            OracleConnection db;
            using (db = GetOracleConnection())
            {
                db.Open();
                activeOrmtCountNotFound = GetCartonNbrWhereActiveOrmtNotFound(db);
                pickLocnNotFound = GetCartonNbrWherePickLocnNotFound(db);
                ActiveLocnNotFound = GetCartonNbrWhereActiveLocationNotFound(db);
            }
        }

        public CartonView GetValidOrderDetails(OracleConnection db, int statCode, int miscNum1)
        {
            var cartonView = new CartonView();
            var query = $"select ch.carton_nbr,ch.wave_nbr,ch.sku_id,ch.MISC_NUM_1, ch.total_qty, ch.stat_code,ch.DEST_LOCN_ID,ph.PKT_CTRL_NBR,ph.SHIP_W_CTRL_NBR,ph.Whse,ph.CO,ph.DIV, im.spl_instr_code_1, im.spl_instr_code_5 from CARTON_HDR ch " +
                $"inner join PKT_HDR ph ON ph.pkt_ctrl_nbr = ch.pkt_ctrl_nbr " +
                $"inner join Pick_Locn_dtl pl ON  pl.sku_id = ch.sku_id  " +
                $"inner join ITEM_MASTER im ON pl.sku_id = im.sku_id " +
                $"inner join locn_hdr lh ON pl.locn_id = lh.locn_id " +
                $"inner join locn_grp lg ON lg.locn_id=lh.locn_id " +
                $"inner join sys_code sc ON sc.code_id=lg.grp_type where ch.stat_code = 12 and ch.MISC_NUM_1 = 0 and ch.MISC_INSTR_CODE_5 is null and sc.code_type='740' and code_id ='18'and lg.grp_attr = DECODE(im.temp_zone,'D','Dry','Freezer') and pick_locn_id = lh.locn_id";
            var command = new OracleCommand(query, db);
            var Reader = command.ExecuteReader();
            if (Reader.Read())
            {
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
            }
            return cartonView;
        }


        public CartonView GetCartonNbrWhereActiveOrmtNotFound(OracleConnection db)
        {
            var cartonView = new CartonView();
            var query = $"select ch.carton_nbr,ch.sku_id,ch.MISC_NUM_1, ch.total_qty, ch.stat_code,pl.actl_invn_qty,ch.DEST_LOCN_ID,ph.PKT_CTRL_NBR,ph.SHIP_W_CTRL_NBR,ph.Whse,ph.CO,ph.DIV, im.spl_instr_code_1, im.spl_instr_code_5 from CARTON_HDR ch inner join PKT_HDR ph ON ph.pkt_ctrl_nbr = ch.pkt_ctrl_nbr inner join Pick_Locn_dtl pl ON  pl.locn_id = ch.pick_locn_id inner join ITEM_MASTER im ON pl.sku_id = im.sku_id inner join locn_hdr lh ON pl.locn_id = lh.locn_id inner join locn_grp lg ON lg.locn_id = lh.locn_id inner join sys_code sc ON sc.code_id = lg.grp_type where sc.code_type = '740' and code_id = '18' and lg.grp_attr = DECODE(im.temp_zone, 'D', 'Dry', 'Freezer') and pl.actl_invn_qty = 0";
            var command = new OracleCommand(query, db);
            var Reader = command.ExecuteReader();
            if (Reader.Read())
            {
                cartonView.CartonNbr = Reader["CARTON_NBR"].ToString();
                cartonView.SkuId = Reader["SKU_ID"].ToString();
                cartonView.WaveNbr = Reader["WAVE_NBR"].ToString();
            }
            return cartonView;
        }

        public CartonView GetCartonNbrWherePickLocnNotFound(OracleConnection db)
        {
            var cartonView = new CartonView();
            var query = $"select ch.carton_nbr,ch.sku_id,pl.locn_id,ch.wave_nbr from CARTON_HDR ch inner join Pick_Locn_dtl pl ON  pl.sku_id! = ch.sku_id where ch.stat_code = 5 and ch.misc_instr_code_5 is null and misc_num_1 = 0 and pl.actl_invn_qty! = 0 ";
            var command = new OracleCommand(query, db);
            var Reader = command.ExecuteReader();
            if (Reader.Read())
            {
                cartonView.CartonNbr = Reader["CARTON_NBR"].ToString();
                cartonView.SkuId = Reader["SKU_ID"].ToString();
                cartonView.WaveNbr = Reader["WAVE_NBR"].ToString();
            }
            return cartonView;

        }

        public CartonView GetCartonNbrWhereActiveLocationNotFound(OracleConnection db)
        {
            var cartonView = new CartonView();
            var query = $"select ch.carton_nbr,ch.sku_id,pl.locn_id,ch.wave_nbr from carton_hdr ch inner join Pick_Locn_dtl pl ON pl.sku_id = ch.sku_id where locn_id not in (select lh.locn_id from locn_hdr lh inner join locn_grp lg on lg.locn_id = lh.locn_id and lg.grp_attr in ('Freezer', 'Dry') inner join sys_code sc on sc.code_id = lg.grp_type and sc.code_type = '740' and code_id = '18')";
            var command = new OracleCommand(query, db);
            var Reader = command.ExecuteReader();
            if (Reader.Read())
            {
                cartonView.CartonNbr = Reader["CARTON_NBR"].ToString();
                cartonView.SkuId = Reader["SKU_ID"].ToString();
                cartonView.WaveNbr = Reader["WAVE_NBR"].ToString();
            }
            return cartonView;
        }

    }
}
