using Sfc.Wms.Asrs.Dematic.Contracts.Dtos;
using Sfc.Wms.Asrs.Shamrock.Contracts.Dtos;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;
using Sfc.Wms.ParserAndTranslator.Contracts.Dto;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData;
using Sfc.Wms.ParserAndTranslator.Contracts.Constants;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures
{
    public class DataBaseFixtureForOrmt : DataBaseFixture
    {
        protected WmsToEmsDto wmsToEmsAddRelease = new WmsToEmsDto();
        protected SwmToMheDto swmToMheAddRelease = new SwmToMheDto();
        protected WmsToEmsDto wmsToEmsCancelation = new WmsToEmsDto();
        protected SwmToMheDto swmToMheCancelation = new SwmToMheDto();
        protected WmsToEmsDto wmsToEmsEPick = new WmsToEmsDto();
        protected SwmToMheDto swmToMheEPick = new SwmToMheDto();
        protected OrmtDto ormt = new OrmtDto();
        protected CustomDto ch = new CustomDto();
        protected CustomDto cancelOrder = new CustomDto();
        protected CustomDto EPick = new CustomDto();

        public void GetDataBeforeTriggerOrmt()
        {
            OracleConnection db;
            using (db = new OracleConnection
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["SfcRbacContextModel"].ConnectionString
            })
            {
                db.Open();
            }
            var cartonHdr = GetValidDataForOnPrintingOfCarton(db);
            ch = OnWaveReleaseOrderDetails(db, cartonHdr.WaveNbr);
            var co = GetValidDataForOnCancellationOfCarton(db);
            cancelOrder = OnCancellationAndEPickOrdersFetchData(db, co.CartonNbr);
            var epick = GetValidDataForEPick(db);
            EPick = OnCancellationAndEPickOrdersFetchData(db, epick.CartonNbr);
        }


        public void GetDataAfterCallingOrmtApiForAddRelease()
        {
            OracleConnection db;
            using (db = new OracleConnection
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["SfcRbacContextModel"].ConnectionString
            })
            {
                db.Open();
            }

            swmToMheAddRelease = SwmToMhe(db, ch.CartonNbr, TransactionCode.Ormt, ch.SkuId);
            wmsToEmsAddRelease = WmsToEmsData(db, swmToMheAddRelease.SourceMessageKey, TransactionCode.Ormt);
        }

        public void GetDataAfterCallingApiForCancellationOfOrders()
        {
            OracleConnection db;
            using (db = new OracleConnection
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["SfcRbacContextModel"].ConnectionString
            })
            {
                db.Open();
            }
            swmToMheCancelation = SwmToMhe(db, cancelOrder.CartonNbr, TransactionCode.Ormt, cancelOrder.SkuId);
            wmsToEmsCancelation = WmsToEmsData(db, swmToMheCancelation.SourceMessageKey, TransactionCode.Ormt);
        }

        public void GetDataAfterCallingApiForEPickOrders()
        {
            OracleConnection db;
            using (db = new OracleConnection
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["SfcRbacContextModel"].ConnectionString
            })
            {
                db.Open();
            }
            swmToMheEPick = SwmToMhe(db, EPick.CartonNbr, TransactionCode.Ormt, EPick.SkuId);
            wmsToEmsEPick = WmsToEmsData(db, swmToMheEPick.SourceMessageKey, TransactionCode.Ormt);
        }

        public CustomDto GetValidDataForOnPrintingOfCarton(OracleConnection db)
        {
            var cd = new CustomDto();
            var query = $"select ch.carton_nbr,ch.wave_nbr,ch.stat_code,cs.cwc_zone_nbr from carton_hdr ch  inner join CWC_STATUS cs on ch.wave_nbr = cs.pkms_wave_nbr and ch.stat_code = 5 and ch.MISC_INSTR_CODE_5 is null and Pick_locn_id = '000192814'";
            var command = new OracleCommand(query, db);
            var dr1 = command.ExecuteReader();
            if (dr1.Read())
            {
                ch.CartonNbr = dr1["CARTON_NBR"].ToString();
                ch.StatusCode = dr1["STAT_CODE"].ToString();
                ch.CwcZoneNbr = dr1["CWC_ZONE_NBR"].ToString();
                ch.WaveNbr = dr1["WAVE_NBR"].ToString();
            }
            return cd;
        }

        public CustomDto GetValidDataForOnCancellationOfCarton(OracleConnection db)
        {
            var cd = new CustomDto();
            var query = $"select * from carton_hdr where stat_code = 99";
            var command = new OracleCommand(query, db);
            var dr2 = command.ExecuteReader();
            if (dr2.Read())
            {
                cd.CartonNbr = dr2["CARTON_NBR"].ToString();
                cd.StatusCode = dr2["STAT_CODE"].ToString();
            }
            return cd;
        }

        public CustomDto GetValidDataForEPick(OracleConnection db)
        {
            var cd = new CustomDto();
            var query = $"select * from carton_hdr where MISC_NUM_1=1";
            var command = new OracleCommand(query, db);
            var dr3 = command.ExecuteReader();
            if (dr3.Read())
            {
                cd.CartonNbr = dr3["CARTON_NBR"].ToString();
                cd.StatusCode = dr3["STAT_CODE"].ToString();
            }
            return cd;
        }

        public CustomDto OnWaveReleaseOrderDetails(OracleConnection db, string waveNbr)
        {
            var cd = new CustomDto();
            var query = $"select ph.PKT_CTRL_NBR,pd.PKT_SEQ_NBR,ch.WAVE_NBR,ph.SHIP_DATE_TIME,pd.SKU_ID,ph.WHSE,ph.CO," +
                $"ph.DIV,pd.pkt_qty,im.SPL_INSTR_CODE_1,im.SPL_INSTR_CODE_5,ch.CARTON_NBR from CARTON_HDR ch inner join " +
                $"CARTON_DTL cd ON ch.carton_nbr = cd.carton_nbr inner join PKT_HDR ph ON ph.pkt_ctrl_nbr = ch.pkt_ctrl_nbr " +
                $"inner join PKT_DTL pd ON pd.pkt_ctrl_nbr = ph.pkt_ctrl_nbr and pd.pkt_seq_nbr = cd.pkt_seq_nbr inner join " +
                $"ITEM_MASTER im ON pd.sku_id = im.sku_id where ch.wave_nbr = '{waveNbr}'";
            var command = new OracleCommand(query, db);
            var dr5 = command.ExecuteReader();
            if (dr5.Read())
            {
                cd.PickTktCtrlNbr = dr5["PKT_CTRL_NBR"].ToString();
                cd.PickTktSeqNbr = dr5["PKT_SEQ_NBR"].ToString();
                cd.SkuId = dr5["SKU_ID"].ToString();
                cd.Whse = dr5["WHSE"].ToString();
                cd.Co = dr5["CO"].ToString();
                cd.Div = dr5["DIV"].ToString();
                cd.PickTktQty = dr5["PKT_QTY"].ToString();
                cd.SplInstrCode1 = dr5["SPL_INSTR_CODE_1"].ToString();
                cd.SplInstrCode5 = dr5["SPL_INSTR_CODE_5"].ToString();
            }
            return cd;
        }

        public CustomDto OnCancellationAndEPickOrdersFetchData(OracleConnection db, string cartonNbr)
        {
            var cd = new CustomDto();
            var query = $"select ph.PKT_CTRL_NBR,pd.PKT_SEQ_NBR,ch.WAVE_NBR,ph.SHIP_DATE_TIME,pd.SKU_ID,ph.WHSE," +
                $"ph.CO,ph.DIV,ch.TOTAL_QTY,im.SPL_INSTR_CODE_1,im.SPL_INSTR_CODE_5,ch.CARTON_NBR from CARTON_HDR ch " +
                $"inner join CARTON_DTL cd ON ch.carton_nbr = cd.carton_nbr inner join PKT_HDR ph ON ph.pkt_ctrl_nbr = ch.pkt_ctrl_nbr" +
                $" inner join PKT_DTL pd ON pd.pkt_ctrl_nbr = ph.pkt_ctrl_nbr and pd.pkt_seq_nbr = cd.pkt_seq_nbr" +
                $" inner join ITEM_MASTER im ON pd.sku_id = im.sku_id where ch.Carton_nbr = '{cartonNbr}'";
            var command = new OracleCommand(query, db);
            var dr6 = command.ExecuteReader();
            if (dr6.Read())
            {
                cd.PickTktCtrlNbr = dr6["PKT_CTRL_NBR"].ToString();
                cd.PickTktSeqNbr = dr6["PKT_SEQ_NBR"].ToString();
                cd.SkuId = dr6["SKU_ID"].ToString();
                cd.Whse = dr6["WHSE"].ToString();
                cd.Co = dr6["CO"].ToString();
                cd.Div = dr6["DIV"].ToString();
                cd.TotalQty = dr6["TOTAL_QTY"].ToString();
                cd.SplInstrCode1 = dr6["SPL_INSTR_CODE_1"].ToString();
                cd.SplInstrCode5 = dr6["SPL_INSTR_CODE_5"].ToString();
            }
            return cd;
        }

        public CustomDto PickLocnDtlExt(OracleConnection db)
        {
            var cd = new CustomDto();
            var query = $"Select PLD.ACTL_QTY-NVL(PLDE.ACTIVE_ORMT_COUNT,0) as AVAILABLE_QTY From Pick_Locn_Dtl PLD Left Outer Join  Pick_Locn_Dtl_Ext PLDE On PLD.Pick_Locn_Dtl_Id = PLDE.Pick_Locn_Dtl_Id";
            var command = new OracleCommand(query, db);
            var dr7 = command.ExecuteReader();
            if (dr7.Read())
            {
            }
            return cd;
        }
    }
}
