using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using RestSharp;
using Sfc.Core.OnPrem.Result;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData;
using Sfc.Wms.Interfaces.Asrs.Dematic.Contracts.Dtos;
using Sfc.Wms.Interfaces.Asrs.Shamrock.Contracts.Dtos;
using Sfc.Wms.Interfaces.Builder.MessageBuilder;
using Sfc.Wms.Interfaces.ParserAndTranslator.Contracts.Constants;
using Sfc.Wms.Interfaces.ParserAndTranslator.Contracts.Dto;
using Sfc.Wms.Interfaces.ParserAndTranslator.Contracts.Validation;


namespace Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures
{

    public class DataBaseFixtureForSynr : CommonFunction
    {

        protected long Nextupcnt;
        protected string PickLocnViewquery = "";
        protected string SqlStatements = "";
        protected long Pldsnapshot;
        protected string PldSnapQty;
        protected string BeoforeApiPickLocn;
        protected SwmToMheDto SwmToMheSynr = new SwmToMheDto();
        protected WmsToEmsDto WmsToEmsSynr = new WmsToEmsDto();
        protected SynrDto Synr = new SynrDto();
        protected PickLocndto SynrMessageData = new PickLocndto();
        protected List<PickLocndto> PldList = new List<PickLocndto>();
        protected new string Query = "";
        protected SyndDto SyndParameters;
        protected EmsToWmsDto EmsToWmsParameters;
        protected SyndDto Synd = new SyndDto();
        protected synd SyndData = new synd();
        protected string BaseUrl = @ConfigurationManager.AppSettings["BaseUrl"];
        protected string SyndUrl;
        protected SwmFromMheDto SwmFromMheSynd = new SwmFromMheDto();
        protected IRestResponse Response;
        protected new SwmFromMheDto SwmFromMhe = new SwmFromMheDto();
        protected synd SyncData = new synd();
        protected SyncDto SyncParameters;
        protected SwmFromMheDto SwmFromMheSyncDto = new SwmFromMheDto();
        protected SyncDto Sync = new SyncDto();
        protected Data.Entities.SwmSyndData WmsSyndData = new Data.Entities.SwmSyndData();
        protected string PldSnapCount;
        protected string SyndDatacount;
        protected string SyndDataQty;




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
            Command.Parameters.Add(new OracleParameter("RecTypeID", Constants.RectTypeId));
            var nextUpCounterReader = Convert.ToInt64(Command.ExecuteScalar().ToString());
            return nextUpCounterReader;
        }


        public string GetPickLocationDetail(OracleConnection db)
        {
            PickLocnViewquery = $"{SynrQueries.PickLocnCountQuery}";
            Command = new OracleCommand(PickLocnViewquery, db);
            Command.Parameters.Add(new OracleParameter("sysCodeType", Constants.SysCodeType));
            Command.Parameters.Add(new OracleParameter("sysCodeId", Constants.SysCodeIdForActiveLocation));
            var pickLocnDtlReader = Command.ExecuteScalar().ToString();
            return pickLocnDtlReader;
        }

        public void GetDataAfterTrigger()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                Command = new OracleCommand();
                SwmToMheSynr = SwmToMhe(db, TransactionCode.Synr);
                Synr = JsonConvert.DeserializeObject<SynrDto>(SwmToMheSynr.MessageJson);
                WmsToEmsSynr = WmsToEmsData(db, SwmToMheSynr.SourceMessageKey, TransactionCode.Synr);
                Pldsnapshot = FetchPldsnapshottable(db, (Nextupcnt + 1).ToString());

            }
        }

        public Int64 FetchPldsnapshottable(OracleConnection db, string syncId)
        {
            var querypld = $"{SynrQueries.PLdSnapShotQuery}";
            Command = new OracleCommand(querypld, db);
            Command.Parameters.Add(new OracleParameter("syncId", syncId));
            Command.Parameters.Add(new OracleParameter("sysCodeType", Constants.SysCodeType));
            Command.Parameters.Add(new OracleParameter("sysCodeId", Constants.SysCodeIdForActiveLocation));

            var pldsnapshotReader = Convert.ToInt64(Command.ExecuteScalar().ToString());
            return pldsnapshotReader;
        }

        public void GetValidData()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                SynrMessageData = GetSyncIdInsertingSynrMessage(db);
                Synr = JsonConvert.DeserializeObject<SynrDto>(SynrMessageData.Messsagejson);
                PldList = PickLocnTable(db);
            }
        }

        public List<PickLocndto> PickLocnTable(OracleConnection db)
        {
            var pldsnap = new List<PickLocndto>();
            var pldQuerys = $"{SynrQueries.ListPldSkuQuery}";
            var command = new OracleCommand(pldQuerys, db);
            //Command.Parameters.Add(new OracleParameter("sysCodeType", Constants.SysCodeType));
            //Command.Parameters.Add(new OracleParameter("sysCodeId", Constants.SysCodeIdForActiveLocation));
            var pldReader = command.ExecuteReader();
            while (pldReader.Read())
            {
                var set = new PickLocndto
                {
                    Skuid = pldReader[FieldName.SkuId].ToString(),
                    ActualQuantity = (pldReader[FieldName.ActlInvnQty].ToString()),
                    LocationId = pldReader[FieldName.LocnId].ToString()

                };
                pldsnap.Add(set);
            }
            return pldsnap;
        }

        public void ValidateForSyndMessagesInsertedIntoSwmToMheTableAndSyndDataTableWithAppropiateValues()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                for (var i = 0; i < PldList.Count; i++)
                {
                    var syndResult = CreateSyndMessage(Synr.SynchronizationId, PldList[i].Skuid, PldList[i].ActualQuantity);
                    EmsToWmsParameters = new EmsToWmsDto
                    {
                        Process = DefaultPossibleValue.MessageProcessor,
                        MessageKey = Convert.ToInt64(SyndData.MsgKey),
                        Status = DefaultValues.Status,
                        Transaction = TransactionCode.Synd,
                        ResponseCode = (short)int.Parse(ReasonCode.Success),
                        MessageText = syndResult,
                    };
                    SyndData.MsgKey = InsertEmsToWms(db, EmsToWmsParameters);
                    SyndUrl = $"{BaseUrl}{TestData.Parameter.EmsToWmsMessage}?{TestData.Parameter.MsgKey}={SyndData.MsgKey}&{TestData.Parameter.MsgProcessor}={EmsToWmsParameters.Process}";
                    var response = ApiIsCalled(SyndUrl);
                    var result = JsonConvert.DeserializeObject<BaseResult>(response.Content);
                    Assert.AreEqual(ResultTypes.Created.ToString(), result.ResultType.ToString());
                    SwmFromMheSynd = SwmFromMhe(db, SyndData.MsgKey, "SYND");
                    Synd = JsonConvert.DeserializeObject<SyndDto>(SwmFromMheSynd.MessageJson);
                    var syndDtl = PldList[i];
                    VerifySyndMessageWasInsertedIntoSwmFromMhe(Synd, syndDtl);
                    WmsSyndData = FetchSyndData(db, Synd.SynchronizationId, PldList[i].Skuid);
                    VerifySyndMessageWasInsertedIntoSwmFromMheTableandSyndDataTable();
                }

            }
        }

        public string CreateSyndMessage(string syncid, string skuid, string qty)
        {
            SyndParameters = new SyndDto()
            {
                SynchronizationId = syncid,
                Sku = skuid,
                Owner = Constants.Owner,
                UnitOfMeasure =Constants.UnitofMesure,
                Quantity = qty,
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

        protected IRestResponse ApiIsCalled(string syndUrl)
        {
            var client = new RestClient(syndUrl);
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", Contents.ContentType);
            request.RequestFormat = DataFormat.Json;
            Response = client.Execute(request);
            return Response;
        }
        public Data.Entities.SwmSyndData FetchSyndData(OracleConnection db, string syncId, string skuid)
        {
            var syndtDataDto = new Data.Entities.SwmSyndData();
            SqlStatements = $"{SynrQueries.SyndDataQuery}";
            Command = new OracleCommand(SqlStatements, db);
            Command.Parameters.Add(new OracleParameter("SyncId", syncId));
            Command.Parameters.Add(new OracleParameter("Skuid", skuid));
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

        protected void VerifySyndMessageWasInsertedIntoSwmFromMheTableandSyndDataTable()
        {
            Assert.AreEqual(WmsSyndData.SynchronizationId, int.Parse(Synd.SynchronizationId));
            Assert.AreEqual(WmsSyndData.SkuId, Synd.Sku);
        }


        protected void VerifySyndMessageWasInsertedIntoSwmFromMhe(SyndDto synd, PickLocndto syndDtl)
        {
            Assert.AreEqual(EmsToWmsParameters.Process, SwmFromMheSynd.SourceMessageProcess);
            Assert.AreEqual(SyndData.MsgKey, SwmFromMheSynd.SourceMessageKey);
            Assert.AreEqual(EmsToWmsParameters.Status, SwmFromMheSynd.SourceMessageStatus);
            Assert.AreEqual(EmsToWmsParameters.Transaction, SwmFromMheSynd.SourceMessageTransactionCode);
            Assert.AreEqual(EmsToWmsParameters.MessageText, SwmFromMheSynd.SourceMessageText);
            Assert.AreEqual(syndDtl.Skuid, synd.Sku);
            Assert.AreEqual(syndDtl.ActualQuantity, synd.Quantity);

        }

        public void SyncGetValidData()
        {
            using (var db = GetOracleConnection())
            {

                db.Open();
                SynrMessageData = GetSyncIdInsertingSynrMessage(db);
                Synr = JsonConvert.DeserializeObject<SynrDto>(SynrMessageData.Messsagejson);
                
            }

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
                PldSnapCount = FetchPldsnapshottables(db, Sync.SynchronizationId);
                SyndDatacount = FetchSyndDataTables(db,Sync.SynchronizationId);
                PldSnapQty = FetchQtyPldSnapshotTable(db, Sync.SynchronizationId);
                SyndDataQty = FetchSyndDataQty(db,Sync.SynchronizationId);


            }
           
        }

        public string FetchSyndDataQty(OracleConnection db, string syncId)
        {
            var querysyndqty = $"select SUM(SWM_SYND_DATA.Quantity) from SWM_SYND_DATA where SYNCHRONIZATION_ID='{syncId}'and status=90 ";
            Command = new OracleCommand(querysyndqty, db);
            var syndDataQtyReader = Command.ExecuteScalar().ToString();
            return syndDataQtyReader;
        }
        public string FetchQtyPldSnapshotTable(OracleConnection db, string syncId)
        {
            var querypldqty = $"select SUM(pld.ACTL_INVN_QTY) from SWM_SYNR_PLD_SNAPSHOT pld join item_master i on  pld.sku_id = i.sku_id where pld.SYNC_ID = '{syncId}' and locn_id|| (DECODE(i.temp_zone, 'D', 'Dry', 'Freezer')) in (select lh.locn_id || lg.grp_attr from locn_hdr lh inner join locn_grp lg on lg.locn_id = lh.locn_id inner join sys_code sc on sc.code_id = lg.grp_type and sc.code_type = '740' and sc.code_id = '18')";
            Command = new OracleCommand(querypldqty, db);
            var pldsnapshotQtyReader = Command.ExecuteScalar().ToString();
            return pldsnapshotQtyReader;
        }

        public string FetchPldsnapshottables(OracleConnection db, string syncId)
        {
            var querypld = $"select Count(*) from SWM_SYNR_PLD_SNAPSHOT pld join item_master i on  pld.sku_id = i.sku_id where pld.SYNC_ID = '{syncId}' and locn_id|| (DECODE(i.temp_zone, 'D', 'Dry', 'Freezer')) in (select lh.locn_id || lg.grp_attr from locn_hdr lh inner join locn_grp lg on lg.locn_id = lh.locn_id inner join sys_code sc on sc.code_id = lg.grp_type and sc.code_type = '740' and sc.code_id = '18')";
            Command = new OracleCommand(querypld, db);
            var pldsnapshotReader = Command.ExecuteScalar().ToString();
            return pldsnapshotReader;
        }

        public string FetchSyndDataTables(OracleConnection db, string syncId)
        {
            var querysynd = $"select Count(*) from SWM_SYND_DATA where SYNCHRONIZATION_ID='{syncId}'and status=90";
            Command = new OracleCommand(querysynd, db);
            var syndDataReader = Command.ExecuteScalar().ToString();
            return syndDataReader;
        }
    }
}
