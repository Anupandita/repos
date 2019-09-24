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
        protected CartonView ch = new CartonView();
        protected CartonView cancelOrder = new CartonView();
        protected CartonView ePick = new CartonView();
        protected PickLocationDetailsExtenstionDto pickLcnDtlExtBeforeApi = new PickLocationDetailsExtenstionDto();
        protected PickLocationDetailsExtenstionDto pickLcnDtlExtAfterApi = new PickLocationDetailsExtenstionDto();
        protected CartonHeaderDto cartonHdr = new CartonHeaderDto();

        public void GetDataBeforeTriggerOrmt()
        {
            OracleConnection db;
            using (db = new OracleConnection
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["SfcRbacContextModel"].ConnectionString
            })
            {
                db.Open();     
                ch = GetValidOrderDetails(db,5,0);
                cancelOrder = GetValidOrderDetails(db,12, 0);
                ePick = GetValidOrderDetails(db,12,1);
                pickLcnDtlExtBeforeApi = GetPickLocnDtlExt(db, ch.SkuId);
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
                swmToMheAddRelease = SwmToMhe(db, ch.CartonNbr, TransactionCode.Ormt, ch.SkuId);
                ormt = JsonConvert.DeserializeObject<OrmtDto>(swmToMheAddRelease.MessageJson);
                wmsToEmsAddRelease = WmsToEmsData(db, swmToMheAddRelease.SourceMessageKey, TransactionCode.Ormt);
                cartonHdr = GetStatusCodeFromCartonHdr(db,ch.CartonNbr);
                pickLcnDtlExtAfterApi = GetPickLocnDtlExt(db,ch.SkuId);
            }
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
                swmToMheCancelation = SwmToMhe(db, cancelOrder.CartonNbr, TransactionCode.Ormt, cancelOrder.SkuId);
                ormtCancel = JsonConvert.DeserializeObject<OrmtDto>(swmToMheCancelation.MessageJson);
                wmsToEmsCancelation = WmsToEmsData(db, swmToMheCancelation.SourceMessageKey, TransactionCode.Ormt);
                cartonHdr = GetStatusCodeFromCartonHdr(db, cancelOrder.CartonNbr);
                pickLcnDtlExtAfterApi = GetPickLocnDtlExt(db, cancelOrder.SkuId);
            }
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
                swmToMheEPick = SwmToMhe(db, ePick.CartonNbr, TransactionCode.Ormt, ePick.SkuId);
                ormtEPick = JsonConvert.DeserializeObject<OrmtDto>(swmToMheEPick.MessageJson);
                wmsToEmsEPick = WmsToEmsData(db, swmToMheEPick.SourceMessageKey, TransactionCode.Ormt);
                cartonHdr = GetStatusCodeFromCartonHdr(db, ePick.CartonNbr);
                pickLcnDtlExtAfterApi = GetPickLocnDtlExt(db, ePick.SkuId);
            }
        }

        public void GetDataAfterCallingApiForEPickOrdersTest()
        {
            OracleConnection db;
            using (db = new OracleConnection
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["SfcRbacContextModel"].ConnectionString
            })
            {
                db.Open();
                Debug.Print(System.DateTime.Now.ToString());
                var query = $"Update carton_hdr set stat_code=6 where carton_nbr='00007999994081751262'";
                var command = new OracleCommand(query, db);
                var printingCartonReader = command.ExecuteNonQuery();
                Debug.Print(System.DateTime.Now.ToString());

            }

        }

       
        public CartonView GetValidOrderDetails(OracleConnection db, int statCode, int miscNum1 )
        {
            var cartonView = new CartonView();
            var query = $"select ch.carton_nbr,ch.sku_id, ch.total_qty, ch.stat_code,ch.DEST_LOCN_ID,ch.MISC_NUM_1,ph.PKT_CTRL_NBR,ph.SHIP_W_CTRL_NBR,ph.Whse,ph.CO,ph.DIV, " +
                $"im.spl_instr_code_1, im.spl_instr_code_5 from CARTON_HDR ch inner join PKT_HDR ph ON ph.pkt_ctrl_nbr = ch.pkt_ctrl_nbr inner join Pick_Locn_dtl pl" +
                $" ON  pl.sku_id = ch.sku_id  inner join ITEM_MASTER im ON pl.sku_id = im.sku_id inner join locn_hdr lh ON pl.locn_id = lh.locn_id where ch.stat_code = {statCode} and ch.MISC_INSTR_CODE_5 is null";
            var command = new OracleCommand(query, db);
            var Reader = command.ExecuteReader();
            if (Reader.Read())
            {
                cartonView.PickTktCtrlNbr = Reader[TestData.PickTicketHeader.PickTktCtrlNbr].ToString();
               // cartonView.PickTktSeqNbr = Reader[TestData.PickLocationDetail.PktSeqNbr].ToString();
                cartonView.SkuId = Reader["SKU_ID"].ToString();
                cartonView.Whse = Reader[TestData.PickTicketHeader.Whse].ToString();
                cartonView.Co = Reader[TestData.PickTicketHeader.Co].ToString();
                cartonView.Div = Reader[TestData.PickTicketHeader.Div].ToString();    
                cartonView.SplInstrCode1 = Reader[TestData.ItemMaster.SplInstrCode1].ToString();
                cartonView.SplInstrCode5 = Reader[TestData.ItemMaster.SplInstrCode5].ToString();
                cartonView.TotalQty = Reader["TOTAL_QTY"].ToString();
                cartonView.CartonNbr = Reader["CARTON_NBR"].ToString();
            }
            return cartonView;
        }

        



    }
}
