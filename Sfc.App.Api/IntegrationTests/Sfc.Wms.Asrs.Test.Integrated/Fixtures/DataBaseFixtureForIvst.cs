using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData;
using Sfc.Wms.Interfaces.ParserAndTranslator.Contracts.Dto;
using Sfc.Wms.Interfaces.ParserAndTranslator.Contracts.Constants;
using Sfc.Wms.Interfaces.ParserAndTranslator.Contracts.Validation;
using Sfc.Wms.Interfaces.Builder.MessageBuilder;
using System.Collections.Generic;
using Sfc.Core.OnPrem.Result;
using Sfc.Wms.Foundation.PixTransaction.Contracts.Dtos;
using Sfc.Wms.Interfaces.Asrs.Shamrock.Contracts.Dtos;
using Sfc.Wms.Interfaces.Asrs.Dematic.Contracts.Dtos;
using Sfc.Wms.Foundation.TransitionalInventory.Contracts.Dtos;
using Sfc.Wms.Foundation.InboundLpn.Contracts.Enums;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures
{
    [TestClass]
    public class DataBaseFixtureForIvst:CommonFunction
    {
        protected decimal UnitWeight;
        protected Ivst IvstData = new Ivst();
        protected Ivst InvShortageInbound = new Ivst();
        protected Ivst InvShortageOutbound = new Ivst();
        protected Ivst DamageInbound = new Ivst();
        protected Ivst DamageOutbound = new Ivst();
        protected Ivst WrongSku = new Ivst();
        protected Ivst WrongSkuOutbound = new Ivst();
        protected Ivst CycleCountAdjustmentPlus = new Ivst();
        protected Ivst CycleCountAdjustmentMinus = new Ivst();
        protected Ivst NoException = new Ivst();
        protected IvstDto Ivst = new IvstDto();
        protected new string Query = "";
        protected new SwmFromMheDto SwmFromMhe = new SwmFromMheDto();
        protected string PixTrnAfterApi;
        protected EmsToWmsDto EmsToWmsParameters;
        protected EmsToWmsDto EmsToWmsParametersInventoryShortage;
        protected EmsToWmsDto EmsToWmsParametersDamage;
        protected EmsToWmsDto EmsToWmsParametersWrongSku;
        protected EmsToWmsDto EmsToWmsParametersNoException;
        protected EmsToWmsDto EmsToWmsParametersCycleCount;
        protected IvstDto IvstParameters;
        protected Foundation.Location.Contracts.Dtos.PickLocationDetailsDto PickLocnDtlAfterApi = new Foundation.Location.Contracts.Dtos.PickLocationDetailsDto();
        protected Foundation.Location.Contracts.Dtos.PickLocationDetailsDto PickLcnDtlBeforeApi = new Foundation.Location.Contracts.Dtos.PickLocationDetailsDto();
        protected decimal Unitweight1;
        protected OracleCommand OracleCommand;
        protected PixTransactionDto Pixtran = new PixTransactionDto();
        protected TransitionalInventoryDto TrnsInvBeforeApi = new TransitionalInventoryDto();
        protected TransitionalInventoryDto TrnsInvAfterApi = new TransitionalInventoryDto();
        protected List<Scenarios> MsgkeyList = new List<Scenarios>();

        public void GetDataBeforeApiTrigger()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                IvstData = GetCaseDetailsForInsertingIvstMessage(db);
                Unitweight1 = FetchUnitWeight(db, IvstData.SkuId);
                TrnsInvBeforeApi = FetchTransInvnentory(db, IvstData.SkuId);
                PickLcnDtlBeforeApi = GetPickLocationDetails(db,IvstData.SkuId,null);                  
            }
        }

        public void InsertIvstMessagetUnexpectedFunction()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                InsertingUnexpectedOverage(db, IvstActionCode.AdjustmentPlus, IvstException.UnexpectedOverage,Constants.InboundPalletY);
                IvstData.Key = InsertEmsToWms(db, EmsToWmsParameters);           
            }
        }
        

        public void InsertIvstMessageForInventoryShortageInboundPalletIsY()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();             
                InsertingInventoryShortage(db, IvstActionCode.AdjustmentMinus, IvstException.InventoryShortage,Constants.InboundPalletY);
                InvShortageInbound.Key = InsertEmsToWms(db, EmsToWmsParametersInventoryShortage);
            }
        }

        public void InsertIvstMessageForInventoryShortageInboundPalletIsN()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();                
                var ivstResult = CreateIvstMessage(IvstData.CaseNumber, IvstData.SkuId, IvstData.Qty, IvstActionCode.AdjustmentMinus, IvstException.InventoryShortage, Constants.InboundPalletN);
                EmsToWmsParametersInventoryShortage = new EmsToWmsDto
                {
                    Process = DefaultPossibleValue.MessageProcessor,
                    Status = RecordStatus.Ready.ToString(),
                    Transaction = TransactionCode.Ivst,
                    MessageText = ivstResult
                };
                InvShortageOutbound.Key = InsertEmsToWms(db, EmsToWmsParametersInventoryShortage);
               
            }
        }

        public void InsertIvstMessageDamageForInboundPalletIsN()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                var ivstResult = CreateIvstMessage(IvstData.CaseNumber, IvstData.SkuId, IvstData.Qty, IvstActionCode.AdjustmentMinus, IvstException.Damage, Constants.InboundPalletN);
                EmsToWmsParametersDamage = new EmsToWmsDto
                {
                    Process = DefaultPossibleValue.MessageProcessor,
                    Status = RecordStatus.Ready.ToString(),
                    Transaction = TransactionCode.Ivst,
                    MessageText = ivstResult
                };
                DamageOutbound.Key = InsertEmsToWms(db, EmsToWmsParametersDamage);               
            }
        }

        public void InsertIvstMessageDamageFunction()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                InsertingDamage(db, IvstActionCode.AdjustmentMinus, IvstException.Damage,Constants.InboundPalletY);
                DamageInbound.Key = InsertEmsToWms(db, EmsToWmsParametersDamage);
            }
        }

        public void InsertIvstMessageWrongSkuFunction()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                InsertingWrongSku(db, IvstActionCode.AdjustmentMinus, IvstException.WrongSku,Constants.InboundPalletY);
                WrongSku.Key = InsertEmsToWms(db, EmsToWmsParametersWrongSku);    
            }
        }

        public void InsertIvstMessageForWrongSkuInboundPalletIsN()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                var ivstResult = CreateIvstMessage(IvstData.CaseNumber, IvstData.SkuId, IvstData.Qty, IvstActionCode.AdjustmentMinus, IvstException.WrongSku, Constants.InboundPalletN);
                EmsToWmsParametersWrongSku = new EmsToWmsDto
                {
                    Process = DefaultPossibleValue.MessageProcessor,
                    Status = RecordStatus.Ready.ToString(),
                    Transaction = TransactionCode.Ivst,
                    MessageText = ivstResult
                };
                WrongSkuOutbound.Key = InsertEmsToWms(db, EmsToWmsParametersWrongSku);
            }
        }

        public void InsertIvstMessageForCycleCountWithAdjustmentPlus()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                var ivstResult = CreateIvstMessage(IvstData.CaseNumber, IvstData.SkuId, IvstData.Qty, IvstActionCode.AdjustmentPlus, IvstException.CycleCount, Constants.InboundPalletN);
                EmsToWmsParametersCycleCount = new EmsToWmsDto
                {
                    Process = DefaultPossibleValue.MessageProcessor,
                    Status = RecordStatus.Ready.ToString(),
                    Transaction = TransactionCode.Ivst,
                    MessageText = ivstResult
                };
                CycleCountAdjustmentPlus.Key = InsertEmsToWms(db, EmsToWmsParametersCycleCount);
            }
        }

        public void InsertIvstMessageForCycleCountWithAdjustmentMinus()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                var ivstResult = CreateIvstMessage(IvstData.CaseNumber, IvstData.SkuId, IvstData.Qty, IvstActionCode.AdjustmentMinus, IvstException.CycleCount, Constants.InboundPalletN);
                EmsToWmsParametersCycleCount = new EmsToWmsDto
                {
                    Process = DefaultPossibleValue.MessageProcessor,
                    Status = RecordStatus.Ready.ToString(),
                    Transaction = TransactionCode.Ivst,
                    MessageText = ivstResult
                };
                CycleCountAdjustmentMinus.Key = InsertEmsToWms(db, EmsToWmsParametersCycleCount);
            }
        }

        public Ivst GetCaseDetailsForInsertingIvstMessage(OracleConnection db)
        {
            var ivstDataDto = new Ivst();
            Query = EmsToWmsQueries.CostQuery;
            Command = new OracleCommand(Query, db);
            Command.Parameters.Add(new OracleParameter("statCode", Constants.StatusCodeConsumed));
            Command.Parameters.Add(new OracleParameter("sysCodeType", Constants.SysCodeType));
            Command.Parameters.Add(new OracleParameter("codeId", Constants.SysCodeIdForActiveLocation));
            Command.Parameters.Add(new OracleParameter("ready", DefaultValues.Status));
            var validData = Command.ExecuteReader();
            if (validData.Read())
            {
                ivstDataDto.CaseNumber = validData[FieldName.ContainerId].ToString();
                ivstDataDto.SkuId = validData[FieldName.SkuId].ToString();
                ivstDataDto.Qty = validData[FieldName.Qty].ToString();
                ivstDataDto.LocnId = validData[FieldName.LocnId].ToString();
            }
            return ivstDataDto;
        }

        public void InsertingUnexpectedOverage(OracleConnection db, string actionCode, string exception, string inboundPallet)
        {
            Command = new OracleCommand(Query, db);
            var ivstmsg = CreateIvstMessage(IvstData.CaseNumber, IvstData.SkuId, IvstData.Qty, actionCode, exception,inboundPallet);
            EmsToWmsParameters = new EmsToWmsDto
            {
                Process = DefaultPossibleValue.MessageProcessor,
                Status = RecordStatus.Ready.ToString(),
                Transaction = TransactionCode.Ivst,
                MessageText = ivstmsg
            };
            
        }

        public void  InsertingInventoryShortage(OracleConnection db, string actionCode, string exception, string inboundPallet)
        {
            Command = new OracleCommand(Query, db);
            var ivstmsg = CreateIvstMessage(IvstData.CaseNumber, IvstData.SkuId, IvstData.Qty, actionCode, exception, inboundPallet);
            EmsToWmsParametersInventoryShortage = new EmsToWmsDto
            {
                Process = DefaultPossibleValue.MessageProcessor,
                Status = RecordStatus.Ready.ToString(),
                Transaction = TransactionCode.Ivst,
                MessageText = ivstmsg
            };       
        }
        public void InsertingDamage(OracleConnection db, string actionCode, string exception, string inboundPallet)
        {
            Command = new OracleCommand(Query, db);
            var ivstmsg = CreateIvstMessage(IvstData.CaseNumber, IvstData.SkuId, IvstData.Qty, actionCode, exception, inboundPallet);
            EmsToWmsParametersDamage = new EmsToWmsDto
            {
                Process = DefaultPossibleValue.MessageProcessor,
                Status = RecordStatus.Ready.ToString(),
                Transaction = TransactionCode.Ivst,
                MessageText = ivstmsg
            };           
        }

        public void InsertingWrongSku(OracleConnection db, string actionCode, string exception, string inboundPallet)
        {
            Command = new OracleCommand(Query, db);
            var ivstmsg = CreateIvstMessage(IvstData.CaseNumber, IvstData.SkuId, IvstData.Qty, actionCode, exception, inboundPallet);
            EmsToWmsParametersWrongSku = new EmsToWmsDto
            {
                Process = DefaultPossibleValue.MessageProcessor,
                Status = RecordStatus.Ready.ToString(),
                Transaction = TransactionCode.Ivst,
                MessageText = ivstmsg
            };           
        }

        public string CreateIvstMessage(string containerNbr, string skuId, string locationId, string actionCode, string adjustmentReasonCode,string inboundPallet)
        {
            IvstParameters = new IvstDto
            {
                ActionCode = actionCode,
                AdjustmentReasonCode = adjustmentReasonCode,
                ContainerId = "00100283000927952633",
                Quantity = Constants.MinQuantity,
                Sku = "3300121",
                Owner = Constants.Owner,
                UserName = Constants.UserName,
                UnitOfMeasure = DefaultValues.ContainerType,
                LotId = Constants.LotId,
                Po = Constants.Po,
                InboundPallet = inboundPallet,
                TransactionCode = TransactionCode.Ivst,
                MessageLength = MessageLength.Ivst,
                ReasonCode = ReasonCode.Success
            };
            var buildMessage = new MessageHeaderBuilder();
            var testResult = buildMessage.BuildMessage<IvstDto, IvstValidationRule>(IvstParameters, TransactionCode.Ivst);
            Assert.AreEqual(testResult.ResultType, ResultTypes.Ok);
            Assert.IsNotNull(testResult.Payload);
            return testResult.Payload;
        }

        public void GetDataAfterTrigger(long key)
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                SwmFromMhe = SwmFromMhe(db, key, TransactionCode.Ivst);
                Ivst = JsonConvert.DeserializeObject<IvstDto>(SwmFromMhe.MessageJson);
                TrnsInvAfterApi = FetchTransInvnentory(db, Ivst.Sku);
                PickLocnDtlAfterApi = GetPickLocationDetails(db, Ivst.Sku,null);                  
                UnitWeight = FetchUnitWeight(db, Ivst.Sku);
                var pixTrnAfterApi = PixTransactionTable(Ivst.AdjustmentReasonCode);
                Pixtran = GetPixtransaction(db, pixTrnAfterApi, SwmFromMhe.ContainerId);
            }
        }

        public PixTransactionDto GetPixtransaction(OracleConnection db, string rsnCode, string caseNbr)
        {
            var pixtran = new PixTransactionDto();
            Query = $"select * from Pix_tran where rsn_code= :reasonCode  and case_nbr = :caseNbr order by create_date_time desc";
            Command = new OracleCommand(Query, db);
            Command.Parameters.Add(new OracleParameter("reasonCode", rsnCode));
            Command.Parameters.Add(new OracleParameter("caseNbr", caseNbr));
            var rsn = Command.ExecuteReader();
            if (rsn.Read())
            {
                pixtran.ReasonCode = rsn["RSN_CODE"].ToString();
                pixtran.InventoryAdjustmentQuantity = Convert.ToDecimal(rsn["INVN_ADJMT_QTY"]);
                pixtran.InventoryAdjustmentType = rsn["INVN_ADJMT_TYPE"].ToString();
            }
            return pixtran;
        }

        public string PixTransactionTable(string adjustmentReasonCode)
        {
            switch (adjustmentReasonCode)
            {
                case IvstException.CycleCount:
                    return Constants.PixRsnCodeForCycleCount;
                case IvstException.UnexpectedOverage:
                    return Constants.PixRsnCodeForUnExpectedOverage;
                case IvstException.InventoryShortage:
                    return Constants.PixRsnCodeForInventoryShortage;
                case IvstException.Damage:
                    return Constants.PixRsnCodeForDamage;
                case IvstException.WrongSku:
                    return Constants.PixRsnCodeForWrongSku;
                default:
                    return Constants.PixRsnCodeForCycleCount;
            }
        }



    }
}
