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
        //protected PickLocndto SynrMessageData = new PickLocndto();
        protected SwmToMheDto SynrMessageData = new SwmToMheDto();
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
                SynrMessageData = SwmToMhe(db,null,TransactionCode.Synr,null);
                Synr = JsonConvert.DeserializeObject<SynrDto>(SynrMessageData.MessageJson);
                PldList = PickLocnTable(db);
            }
        }

        public PickLocationDetailsDto PickLocnTable(OracleConnection db)
        {
            var pldsnap = new PickLocationDetailsDto();
            var pldQuerys = $"{SynrQueries.pldTableQuery}";
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
                Owner = Constants.Owner,
                UnitOfMeasure =Constants.UnitofMesure,
                Quantity = qty,
                LocationStatus =Constants.AvailableSatus,
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
            SqlStatements = $"{SynrQueries.SyndQtydefferenceQuery}";
            Command = new OracleCommand(SqlStatements, db);
            Command.Parameters.Add(new OracleParameter("syncId", syncId));
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
                SynrMessageData = SwmToMhe(db,null,TransactionCode.Synr,null);
                Synr = JsonConvert.DeserializeObject<SynrDto>(SynrMessageData.MessageJson);
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
            var syndDataQuery = $"{SynrQueries.syndDataFilterQuery}";
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
            var pldQuery = $"{SynrQueries.PldSnapTableQuery}";
            Command = new OracleCommand(pldQuery, db);
            Command.Parameters.Add(new OracleParameter("syncId", syncid));
            Command.Parameters.Add(new OracleParameter("Skuid", skuid));
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
