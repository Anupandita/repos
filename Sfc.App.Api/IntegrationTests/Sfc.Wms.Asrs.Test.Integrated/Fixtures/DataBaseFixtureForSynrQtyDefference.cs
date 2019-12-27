using System;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using RestSharp;
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

    public class DataBaseFixtureForSynrQtyDefference : CommonFunction
    {
        protected long Nextupcnt;
        protected string BeoforeApiPickLocn;
        protected string PickLocnViewquery = "";
        protected SwmToMheDto SwmToMheSynr = new SwmToMheDto();
        protected SynrDto Synr = new SynrDto();
        protected WmsToEmsDto WmsToEmsSynr = new WmsToEmsDto();
        protected long Pldsnapshot;
        protected new string Query = "";
        protected string Syndquery = "";
        protected PickLocndto SynrMessageData = new PickLocndto();
        protected PickLocationDetailsDto PldList = new PickLocationDetailsDto();
        protected EmsToWmsDto EmsToWmsParameters;
        protected synd SyndData = new synd();
        protected string BaseUrl = @ConfigurationManager.AppSettings["BaseUrl"];
        protected SwmFromMheDto SwmFromMheSynd = new SwmFromMheDto();
        protected SyndDto SyndParameters;
        protected IRestResponse Response;
        protected new SwmFromMheDto SwmFromMhe = new SwmFromMheDto();
        protected SyndDto Synd = new SyndDto();
        protected SyncDto SyncParameters;
        protected synd SyncData = new synd();
        protected synd SyncDataDuplicate = new synd();
        protected SwmFromMheDto SwmFromMheSyncDto = new SwmFromMheDto();
        protected SyncDto Sync = new SyncDto();
        protected PickLocationDetailsDto Picklocndetail = new PickLocationDetailsDto();
        protected string SyndUrl;
        protected string SqlStatements;
        protected Data.Entities.PixTransaction PixTran =new  Data.Entities.PixTransaction();
        protected Data.Entities.SwmSyndData WmsSyndData = new Data.Entities.SwmSyndData();
        protected Data.Entities.SwmSynrPickLocationDetailSnapshot PldSnapQtyDefference = new Data.Entities.SwmSynrPickLocationDetailSnapshot();
        protected Data.Entities.SwmSyndData SyndDataQtyDefference = new Data.Entities.SwmSyndData();
        protected PickLocationDetailsDto PickLocnAfterApi = new PickLocationDetailsDto();
        protected PickLocationDetailsDto PickLocnBeforeApi = new PickLocationDetailsDto();
        protected int SyndSum ;
        



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
            var query = $"select max(CURR_NBR) from nxt_up_cnt where rec_type_id = 'SYN'";
            Command = new OracleCommand(query, db);
            var nextUpCounterReader = Convert.ToInt64(Command.ExecuteScalar().ToString());
            return nextUpCounterReader;
        }
        public string GetPickLocationDetail(OracleConnection db)
        {
            PickLocnViewquery = $"select Count(*) from pick_locn_dtl p join item_master i on  p.sku_id = i.sku_id where  locn_id||(DECODE(i.temp_zone, 'D', 'Dry', 'Freezer')) in (select lh.locn_id||lg.grp_attr from locn_hdr lh inner join locn_grp lg on lg.locn_id = lh.locn_id inner join sys_code sc on sc.code_id = lg.grp_type and sc.code_type = '740' and sc.code_id = '18')";
            Command = new OracleCommand(PickLocnViewquery, db);
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
            var querypld = $"select Count(*) from SWM_SYNR_PLD_SNAPSHOT pld join item_master i on  pld.sku_id = i.sku_id where pld.SYNC_ID = '{syncId}' and locn_id|| (DECODE(i.temp_zone, 'D', 'Dry', 'Freezer')) in (select lh.locn_id || lg.grp_attr from locn_hdr lh inner join locn_grp lg on lg.locn_id = lh.locn_id inner join sys_code sc on sc.code_id = lg.grp_type and sc.code_type = '740' and sc.code_id = '18')";
            Command = new OracleCommand(querypld, db);
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

        public PickLocationDetailsDto PickLocnTable(OracleConnection db)
        {
            var pldsnap = new PickLocationDetailsDto();
            var pldQuerys = $"select * from pick_locn_dtl p join item_master i on  p.sku_id = i.sku_id where p.ACTL_INVN_QTY! = 0 and  locn_id||(DECODE(i.temp_zone, 'D', 'Dry', 'Freezer')) in (select lh.locn_id||lg.grp_attr from locn_hdr lh inner join locn_grp lg on lg.locn_id = lh.locn_id inner join sys_code sc on sc.code_id = lg.grp_type and sc.code_type = '740' and sc.code_id = '18')";
            var command = new OracleCommand(pldQuerys, db);
            var pldReader = command.ExecuteReader();
            if (pldReader.Read())
            {

                pldsnap.SkuId = pldReader[PickLocationDetail.SkuId].ToString();
                pldsnap.ActualInventoryQuantity = Convert.ToInt32(pldReader[PickLocationDetail.ActlInvnQty]);
                pldsnap.LocationId = pldReader[TestData.Pldsnapshot.LocnId].ToString();

            
            
            }
            return pldsnap;
        }

        public void ValidateForSyndMessagesInsertedIntoSwmToMheTableAndSyndDataTableWithAppropiateValues()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                for (var i = 0; i < 1; i++)
                {
                    var syndResult = CreateSyndMessage(Synr.SynchronizationId,PldList.SkuId,Convert.ToInt32((PldList.ActualInventoryQuantity)+1).ToString());
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
                    var syndDtl = PldList;
                    VerifySyndMessageWasInsertedIntoSwmFromMhe(Synd, syndDtl);
                    WmsSyndData = FetchSyndData(db, Synd.SynchronizationId, PldList.SkuId);
                    VerifySyndMessageWasInsertedIntoSwmFromMheTableandSyndDataTable();
                }
            }
        }
        public string CreateSyndMessage(string syncid, string skuid,string qty)
        {
            SyndParameters = new SyndDto()
            {
                SynchronizationId = syncid,
                Sku = skuid,
                Owner = "008",
                UnitOfMeasure = "Case",
                Quantity = qty,
                LocationStatus = "Available",
                HoldStatus = "Y",
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
            SqlStatements = $"select * from SWM_SYND_DATA where SYNCHRONIZATION_ID='{syncId}' and SKU='{skuid}'";
            Command = new OracleCommand(SqlStatements, db);
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
            //Assert.AreEqual(Convert.ToString(WmsSyndData.Quantity), Synd.Quantity);
            //Assert.AreEqual(WmsSyndData.LocationId, SwmFromMheSynd.LocationId);
        }

        protected void VerifySyndMessageWasInsertedIntoSwmFromMhe(SyndDto synd, PickLocationDetailsDto syndDtl)
        {
            Assert.AreEqual(EmsToWmsParameters.Process, SwmFromMheSynd.SourceMessageProcess);
            Assert.AreEqual(SyndData.MsgKey, SwmFromMheSynd.SourceMessageKey);
            Assert.AreEqual(EmsToWmsParameters.Status, SwmFromMheSynd.SourceMessageStatus);
            Assert.AreEqual(EmsToWmsParameters.Transaction, SwmFromMheSynd.SourceMessageTransactionCode);
            Assert.AreEqual(EmsToWmsParameters.MessageText, SwmFromMheSynd.SourceMessageText);
            Assert.AreEqual(syndDtl.SkuId, synd.Sku);

        }
        public void SyncGetValidData()
        {
            using (var db = GetOracleConnection())
            {

                db.Open();
                SynrMessageData = GetSyncIdInsertingSynrMessage(db);
                Synr = JsonConvert.DeserializeObject<SynrDto>(SynrMessageData.Messsagejson);
                SyndDataQtyDefference = SyndDataTable(db);
                PickLocnBeforeApi = PickLocnTable(db);
                  
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
                SyndDataQtyDefference = SyndDataTable(db);
                PldSnapQtyDefference = PldSnapTable(db, SyndDataQtyDefference.SkuId, SyndDataQtyDefference.SynchronizationId);
                PickLocnAfterApi = GetPickLocationDetails(db, SyndDataQtyDefference.SkuId,null);
                PixTran = PixTransactionTable(db,Sync.SynchronizationId);
            }
        }

        public Data.Entities.SwmSyndData SyndDataTable(OracleConnection db)
        {
            var syndData = new Data.Entities.SwmSyndData();
            var syndDataQuery = $"select * from SWM_SYND_DATA where status= 90 order by SYNCHRONIZATION_ID desc ";
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

        public Data.Entities.SwmSynrPickLocationDetailSnapshot PldSnapTable(OracleConnection db,string skuid,int syncid)
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


        public Data.Entities.PixTransaction PixTransactionTable(OracleConnection db,string syncId)
        {
            var pixTranDtl = new Data.Entities.PixTransaction();
            var pixTranQuery = $"select * from pix_tran where ref_field_2='{syncId}'";
            Command = new OracleCommand(pixTranQuery, db);
            var pixReader = Command.ExecuteReader();
            if (pixReader.Read())
            {
                pixTranDtl.Style = pixReader[PixTransaction.Style].ToString();
                pixTranDtl.InventoryAdjustmentQuantity = Convert.ToInt32(pixReader[PixTransaction.InventoryAdjustmentQty]);
                pixTranDtl.InventoryAdjustmentType = pixReader[PixTransaction.InventoryAdjustmentType].ToString();
                pixTranDtl.ReferenceField1= pixReader[PixTransaction.RefferenceField1].ToString();
                pixTranDtl.ReferenceField2 = pixReader[PixTransaction.RefferenceField2].ToString();
                pixTranDtl.ReferenceField4= pixReader[PixTransaction.RefferenceField4].ToString();
            }
            return pixTranDtl;
        }


    }
}
