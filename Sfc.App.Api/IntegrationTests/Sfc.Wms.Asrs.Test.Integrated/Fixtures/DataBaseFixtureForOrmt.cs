using Sfc.Wms.Asrs.Dematic.Contracts.Dtos;
using Sfc.Wms.Asrs.Shamrock.Contracts.Dtos;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;
using Sfc.Wms.ParserAndTranslator.Contracts.Dto;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData;
using Sfc.Wms.ParserAndTranslator.Contracts.Constants;
using Newtonsoft.Json;

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
        protected OrmtDto ormtCancel = new OrmtDto();
        protected OrmtDto ormtEPick = new OrmtDto();
        protected CartonView ch = new CartonView();
        protected CartonView cancelOrder = new CartonView();
        protected CartonView ePick = new CartonView();

        public void GetDataBeforeTriggerOrmt()
        {
            OracleConnection db;
            using (db = new OracleConnection
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["SfcRbacContextModel"].ConnectionString
            })
            {
                db.Open();

                var cartonHdr = GetValidDataForOnPrintingOfCarton(db);
                ch = OnWaveReleaseOrderDetails(db, cartonHdr.WaveNbr);
                var co = GetValidDataForOnCancellationOfCarton(db);
                cancelOrder = OnCancellationAndEPickOrdersFetchData(db, co.CartonNbr);
                var epick = GetValidDataForEPick(db);
                ePick = OnCancellationAndEPickOrdersFetchData(db, epick.CartonNbr);
            }
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
            ormt = JsonConvert.DeserializeObject<OrmtDto>(swmToMheAddRelease.MessageJson);
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
            ormtCancel = JsonConvert.DeserializeObject<OrmtDto>(swmToMheCancelation.MessageJson);
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
            swmToMheEPick = SwmToMhe(db, ePick.CartonNbr, TransactionCode.Ormt, ePick.SkuId);
            ormtEPick = JsonConvert.DeserializeObject<OrmtDto>(swmToMheEPick.MessageJson);
            wmsToEmsEPick = WmsToEmsData(db, swmToMheEPick.SourceMessageKey, TransactionCode.Ormt);
        }

        public CartonView GetValidDataForOnPrintingOfCarton(OracleConnection db)
        {
            var ch = new CartonView();
            var query = $"select * from carton_hdr where stat_code = 5 and MISC_INSTR_CODE_5 is null";
            var command = new OracleCommand(query, db);
            var printingCartonReader = command.ExecuteReader();
            if (printingCartonReader.Read())
            {
                ch.CartonNbr = printingCartonReader[TestData.CartonHeader.cartonNbr].ToString();
                ch.StatusCode = printingCartonReader[CartonHeader.statCode].ToString();
                ch.WaveNbr = printingCartonReader[CartonHeader.waveNbr].ToString();
            }
            return ch;
        }

        public CartonView GetValidDataForOnCancellationOfCarton(OracleConnection db)
        {
            var cd = new CartonView();
            var query = $"select * from carton_hdr where stat_code = 99";
            var command = new OracleCommand(query, db);
            var cancelledCartonReader = command.ExecuteReader();
            if (cancelledCartonReader.Read())
            {
                cd.CartonNbr = cancelledCartonReader[CartonHeader.cartonNbr].ToString();
                cd.StatusCode = cancelledCartonReader[CartonHeader.statCode].ToString();
            }
            return cd;
        }

        public CartonView GetValidDataForEPick(OracleConnection db)
        {
            var cd = new CartonView();
            var query = $"select * from carton_hdr where MISC_NUM_1=1";
            var command = new OracleCommand(query, db);
            var ePickCartonReader = command.ExecuteReader();
            if (ePickCartonReader.Read())
            {
                cd.CartonNbr = ePickCartonReader[CartonHeader.cartonNbr].ToString();
                cd.StatusCode = ePickCartonReader[CartonHeader.statCode].ToString();
            }
            return cd;
        }

        public CartonView OnWaveReleaseOrderDetails(OracleConnection db, string waveNbr)
        {
            var cartonView = new CartonView();
            var query = $"select ph.PKT_CTRL_NBR,pd.PKT_SEQ_NBR,ch.WAVE_NBR,ph.SHIP_DATE_TIME,pd.SKU_ID,ph.WHSE,ph.CO," +
                $"ph.DIV,pd.pkt_qty,im.SPL_INSTR_CODE_1,im.SPL_INSTR_CODE_5,ch.CARTON_NBR from CARTON_HDR ch inner join " +
                $"CARTON_DTL cd ON ch.carton_nbr = cd.carton_nbr inner join PKT_HDR ph ON ph.pkt_ctrl_nbr = ch.pkt_ctrl_nbr " +
                $"inner join PKT_DTL pd ON pd.pkt_ctrl_nbr = ph.pkt_ctrl_nbr and pd.pkt_seq_nbr = cd.pkt_seq_nbr inner join " +
                $"ITEM_MASTER im ON pd.sku_id = im.sku_id where ch.wave_nbr = '{waveNbr}'";
            var command = new OracleCommand(query, db);
            var Reader = command.ExecuteReader();
            if (Reader.Read())
            {
                cartonView.PickTktCtrlNbr = Reader[PickHeader.PickTktCtrlNbr].ToString();
                cartonView.PickTktSeqNbr = Reader[PickDetail.PktSeqNbr].ToString();
                cartonView.SkuId = Reader[PickDetail.SkuId].ToString();
                cartonView.Whse = Reader[PickHeader.Whse].ToString();
                cartonView.Co = Reader[PickHeader.Co].ToString();
                cartonView.Div = Reader[PickHeader.Div].ToString();
                cartonView.PickTktQty = Reader[PickDetail.PktQty].ToString();
                cartonView.SplInstrCode1 = Reader[ItemMaster.SplInstrCode1].ToString();
                cartonView.SplInstrCode5 = Reader[ItemMaster.SplInstrCode5].ToString();
            }
            return cartonView;
        }

        public CartonView OnCancellationAndEPickOrdersFetchData(OracleConnection db, string cartonNbr)
        {
            var cartonView = new CartonView();
            var query = $"select ph.PKT_CTRL_NBR,pd.PKT_SEQ_NBR,ch.WAVE_NBR,ph.SHIP_DATE_TIME,pd.SKU_ID,ph.WHSE," +
                $"ph.CO,ph.DIV,ch.TOTAL_QTY,im.SPL_INSTR_CODE_1,im.SPL_INSTR_CODE_5,ch.CARTON_NBR from CARTON_HDR ch " +
                $"inner join CARTON_DTL cd ON ch.carton_nbr = cd.carton_nbr inner join PKT_HDR ph ON ph.pkt_ctrl_nbr = ch.pkt_ctrl_nbr" +
                $" inner join PKT_DTL pd ON pd.pkt_ctrl_nbr = ph.pkt_ctrl_nbr and pd.pkt_seq_nbr = cd.pkt_seq_nbr" +
                $" inner join ITEM_MASTER im ON pd.sku_id = im.sku_id where ch.Carton_nbr = '{cartonNbr}'";
            var command = new OracleCommand(query, db);
            var Reader = command.ExecuteReader();
            if (Reader.Read())
            {
                cartonView.PickTktCtrlNbr = Reader[PickHeader.PickTktCtrlNbr].ToString();
                cartonView.PickTktSeqNbr = Reader[PickDetail.PktSeqNbr].ToString();
                cartonView.SkuId = Reader[PickDetail.SkuId].ToString();
                cartonView.Whse = Reader[PickHeader.Whse].ToString();
                cartonView.Co = Reader[PickHeader.Co].ToString();
                cartonView.Div = Reader[PickHeader.Div].ToString();
                cartonView.PickTktQty = Reader[PickDetail.PktQty].ToString();
                cartonView.SplInstrCode1 = Reader[ItemMaster.SplInstrCode1].ToString();
                cartonView.SplInstrCode5 = Reader[ItemMaster.SplInstrCode5].ToString();
            }
            return cartonView;
        }

        public CartonView PickLocnDtlExt(OracleConnection db)
        {
            var cartonview = new CartonView();
            var query = $"Select PLD.ACTL_QTY-NVL(PLDE.ACTIVE_ORMT_COUNT,0) as AVAILABLE_QTY From Pick_Locn_Dtl PLD Left Outer Join  Pick_Locn_Dtl_Ext PLDE On PLD.Pick_Locn_Dtl_Id = PLDE.Pick_Locn_Dtl_Id";
            var command = new OracleCommand(query, db);
            var dr7 = command.ExecuteReader();
            if (dr7.Read())
            {
            }
            return cartonview;
        }
    }
}
