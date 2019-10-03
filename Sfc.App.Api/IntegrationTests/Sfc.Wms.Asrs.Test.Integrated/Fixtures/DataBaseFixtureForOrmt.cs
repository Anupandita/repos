﻿using Sfc.Wms.Asrs.Dematic.Contracts.Dtos;
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
using System.Collections.Generic;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
        protected List<CartonView> orderList = new List<CartonView>();
        protected List<PickLocationDetailsExtenstionDto> activeOrmtCountList = new List<PickLocationDetailsExtenstionDto>();

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


        public void GetValidDataBeforeTriggerOrmtForPrintingOfCartonsThroughWaveNumber()
        {
            OracleConnection db;
            using (db = GetOracleConnection())
            {
                db.Open();
                orderList = GetValidOrderDetailsForPrintingOfCartons(db);
                activeOrmtCountList = FetchActiveOrmtCount(db);
            }
        }


        public List<PickLocationDetailsExtenstionDto> FetchActiveOrmtCount(OracleConnection db)
        {
            var pickLocnDtlExt = new List<PickLocationDetailsExtenstionDto>();
            for (var j = 0; j < orderList.Count; j++)
            {            
                var query = $"select * from pick_locn_dtl_ext WHERE  SKU_ID='{orderList[j].SkuId}'";
                command = new OracleCommand(query, db);
                var pickLocnDtlExtReader = command.ExecuteReader();
                if (pickLocnDtlExtReader.Read())
                {
                    var activeOrmt = new PickLocationDetailsExtenstionDto();
                    activeOrmt.ActiveOrmtCount = Convert.ToInt16(pickLocnDtlExtReader[TestData.PickLocnDtlExt.ActiveOrmtCount].ToString());
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

        public void GetDataAfterCallingOrmtApiAfterWaveRelease()
        {
            OracleConnection db;
            using (db = GetOracleConnection())
            {
                db.Open();
                for (var i = 0; i < orderList.Count; i++)
                {
                    swmToMheAddRelease = SwmToMhe(db, TransactionCode.Ormt, orderList[i].SkuId);
                    ormt = JsonConvert.DeserializeObject<OrmtDto>(swmToMheAddRelease.MessageJson);
                    var printCarton = orderList[i];
                    VerifyOrmtMessageWasInsertedInToSwmToMhe(ormt,swmToMheAddRelease,printCarton);
                    VerifyOrmtMessageWasInsertedInToWmsToEmsForPrintingOfOrder(swmToMheAddRelease,wmsToEmsAddRelease);
                    cartonHdr = GetStatusCodeFromCartonHdrForWaveRelease(db, orderList[i].CartonNbr);
                    pickLcnDtlExtAfterApi = GetPickLocnDtlExt(db, orderList[i].SkuId);
                }
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

        public List<CartonView> GetValidOrderDetailsForPrintingOfCartons(OracleConnection db)
        {
            var orderdtls = new List<CartonView>();
            var query = $"select ch.carton_nbr,ch.sku_id,ch.wave_nbr,ch.MISC_NUM_1, ch.total_qty, ch.stat_code,pl.actl_invn_qty,ch.DEST_LOCN_ID,ph.PKT_CTRL_NBR,ph.SHIP_W_CTRL_NBR,ph.Whse,ph.CO,ph.DIV, im.spl_instr_code_1, im.spl_instr_code_5 from CARTON_HDR ch inner join PKT_HDR ph ON ph.pkt_ctrl_nbr = ch.pkt_ctrl_nbr inner join Pick_Locn_dtl pl ON  pl.sku_id = ch.sku_id inner join ITEM_MASTER im ON pl.sku_id = im.sku_id inner join locn_hdr lh ON pl.locn_id = lh.locn_id inner join locn_grp lg ON lg.locn_id = lh.locn_id inner join sys_code sc ON sc.code_id = lg.grp_type where ch.stat_code = 5 and ch.misc_instr_code_5 is null and misc_num_1 = 0 and sc.code_type = '740' and code_id = '18' and lg.grp_attr = DECODE(im.temp_zone, 'D', 'Dry', 'Freezer') and pl.actl_invn_qty! = 0 and ch.wave_nbr = '20190804052'";
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
        public CartonHeaderDto GetStatusCodeFromCartonHdrForWaveRelease(OracleConnection db,string cartonNbr)
        {
            var query = $"Select * from carton_hdr where carton_nbr = '{cartonNbr}'";
            command = new OracleCommand(query, db);
            var cartonHdrReader = command.ExecuteReader();
            if (cartonHdrReader.Read())
            {
                var cartonHdrView = new CartonHeaderDto();
                cartonHdrView.StatusCode = Convert.ToInt16(cartonHdrReader["STAT_CODE"].ToString());
            }
            return cartonHdr;
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

        protected void VerifyOrmtMessageWasInsertedInToSwmToMhe(OrmtDto ormt, SwmToMheDto swmToMheAddRelease, CartonView printCarton)
        {
            Assert.AreEqual("Ready", swmToMheAddRelease.SourceMessageStatus);
            Assert.AreEqual(TransactionCode.Ormt, ormt.TransactionCode);
            Assert.AreEqual(MessageLength.Ormt, ormt.MessageLength);
            Assert.AreEqual("AddRelease", ormt.ActionCode);
            Assert.AreEqual(printCarton.SkuId, ormt.Sku);
            Assert.AreEqual(printCarton.TotalQty, ormt.Quantity);
            Assert.AreEqual("Case", ormt.UnitOfMeasure);
            Assert.AreEqual(printCarton.CartonNbr, ormt.OrderId);
            Assert.AreEqual("1", ormt.OrderLineId);
            Assert.AreEqual("SHIPMENT", ormt.OrderType);
            Assert.AreEqual(printCarton.WaveNbr, ormt.WaveId);
            Assert.AreEqual("N", ormt.EndOfWaveFlag);
            Assert.AreEqual(printCarton.DestLocnId + "-" + printCarton.ShipWCtrlNbr, ormt.DestinationLocationId);
            Assert.AreEqual(printCarton.Whse + printCarton.Co + printCarton.Div, ormt.Owner);
            Assert.AreEqual("FIFO", ormt.OpRule);
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
