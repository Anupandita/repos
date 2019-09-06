using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData;
using Sfc.Wms.Asrs.Dematic.Contracts.Dtos;
using Sfc.Wms.Asrs.Shamrock.Contracts.Dtos;
using Sfc.Wms.Builder.MessageBuilder;
using Sfc.Wms.ParserAndTranslator.Contracts.Constants;
using Sfc.Wms.ParserAndTranslator.Contracts.Dto;
using Sfc.Wms.ParserAndTranslator.Contracts.Interfaces;
using Sfc.Wms.ParserAndTranslator.Contracts.Validation;
using Sfc.Wms.Result;
using System;


namespace Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures
{
    public class DataBaseFixtureForOrst : DataBaseFixtureForCost
    {
        protected SwmToMheDto case1 = new SwmToMheDto();
        protected SwmToMheDto case2 = new SwmToMheDto();
        protected SwmToMheDto case3 = new SwmToMheDto();
        protected SwmToMheDto case4 = new SwmToMheDto();
        protected SwmFromMheDto swmToMheCase1 = new SwmFromMheDto();
        protected OrstDto OrstParameters;
        private readonly IHaveDataTypeValidation _dataTypeValidation;
        protected OrmtDto OrmtCase1 = new OrmtDto();
        protected OrmtDto OrmtCase2 = new OrmtDto();
        protected OrmtDto OrmtCase3 = new OrmtDto();
        protected OrmtDto OrmtCase4 = new OrmtDto();
        protected Orst msgKeyForCase1 = new Orst();
        protected Orst msgKeyForCase2 = new Orst();
        protected Orst msgKeyForCase3 = new Orst();
        protected Orst msgKeyForCase4 = new Orst();

        

        public DataBaseFixtureForOrst()
        {
            _dataTypeValidation = new DataTypeValidation();
        }

        public SwmToMheDto GetCartonDetailsForInsertingOrstMessage(OracleConnection db,int statusCode)
        {
            var swmToMheView = new SwmToMheDto();
            var sqlStatement = $"select swm_to_mhe.container_id,swm_to_mhe.sku_id,swm_to_mhe.qty,swm_to_mhe.msg_json from swm_to_mhe inner join carton_hdr on swm_to_mhe.container_id = carton_hdr.carton_nbr and swm_to_mhe.source_msg_status = 'Ready' and swm_to_mhe.qty!= 0 and carton_hdr.stat_code = {statusCode}";
            command = new OracleCommand(sqlStatement, db);
            var swmToMheReader = command.ExecuteReader();
            if(swmToMheReader.Read())
            {
                swmToMheView.ContainerId = swmToMheReader["CONTAINER_ID"].ToString();
                swmToMheView.SkuId = swmToMheReader["SKU_ID"].ToString();
                swmToMheView.Quantity = Convert.ToInt16(swmToMheReader["QTY"].ToString());
                swmToMheView.MessageJson = swmToMheReader["MSG_JSON"].ToString();         
            }
            return swmToMheView;
        }
          
        public void GetDataBeforeTriggerOrst()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                case1 = GetCartonDetailsForInsertingOrstMessage(db, 12);
                OrmtCase1 = JsonConvert.DeserializeObject<OrmtDto>(case1.MessageJson);
                case2 = GetCartonDetailsForInsertingOrstMessage(db, 15);
                OrmtCase2 = JsonConvert.DeserializeObject<OrmtDto>(case2.MessageJson);
                case3 = GetCartonDetailsForInsertingOrstMessage(db, 30);
                OrmtCase3 = JsonConvert.DeserializeObject<OrmtDto>(case3.MessageJson);
                case4 = GetCartonDetailsForInsertingOrstMessage(db, 30);
                OrmtCase4 = JsonConvert.DeserializeObject<OrmtDto>(case4.MessageJson);

            }
        }

        public void GetDataAfterTriggerOrst()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                swmToMheCase1 = SwmFromMhe(db,case1.ContainerId,"ORST",case1.SkuId);

            }
        }

        public string CreateOrstMessage(string actionCode,string containerId,string skuId,string qty,string waveNbr)
        {
            OrstParameters = new OrstDto
            {
                TransactionCode = TransactionCode.Orst,
                MessageLength = MessageLength.Orst,
                ActionCode = actionCode,
                OrderId = containerId,
                OrderLineId = "1",
                CompletionTime = DateTime.Now.ToString("dd-MMM-yy"),
                Sku = skuId,
                Owner = "",
                UnitOfMeasure = "Case",
                ParentContainerId = containerId,
                QuantityOrdered = qty,
                QuantityDelivered = "1",
                DestinationLocationId = OrmtCase1.DestinationLocationId,
                CurrentLocationId = "asrsLocation",
                Priority = "",
                OrderReasonCodeMap = "0",
                WaveId = waveNbr
            };
            GenericMessageBuilder gm = new GenericMessageBuilder(_dataTypeValidation);
            var testResult = gm.BuildMessage<OrstDto, OrstValidationRule>(OrstParameters, TransactionCode.Orst);
            Assert.AreEqual(testResult.ResultType, ResultTypes.Ok);
            Assert.IsNotNull(testResult.Payload);
            return testResult.Payload;
        }

        public void OrstMessageCreatedForAllocatedStatus(OracleConnection db)
        {
            var orstmsg = CreateOrstMessage("Allocated",case1.ContainerId,case1.SkuId, OrmtCase1.Quantity, OrmtCase1.WaveId);
            var emsToWms = new EmsToWmsDto
            {
                Process = DefaultValues.Process,
                Status = DefaultValues.Status,
                Transaction = TransactionCode.Cost,
                ResponseCode = (short)int.Parse(ReasonCode.Success),
                MessageText = orstmsg
            };
            msgKeyForCase1.MsgKey = InsertEmsToWMS(db, emsToWms);
        }

        public void OrstMessageCreatedForCompletedStatus(OracleConnection db)
        {
            var orstmsg = CreateOrstMessage("Completed", case2.ContainerId, case2.SkuId, OrmtCase2.Quantity, OrmtCase2.WaveId);
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
            var orstmsg = CreateOrstMessage("Deallocate", case3.ContainerId, case3.SkuId, OrmtCase3.Quantity, OrmtCase3.WaveId);
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
            var orstmsg = CreateOrstMessage("Deallocate", case4.ContainerId, case4.SkuId, OrmtCase4.Quantity, OrmtCase4.WaveId);
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
  
        public CartonHdr GetCartonHeaderDetails(OracleConnection db,string cartonNumber)
        {
            var cartonHeader = new CartonHdr();
            var query = $"select CURR_LOCN_ID, DEST_LOCN_ID,MOD_DATE_TIME, USER_ID, STAT_CODE from CARTON_HDR where CARTON_NBR='{cartonNumber}'";
            command = new OracleCommand(query,db);
            var cartonHeaderReader = command.ExecuteReader();
            if (cartonHeaderReader.Read())
            {
                cartonHeader.StatCode = cartonHeaderReader[CartonHeader.StatCode].ToString();
                cartonHeader.ModificationDateTime = cartonHeaderReader[CartonHeader.ModificationDateTime].ToString();
                cartonHeader.UserId = cartonHeaderReader[CartonHeader.UserId].ToString();
                cartonHeader.CurrentLocationId = cartonHeaderReader[CartonHeader.CurrentLocationId].ToString();
                cartonHeader.Pikr = cartonHeaderReader[CartonHeader.Pikr].ToString();
                cartonHeader.Pakr = cartonHeaderReader[CartonHeader.Pakr].ToString();
                cartonHeader.CartonNbr = cartonHeaderReader[CartonHeader.CartonNbr].ToString();
                cartonHeader.PickLocationId = cartonHeaderReader[CartonHeader.PickLocationId].ToString();
                cartonHeader.MiscInstrCode5 = cartonHeaderReader[CartonHeader.MiscInstrCode5].ToString();
            }
            return cartonHeader;
        }

        public CartonDtl GetCartonDetails(OracleConnection db, string cartonNumber)
        {
            var cartonDtl = new CartonDtl();
            var query = $"select * from carton_dtl where carton_nbr = '{cartonNumber}'";
            command = new OracleCommand(query, db);
            var cartonReader = command.ExecuteReader();
            if(cartonReader.Read())
            {
                cartonDtl.CartonNumber = cartonReader[CartonDetail.CartonNumber].ToString();
                cartonDtl.ToBePackdUnits = cartonReader[CartonDetail.ToBePackdUnits].ToString();
                cartonDtl.ModificationDateTime = cartonReader[CartonDetail.ModificationDateTime].ToString();
                cartonDtl.UnitsPakd = cartonReader[CartonDetail.UnitsPakd].ToString();
                cartonDtl.UserId = cartonReader[CartonDetail.UserId].ToString();
            }
            return cartonDtl;
        }

        public PickTktHdr GetPickTktHeaderDetails(OracleConnection db,string cartonNumber)
        {
            var pkTktHeader = new PickTktHdr();
            var query = $"select PKT_HDR.PKT_STAT_CODE,PKT_HDR.MOD_DATE_TIME,PKT_HDR.USER_ID from PKT_HDR WHERE PKT_CTRL_NBR = (SELECT PKT_CTRL_NBR FROM CARTON_HDR WHERE CARTON_NBR = '{cartonNumber}')";
            command = new OracleCommand(query,db);
            var pickTktHeaderReader = command.ExecuteReader();
            if (pickTktHeaderReader.Read())
            {
                pkTktHeader.CartonNbr = pickTktHeaderReader[PickTicketHeader.CartonNumber].ToString();
                pkTktHeader.PktStatCode = pickTktHeaderReader[PickTicketHeader.PktStatCode].ToString();
                pkTktHeader.ModDateTime = pickTktHeaderReader[PickTicketHeader.ModDateTime].ToString();
                pkTktHeader.UserId = pickTktHeaderReader[PickTicketHeader.UserId].ToString();
            }
            return pkTktHeader;
        }


        public PickLocnDtl GetPickLocationDetails(OracleConnection db, string locnId, string skuId)
        {
            var pickLocnDtl = new PickLocnDtl();
            var query = $"select TO_BE_PIKD_QTY,MOD_DATE_TIME,USER_ID,LOCN_ID, SKU_ID from PICK_LOCN_DTL where LOCN_ID= '{locnId}' AND SKU_ID='{skuId}'";
            command = new OracleCommand(query, db);
            var pickLocnDtlReader = command.ExecuteReader();
            if(pickLocnDtlReader.Read())
            {
                pickLocnDtl.ToBePickedQuantity = pickLocnDtlReader[TestData.PickLocationDetail.ToBePickedQuantity].ToString();
                pickLocnDtl.ModDateTime = pickLocnDtlReader[TestData.PickLocationDetail.ModDateTime].ToString();
                pickLocnDtl.UserId = pickLocnDtlReader[TestData.PickLocationDetail.UserId].ToString();
            }
            return pickLocnDtl;
        }

        public PkLcnDtlExt GetPickLocnDtlExt(OracleConnection db,string locnId,string skuId)
        {
            var pickLocnDtlExt = new PkLcnDtlExt();
            var query = $"select ACTIVE_ORMT_COUNT,UPDATED_BY,UPDATED_DATE_TIME from PICK_LOCN_DTL_EXT WHERE LOCN_ID= '{locnId}' AND SKU_ID= '{skuId}')";
            command = new OracleCommand(query, db);
            var pickLocnDtlExtReader = command.ExecuteReader();
            if(pickLocnDtlExtReader.Read())
            {
                pickLocnDtlExt.ActiveOrmtCount = pickLocnDtlExtReader[TestData.PickLocnDtlExt.ActiveOrmtCount].ToString();
                pickLocnDtlExt.UpdatedBy = pickLocnDtlExtReader[TestData.PickLocnDtlExt.UpdatedBy].ToString();
                pickLocnDtlExt.UpdatedDateTime = pickLocnDtlExtReader[TestData.PickLocnDtlExt.UpdatedDateTime].ToString();
            }
            return pickLocnDtlExt;
        }

        public PickTktDtl GetPickTicketDetailData(OracleConnection db,string cartonNbr,string pktSeqNbr)
        {
            var pickTktDtl = new PickTktDtl();
            var query = $"select PKT_DTL.UNITS_PAKD,PKT_DTL.MOD_DATE_TIME,PKT_DTL.USER_ID ,PKT_DTL.VERF_AS_PAKD,PKT_DTL.PKT_CTRL_NBR from PKT_DTL where PKT_DTL.PKT_CTRL_NBR = (SELECT PKT_CTRL_NBR FROM CARTON_HDR WHERE CARTON_NBR= '{cartonNbr}') AND PKT_SEQ_NBR= '{pktSeqNbr}'";
            command = new OracleCommand(query, db);
            var pickTktDtlReader = command.ExecuteReader();
            if(pickTktDtlReader.Read())
            {
                pickTktDtl.CartonNumber = pickTktDtlReader[TestData.PickTicketDetail.CartonNumber].ToString();
                pickTktDtl.ModificationDateTime = pickTktDtlReader[PickTicketDetail.ModificationDateTime].ToString();
                pickTktDtl.PickTicketSeqNbr = pickTktDtlReader[PickTicketDetail.PickTicketSeqNbr].ToString();
                pickTktDtl.PickTktCtrlNbr = pickTktDtlReader[PickTicketDetail.PickTktCtrlNbr].ToString();
                pickTktDtl.UnitsPacked = pickTktDtlReader[PickTicketDetail.UnitsPacked].ToString();
                pickTktDtl.UserId = pickTktDtlReader[PickTicketDetail.UserId].ToString();
                pickTktDtl.VerfAsPakd = pickTktDtlReader[PickTicketDetail.VerfAsPakd].ToString();
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
                allocInvnDtl.CntrNbr = allocInvnDtlReader[TestData.AllocInvnDetail.CntrNbr].ToString();
                allocInvnDtl.QtyPulled = allocInvnDtlReader[AllocInvnDetail.QtyPulled].ToString();
                allocInvnDtl.StatCode = allocInvnDtlReader[AllocInvnDetail.StatCode].ToString();
                allocInvnDtl.UserId = allocInvnDtlReader[AllocInvnDetail.UserId].ToString();
            }
            return allocInvnDtl;
        }


    }
}
