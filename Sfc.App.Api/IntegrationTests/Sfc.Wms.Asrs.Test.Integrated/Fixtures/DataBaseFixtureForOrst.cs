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
using Sfc.Wms.Foundation.Message.Contracts.Dtos;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures
{
   
    public class DataBaseFixtureForOrst : DataBaseFixtureForOrmt
    {
        protected OrstTestData Allocated = new OrstTestData();
        protected OrstTestData Complete = new OrstTestData();
        protected OrstTestData Deallocate = new OrstTestData();
        protected OrstTestData Canceled = new OrstTestData();
        protected SwmFromMheDto SwmFromMheAllocated = new SwmFromMheDto();
        protected SwmFromMheDto SwmFromMheComplete = new SwmFromMheDto();
        protected SwmFromMheDto SwmFromMheDeallocate = new SwmFromMheDto();
        protected SwmFromMheDto SwmFromMheCancel = new SwmFromMheDto();
        protected OrstDto OrstParameters;
        protected OrstDto OrstAllocated = new OrstDto();
        protected OrstDto OrstCompleted = new OrstDto();
        protected OrstDto OrstDeallocate = new OrstDto();
        protected OrstDto OrstCanceled = new OrstDto();
        protected OrmtDto OrmtCase1 = new OrmtDto();
        protected OrmtDto OrmtCase2 = new OrmtDto();
        protected OrmtDto OrmtCase3 = new OrmtDto(); 
        protected OrmtDto OrmtCase4 = new OrmtDto();
        protected Orst MsgKeyForAllocated = new Orst();
        protected Orst MsgKeyForCompleted = new Orst();
        protected Orst MsgKeyForDeallocated = new Orst();
        protected Orst MsgKeyForCanceled = new Orst();
        protected Orst MsgKeysForCase5 = new Orst();
        protected EmsToWmsDto EmsToWmsAllocated = new EmsToWmsDto();
        protected EmsToWmsDto EmsToWmsCompleted = new EmsToWmsDto();
        protected MessageToSortViewDto MessageToSort = new MessageToSortViewDto();
        protected EmsToWmsDto EmsToWmsDeallocated = new EmsToWmsDto();
        protected EmsToWmsDto EmsToWmsCanceled = new EmsToWmsDto();
        protected PickTicketHeaderDto PickTktHdrAllocated = new PickTicketHeaderDto();
        protected PickTicketHeaderDto PickTktHdrCompleted = new PickTicketHeaderDto();
        protected CartonHeaderDto CartonHdrAllocated = new CartonHeaderDto();
        protected CartonHeaderDto CartonHdrCompleted = new CartonHeaderDto();
        protected CartonHeaderDto CartonHdrDeallocated = new CartonHeaderDto();
        protected CartonHeaderDto CartonHdrCanceled = new CartonHeaderDto();
        protected CartonDetailDto CartonDtlCase2BeforeApi  = new CartonDetailDto();
        protected PickLocationDetailsDto PickLcnCase1 = new PickLocationDetailsDto();
        protected PickLocationDetailsDto PickLcnCase1BeforeApi = new PickLocationDetailsDto();
        protected PickLocationDetailsDto PickLcnCase2 = new PickLocationDetailsDto();
        protected PickLocationDetailsDto PickLcnCase2BeforeApi = new PickLocationDetailsDto();
        protected PickLocationDetailsDto PickLcnCase4 = new PickLocationDetailsDto();
        protected PickLocationDetailsDto PickLcnCase4BeforeApi = new PickLocationDetailsDto();                
        protected PickLocationDetailsExtenstionDto PickLcnExtCase2BeforeApi = new PickLocationDetailsExtenstionDto();
        protected PickLocationDetailsExtenstionDto PickLcnExtCase2 = new PickLocationDetailsExtenstionDto();
        protected PickLocationDetailsExtenstionDto PickLcnExtCase4BeforeApi = new PickLocationDetailsExtenstionDto();
        protected PickLocationDetailsExtenstionDto PickLcnExtCase4 = new PickLocationDetailsExtenstionDto();
        protected PickTicketDetail PickTktDtlCase2BeforeApi = new PickTicketDetail();
        protected PickTicketDetail PickTktDtlCase2AfterApi  = new PickTicketDetail();
        protected CartonDetailDto CartonDtlCase2AfterApi = new CartonDetailDto();
        protected AllocInvnDtl AllocInvnDtlCompletedBeforeApi = new AllocInvnDtl();
        protected AllocInvnDtl AllocInvnDtlCompletedAfterApi = new AllocInvnDtl();
         
        public OrstTestData GetCartonDetailsForInsertingOrstMessage(OracleConnection db,string cartonStatusCode, string pktStatusCode, bool completed = true)
        {
            var orstTestData = new OrstTestData();
            var sqlStatement = $"select sm.order_id,sm.sku_id,sm.qty,sm.msg_json,ch.curr_locn_id,pl.locn_id,ph.ship_w_ctrl_nbr,ch.dest_locn_id from swm_to_mhe sm " +
                $"inner join carton_hdr ch ON sm.order_id = ch.carton_nbr " +
                $"inner join pkt_hdr ph ON ph.pkt_ctrl_nbr = ch.pkt_ctrl_nbr " +
                $"inner join alloc_invn_dtl al ON al.task_cmpl_ref_nbr = ch.carton_nbr " +
                $"inner join pick_locn_dtl pl ON pl.sku_id = sm.sku_id "+
                $"inner join pkt_dtl pd ON pd.pkt_ctrl_nbr =ch.pkt_ctrl_nbr";
            if (completed)
            {
                sqlStatement = sqlStatement + $" where sm.source_msg_status = 'Ready' and ch.stat_code = '{cartonStatusCode}' and ph.pkt_stat_code = '{pktStatusCode}' and pd.pkt_seq_nbr > 0";
            }
            else
            {
                sqlStatement = sqlStatement + $" where sm.source_msg_status = 'Ready' and ch.stat_code < '{cartonStatusCode}' and ph.pkt_stat_code < '{pktStatusCode}' and pd.pkt_seq_nbr > 0";
            }
            Command = new OracleCommand(sqlStatement, db);
            var swmToMheReader = Command.ExecuteReader();
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
                Command = new OracleCommand(sqlStatement, db);
                var swmToMheReader = Command.ExecuteReader();
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
                Allocated = GetCartonDetailsForInsertingOrstMessage(db, "15","35",false);
                OrmtCase1 = JsonConvert.DeserializeObject<OrmtDto>(Allocated.MessageJson);
                OrstMessageCreatedForAllocatedStatus(db);            
                EmsToWmsAllocated = GetEmsToWmsData(db, MsgKeyForAllocated.MsgKey);                           
            }
        }

        public void GetDataBeforeCallingApiForActionCodeComplete()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                Complete = GetCartonDetailsForInsertingOrstMessage(db, "15", "35");
                OrmtCase2 = JsonConvert.DeserializeObject<OrmtDto>(Complete.MessageJson);
                OrstMessageCreatedForCompletedStatus(db);
                EmsToWmsCompleted = GetEmsToWmsData(db, MsgKeyForCompleted.MsgKey);
                CartonDtlCase2BeforeApi = GetCartonDetails(db, Complete.OrderId);
                GetCartonHeaderDetails(db, Complete.OrderId);
                PickTktDtlCase2BeforeApi = GetPickTicketDetailData(db, Complete.OrderId, "1");
                AllocInvnDtlCompletedBeforeApi = GetAllocInvnDetails(db, Complete.OrderId);
                PickLcnCase2BeforeApi = GetPickLocationDetails(db, OrmtCase2.Sku,null);
                PickLcnExtCase2BeforeApi = GetPickLocnDtlExt(db,  Complete.SkuId, PickLcnCase2BeforeApi.LocationId);         
            }
        }

        public void GetDataBeforeCallingApiForActionCodeDeAllocate()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                Deallocate = GetCartonDetailsForInsertingOrstMessage(db,"15","35");
                OrmtCase3 = JsonConvert.DeserializeObject<OrmtDto>(Deallocate.MessageJson);
                OrstMessageCreatedForDeallocateStatus(db);
                EmsToWmsDeallocated  = GetEmsToWmsData(db, MsgKeyForDeallocated.MsgKey);
            }
        }

        public void GetDataBeforeCallingApiForActionCodeCancel()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                Canceled = GetCartonDetailsForInsertingOrstMessage(db, "30","50");
                OrmtCase4 = JsonConvert.DeserializeObject<OrmtDto>(Canceled.MessageJson);
                OrstMessageCreatedForCancelledStatus(db);
                EmsToWmsCanceled = GetEmsToWmsData(db, MsgKeyForCanceled.MsgKey);
                PickLcnExtCase4BeforeApi = GetPickLocnDtlExt(db, OrmtCase4.Sku, PickLcnCase4BeforeApi.LocationId);
            }
        }

        public void GetDataAfterTriggerOrstCase1()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                SwmFromMheAllocated = SwmFromMhe(db, MsgKeyForAllocated.MsgKey,"ORST");
                OrstAllocated = JsonConvert.DeserializeObject<OrstDto>(SwmFromMheAllocated.MessageJson);
                PickTktHdrAllocated = GetPickTktHeaderDetails(db, OrstAllocated.OrderId);
                CartonHdrAllocated = GetCartonHeaderDetails(db, OrstAllocated.OrderId);
            }
        }

        public void GetDataAfterTriggerOrstCase2()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                SwmFromMheComplete = SwmFromMhe(db, MsgKeyForCompleted.MsgKey, "ORST");
                OrstCompleted = JsonConvert.DeserializeObject<OrstDto>(SwmFromMheComplete.MessageJson);
                CartonHdrCompleted = GetCartonHeaderDetails(db, OrstCompleted.OrderId);
                CartonDtlCase2AfterApi = GetCartonDetails(db,OrstCompleted.OrderId);
                PickTktDtlCase2AfterApi = GetPickTicketDetailData(db, OrstCompleted.OrderId, OrstCompleted.OrderLineId);
                PickTktHdrCompleted = GetPickTktHeaderDetails(db, OrstCompleted.OrderId);
                AllocInvnDtlCompletedAfterApi = GetAllocInvnDetails(db, OrstCompleted.OrderId);
                PickLcnCase2 = GetPickLocationDetails(db, OrstCompleted.Sku,null);
                PickLcnExtCase2 = GetPickLocnDtlExt(db, OrstCompleted.Sku,PickLcnCase1BeforeApi.LocationId);
            }
        }

        public void GetDataAfterTriggerOrstCase3()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                SwmFromMheDeallocate = SwmFromMhe(db, MsgKeyForDeallocated.MsgKey, "ORST");
                OrstDeallocate = JsonConvert.DeserializeObject<OrstDto>(SwmFromMheDeallocate.MessageJson);
            }
        }

        public void GetDataAfterTriggerOrstCase4()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                SwmFromMheCancel = SwmFromMhe(db, MsgKeyForCanceled.MsgKey, "ORST");
                OrstCanceled = JsonConvert.DeserializeObject<OrstDto>(SwmFromMheCancel.MessageJson);
                CartonHdrCanceled = GetCartonHeaderDetails(db, OrstCanceled.OrderId);
                PickLcnExtCase4  = GetPickLocnDtlExt(db, OrstCanceled.Sku,PickLcnCase1BeforeApi.LocationId);
                MessageToSort = GetMsgTosvDetail(db, CancelOrder.CartonNbr);
            }
        }

        public void GetDataBeforeApiForActionCodeCompletedWithBitsEnabled()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                Complete = GetCartonDetailsForInsertingOrstMessage(db, "15", "35");
                OrmtCase2 = JsonConvert.DeserializeObject<OrmtDto>(Complete.MessageJson);
                OrstMessageCreatedForCompletedStatusWithBitsEnabled(db);
                EmsToWmsCompleted = GetEmsToWmsData(db, MsgKeysForCase5.MsgKey);
                GetCartonHeaderDetails(db, Complete.OrderId);
                PickLcnCase2BeforeApi = GetPickLocationDetails(db, OrmtCase2.Sku, null);
                PickLcnExtCase2BeforeApi = GetPickLocnDtlExt(db, Complete.SkuId, PickLcnCase2BeforeApi.LocationId);
            }
        }

        public void GetDataBeforeApiForActionCodeCompleteWithPickTicketSeqNumberLessThan1()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                Complete = GetCartonDetailsForInsertingOrstMessageForNegativeCaseWherePickTicketSeqNumberIsLessThan1(db, "12", "35");
                OrmtCase2 = JsonConvert.DeserializeObject<OrmtDto>(Complete.MessageJson);
                OrstMessageCreatedForCompletedStatus(db);
            }
        }
        public void GetDataAfterApiForActionCodeCompleteWithBitsEnabled()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                SwmFromMheComplete = SwmFromMhe(db, MsgKeysForCase5.MsgKey, "ORST");
                OrstCompleted = JsonConvert.DeserializeObject<OrstDto>(SwmFromMheComplete.MessageJson);
                CartonHdrCompleted = GetCartonHeaderDetails(db, OrstCompleted.OrderId);              
                PickLcnCase2 = GetPickLocationDetails(db, OrstCompleted.Sku, null);
                PickLcnExtCase2 = GetPickLocnDtlExt(db, OrstCompleted.Sku, PickLcnCase1BeforeApi.LocationId);
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
            var orstmsg = CreateOrstMessage("Allocated",Allocated.OrderId,Allocated.SkuId, OrmtCase1.Quantity, OrmtCase1.WaveId,"0", OrmtCase1.Owner,null,Allocated.DestLocnId);
            var emsToWms = new EmsToWmsDto
            {
                Process = DefaultPossibleValue.MessageProcessor,
                Status = DefaultValues.Status,
                Transaction = TransactionCode.Orst,
                ResponseCode = (short)int.Parse(ReasonCode.Success),
                MessageText = orstmsg
            };
            MsgKeyForAllocated.MsgKey = InsertEmsToWms(db, emsToWms);
        }

        public MessageToSortViewDto GetMsgTosvDetail(OracleConnection db, string cartonNo)
        {
            var messageTosv = new MessageToSortViewDto();
            var query = $"select * from msg_to_sv where message_type='USL' and PTN='{cartonNo}'";
            Command = new OracleCommand(query, db); var cartonHeaderReader = Command.ExecuteReader();
            if (cartonHeaderReader.Read()) { messageTosv.Ptn = (cartonHeaderReader[MessageToSv.Ptn].ToString());
            }
            return messageTosv;
        }

        public void OrstMessageCreatedForCompletedStatus(OracleConnection db)
        {
            var orstmsg = CreateOrstMessage("Complete", Complete.OrderId, Complete.SkuId, OrmtCase2.Quantity, OrmtCase2.WaveId,"0",OrmtCase2.Owner,null,Complete.DestLocnId);
            var emsToWms = new EmsToWmsDto
            {
                Process = DefaultPossibleValue.MessageProcessor,
                Status = DefaultValues.Status,
                Transaction = TransactionCode.Orst,
                ResponseCode = (short)int.Parse(ReasonCode.Success),
                MessageText = orstmsg
            };
            MsgKeyForCompleted.MsgKey = InsertEmsToWms(db, emsToWms);
        }

        public void OrstMessageCreatedForCompletedStatusWithBitsEnabled(OracleConnection db)
        {
            var orstmsg = CreateOrstMessage("Complete", Complete.OrderId, Complete.SkuId, OrmtCase2.Quantity, OrmtCase2.WaveId, "01110605", OrmtCase2.Owner, "12890", Complete.DestLocnId);
            var emsToWms = new EmsToWmsDto
            {
                Process = DefaultPossibleValue.MessageProcessor,
                Status = DefaultValues.Status,
                Transaction = TransactionCode.Orst,
                ResponseCode = (short)int.Parse(ReasonCode.Success),
                MessageText = orstmsg
            };
            MsgKeysForCase5.MsgKey = InsertEmsToWms(db, emsToWms);
        }


        public void OrstMessageCreatedForDeallocateStatus(OracleConnection db)
        {
            var orstmsg = CreateOrstMessage("Deallocate", Deallocate.OrderId, Deallocate.SkuId, OrmtCase3.Quantity, OrmtCase3.WaveId,"05",null,null,Deallocate.DestLocnId);
            var emsToWms = new EmsToWmsDto
            {
                Process = DefaultPossibleValue.MessageProcessor,
                Status = DefaultValues.Status,
                Transaction = TransactionCode.Orst,
                ResponseCode = (short)int.Parse(ReasonCode.Success),
                MessageText = orstmsg
            };
            MsgKeyForDeallocated.MsgKey = InsertEmsToWms(db, emsToWms);
        }

        public void OrstMessageCreatedForCancelledStatus(OracleConnection db)
        {
            var orstmsg = CreateOrstMessage("Canceled", Canceled.OrderId, Canceled.SkuId, OrmtCase4.Quantity, OrmtCase4.WaveId,"06",null,null,Canceled.DestLocnId);
            var emsToWms = new EmsToWmsDto
            {
                Process = DefaultPossibleValue.MessageProcessor,
                Status = DefaultValues.Status,
                Transaction = TransactionCode.Orst,
                ResponseCode = (short)int.Parse(ReasonCode.Success),
                MessageText = orstmsg
            };
            MsgKeyForCanceled.MsgKey = InsertEmsToWms(db, emsToWms);
        }

  
        public CartonHeaderDto GetCartonHeaderDetails(OracleConnection db,string cartonNumber)
        {
            var cartonHeader = new CartonHeaderDto();
            var query = $"select CURR_LOCN_ID, DEST_LOCN_ID,MOD_DATE_TIME, USER_ID, STAT_CODE from CARTON_HDR where CARTON_NBR='{cartonNumber}'";
            Command = new OracleCommand(query,db);
            var cartonHeaderReader = Command.ExecuteReader();
            if (cartonHeaderReader.Read())
            {
                cartonHeader.StatusCode = Convert.ToInt16(cartonHeaderReader[CartonHeader.StatCode].ToString());     
                cartonHeader.CurrentLocationId = cartonHeaderReader[CartonHeader.CurrentLocationId].ToString();
            }
            return cartonHeader;
        }

        public CartonDetailDto GetCartonDetails(OracleConnection db, string cartonNumber)
        {
            var cartonDtl = new CartonDetailDto();
            var query = $"select * from carton_dtl where carton_nbr = '{cartonNumber}'";
            Command = new OracleCommand(query, db);
            var cartonReader = Command.ExecuteReader();
            if(cartonReader.Read())
            {
                cartonDtl.CartonNumber = cartonReader[CartonDetail.CartonNumber].ToString();
                cartonDtl.ToBePackedUnits = Convert.ToDecimal(cartonReader[CartonDetail.ToBePackdUnits].ToString());
                cartonDtl.UnitsPacked = Convert.ToDecimal(cartonReader[CartonDetail.UnitsPakd].ToString());
            }
            return cartonDtl;
        }

        public PickTicketHeaderDto GetPickTktHeaderDetails(OracleConnection db,string cartonNumber)
        {
            var pkTktHeader = new PickTicketHeaderDto();
            var query = $"select PKT_HDR.PKT_STAT_CODE,PKT_HDR.MOD_DATE_TIME,PKT_HDR.USER_ID from PKT_HDR WHERE PKT_CTRL_NBR = (SELECT PKT_CTRL_NBR FROM CARTON_HDR WHERE CARTON_NBR = '{cartonNumber}')";
            Command = new OracleCommand(query,db);
            var pickTktHeaderReader = Command.ExecuteReader();
            if (pickTktHeaderReader.Read())
            {        
                pkTktHeader.PickTicketStatusCode = Convert.ToInt16(pickTktHeaderReader[PickTicketHeader.PktStatCode].ToString());
            }
            return pkTktHeader;
        }
       
        public PickTicketDetail GetPickTicketDetailData(OracleConnection db,string cartonNbr,string pktSeqNbr)
        {
            var pickTktDtl = new PickTicketDetail();
            var query = $"select PKT_DTL.UNITS_PAKD,PKT_DTL.MOD_DATE_TIME,PKT_DTL.USER_ID ,PKT_DTL.VERF_AS_PAKD,PKT_DTL.PKT_CTRL_NBR from PKT_DTL where PKT_DTL.PKT_CTRL_NBR = (SELECT PKT_CTRL_NBR FROM CARTON_HDR WHERE CARTON_NBR= '{cartonNbr}') AND PKT_SEQ_NBR= '{pktSeqNbr}'";
            Command = new OracleCommand(query, db);
            var pickTktDtlReader = Command.ExecuteReader();
            if(pickTktDtlReader.Read())
            {
               pickTktDtl.UnitsPacked = Convert.ToDecimal(pickTktDtlReader[TestData.PickTicketDetail.UnitsPacked].ToString());            
               pickTktDtl.VerifiedAsPacked = Convert.ToDecimal(pickTktDtlReader[TestData.PickTicketDetail.VerfAsPakd].ToString());             
            }
            return pickTktDtl;
        }

        public AllocInvnDtl GetAllocInvnDetails(OracleConnection db,string cntrNbr)
        {
            var allocInvnDtl = new AllocInvnDtl();
            var query = $"select * from ALLOC_INVN_DTL WHERE task_cmpl_ref_nbr='{cntrNbr}' order by mod_date_time desc";
            Command = new OracleCommand(query,db);
            var allocInvnDtlReader = Command.ExecuteReader();
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
            var emsToWms = new EmsToWmsDto();
            var query = $"select * from emstowms where msgKey ={msgKey}";
            Command = new OracleCommand(query,db);
            var emsToWmsReader = Command.ExecuteReader();
            if(emsToWmsReader.Read())
            {
                emsToWms.MessageKey = Convert.ToInt64(emsToWmsReader[EmsToWms.MsgKey].ToString());
                emsToWms.MessageText = emsToWmsReader[EmsToWms.MsgTxt].ToString();
                emsToWms.ResponseCode = Convert.ToInt16(emsToWmsReader[EmsToWms.ReasonCode].ToString());
                emsToWms.Process = emsToWmsReader[EmsToWms.Prc].ToString();
                emsToWms.Status = emsToWmsReader[EmsToWms.Status].ToString();
                emsToWms.Transaction = emsToWmsReader[EmsToWms.Trx].ToString();
            }
            return emsToWms;
        }
    }
}
