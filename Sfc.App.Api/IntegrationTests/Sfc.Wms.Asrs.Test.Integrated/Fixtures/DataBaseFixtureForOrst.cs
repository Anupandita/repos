using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData;
using Sfc.Wms.Interfaces.Asrs.Shamrock.Contracts.Dtos;
using Sfc.Wms.Interfaces.Asrs.Dematic.Contracts.Dtos;
using Sfc.Wms.Interfaces.ParserAndTranslator.Contracts.Constants;
using Sfc.Wms.Interfaces.ParserAndTranslator.Contracts.Dto;
using Sfc.Wms.Foundation.Location.Contracts.Dtos;
using Sfc.Core.OnPrem.Result;
using System;
using Sfc.Wms.Foundation.Carton.Contracts.Dtos;
using PickTicketDetail = Sfc.Wms.Data.Entities.PickTicketDetail;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures
{
   
    public class DataBaseFixtureForOrst : DataBaseFixtureForOrmt
    {
        protected OrstTestData allocated = new OrstTestData();
        protected OrstTestData complete = new OrstTestData();
        protected OrstTestData deallocate = new OrstTestData();
        protected OrstTestData canceled = new OrstTestData();
        protected SwmFromMheDto swmFromMheAllocated = new SwmFromMheDto();
        protected SwmFromMheDto swmFromMheComplete = new SwmFromMheDto();
        protected SwmFromMheDto swmFromMheDeallocate = new SwmFromMheDto();
        protected SwmFromMheDto swmFromMheCancel = new SwmFromMheDto();
        protected OrstDto OrstParameters;
        protected OrstDto orstAllocated = new OrstDto();
        protected OrstDto orstCompleted = new OrstDto();
        protected OrstDto orstDeallocate = new OrstDto();
        protected OrstDto orstCanceled = new OrstDto();
        protected OrmtDto OrmtCase1 = new OrmtDto();
        protected OrmtDto OrmtCase2 = new OrmtDto();
        protected OrmtDto OrmtCase3 = new OrmtDto(); 
        protected OrmtDto OrmtCase4 = new OrmtDto();
        protected Orst msgKeyForAllocated = new Orst();
        protected Orst msgKeyForCompleted = new Orst();
        protected Orst msgKeyForDeallocated = new Orst();
        protected Orst msgKeyForCanceled = new Orst();
        protected Orst msgKeyForCase5 = new Orst();
        protected EmsToWmsDto emsToWmsAllocated = new EmsToWmsDto();
        protected EmsToWmsDto emsToWmsCompleted = new EmsToWmsDto();
        protected EmsToWmsDto emsToWmsDeallocated = new EmsToWmsDto();
        protected EmsToWmsDto emsToWmsCanceled = new EmsToWmsDto();
        protected PickTicketHeaderDto pickTktHdrAllocated = new PickTicketHeaderDto();
        protected PickTicketHeaderDto pickTktHdrCompleted = new PickTicketHeaderDto();
        protected CartonHeaderDto cartonHdrAllocated = new CartonHeaderDto();
        protected CartonHeaderDto cartonHdrCompleted = new CartonHeaderDto();
        protected CartonHeaderDto cartonHdrDeallocated = new CartonHeaderDto();
        protected CartonHeaderDto cartonHdrCanceled = new CartonHeaderDto();
        protected CartonDetailDto cartonDtlCase2BeforeApi  = new CartonDetailDto();
        protected PickLocationDetailsDto pickLcnCase1 = new PickLocationDetailsDto();
        protected PickLocationDetailsDto pickLcnCase1BeforeApi = new PickLocationDetailsDto();
        protected PickLocationDetailsDto pickLcnCase2 = new PickLocationDetailsDto();
        protected PickLocationDetailsDto pickLcnCase2BeforeApi = new PickLocationDetailsDto();
        protected PickLocationDetailsDto pickLcnCase4 = new PickLocationDetailsDto();
        protected PickLocationDetailsDto pickLcnCase4BeforeApi = new PickLocationDetailsDto();                
        protected PickLocationDetailsExtenstionDto pickLcnExtCase2BeforeApi = new PickLocationDetailsExtenstionDto();
        protected PickLocationDetailsExtenstionDto pickLcnExtCase2 = new PickLocationDetailsExtenstionDto();
        protected PickLocationDetailsExtenstionDto pickLcnExtCase4BeforeApi = new PickLocationDetailsExtenstionDto();
        protected PickLocationDetailsExtenstionDto pickLcnExtCase4 = new PickLocationDetailsExtenstionDto();
        protected PickTicketDetail pickTktDtlCase2BeforeApi = new PickTicketDetail();
        protected PickTicketDetail pickTktDtlCase2AfterApi  = new PickTicketDetail();
        protected CartonDetailDto cartonDtlCase2AfterApi = new CartonDetailDto();
        protected AllocInvnDtl allocInvnDtlCompletedBeforeApi = new AllocInvnDtl();
        protected AllocInvnDtl allocInvnDtlCompletedAfterApi = new AllocInvnDtl();
         
        public OrstTestData GetCartonDetailsForInsertingOrstMessage(OracleConnection db,string cartonStatusCode, string pktStatusCode, bool completed = true)
        {
            var orstTestData = new OrstTestData();
            var sqlStatement = $"select sm.order_id,sm.sku_id,sm.qty,sm.msg_json,ch.curr_locn_id,pl.locn_id,ph.ship_w_ctrl_nbr,ch.dest_locn_id from swm_to_mhe sm " +
                $"inner join carton_hdr ch ON sm.order_id = ch.carton_nbr " +
                $"inner join pkt_hdr ph ON ph.pkt_ctrl_nbr = ch.pkt_ctrl_nbr " +
                $"inner join alloc_invn_dtl al ON al.task_cmpl_ref_nbr = ch.carton_nbr " +
                $"inner join pick_locn_dtl pl ON pl.sku_id = sm.sku_id "+
                $"inner join pkt_dtl pd ON pd.pkt_ctrl_nbr =ch.pkt_ctrl_nbr";
            if (completed == true)
            {
                sqlStatement = sqlStatement + $" where sm.source_msg_status = 'Ready' and ch.stat_code = '{cartonStatusCode}' and ph.pkt_stat_code = '{pktStatusCode}' and pd.pkt_seq_nbr > 0";
            }
            else
            {
                sqlStatement = sqlStatement + $" where sm.source_msg_status = 'Ready' and ch.stat_code < '{cartonStatusCode}' and ph.pkt_stat_code < '{pktStatusCode}' and pd.pkt_seq_nbr > 0";
            }
            command = new OracleCommand(sqlStatement, db);
            var swmToMheReader = command.ExecuteReader();
            if(swmToMheReader.Read())
            {
                orstTestData.OrderId = swmToMheReader["ORDER_ID"].ToString();
                orstTestData.SkuId = swmToMheReader["SKU_ID"].ToString();
                orstTestData.Quantity = Convert.ToInt16(swmToMheReader["QTY"].ToString());
                orstTestData.MessageJson = swmToMheReader["MSG_JSON"].ToString();
                orstTestData.CurrentLocationId = swmToMheReader["CURR_LOCN_ID"].ToString();
                orstTestData.LocnId = swmToMheReader["LOCN_ID"].ToString();
                orstTestData.ShipWCtrlNbr = swmToMheReader["SHIP_W_CTRL_NBR"].ToString();
                orstTestData.DestLocnId = swmToMheReader["DEST_LOCN_ID"].ToString();     
            }
            return orstTestData;
        }
          
        public OrstTestData GetCartonDetailsForInsertingOrstMessageForNegativeCaseWherePickTicketSeqNumberIsLessThan1(OracleConnection db, string cartonStatusCode, string pktStatusCode)
        {
            var orstTestData = new OrstTestData();
            var sqlStatement = $"select sm.order_id,sm.sku_id,sm.qty,sm.msg_json,ch.curr_locn_id,pl.locn_id,ph.ship_w_ctrl_nbr,ch.dest_locn_id from swm_to_mhe sm " +
                $"inner join carton_hdr ch ON sm.order_id = ch.carton_nbr " +
                $"inner join pkt_hdr ph ON ph.pkt_ctrl_nbr = ch.pkt_ctrl_nbr " +
                $"inner join alloc_invn_dtl al ON al.task_cmpl_ref_nbr = ch.carton_nbr " +
                $"inner join pick_locn_dtl pl ON pl.sku_id = sm.sku_id " +
                $"inner join pkt_dtl pd ON pd.pkt_ctrl_nbr =ch.pkt_ctrl_nbr"+
                $" where sm.source_msg_status = 'Ready' and ch.stat_code = '{cartonStatusCode}' and ph.pkt_stat_code < '{pktStatusCode}' and pd.pkt_seq_nbr <= 0";
                command = new OracleCommand(sqlStatement, db);
                var swmToMheReader = command.ExecuteReader();
                if (swmToMheReader.Read())
                {
                    orstTestData.OrderId = swmToMheReader["ORDER_ID"].ToString();
                    orstTestData.SkuId = swmToMheReader["SKU_ID"].ToString();
                    orstTestData.Quantity = Convert.ToInt16(swmToMheReader["QTY"].ToString());
                    orstTestData.MessageJson = swmToMheReader["MSG_JSON"].ToString();
                    orstTestData.CurrentLocationId = swmToMheReader["CURR_LOCN_ID"].ToString();
                    orstTestData.LocnId = swmToMheReader["LOCN_ID"].ToString();
                    orstTestData.ShipWCtrlNbr = swmToMheReader["SHIP_W_CTRL_NBR"].ToString();
                    orstTestData.DestLocnId = swmToMheReader["DEST_LOCN_ID"].ToString();
                }
                return orstTestData;
        }

        public void GetDataBeforeTriggerOrst()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                allocated = GetCartonDetailsForInsertingOrstMessage(db, "15","35",false);
                OrmtCase1 = JsonConvert.DeserializeObject<OrmtDto>(allocated.MessageJson);
                OrstMessageCreatedForAllocatedStatus(db);            
                emsToWmsAllocated = GetEmsToWmsData(db, msgKeyForAllocated.MsgKey);                           
            }
        }

        public void GetDataBeforeCallingApiForActionCodeComplete()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                complete = GetCartonDetailsForInsertingOrstMessage(db, "15", "35");
                OrmtCase2 = JsonConvert.DeserializeObject<OrmtDto>(complete.MessageJson);
                OrstMessageCreatedForCompletedStatus(db);
                emsToWmsCompleted = GetEmsToWmsData(db, msgKeyForCompleted.MsgKey);
                cartonDtlCase2BeforeApi = GetCartonDetails(db, complete.OrderId);
                var cartonHdr = GetCartonHeaderDetails(db, complete.OrderId);
                pickTktDtlCase2BeforeApi = GetPickTicketDetailData(db, complete.OrderId, "1");
                allocInvnDtlCompletedBeforeApi = GetAllocInvnDetails(db, complete.OrderId);
                pickLcnCase2BeforeApi = GetPickLocationDetails(db, OrmtCase2.Sku,null);
                pickLcnExtCase2BeforeApi = GetPickLocnDtlExt(db,  complete.SkuId, pickLcnCase2BeforeApi.LocationId);         
            }
        }

        public void GetDataBeforeCallingApiForActionCodeDeAllocate()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                deallocate = GetCartonDetailsForInsertingOrstMessage(db,"15","35");
                OrmtCase3 = JsonConvert.DeserializeObject<OrmtDto>(deallocate.MessageJson);
                OrstMessageCreatedForDeallocateStatus(db);
                emsToWmsDeallocated  = GetEmsToWmsData(db, msgKeyForDeallocated.MsgKey);
            }
        }

        public void GetDataBeforeCallingApiForActionCodeCancel()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                canceled = GetCartonDetailsForInsertingOrstMessage(db, "30","50");
                OrmtCase4 = JsonConvert.DeserializeObject<OrmtDto>(canceled.MessageJson);
                OrstMessageCreatedForCancelledStatus(db);
                emsToWmsCanceled = GetEmsToWmsData(db, msgKeyForCanceled.MsgKey);
                pickLcnExtCase4BeforeApi = GetPickLocnDtlExt(db, OrmtCase4.Sku, pickLcnCase4BeforeApi.LocationId);
            }
        }

        public void GetDataAfterTriggerOrstCase1()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                swmFromMheAllocated = SwmFromMhe(db, msgKeyForAllocated.MsgKey,"ORST");
                orstAllocated = JsonConvert.DeserializeObject<OrstDto>(swmFromMheAllocated.MessageJson);
                pickTktHdrAllocated = GetPickTktHeaderDetails(db, orstAllocated.OrderId);
                cartonHdrAllocated = GetCartonHeaderDetails(db, orstAllocated.OrderId);
            }
        }

        public void GetDataAfterTriggerOrstCase2()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                swmFromMheComplete = SwmFromMhe(db, msgKeyForCompleted.MsgKey, "ORST");
                orstCompleted = JsonConvert.DeserializeObject<OrstDto>(swmFromMheComplete.MessageJson);
                cartonHdrCompleted = GetCartonHeaderDetails(db, orstCompleted.OrderId);
                cartonDtlCase2AfterApi = GetCartonDetails(db,orstCompleted.OrderId);
                pickTktDtlCase2AfterApi = GetPickTicketDetailData(db, orstCompleted.OrderId, orstCompleted.OrderLineId);
                pickTktHdrCompleted = GetPickTktHeaderDetails(db, orstCompleted.OrderId);
                allocInvnDtlCompletedAfterApi = GetAllocInvnDetails(db, orstCompleted.OrderId);
                pickLcnCase2 = GetPickLocationDetails(db, orstCompleted.Sku,null);
                pickLcnExtCase2 = GetPickLocnDtlExt(db, orstCompleted.Sku,pickLcnCase1BeforeApi.LocationId);
            }
        }

        public void GetDataAfterTriggerOrstCase3()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                swmFromMheDeallocate = SwmFromMhe(db, msgKeyForDeallocated.MsgKey, "ORST");
                orstDeallocate = JsonConvert.DeserializeObject<OrstDto>(swmFromMheDeallocate.MessageJson);
            }
        }

        public void GetDataAfterTriggerOrstCase4()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                swmFromMheCancel = SwmFromMhe(db, msgKeyForCanceled.MsgKey, "ORST");
                orstCanceled = JsonConvert.DeserializeObject<OrstDto>(swmFromMheCancel.MessageJson);
                cartonHdrCanceled = GetCartonHeaderDetails(db, orstCanceled.OrderId);
                pickLcnExtCase4  = GetPickLocnDtlExt(db, orstCanceled.Sku,pickLcnCase1BeforeApi.LocationId);
            }
        }

        public void GetDataBeforeApiForActionCodeCompletedWithBitsEnabled()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                complete = GetCartonDetailsForInsertingOrstMessage(db, "15", "35");
                OrmtCase2 = JsonConvert.DeserializeObject<OrmtDto>(complete.MessageJson);
                OrstMessageCreatedForCompletedStatusWithBitsEnabled(db);
                emsToWmsCompleted = GetEmsToWmsData(db, msgKeyForCase5.MsgKey);
                var cartonHdr = GetCartonHeaderDetails(db, complete.OrderId);
                pickLcnCase2BeforeApi = GetPickLocationDetails(db, OrmtCase2.Sku, null);
                pickLcnExtCase2BeforeApi = GetPickLocnDtlExt(db, complete.SkuId, pickLcnCase2BeforeApi.LocationId);
            }
        }

        public void GetDataBeforeApiForActionCodeCompleteWithPickTicketSeqNumberLessThan1()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                complete = GetCartonDetailsForInsertingOrstMessageForNegativeCaseWherePickTicketSeqNumberIsLessThan1(db, "12", "35");
                OrmtCase2 = JsonConvert.DeserializeObject<OrmtDto>(complete.MessageJson);
                OrstMessageCreatedForCompletedStatus(db);
            }
        }
        public void GetDataAfterApiForActionCodeCompleteWithBitsEnabled()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                swmFromMheComplete = SwmFromMhe(db, msgKeyForCase5.MsgKey, "ORST");
                orstCompleted = JsonConvert.DeserializeObject<OrstDto>(swmFromMheComplete.MessageJson);
                cartonHdrCompleted = GetCartonHeaderDetails(db, orstCompleted.OrderId);              
                pickLcnCase2 = GetPickLocationDetails(db, orstCompleted.Sku, null);
                pickLcnExtCase2 = GetPickLocnDtlExt(db, orstCompleted.Sku, pickLcnCase1BeforeApi.LocationId);
            }
        }
        public string CreateOrstMessage(string actionCode,string containerId,string skuId,string qty,string waveNbr,string orderReasonCodeMap,string owner, string currentLocationId,string destinationLocnId)
        {
            OrstParameters = new OrstDto
            {
                TransactionCode = TransactionCode.Orst,
                MessageLength = MessageLength.Orst,
                ActionCode = actionCode,
                OrderId = containerId,
                OrderLineId = "1",
                CompletionTime = "20190909121212",
                Sku = skuId,
                Owner = owner,
                UnitOfMeasure = "Case",
                ParentContainerId = containerId,
                QuantityOrdered = qty,
                QuantityDelivered = "1",
                DestinationLocationId = destinationLocnId,
                CurrentLocationId = currentLocationId,
                Priority = "",
                OrderReasonCodeMap = orderReasonCodeMap,
                WaveId = waveNbr
            };
            var buildMessage = new Interfaces.Builder.MessageBuilder.MessageHeaderBuilder();
            var testResult = buildMessage.BuildMessage<OrstDto, Interfaces.ParserAndTranslator.Contracts.Validation.OrstValidationRule>(OrstParameters, TransactionCode.Orst);
            Assert.AreEqual(testResult.ResultType, ResultTypes.Ok);
            Assert.IsNotNull(testResult.Payload);
            return testResult.Payload;
        }

        public void OrstMessageCreatedForAllocatedStatus(OracleConnection db)
        {
            var orstmsg = CreateOrstMessage("Allocated",allocated.OrderId,allocated.SkuId, OrmtCase1.Quantity, OrmtCase1.WaveId,"0", OrmtCase1.Owner,null,allocated.DestLocnId);
            var emsToWms = new EmsToWmsDto
            {
                Process = DefaultValues.Process,
                Status = DefaultValues.Status,
                Transaction = TransactionCode.Orst,
                ResponseCode = (short)int.Parse(ReasonCode.Success),
                MessageText = orstmsg
            };
            msgKeyForAllocated.MsgKey = InsertEmsToWMS(db, emsToWms);
        }

        public void OrstMessageCreatedForCompletedStatus(OracleConnection db)
        {
            var orstmsg = CreateOrstMessage("Complete", complete.OrderId, complete.SkuId, OrmtCase2.Quantity, OrmtCase2.WaveId,"0",OrmtCase2.Owner,null,complete.DestLocnId);
            var emsToWms = new EmsToWmsDto
            {
                Process = DefaultValues.Process,
                Status = DefaultValues.Status,
                Transaction = TransactionCode.Orst,
                ResponseCode = (short)int.Parse(ReasonCode.Success),
                MessageText = orstmsg
            };
            msgKeyForCompleted.MsgKey = InsertEmsToWMS(db, emsToWms);
        }

        public void OrstMessageCreatedForCompletedStatusWithBitsEnabled(OracleConnection db)
        {
            var orstmsg = CreateOrstMessage("Complete", complete.OrderId, complete.SkuId, OrmtCase2.Quantity, OrmtCase2.WaveId, "01110605", OrmtCase2.Owner, "12890", complete.DestLocnId);
            var emsToWms = new EmsToWmsDto
            {
                Process = DefaultValues.Process,
                Status = DefaultValues.Status,
                Transaction = TransactionCode.Orst,
                ResponseCode = (short)int.Parse(ReasonCode.Success),
                MessageText = orstmsg
            };
            msgKeyForCase5.MsgKey = InsertEmsToWMS(db, emsToWms);
        }


        public void OrstMessageCreatedForDeallocateStatus(OracleConnection db)
        {
            var orstmsg = CreateOrstMessage("Deallocate", deallocate.OrderId, deallocate.SkuId, OrmtCase3.Quantity, OrmtCase3.WaveId,"05",null,null,deallocate.DestLocnId);
            var emsToWms = new EmsToWmsDto
            {
                Process = DefaultValues.Process,
                Status = DefaultValues.Status,
                Transaction = TransactionCode.Orst,
                ResponseCode = (short)int.Parse(ReasonCode.Success),
                MessageText = orstmsg
            };
            msgKeyForDeallocated.MsgKey = InsertEmsToWMS(db, emsToWms);
        }

        public void OrstMessageCreatedForCancelledStatus(OracleConnection db)
        {
            var orstmsg = CreateOrstMessage("Canceled", canceled.OrderId, canceled.SkuId, OrmtCase4.Quantity, OrmtCase4.WaveId,"06",null,null,canceled.DestLocnId);
            var emsToWms = new EmsToWmsDto
            {
                Process = DefaultValues.Process,
                Status = DefaultValues.Status,
                Transaction = TransactionCode.Orst,
                ResponseCode = (short)int.Parse(ReasonCode.Success),
                MessageText = orstmsg
            };
            msgKeyForCanceled.MsgKey = InsertEmsToWMS(db, emsToWms);
        }

  
        public CartonHeaderDto GetCartonHeaderDetails(OracleConnection db,string cartonNumber)
        {
            var cartonHeader = new CartonHeaderDto();
            var query = $"select CURR_LOCN_ID, DEST_LOCN_ID,MOD_DATE_TIME, USER_ID, STAT_CODE from CARTON_HDR where CARTON_NBR='{cartonNumber}'";
            command = new OracleCommand(query,db);
            var cartonHeaderReader = command.ExecuteReader();
            if (cartonHeaderReader.Read())
            {
                cartonHeader.StatusCode = Convert.ToInt16(cartonHeaderReader[TestData.CartonHeader.StatCode].ToString());     
                cartonHeader.CurrentLocationId = cartonHeaderReader[TestData.CartonHeader.CurrentLocationId].ToString();
            }
            return cartonHeader;
        }

        public CartonDetailDto GetCartonDetails(OracleConnection db, string cartonNumber)
        {
            var cartonDtl = new CartonDetailDto();
            var query = $"select * from carton_dtl where carton_nbr = '{cartonNumber}'";
            command = new OracleCommand(query, db);
            var cartonReader = command.ExecuteReader();
            if(cartonReader.Read())
            {
                cartonDtl.CartonNumber = cartonReader[TestData.CartonDetail.CartonNumber].ToString();
                cartonDtl.ToBePackedUnits = Convert.ToDecimal(cartonReader[TestData.CartonDetail.ToBePackdUnits].ToString());
                cartonDtl.UnitsPacked = Convert.ToDecimal(cartonReader[TestData.CartonDetail.UnitsPakd].ToString());
            }
            return cartonDtl;
        }

        public PickTicketHeaderDto GetPickTktHeaderDetails(OracleConnection db,string cartonNumber)
        {
            var pkTktHeader = new PickTicketHeaderDto();
            var query = $"select PKT_HDR.PKT_STAT_CODE,PKT_HDR.MOD_DATE_TIME,PKT_HDR.USER_ID from PKT_HDR WHERE PKT_CTRL_NBR = (SELECT PKT_CTRL_NBR FROM CARTON_HDR WHERE CARTON_NBR = '{cartonNumber}')";
            command = new OracleCommand(query,db);
            var pickTktHeaderReader = command.ExecuteReader();
            if (pickTktHeaderReader.Read())
            {        
                pkTktHeader.PickTicketStatusCode = Convert.ToInt16(pickTktHeaderReader[TestData.PickTicketHeader.PktStatCode].ToString());
            }
            return pkTktHeader;
        }

        
        public PickTicketDetail GetPickTicketDetailData(OracleConnection db,string cartonNbr,string pktSeqNbr)
        {
            var pickTktDtl = new PickTicketDetail();
            var query = $"select PKT_DTL.UNITS_PAKD,PKT_DTL.MOD_DATE_TIME,PKT_DTL.USER_ID ,PKT_DTL.VERF_AS_PAKD,PKT_DTL.PKT_CTRL_NBR from PKT_DTL where PKT_DTL.PKT_CTRL_NBR = (SELECT PKT_CTRL_NBR FROM CARTON_HDR WHERE CARTON_NBR= '{cartonNbr}') AND PKT_SEQ_NBR= '{pktSeqNbr}'";
            command = new OracleCommand(query, db);
            var pickTktDtlReader = command.ExecuteReader();
            if(pickTktDtlReader.Read())
            {
               pickTktDtl.UnitsPacked = Convert.ToDecimal(pickTktDtlReader[TestData.PickTicketDetail.UnitsPacked].ToString());            
               pickTktDtl.VerifiedAsPacked = Convert.ToDecimal(pickTktDtlReader[TestData.PickTicketDetail.VerfAsPakd].ToString());             
            }
            return pickTktDtl;
        }

        public AllocInvnDtl GetAllocInvnDetails(OracleConnection db,string CntrNbr)
        {
            var allocInvnDtl = new AllocInvnDtl();
            var query = $"select * from ALLOC_INVN_DTL WHERE task_cmpl_ref_nbr='{CntrNbr}' order by mod_date_time desc";
            command = new OracleCommand(query,db);
            var allocInvnDtlReader = command.ExecuteReader();
            if(allocInvnDtlReader.Read())
            {
                allocInvnDtl.QtyPulled = allocInvnDtlReader[AllocInvnDetail.QtyPulled].ToString();
                allocInvnDtl.StatCode = allocInvnDtlReader[AllocInvnDetail.StatCode].ToString();
                allocInvnDtl.UserId = allocInvnDtlReader[AllocInvnDetail.UserId].ToString();
            }
            return allocInvnDtl;
        }

        public EmsToWmsDto GetEmsToWmsData(OracleConnection db,long msgKey)
        {
            var EmsToWms = new EmsToWmsDto();
            var query = $"select * from emstowms where msgKey ={msgKey}";
            command = new OracleCommand(query,db);
            var EmsToWmsReader = command.ExecuteReader();
            if(EmsToWmsReader.Read())
            {
                EmsToWms.MessageKey = Convert.ToInt64(EmsToWmsReader[TestData.EmsToWms.MsgKey].ToString());
                EmsToWms.MessageText = EmsToWmsReader[TestData.EmsToWms.MsgTxt].ToString();
                EmsToWms.ResponseCode = Convert.ToInt16(EmsToWmsReader[TestData.EmsToWms.ReasonCode].ToString());
                EmsToWms.Process = EmsToWmsReader[TestData.EmsToWms.prc].ToString();
                EmsToWms.Status = EmsToWmsReader[TestData.EmsToWms.Status].ToString();
                EmsToWms.Transaction = EmsToWmsReader[TestData.EmsToWms.Trx].ToString();
            }
            return EmsToWms;
        }
    }
}
