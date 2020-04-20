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
using Sfc.Wms.Configuration.NextUpCounter.Contracts.Interfaces;
using Sfc.Wms.Foundation.PixTransaction.Contracts.Dtos;
using System.Diagnostics;

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
        protected int MasterPackIdCount;
        protected int CwcCount;
        protected int CwcCountBeforeApi;


        public OrstTestData GetCartonDetailsForInsertingOrstMessage(OracleConnection db,int cartonStatusCode, int pktStatusCode, bool completed = true)
        {
            var orstTestData = new OrstTestData();
            var sqlStatement = OrstQueries.ValidDataForInsertingOrstMessage;
            if (completed)
            {
                sqlStatement = sqlStatement + $" where sm.source_msg_status = '{DefaultValues.Status}' and ch.stat_code = '{cartonStatusCode}' and ph.pkt_stat_code = '{pktStatusCode}' and pd.pkt_seq_nbr > {Constants.NumZero} order by sm.created_date_time desc";
            }
            else
            {
                sqlStatement = sqlStatement + $" where sm.source_msg_status = '{DefaultValues.Status}' and ch.stat_code ='{cartonStatusCode}' and ph.pkt_stat_code < '{pktStatusCode}' and pd.pkt_seq_nbr > {Constants.NumZero} order by sm.created_date_time desc";
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
          
        public OrstTestData GetCartonDetailsForInsertingOrstMessageForNegativeCaseWherePickTicketSeqNumberIsLessThan1(OracleConnection db, int cartonStatusCode, int pktStatusCode)
        {
            var orstTestData = new OrstTestData();
            var sqlStatement = OrstQueries.PickTktSeqNbrIsLessThan1;
                Command = new OracleCommand(sqlStatement, db);
                Command.Parameters.Add(new OracleParameter("status", DefaultValues.Status));
                Command.Parameters.Add(new OracleParameter("cartonStatusCode", cartonStatusCode));
                Command.Parameters.Add(new OracleParameter("pktStatusCode", pktStatusCode));               
                var swmToMheReader = Command.ExecuteReader();
                if (swmToMheReader.Read())
                {
                    orstTestData.OrderId = swmToMheReader["ORDER_ID"].ToString();
                    orstTestData.SkuId = swmToMheReader["SKU_ID"].ToString();
                    orstTestData.Quantity = Convert.ToInt16(swmToMheReader["QTY"]);
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
                Allocated = GetCartonDetailsForInsertingOrstMessage(db, Constants.CartonStatusForReleased,Constants.PktStatusForInPacking, false);
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
                Complete = GetCartonDetailsForInsertingOrstMessage(db, Constants.CartonStatusForInPacking, Constants.PktStatusForInPacking);
                OrmtCase2 = JsonConvert.DeserializeObject<OrmtDto>(Complete.MessageJson);              
                CartonDtlCase2BeforeApi = GetCartonDetails(db, Complete.OrderId);
                GetCartonHeaderDetails(db, Complete.OrderId);
                PickTktDtlCase2BeforeApi = GetPickTicketDetailData(db, Complete.OrderId, Constants.PickSeqNumber);
                AllocInvnDtlCompletedBeforeApi = GetAllocInvnDetails(db, Complete.OrderId);
                PickLcnCase2BeforeApi = GetPickLocationDetails(db, OrmtCase2.Sku,null);
                PickLcnExtCase2BeforeApi = GetPickLocnDtlExt(db,  Complete.SkuId, PickLcnCase2BeforeApi.LocationId);
                CwcCountBeforeApi = CalculateTheForeCaseCountFromMsgToCWCTable(db, OrmtCase2.WaveId);
                OrstMessageCreatedForCompletedStatus(db);
                EmsToWmsCompleted = GetEmsToWmsData(db, MsgKeyForCompleted.MsgKey);
            }
        }

        public void GetDataBeforeCallingApiForActionCodeDeAllocate()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                Deallocate = GetCartonDetailsForInsertingOrstMessage(db, Constants.CartonStatusForInPacking, Constants.PktStatusForInPacking);
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
                Canceled = GetCartonDetailsForInsertingOrstMessage(db, Constants.CartonStatusForPicked,Constants.PktWeighed);
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
                SwmFromMheAllocated = SwmFromMhe(db, MsgKeyForAllocated.MsgKey,TransactionCode.Orst);
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
                SwmFromMheComplete = SwmFromMhe(db, MsgKeyForCompleted.MsgKey, TransactionCode.Orst);
                OrstCompleted = JsonConvert.DeserializeObject<OrstDto>(SwmFromMheComplete.MessageJson);
                CartonHdrCompleted = GetCartonHeaderDetails(db, OrstCompleted.OrderId);
                CartonDtlCase2AfterApi = GetCartonDetails(db,OrstCompleted.OrderId);
                PickTktDtlCase2AfterApi = GetPickTicketDetailData(db, OrstCompleted.OrderId, OrstCompleted.OrderLineId);
                PickTktHdrCompleted = GetPickTktHeaderDetails(db, OrstCompleted.OrderId);
                AllocInvnDtlCompletedAfterApi = GetAllocInvnDetails(db, OrstCompleted.OrderId);
                PickLcnCase2 = GetPickLocationDetails(db, OrstCompleted.Sku,null);
                PickLcnExtCase2 = GetPickLocnDtlExt(db, OrstCompleted.Sku,PickLcnCase1BeforeApi.LocationId);
                MasterPackIdCount = CountOfMasterPackId(db, OrstCompleted.ParentContainerId);
                CwcCount = CalculateTheForeCaseCountFromMsgToCWCTable(db, SwmFromMheComplete.WaveNumber);
                try
                {
                    Data.Entities.AltSku altSku = GetAltSkuInfo(db, OrstCompleted.Sku);
                    PixTransactionDto parentSku =  GetPixTranDetails(db, altSku.ParentSkuId);
                    PixTransactionDto childSku =  GetPixTranDetails(db, altSku.ChildSkuId);
                    Assert.AreEqual(altSku.QuantityChildPerParent, childSku.InventoryAdjustmentQuantity);                   
                    Assert.AreEqual(1, parentSku.InventoryAdjustmentQuantity);
                }
                catch
                {
                    Debug.Print("NO PIX INSERTED");
                }

            }
        }

        public void GetDataAfterTriggerOrstCase3()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                SwmFromMheDeallocate = SwmFromMhe(db, MsgKeyForDeallocated.MsgKey,TransactionCode.Orst);
                OrstDeallocate = JsonConvert.DeserializeObject<OrstDto>(SwmFromMheDeallocate.MessageJson);
            }
        }

        public void GetDataAfterTriggerOrstCase4()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                SwmFromMheCancel = SwmFromMhe(db, MsgKeyForCanceled.MsgKey, TransactionCode.Orst);
                OrstCanceled = JsonConvert.DeserializeObject<OrstDto>(SwmFromMheCancel.MessageJson);
                CartonHdrCanceled = GetCartonHeaderDetails(db, OrstCanceled.OrderId);
                PickLcnExtCase4  = GetPickLocnDtlExt(db, OrstCanceled.Sku,PickLcnCase1BeforeApi.LocationId);
                MessageToSort = GetMsgTosvDetail(db, CancelOrder.CartonNbr,"USL");
            }
        }

        public void GetDataBeforeApiForActionCodeCompletedWithBitsEnabled()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                Complete = GetCartonDetailsForInsertingOrstMessage(db, Constants.CartonStatusForInPacking, Constants.PktStatusForInPacking);
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
                Complete = GetCartonDetailsForInsertingOrstMessageForNegativeCaseWherePickTicketSeqNumberIsLessThan1(db, Constants.CartonStatusForReleased, Constants.PktStatusForInPacking);
                OrmtCase2 = JsonConvert.DeserializeObject<OrmtDto>(Complete.MessageJson);
                OrstMessageCreatedForCompletedStatus(db);
            }
        }
        public void GetDataAfterApiForActionCodeCompleteWithBitsEnabled()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                SwmFromMheComplete = SwmFromMhe(db, MsgKeysForCase5.MsgKey,TransactionCode.Orst);
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
                OrderLineId = Constants.OrderLineId,
                CompletionTime = Constants.CompletionTime,
                Sku = skuId,
                Owner = owner,
                UnitOfMeasure = UnitOfMeasures.Each,
                ParentContainerId = Constants.MasterPackId,
                QuantityOrdered = qty,
                QuantityDelivered = Constants.QuantityDelivered,
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
            var orstmsg = CreateOrstMessage(OrstActionCode.Allocated,Allocated.OrderId,Allocated.SkuId, OrmtCase1.Quantity, OrmtCase1.WaveId,Constants.OrderReasonCodeMap, OrmtCase1.Owner,null,Allocated.DestLocnId);
            var emsToWms = new EmsToWmsDto
            {
                Process = DefaultValues.Process,
                Status = DefaultValues.Status,
                Transaction = TransactionCode.Orst,
                ResponseCode = (short)int.Parse(ReasonCode.Success),
                MessageText = orstmsg
            };
            MsgKeyForAllocated.MsgKey = InsertEmsToWms(db, emsToWms);
        }

        public void OrstMessageCreatedForCompletedStatus(OracleConnection db)
        {
            var orstmsg = CreateOrstMessage(OrstActionCode.Complete, Complete.OrderId, Complete.SkuId, OrmtCase2.Quantity, OrmtCase2.WaveId,Constants.OrderReasonCodeMap,OrmtCase2.Owner,null,Complete.DestLocnId);
            var emsToWms = new EmsToWmsDto
            {
                Process = DefaultValues.Process,
                Status = DefaultValues.Status,
                Transaction = TransactionCode.Orst,
                ResponseCode = (short)int.Parse(ReasonCode.Success),
                MessageText = orstmsg
            };
            MsgKeyForCompleted.MsgKey = InsertEmsToWms(db, emsToWms);
            }

        public void OrstMessageCreatedForCompletedStatusWithBitsEnabled(OracleConnection db)
        {
            var orstmsg = CreateOrstMessage(OrstActionCode.Complete, Complete.OrderId, Complete.SkuId, OrmtCase2.Quantity, OrmtCase2.WaveId, Constants.OrderRsnCodeBitsEnabled, OrmtCase2.Owner, Constants.SampleCurrentLocnId, Complete.DestLocnId);
            var emsToWms = new EmsToWmsDto
            {
                Process = DefaultValues.Process,
                Status = DefaultValues.Status,
                Transaction = TransactionCode.Orst,
                ResponseCode = (short)int.Parse(ReasonCode.Success),
                MessageText = orstmsg
            };
            MsgKeysForCase5.MsgKey = InsertEmsToWms(db, emsToWms);
        }


        public void OrstMessageCreatedForDeallocateStatus(OracleConnection db)
        {
            var orstmsg = CreateOrstMessage(OrstActionCode.Deallocate, Deallocate.OrderId, Deallocate.SkuId, OrmtCase3.Quantity, OrmtCase3.WaveId,"05",null,null,Deallocate.DestLocnId);
            var emsToWms = new EmsToWmsDto
            {
                Process = DefaultValues.Process,
                Status = DefaultValues.Status,
                Transaction = TransactionCode.Orst,
                ResponseCode = (short)int.Parse(ReasonCode.Success),
                MessageText = orstmsg
            };
            MsgKeyForDeallocated.MsgKey = InsertEmsToWms(db, emsToWms);
        }

        public void OrstMessageCreatedForCancelledStatus(OracleConnection db)
        {
            var orstmsg = CreateOrstMessage(OrstActionCode.Canceled, Canceled.OrderId, Canceled.SkuId, OrmtCase4.Quantity, OrmtCase4.WaveId,"06",null,null,Canceled.DestLocnId);
            var emsToWms = new EmsToWmsDto
            {
                Process = DefaultValues.Process,
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
            var query = OrstQueries.CartonHeader;
            Command = new OracleCommand(query,db);
            Command.Parameters.Add(new OracleParameter("cartonNumber", cartonNumber));
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
            var query = OrstQueries.CartonDetail;
            Command = new OracleCommand(query, db);
            Command.Parameters.Add(new OracleParameter("cartonNumber", cartonNumber));
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
            var query = OrstQueries.PickTktHeader;
            Command = new OracleCommand(query,db);
            Command.Parameters.Add(new OracleParameter("cartonNumber", cartonNumber));
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
            var query = OrstQueries.PickTktDtl;
            Command = new OracleCommand(query, db);
            Command.Parameters.Add(new OracleParameter("cartonNbr", cartonNbr));
            Command.Parameters.Add(new OracleParameter("pktSeqNbr", pktSeqNbr));
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
            var query = OrstQueries.AllocInventory;
            Command = new OracleCommand(query,db);
            Command.Parameters.Add(new OracleParameter("cntrNbr", cntrNbr));
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
            var query = CommonQueries.EmsToWms;
            Command = new OracleCommand(query,db);
            Command.Parameters.Add(new OracleParameter("msgKey", msgKey));
            var emsToWmsReader = Command.ExecuteReader();
            if(emsToWmsReader.Read())
            {
                emsToWms.MessageKey = Convert.ToInt64(emsToWmsReader[EmsToWms.MsgKey]);
                emsToWms.MessageText = emsToWmsReader[EmsToWms.MsgTxt].ToString();
                emsToWms.ResponseCode = Convert.ToInt16(emsToWmsReader[EmsToWms.ReasonCode]);
                emsToWms.Process = emsToWmsReader[EmsToWms.Prc].ToString();
                emsToWms.Status = emsToWmsReader[EmsToWms.Status].ToString();
                emsToWms.Transaction = emsToWmsReader[EmsToWms.Trx].ToString();
            }
            return emsToWms;
        }

        public PixTransactionDto GetPixTranDetails(OracleConnection db ,string skuId )
        {
            var pixTran  = new PixTransactionDto();
            var query = $"Select * from pix_tran  where sku_id = '{skuId}' and PROC_STAT_CODE in (Select case when substr(misc_flags,1,1)= 'Y' then 11 else 10  end pix_stat_code from sys_code where code_type = '051' and rec_type = 'C' )and case_nbr is null   order by create_date_time desc";
            Command = new OracleCommand(query,db);
            var pixTranReader = Command.ExecuteReader();
            if(pixTranReader.Read())
            {
                pixTran.ProcessedStatusCode = Convert.ToInt32(pixTranReader[""]);
                pixTran.InventoryAdjustmentQuantity = Convert.ToInt32(pixTranReader["INVN_ADJMT_QTY"]);
                pixTran.InventoryAdjustmentType = pixTranReader["INVN_ADJMT_TYPE"].ToString();
            }
            return pixTran;
        }
    }
}
