using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using Sfc.Core.OnPrem.Result;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData;
using Sfc.Wms.Foundation.Location.Contracts.Dtos;
using Sfc.Wms.Interfaces.Asrs.Dematic.Contracts.Dtos;
using Sfc.Wms.Interfaces.Asrs.Shamrock.Contracts.Dtos;
using Sfc.Wms.Interfaces.Builder.MessageBuilder;
using Sfc.Wms.Interfaces.ParserAndTranslator.Contracts.Constants;
using Sfc.Wms.Interfaces.ParserAndTranslator.Contracts.Dto;
using Sfc.Wms.Interfaces.ParserAndTranslator.Contracts.Validation;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures
{
    public class DataBaseFixtureForSyndwithsameskuCombination:CommonFunction
    {

        protected string PickLocnViewquery = "";
        protected string SqlStatements = "";
        protected SwmToMheDto SwmToMheSynr = new SwmToMheDto();
        protected SynrDto Synr = new SynrDto();
        protected WmsToEmsDto WmsToEmsSynr = new WmsToEmsDto();
        protected long Pldsnapshot;
        protected SwmToMheDto SynrMessageData = new SwmToMheDto();
        protected long Nextupcnt;
        protected string BeoforeApiPickLocn;
        protected PickLocationDetailsDto PldList = new PickLocationDetailsDto();
        protected PickLocationDetailsDto Picklocndetail = new PickLocationDetailsDto();
        protected EmsToWmsDto EmsToWmsParameters;
        protected Synd SyndData = new Synd();
        protected Synd SyncDataDuplicate = new Synd();
        protected SyndDto SyndParameters;
        protected SwmFromMheDto SwmFromMheSynd = new SwmFromMheDto();
        protected SyndDto Synd = new SyndDto();
        protected int SyndSum;
        protected string Syndquery = "";
        protected SyncDto SyncParameters;
        protected Data.Entities.SwmSynrPickLocationDetailSnapshot PldSnapQtyDefference = new Data.Entities.SwmSynrPickLocationDetailSnapshot();
        protected PickLocationDetailsDto PickLocnAfterApi = new PickLocationDetailsDto();
        protected PickLocationDetailsDto PickLocnBeforeApi = new PickLocationDetailsDto();
        protected Data.Entities.SwmSyndData SyndDataQtyDefference = new Data.Entities.SwmSyndData();
        protected Synd SyncData = new Synd();
        protected SwmFromMheDto SwmFromMheSyncDto = new SwmFromMheDto();
        protected SyncDto Sync = new SyncDto();
        protected Data.Entities.PixTransaction PixTran = new Data.Entities.PixTransaction();
        protected Data.Entities.SwmSyndData WmsSyndData = new Data.Entities.SwmSyndData();


        public void GetDataBeforeTriggerSynr()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                Nextupcnt = FetchNextupCount(db);
                BeoforeApiPickLocn = GetPickLocationDetail(db);
            }
        }

        public Int64 FetchNextupCount(OracleConnection db)
        {
            var query = $"{SynrQueries.NextupCountQuery}";
            Command = new OracleCommand(query, db);
            Command.Parameters.Add(new OracleParameter(TestData.Parameter.RecTypeId, Constants.RectTypeId));
            var nextUpCounterReader = Convert.ToInt64(Command.ExecuteScalar().ToString());
            return nextUpCounterReader;
        }
        public string GetPickLocationDetail(OracleConnection db)
        {
            
            PickLocnViewquery = $"{SynrQueries.PickLocnCountQuery}";
            Command = new OracleCommand(PickLocnViewquery, db);
            Command.Parameters.Add(new OracleParameter(TestData.Parameter.SysCodeType, Constants.SysCodeType));
            Command.Parameters.Add(new OracleParameter(TestData.Parameter.SysCodeId, Constants.SysCodeIdForActiveLocation));
            var pickLocnDtlReader = Command.ExecuteScalar().ToString();
            return pickLocnDtlReader;
        }

        public void GetDataAfterTrigger()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                Command = new OracleCommand();
                SwmToMheSynr = SwmToMhe(db, null,TransactionCode.Synr,null);
                Synr = JsonConvert.DeserializeObject<SynrDto>(SwmToMheSynr.MessageJson);
                WmsToEmsSynr = WmsToEmsData(db, SwmToMheSynr.SourceMessageKey, TransactionCode.Synr);
                Pldsnapshot = FetchPldsnapshottable(db, (Nextupcnt + 1).ToString());
            }
        }
        public Int64 FetchPldsnapshottable(OracleConnection db, string syncId)
        {
            var querypld = $"{SynrQueries.PLdSnapShotQuery}";
            Command = new OracleCommand(querypld, db);
            Command.Parameters.Add(new OracleParameter(TestData.Parameter.SysCodeType, Constants.SysCodeType));
            Command.Parameters.Add(new OracleParameter(TestData.Parameter.SysCodeId, Constants.SysCodeIdForActiveLocation));
            var pldsnapshotReader = Convert.ToInt64(Command.ExecuteScalar().ToString());
            return pldsnapshotReader;

        }

        public void SyndMessageForSameSkuWithQuantities()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                SynrMessageData = SwmToMhe(db,null,TransactionCode.Synr,null);
                Synr = JsonConvert.DeserializeObject<SynrDto>(SynrMessageData.MessageJson);
                Picklocndetail = PicklocationDetail(db);
                InsertSyndMessageForSameSkuWithQuantities(db, Picklocndetail.SkuId, Synr.SynchronizationId);
            }
        }

        public PickLocationDetailsDto PicklocationDetail(OracleConnection db)
        {
            var pickLocnDtl = new PickLocationDetailsDto();
            var pickLocnView = $"{SynrQueries.PldTableQuery}";
            Command = new OracleCommand(pickLocnView, db);
            Command.Parameters.Add(new OracleParameter(TestData.Parameter.SysCodeType, Constants.SysCodeType));
            Command.Parameters.Add(new OracleParameter(TestData.Parameter.SysCodeId, Constants.SysCodeIdForActiveLocation));
            var pickLocnDtlReader = Command.ExecuteReader();
            if (pickLocnDtlReader.Read())
            {
                pickLocnDtl.ActualInventoryQuantity = Convert.ToDecimal(pickLocnDtlReader[PickLocationDetail.ActlInvnQty].ToString());
                pickLocnDtl.SkuId = (pickLocnDtlReader[PickLocationDetail.SkuId].ToString());
                pickLocnDtl.LocationId = pickLocnDtlReader[PickLocationDetail.LocnId].ToString();
            }
            return pickLocnDtl;
        }
        public void InsertSyndMessageForSameSkuWithQuantities(OracleConnection db, string skuid, string synchronizationId)
        {
            var syndmsg = CreateSyndMessageForSameSkuWithQty(synchronizationId, skuid);
            EmsToWmsParameters = new EmsToWmsDto
            {
                Process = DefaultPossibleValue.MessageProcessor,
                MessageKey = Convert.ToInt64(SyndData.MsgKey),
                Status = DefaultValues.Status,
                Transaction = TransactionCode.Synd,
                ResponseCode = (short)int.Parse(ReasonCode.Success),
                MessageText = syndmsg,
            };
            SyncDataDuplicate.MsgKey = InsertEmsToWms(db, EmsToWmsParameters);
        }

        public string CreateSyndMessageForSameSkuWithQty(string syncid, string skuid)
        {
            SyndParameters = new SyndDto()
            {
                SynchronizationId = syncid,
                Sku = skuid,
                Owner = Constants.Owner,
                UnitOfMeasure = DefaultPossibleValue.Case,
                Quantity = "10",
                LocationStatus = Constants.AvailableSatus,
                HoldStatus = Constants.HoldStatus,
                TransactionCode = TransactionCode.Synd,
                MessageLength = MessageLength.Synd,
                ReasonCode = ReasonCode.Success
            };
            var buildMessage = new MessageHeaderBuilder();
            var testResult = buildMessage.BuildMessage<SyndDto, SyndValidationRule>(SyndParameters, TransactionCode.Synd);
            Assert.AreEqual(testResult.ResultType, ResultTypes.Ok);
            Assert.IsNotNull(testResult.Payload);
            return testResult.Payload;
        }

        public void GetDataAfterTriggerForQtyDifference()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                Command = new OracleCommand();
                SwmFromMheSynd = SwmFromMheqtydefference(db, "SYND");
                Synd = JsonConvert.DeserializeObject<SyndDto>(SwmFromMheSynd.MessageJson);
                PldSnapQtyDefference = PldSnapTable(db, Synd.Sku, Convert.ToInt32(Synd.SynchronizationId));
                PickLocnAfterApi = PicklocationTable(db, Synd.Sku);
                WmsSyndData = FetchSyndData(db, Synd.SynchronizationId, Synd.Sku);
            }
        }

        public Data.Entities.SwmSynrPickLocationDetailSnapshot PldSnapTable(OracleConnection db, string skuid, int syncid)
        {
            var pldsnapDtl = new Data.Entities.SwmSynrPickLocationDetailSnapshot();
            var pldQuery = $"select * from SWM_SYNR_PLD_SNAPSHOT where sku_id='{skuid}' and SYNC_ID='{syncid}' order by sync_id desc ";
            Command = new OracleCommand(pldQuery, db);
            var pixReader = Command.ExecuteReader();
            if (pixReader.Read())
            {
                pldsnapDtl.ActualInventoryQuantity = Convert.ToInt32(pixReader[TestData.Pldsnapshot.ActualInventoryqty]);
            }
            return pldsnapDtl;
        }

        public PickLocationDetailsDto PicklocationTable(OracleConnection db, string skuid)
        {
            var pickLocnDtl = new PickLocationDetailsDto();
            var pickLocnView = $"{SynrQueries.PldFromSku}";
            Command = new OracleCommand(pickLocnView, db);
            Command.Parameters.Add(new OracleParameter(TestData.Parameter.SkuId, skuid));
            Command.Parameters.Add(new OracleParameter(TestData.Parameter.SysCodeType, Constants.SysCodeType));
            Command.Parameters.Add(new OracleParameter(TestData.Parameter.SysCodeId, Constants.SysCodeIdForActiveLocation));
            var pickLocnDtlReader = Command.ExecuteReader();
            if (pickLocnDtlReader.Read())
            {
                pickLocnDtl.ActualInventoryQuantity = Convert.ToDecimal(pickLocnDtlReader[PickLocationDetail.ActlInvnQty].ToString());
                pickLocnDtl.SkuId = (pickLocnDtlReader[PickLocationDetail.SkuId].ToString());
                pickLocnDtl.LocationId = pickLocnDtlReader[PickLocationDetail.LocnId].ToString();
            }
            return pickLocnDtl;
        }
        public Data.Entities.SwmSyndData FetchSyndData(OracleConnection db, string syncId, string skuid)
        {
            var syndtDataDto = new Data.Entities.SwmSyndData();
            SqlStatements = $"{SynrQueries.SyndDataQuery}";
            Command = new OracleCommand(SqlStatements, db);
            Command.Parameters.Add(new OracleParameter(TestData.Parameter.SyncId, syncId));
            Command.Parameters.Add(new OracleParameter(TestData.Parameter.SkuId, skuid));
            var validData = Command.ExecuteReader();
            if (validData.Read())
            {
                syndtDataDto.SynchronizationId = Convert.ToInt32(validData[SwmSyndData.Synchronizid]);
                syndtDataDto.SkuId = validData[SwmSyndData.Skuid].ToString();
                syndtDataDto.LocationId = validData[SwmSyndData.Wmslocnid].ToString();
                syndtDataDto.Quantity = Convert.ToInt32(validData[SwmSyndData.Qty]);
            }
            return syndtDataDto;
        }



        public void SyncGetValidData()
        {
            using (var db = GetOracleConnection())
            {

                db.Open();
                SynrMessageData = SwmToMhe(db,null,TransactionCode.Synr,null);
                Synr = JsonConvert.DeserializeObject<SynrDto>(SynrMessageData.MessageJson);
                SyndDataQtyDefference = SyndDataTable(db);
                PickLocnBeforeApi = PicklocationTable(db, SyndDataQtyDefference.SkuId);
              

            }

        }
        public Data.Entities.SwmSyndData SyndDataTable(OracleConnection db)
        {
            var syndData = new Data.Entities.SwmSyndData();
            var syndDataQuery = $"{SynrQueries.SyndDataFilterQuery}";
            Command = new OracleCommand(syndDataQuery, db);
            var syndReader = Command.ExecuteReader();
            if (syndReader.Read())
            {
                syndData.Quantity = Convert.ToInt32(syndReader[SwmSyndData.Qty]);
                syndData.SkuId = syndReader["SKU"].ToString();
                syndData.SynchronizationId = Convert.ToInt32(syndReader[SwmSyndData.Synchronizid]);
                syndData.MessageKey = Convert.ToInt32(syndReader[SwmSyndData.Messagekey]);
                syndData.LocationId = syndReader[SwmSyndData.Wmslocnid].ToString();
            }
            return syndData;
        }
        public PickLocationDetailsDto PickLocnTable(OracleConnection db)
        {
            var pldsnap = new PickLocationDetailsDto();
            var pldQuerys = $"{SynrQueries.PldTableQuery}";
            var command = new OracleCommand(pldQuerys, db);
            Command.Parameters.Add(new OracleParameter(TestData.Parameter.SysCodeType, Constants.SysCodeType));
            Command.Parameters.Add(new OracleParameter(TestData.Parameter.SysCodeId, Constants.SysCodeIdForActiveLocation));
            var pldReader = command.ExecuteReader();
            if (pldReader.Read())
            {

                pldsnap.SkuId = pldReader[PickLocationDetail.SkuId].ToString();
                pldsnap.ActualInventoryQuantity = Convert.ToInt32(pldReader[PickLocationDetail.ActlInvnQty]);
                pldsnap.LocationId = pldReader[TestData.Pldsnapshot.LocnId].ToString();
            }
            return pldsnap;
        }

        public void GetDataBeforeTriggerSync()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                var syncResult = CreateSyncMessage(Synr.SynchronizationId);
                EmsToWmsParameters = new EmsToWmsDto
                {
                    Process = DefaultPossibleValue.MessageProcessor,
                    MessageKey = Convert.ToInt64(SyncData.MsgKey),
                    Status = DefaultValues.Status,
                    Transaction = TransactionCode.Sync,
                    ResponseCode = (short)int.Parse(ReasonCode.Success),
                    MessageText = syncResult,
                };
                SyncData.MsgKey = InsertEmsToWms(db, EmsToWmsParameters);

            }
        }

        public string CreateSyncMessage(string syncid)
        {
            SyncParameters = new SyncDto()
            {
                SynchronizationId = syncid,
                TransactionCode = TransactionCode.Sync,
                MessageLength = MessageLength.Sync,
                ReasonCode = ReasonCode.Success
            };
            var buildMessage = new MessageHeaderBuilder();
            var testResult = buildMessage.BuildMessage<SyncDto, SyncValidationRule>(SyncParameters, TransactionCode.Sync);
            Assert.AreEqual(testResult.ResultType, ResultTypes.Ok);
            Assert.IsNotNull(testResult.Payload);
            return testResult.Payload;
        }

        public void GetDataAfterTriggerSync()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                SwmFromMheSyncDto = SwmFromMhe(db, SyncData.MsgKey, TransactionCode.Sync);
                Sync = JsonConvert.DeserializeObject<SyncDto>(SwmFromMheSyncDto.MessageJson);
                SwmFromMheSynd = SwmFromMheqtydefference(db, "SYND");
                Synd = JsonConvert.DeserializeObject<SyndDto>(SwmFromMheSynd.MessageJson);
                PldSnapQtyDefference = PldSnapTable(db, Synd.Sku, Convert.ToInt32(Synd.SynchronizationId));
                PickLocnAfterApi = PicklocationTable(db, Synd.Sku);
                SyndSum = GetSyndData(db, Synd.SynchronizationId, Synd.Sku);
                
            }
        }


        public int GetSyndData(OracleConnection db, string synchronization, string skuId)
        {
            Syndquery = $"{SynrQueries.SynrDataQty}";
            Command = new OracleCommand(Syndquery, db);
            Command.Parameters.Add(new OracleParameter(TestData.Parameter.SyncId, synchronization));
            Command.Parameters.Add(new OracleParameter(TestData.Parameter.SkuId, skuId));
            var syndReader = Convert.ToInt32(Command.ExecuteScalar().ToString());
            return syndReader;
        }


    }
}
