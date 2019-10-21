using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData;
using Sfc.Wms.Asrs.Dematic.Contracts.Dtos;
using Sfc.Wms.Asrs.Shamrock.Contracts.Dtos;
using Sfc.Wms.Interfaces.ParserAndTranslator.Contracts.Constants;
using Sfc.Wms.Interfaces.ParserAndTranslator.Contracts.Dto;
using Sfc.Wms.Foundation.Location.Contracts.Dtos;
using Sfc.Core.OnPrem.Result;
using System;
using Sfc.Wms.ParserAndTranslator.Contracts.Interfaces;
using Sfc.Wms.Foundation.Carton.Contracts.Dtos;
using PickTicketDetail = Sfc.Wms.Data.Entities.PickTicketDetail;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures
{
    /*Case1,Case2,Case3,Case4 are defined in story*/

    public class DataBaseFixtureForOrst : DataBaseFixtureForOrmt
    {
        protected OrstTestData case1 = new OrstTestData();
        protected OrstTestData case2 = new OrstTestData();
        protected OrstTestData case3 = new OrstTestData();
        protected OrstTestData case4 = new OrstTestData();
        protected SwmFromMheDto swmFromMheCase1 = new SwmFromMheDto();
        protected SwmFromMheDto swmFromMheCase2 = new SwmFromMheDto();
        protected SwmFromMheDto swmFromMheCase3 = new SwmFromMheDto();
        protected SwmFromMheDto swmFromMheCase4 = new SwmFromMheDto();
        protected Interfaces.ParserAndTranslator.Contracts.Dto.OrstDto OrstParameters;
        protected OrstDto orstCase1 = new OrstDto();
        protected OrstDto orstCase2 = new OrstDto();
        protected OrstDto orstCase3 = new OrstDto();
        protected OrstDto orstCase4 = new OrstDto();
        private readonly IHaveDataTypeValidation _dataTypeValidation;
        protected OrmtDto OrmtCase1 = new OrmtDto();
        protected OrmtDto OrmtCase2 = new OrmtDto();
        protected OrmtDto OrmtCase3 = new OrmtDto(); 
        protected OrmtDto OrmtCase4 = new OrmtDto();
        protected Orst msgKeyForCase1 = new Orst();
        protected Orst msgKeyForCase2 = new Orst();
        protected Orst msgKeyForCase3 = new Orst();
        protected Orst msgKeyForCase4 = new Orst();
        protected EmsToWmsDto emsToWmsCase1 = new EmsToWmsDto();
        protected EmsToWmsDto emsToWmsCase2 = new EmsToWmsDto();
        protected EmsToWmsDto emsToWmsCase3 = new EmsToWmsDto();
        protected EmsToWmsDto emsToWmsCase4 = new EmsToWmsDto();
        protected PickTicketHeaderDto pickTktHdrCase1 = new PickTicketHeaderDto();
        protected PickTicketHeaderDto pickTktHdrCase2 = new PickTicketHeaderDto();
        protected CartonHeaderDto cartonHdrCase1 = new CartonHeaderDto();
        protected CartonHeaderDto cartonHdrCase2 = new CartonHeaderDto();
        protected CartonHeaderDto cartonHdrCase3 = new CartonHeaderDto();
        protected CartonHeaderDto cartonHdrCase4 = new CartonHeaderDto();
        protected CartonDetailDto cartonDtlCase2BeforeApi  = new CartonDetailDto();
        protected PickLocationDetailsDto pickLcnCase1 = new PickLocationDetailsDto();
        protected PickLocationDetailsDto pickLcnCase1BeforeApi = new PickLocationDetailsDto();
        protected PickLocationDetailsDto pickLcnCase2 = new PickLocationDetailsDto();
        protected PickLocationDetailsDto pickLcnCase2BeforeApi = new PickLocationDetailsDto();
        protected PickLocationDetailsDto pickLcnCase4 = new PickLocationDetailsDto();
        protected PickLocationDetailsDto pickLcnCase4BeforeApi = new PickLocationDetailsDto(); 
        protected PickLocationDetailsExtenstionDto pickLcnExtCase1 = new PickLocationDetailsExtenstionDto();    
        protected PickLocationDetailsExtenstionDto pickLcnExtCase1BeforeApi = new PickLocationDetailsExtenstionDto();
        protected PickLocationDetailsExtenstionDto pickLcnExtCase2BeforeApi = new PickLocationDetailsExtenstionDto();
        protected PickLocationDetailsExtenstionDto pickLcnExtCase2 = new PickLocationDetailsExtenstionDto();
        protected PickLocationDetailsExtenstionDto pickLcnExtCase4BeforeApi = new PickLocationDetailsExtenstionDto();
        protected PickLocationDetailsExtenstionDto pickLcnExtCase4 = new PickLocationDetailsExtenstionDto();
        protected PickTicketDetail pickTktDtlCase2BeforeApi = new PickTicketDetail();
        protected PickTicketDetail pickTktDtlCase2AfterApi  = new PickTicketDetail();
        protected CartonDetailDto cartonDtlCase2AfterApi = new CartonDetailDto();
        protected AllocInvnDtl allocInvnDtlCase2BeforeApi = new AllocInvnDtl();
        protected AllocInvnDtl allocInvnDtlCase2AfterApi = new AllocInvnDtl();
         


        public OrstTestData GetCartonDetailsForInsertingOrstMessage(OracleConnection db,int statusCode)
        {
            var orstTestData = new OrstTestData();
            var sqlStatement = $"select sm.order_id,sm.sku_id,sm.qty,sm.msg_json,ch.curr_locn_id,pl.locn_id,ph.ship_w_ctrl_nbr,ch.dest_locn_id from swm_to_mhe sm " +
                $"inner join carton_hdr ch ON sm.order_id = ch.carton_nbr " +
                $"inner join pkt_hdr ph ON ph.pkt_ctrl_nbr = ch.pkt_ctrl_nbr " +
                $"inner join alloc_invn_dtl al ON al.task_cmpl_ref_nbr = ch.carton_nbr " +
                $"inner join pick_locn_dtl pl ON pl.sku_id = sm.sku_id"+
                $" where sm.source_msg_status = 'Ready' and ch.stat_code < 15 and ph.pkt_stat_code <{statusCode}";
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
          
        public void GetDataBeforeTriggerOrst()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                case1 = GetCartonDetailsForInsertingOrstMessage(db, 35);
                OrmtCase1 = JsonConvert.DeserializeObject<OrmtDto>(case1.MessageJson);
                OrstMessageCreatedForAllocatedStatus(db);            
                emsToWmsCase1 = GetEmsToWmsData(db, msgKeyForCase1.MsgKey);             
               // pickLcnCase1BeforeApi = GetPickLocationDetails(db, OrmtCase1.Sku, case1.LocnId);
                //pickLcnExtCase1BeforeApi = GetPickLocnDtlExt(db, pickLcnCase1BeforeApi.SkuId,pickLcnCase1BeforeApi.LocationId);            
            }
        }

        public void GetDataBeforeCallingApiForActionCodeComplete()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                case2 = GetCartonDetailsForInsertingOrstMessage(db, 15);
                OrmtCase2 = JsonConvert.DeserializeObject<OrmtDto>(case2.MessageJson);
                emsToWmsCase2 = GetEmsToWmsData(db, msgKeyForCase2.MsgKey);
                cartonDtlCase2BeforeApi = GetCartonDetails(db, case2.OrderId);
                var cartonHdr = GetCartonHeaderDetails(db, case2.OrderId);
                pickTktDtlCase2BeforeApi = GetPickTicketDetailData(db, cartonHdr.CartonNumber, "1");
                allocInvnDtlCase2BeforeApi = GetAllocInvnDetails(db, case2.OrderId);
                pickLcnCase2BeforeApi = GetPickLocationDetails(db, OrmtCase2.Sku, case2.LocnId);
                pickLcnExtCase2BeforeApi = GetPickLocnDtlExt(db,  pickLcnCase2BeforeApi.SkuId, pickLcnCase2BeforeApi.LocationId);         
            }
        }

        public void GetDataBeforeCallingApiForActionCodeDeAllocate()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                case3 = GetCartonDetailsForInsertingOrstMessage(db, 30);
                OrmtCase3 = JsonConvert.DeserializeObject<OrmtDto>(case3.MessageJson);
                emsToWmsCase3  = GetEmsToWmsData(db, msgKeyForCase3.MsgKey);

            }
        }

        public void GetDataBeforeCallingApiForActionCodeCancel()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                case4 = GetCartonDetailsForInsertingOrstMessage(db, 30);
                OrmtCase4 = JsonConvert.DeserializeObject<OrmtDto>(case4.MessageJson);
                emsToWmsCase4 = GetEmsToWmsData(db, msgKeyForCase4.MsgKey);
                pickLcnCase4BeforeApi = GetPickLocationDetails(db, "carton_dtl.sku", case4.LocnId);
                pickLcnExtCase4BeforeApi = GetPickLocnDtlExt(db, pickLcnCase4BeforeApi.SkuId, pickLcnCase4BeforeApi.LocationId);
            }
        }

        public void GetDataAfterTriggerOrstCase1()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                swmFromMheCase1 = SwmFromMhe(db, msgKeyForCase1.MsgKey,"ORST");
                orstCase1 = JsonConvert.DeserializeObject<OrstDto>(swmFromMheCase1.MessageJson);
                pickTktHdrCase1 = GetPickTktHeaderDetails(db, orstCase1.OrderId);
                cartonHdrCase1 = GetCartonHeaderDetails(db, orstCase1.OrderId);
                //pickLcnCase1 = GetPickLocationDetails(db, orstCase1.Sku,case1.LocnId);              
               // pickLcnExtCase1 = GetPickLocnDtlExt(db, pickLcnCase1BeforeApi.SkuId,pickLcnCase1BeforeApi.LocationId);
            }
        }

        public void GetDataAfterTriggerOrstCase2()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                swmFromMheCase2 = SwmFromMhe(db, msgKeyForCase2.MsgKey, "ORST");
                orstCase2 = JsonConvert.DeserializeObject<OrstDto>(swmFromMheCase2.MessageJson);
                cartonHdrCase2 = GetCartonHeaderDetails(db, orstCase2.OrderId);
                cartonDtlCase2AfterApi = GetCartonDetails(db,orstCase2.OrderId);
                pickTktDtlCase2AfterApi = GetPickTicketDetailData(db, orstCase2.OrderId, orstCase2.OrderLineId);
                pickTktHdrCase2 = GetPickTktHeaderDetails(db, orstCase2.OrderId);
                allocInvnDtlCase2BeforeApi = GetAllocInvnDetails(db, orstCase2.OrderId);
                pickLcnCase2 = GetPickLocationDetails(db, orstCase2.Sku,case2.LocnId);
                pickLcnExtCase2 = GetPickLocnDtlExt(db, pickLcnCase1BeforeApi.SkuId,pickLcnCase1BeforeApi.LocationId);
            }
        }

        public void GetDataAfterTriggerOrstCase3()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                swmFromMheCase3 = SwmFromMhe(db, msgKeyForCase3.MsgKey, "ORST");
                orstCase3 = JsonConvert.DeserializeObject<OrstDto>(swmFromMheCase3.MessageJson);
                cartonHdrCase3  = GetCartonHeaderDetails(db, orstCase3.OrderId);
            }
        }

        public void GetDataAfterTriggerOrstCase4()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                swmFromMheCase4 = SwmFromMhe(db, msgKeyForCase4.MsgKey, "ORST");
                orstCase4 = JsonConvert.DeserializeObject<OrstDto>(swmFromMheCase4.MessageJson);
                cartonHdrCase4 = GetCartonHeaderDetails(db, orstCase3.OrderId);
                pickLcnCase4 = GetPickLocationDetails(db, orstCase4.Sku, case4.LocnId);
                pickLcnExtCase4  = GetPickLocnDtlExt(db, pickLcnCase1BeforeApi.SkuId,pickLcnCase1BeforeApi.LocationId);
            }
        }

        public string CreateOrstMessage(string actionCode,string containerId,string skuId,string qty,string waveNbr,string orderReasonCodeMap,string owner, string currentLocationId)
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
                DestinationLocationId = OrmtCase1.DestinationLocationId,
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
//------------------------------------------------
        public void OrstMessageCreatedForAllocatedStatus(OracleConnection db)
        {
            var orstmsg = CreateOrstMessage("Allocated",case1.OrderId,case1.SkuId, OrmtCase1.Quantity, OrmtCase1.WaveId,"0", OrmtCase1.Owner,null);
            var emsToWms = new EmsToWmsDto
            {
                Process = DefaultValues.Process,
                Status = DefaultValues.Status,
                Transaction = TransactionCode.Orst,
                ResponseCode = (short)int.Parse(ReasonCode.Success),
                MessageText = orstmsg
            };
            msgKeyForCase1.MsgKey = InsertEmsToWMS(db, emsToWms);
        }

        public void OrstMessageCreatedForCompletedStatus(OracleConnection db)
        {
            var orstmsg = CreateOrstMessage("Completed", case2.OrderId, case2.SkuId, OrmtCase2.Quantity, OrmtCase2.WaveId,"0",null,null);
            var emsToWms = new EmsToWmsDto
            {
                Process = DefaultValues.Process,
                Status = DefaultValues.Status,
                Transaction = TransactionCode.Cost,
                ResponseCode = (short)int.Parse(ReasonCode.Success),
                MessageText = orstmsg
            };
            msgKeyForCase2.MsgKey = InsertEmsToWMS(db, emsToWms);
        }

        public void OrstMessageCreatedForDeallocateStatus(OracleConnection db)
        {
            var orstmsg = CreateOrstMessage("Deallocate", case3.OrderId, case3.SkuId, OrmtCase3.Quantity, OrmtCase3.WaveId,"0",null,null);
            var emsToWms = new EmsToWmsDto
            {
                Process = DefaultValues.Process,
                Status = DefaultValues.Status,
                Transaction = TransactionCode.Cost,
                ResponseCode = (short)int.Parse(ReasonCode.Success),
                MessageText = orstmsg
            };
            msgKeyForCase3.MsgKey = InsertEmsToWMS(db, emsToWms);
        }

        public void OrstMessageCreatedForCancelledStatus(OracleConnection db)
        {
            var orstmsg = CreateOrstMessage("Deallocate", case4.OrderId, case4.SkuId, OrmtCase4.Quantity, OrmtCase4.WaveId,"0",null,null);
            var emsToWms = new EmsToWmsDto
            {
                Process = DefaultValues.Process,
                Status = DefaultValues.Status,
                Transaction = TransactionCode.Cost,
                ResponseCode = (short)int.Parse(ReasonCode.Success),
                MessageText = orstmsg
            };
            msgKeyForCase4.MsgKey = InsertEmsToWMS(db, emsToWms);
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
                //cartonHeader.ModificationDateTime = cartonHeaderReader[CartonHeader.ModificationDateTime].ToString();
                cartonHeader.CurrentLocationId = cartonHeaderReader[TestData.CartonHeader.CurrentLocationId].ToString();
                //cartonHeader.Picker = cartonHeaderReader[TestData.CartonHeader.Pikr].ToString();
                //cartonHeader.Packer = cartonHeaderReader[TestData.CartonHeader.Pakr].ToString();
                //cartonHeader.CartonNumber = cartonHeaderReader[TestData.CartonHeader.CartonNbr].ToString();
                //cartonHeader.PickLocationId = cartonHeaderReader[TestData.CartonHeader.PickLocationId].ToString();
                //cartonHeader.MiscellaneousInstructionCode5 = cartonHeaderReader[TestData.CartonHeader.MiscInstrCode5].ToString();
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
                pickTktDtl.PickTicketSequenceNumber = Convert.ToInt16(pickTktDtlReader[TestData.PickTicketDetail.PickTicketSeqNbr].ToString());
                pickTktDtl.PickTicketControlNumber = pickTktDtlReader[TestData.PickTicketDetail.PickTktCtrlNbr].ToString();
                pickTktDtl.UnitsPacked = Convert.ToDecimal(pickTktDtlReader[TestData.PickTicketDetail.UnitsPacked].ToString());            
                pickTktDtl.VerifiedAsPacked = Convert.ToDecimal(pickTktDtlReader[TestData.PickTicketDetail.VerfAsPakd].ToString());
                pickTktDtl.UpdatedOn = Convert.ToDateTime(pickTktDtlReader[TestData.PickTicketDetail.ModificationDateTime].ToString());
            }
            return pickTktDtl;
        }

        public AllocInvnDtl GetAllocInvnDetails(OracleConnection db,string CntrNbr)
        {
            var allocInvnDtl = new AllocInvnDtl();
            var query = $"select * from ALLOC_INVN_DTL WHERE CNTR_NBR='{CntrNbr}'";
            command = new OracleCommand(query,db);
            var allocInvnDtlReader = command.ExecuteReader();
            if(allocInvnDtlReader.Read())
            {
                allocInvnDtl.CntrNbr = allocInvnDtlReader[AllocInvnDetail.CntrNbr].ToString();
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
