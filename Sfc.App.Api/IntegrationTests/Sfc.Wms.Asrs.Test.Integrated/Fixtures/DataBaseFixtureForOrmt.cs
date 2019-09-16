using Sfc.Wms.Asrs.Dematic.Contracts.Dtos;
using Sfc.Wms.Asrs.Shamrock.Contracts.Dtos;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;
using Sfc.Wms.ParserAndTranslator.Contracts.Dto;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData;
using Sfc.Wms.ParserAndTranslator.Contracts.Constants;
using Sfc.Wms.Data.Entities;
using Newtonsoft.Json;

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
                var tempzone = GetTempZone(db, "sku");
                var grpAttr = TempZoneRelate(tempzone.TempZone);
                var locnId = GetLocnId(db, ch.SkuId, grpAttr);
                var cartonHdr = GetValidDataForOnPrintingOfCarton(db, locnId.LocationId);
                ch = OnWaveReleaseOrderDetails(db, cartonHdr.WaveNbr);
                var tempzone1 = GetTempZone(db, ch.SkuId);
                var grpAttr1 = TempZoneRelate(tempzone1.TempZone);
                var locnId1 = GetLocnId(db, ch.SkuId, grpAttr);
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

        public CartonView GetValidDataForOnPrintingOfCarton(OracleConnection db, string locnId)
        {
            var ch = new CartonView();
            var query = $"select * from carton_hdr where stat_code = 5 and MISC_INSTR_CODE_5 is null and Pick_locn_id = '{locnId}'";
            var command = new OracleCommand(query, db);
            var printingCartonReader = command.ExecuteReader();
            if (printingCartonReader.Read())
            {
                ch.CartonNbr = printingCartonReader[TestData.CartonHeader.CartonNbr].ToString();
                ch.StatusCode = printingCartonReader[TestData.CartonHeader.StatCode].ToString();
                ch.WaveNbr = printingCartonReader[TestData.CartonHeader.WaveNbr].ToString();
            }
            return ch;
        }

        public CartonView GetValidDataForOnCancellationOfCarton(OracleConnection db)
        {
            var cd = new CartonView();
            var query = $"select * from carton_hdr where stat_code BETWEEN 6 AND 29";
            var command = new OracleCommand(query, db);
            var cancelledCartonReader = command.ExecuteReader();
            if (cancelledCartonReader.Read())
            {
                cd.CartonNbr = cancelledCartonReader[TestData.CartonHeader.CartonNbr].ToString();
                cd.StatusCode = cancelledCartonReader[TestData.CartonHeader.StatCode].ToString();
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
                cd.CartonNbr = ePickCartonReader[TestData.CartonHeader.CartonNbr].ToString();
                cd.StatusCode = ePickCartonReader[TestData.CartonHeader.StatCode].ToString();
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
                cartonView.PickTktCtrlNbr = Reader[TestData.PickTicketHeader.PickTktCtrlNbr].ToString();
                cartonView.PickTktSeqNbr = Reader[TestData.PickLocationDetail.PktSeqNbr].ToString();
                cartonView.SkuId = Reader[TestData.PickLocationDetail.SkuId].ToString();
                cartonView.Whse = Reader[TestData.PickTicketHeader.Whse].ToString();
                cartonView.Co = Reader[TestData.PickTicketHeader.Co].ToString();
                cartonView.Div = Reader[TestData.PickTicketHeader.Div].ToString();
                cartonView.PickTktQty = Reader[TestData.PickLocationDetail.PktQty].ToString();
                cartonView.SplInstrCode1 = Reader[TestData.ItemMaster.SplInstrCode1].ToString();
                cartonView.SplInstrCode5 = Reader[TestData.ItemMaster.SplInstrCode5].ToString();
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
                cartonView.PickTktCtrlNbr = Reader[TestData.PickTicketHeader.PickTktCtrlNbr].ToString();
                cartonView.PickTktSeqNbr = Reader[TestData.PickLocationDetail.PktSeqNbr].ToString();
                cartonView.SkuId = Reader[TestData.PickLocationDetail.SkuId].ToString();
                cartonView.Whse = Reader[TestData.PickTicketHeader.Whse].ToString();
                cartonView.Co = Reader[TestData.PickTicketHeader.Co].ToString();
                cartonView.Div = Reader[TestData.PickTicketHeader.Div].ToString();
                cartonView.PickTktQty = Reader[TestData.PickLocationDetail.PktQty].ToString();
                cartonView.SplInstrCode1 = Reader[TestData.ItemMaster.SplInstrCode1].ToString();
                cartonView.SplInstrCode5 = Reader[TestData.ItemMaster.SplInstrCode5].ToString();
            }
            return cartonView;
        }

       


    }
}
