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
        protected string DestinationLocationForNormalCarton;
        protected string DestinationLocationForEpickCarton;
        protected List<PickLocationDetailsExtenstionDto> ActiveOrmtCountList = new List<PickLocationDetailsExtenstionDto>();
        public string Uom;
       

        public void GetDataBeforeTriggerOrmtForPrintingOfCartons()
        {
            OracleConnection db;
            using (db = GetOracleConnection())
            {
                db.Open();     
                PrintCarton = GetValidOrderDetails(db,"5","0","0");               
                PickLcnDtlExtBeforeApi = GetPickLocnDtlExt(db, PrintCarton.SkuId, null);
                DestinationLocationForNormalCarton = CartonHeaderDestinationLocationMatchForNormalCarton(PrintCarton.TempZone);
                var UOM = GetUnitOfMeasureFromItemMaster(db, PrintCarton.SkuId);
                Uom = ItemMasterUnitOfMeasure(UOM);
            }
        }    

       public void GetValidDataBeforeTriggerOrmtForPrintingOfCartonsThroughWaveNumber()
       {
            OracleConnection db;
            using (db = GetOracleConnection())
            {
                db.Open();
               // var waveNumber = GetWaveNumber(db);
                OrderList = GetValidOrderDetailsForWaveRelease(db, null);
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
                CancelOrder = GetValidOrderDetails(db, "99", "0","90");
                PickLcnDtlExtBeforeApi = GetPickLocnDtlExt(db, CancelOrder.SkuId,null);
                DestinationLocationForNormalCarton = CartonHeaderDestinationLocationMatchForNormalCarton(PrintCarton.TempZone);
                var UOM = GetUnitOfMeasureFromItemMaster(db, CancelOrder.SkuId);
                Uom = ItemMasterUnitOfMeasure(UOM);
            }
        }
        
     
        public void GetDataBeforeCallingApiForEpickOfOrders()
        {
            OracleConnection db;
            using (db = GetOracleConnection())
            {
                db.Open();
                EPick = GetValidOrderDetails(db, "12", "1","90");
                PickLcnDtlExtBeforeApi = GetPickLocnDtlExt(db, EPick.SkuId, EPick.LocnId);
                DestinationLocationForEpickCarton = CartonHeaderDestinationMatchForEPickCarton(EPick.TempZone);
                var UOM = GetUnitOfMeasureFromItemMaster(db, EPick.SkuId);
                Uom = ItemMasterUnitOfMeasure(UOM);
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
                    //PickLcnDtlExtAfterApi = GetPickLocnDtlExt(db, OrderList[i].SkuId,OrderList[i].LocnId);
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

        public CartonView GetValidOrderDetails(OracleConnection db, string statCode, string miscNum1,string status)
        {           
            var cartonView = new CartonView();
            var query = OrmtQueries.ValidCartonsForAddRelease;
            var command = new OracleCommand(query, db);
            command.Parameters.Add(new OracleParameter("statCode", statCode));
            command.Parameters.Add(new OracleParameter("miscNum1", miscNum1));
            command.Parameters.Add(new OracleParameter("status", status));
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
                cartonView.TempZone = reader["TEMP_ZONE"].ToString();
            }
            return cartonView;
        }

        public CartonView GetValidOnProcessCostMessage(OracleConnection db)
        {
            var onproCostView= new CartonView();
            var onQuery = OrmtQueries.ValidDataForOnProcessingCostMessage;
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
            var updateQuery = OrmtQueries.UpdatePickStatCode;
            Command = new OracleCommand(updateQuery, db);
            Command.Parameters.Add(new OracleParameter("pktStatCode", pktStatCode));
            Command.Parameters.Add(new OracleParameter("pktCtrlNbr", pktCtrlNbr));
            Command.ExecuteNonQuery();
            Transaction.Commit();
        }

        protected string GetWaveNumber(OracleConnection db)
        {         
            var query = OrmtQueries.WaveRelease;             
            Command = new OracleCommand(query,db);
            Command.Parameters.Add(new OracleParameter("miscNum1", Constants.MiscNum1));
            Command.Parameters.Add(new OracleParameter("sysType", Constants.SysCodeType));
            Command.Parameters.Add(new OracleParameter("sysCodeId", Constants.SysCodeIdForActiveLocation));
            Command.Parameters.Add(new OracleParameter("status", Constants.EgblOrmtStatus));
            var waveNbr = Command.ExecuteScalar().ToString();
            return waveNbr;
        }
   
        public List<CartonView> GetValidOrderDetailsForWaveRelease(OracleConnection db, string waveNbr)
        {
            var orderdtls = new List<CartonView>();
            var query = OrmtQueries.WaveRelease;
            var command = new OracleCommand(query,db);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var cartonView = new CartonView();
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
                orderdtls.Add(cartonView);
            }
            return orderdtls;
        }

        public CartonView GetCartonNbrWhereActiveOrmtNotFound(OracleConnection db)
        {
            var cartonView = new CartonView();
            var query = OrmtQueries.ActiveOrmtNotFound;
            var command = new OracleCommand(query, db);
            command.Parameters.Add(new OracleParameter("sysType", Constants.SysCodeType));
            command.Parameters.Add(new OracleParameter("sysCodeId", Constants.SysCodeIdForActiveLocation));           
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
            var query = OrmtQueries.PickLocnNotFound;
            var command = new OracleCommand(query, db);
            command.Parameters.Add(new OracleParameter("sysType", Constants.SysCodeType));
            command.Parameters.Add(new OracleParameter("sysCodeId", Constants.SysCodeIdForActiveLocation));     
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
           var query = OrmtQueries.CartonHeader;
           var command = new OracleCommand(query, db);
           command.Parameters.Add(new OracleParameter("cartonNbr", cartonNbr));
           var cartonHdrReader = command.ExecuteReader();
           if (cartonHdrReader.Read())
           {              
              cartonHdrView.StatusCode = Convert.ToInt16(cartonHdrReader["STAT_CODE"]);
           }
            return cartonHdrView;
        }

        public SwmEligibleOrmtCartonDto GetStatusCodeFromEligibleOrmtCount(OracleConnection db, string cartonNbr)
        {
            var swmEligibleOrmtCount = new SwmEligibleOrmtCartonDto();
            var query = OrmtQueries.SwmElgblOrmtCount;
            var command = new OracleCommand(query,db);
            command.Parameters.Add(new OracleParameter("cartonNbr", cartonNbr));
            var reader = command.ExecuteReader();
            if(reader.Read())
            {
                swmEligibleOrmtCount.Status = Convert.ToInt16(reader["STATUS"]);
            }
            return swmEligibleOrmtCount;
        }

        public CartonView GetCartonNbrWhereActiveLocationNotFound(OracleConnection db)
        {
            var cartonView = new CartonView();
            var query = OrmtQueries.ActiveLocnNotFound;
            var command = new OracleCommand(query, db);
            command.Parameters.Add(new OracleParameter("sysCodeType", Constants.SysCodeType));
            command.Parameters.Add(new OracleParameter("sysCodeId", Constants.SysCodeIdForActiveLocation));            
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
            Assert.AreEqual(DestinationLocationForNormalCarton, ormt.DestinationLocationId);
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

       

        public string CartonHeaderDestinationLocationMatchForNormalCarton(string tempZone)
        {
            switch (tempZone)
            {
                case "D":
                    return "AM-SHIP";
                case "F":
                    return "FR-SHIP";
                default:
                    return "";
            }
        }

        public string CartonHeaderDestinationMatchForEPickCarton(string tempZone)
        {
            switch (tempZone)
            {
                case "D":
                    return "AM-REJECT";
                case "F":
                    return "FR-REJECT";
                default:
                    return "";
            }
        }










    }
}
