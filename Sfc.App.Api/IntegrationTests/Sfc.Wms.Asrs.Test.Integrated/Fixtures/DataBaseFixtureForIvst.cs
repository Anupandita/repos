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
using Sfc.Wms.Foundation.PickLocationDetail.Contracts.Dtos;

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
        protected string Query = "";
        protected new SwmFromMheDto SwmFromMhe = new SwmFromMheDto();
        protected string PixTrnAfterApi;
        protected EmsToWmsDto EmsToWmsParameters;
        protected EmsToWmsDto EmsToWmsParametersInventoryShortage;
        protected EmsToWmsDto EmsToWmsParametersDamage;
        protected EmsToWmsDto EmsToWmsParametersWrongSku;
        protected EmsToWmsDto EmsToWmsParametersNoException;
        protected EmsToWmsDto EmsToWmsParametersCycleCount;
        protected IvstDto IvstParameters;
        protected PickLocationDtlDto PickLocnDtlAfterApi = new PickLocationDtlDto();
        protected PickLocationDtlDto PickLcnDtlBeforeApi = new PickLocationDtlDto();
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
                PickLcnDtlBeforeApi = PickLocnData(db, IvstData.SkuId);
            }
        }

        public void InsertIvstMessagetUnexpectedFunction()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                InsertingUnexpectedOverage(db, IvstActionCode.AdjustmentPlus, IvstException.UnexpectedOverage,"Y");
                IvstData.Key = InsertEmsToWms(db, EmsToWmsParameters);           
            }
        }


        public void InsertIvstMessageForInventoryShortageInboundPalletIsY()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
               
                InsertingInventoryShortage(db, IvstActionCode.AdjustmentMinus, IvstException.InventoryShortage,"Y");
                InvShortageInbound.Key = InsertEmsToWms(db, EmsToWmsParametersInventoryShortage);
            }
        }

        public void InsertIvstMessageForInventoryShortageInboundPalletIsN()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();                
                var ivstResult = CreateIvstMessage(IvstData.CaseNumber, IvstData.SkuId, IvstData.Qty, IvstActionCode.AdjustmentMinus, IvstException.InventoryShortage, "N");
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
                var ivstResult = CreateIvstMessage(IvstData.CaseNumber, IvstData.SkuId, IvstData.Qty, IvstActionCode.AdjustmentMinus, IvstException.Damage, "N");
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
                InsertingDamage(db, IvstActionCode.AdjustmentMinus, IvstException.Damage,"Y");
                DamageInbound.Key = InsertEmsToWms(db, EmsToWmsParametersDamage);
            }
        }

        public void InsertIvstMessageWrongSkuFunction()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                InsertingWrongSku(db, IvstActionCode.AdjustmentMinus, IvstException.WrongSku,"Y");
                WrongSku.Key = InsertEmsToWms(db, EmsToWmsParametersWrongSku);    
            }
        }

        public void InsertIvstMessageForWrongSkuInboundPalletIsN()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                var ivstResult = CreateIvstMessage(IvstData.CaseNumber, IvstData.SkuId, IvstData.Qty, IvstActionCode.AdjustmentMinus, IvstException.WrongSku, "N");
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
                var ivstResult = CreateIvstMessage(IvstData.CaseNumber, IvstData.SkuId, IvstData.Qty, IvstActionCode.AdjustmentPlus, IvstException.CycleCount, "N");
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
                var ivstResult = CreateIvstMessage(IvstData.CaseNumber, IvstData.SkuId, IvstData.Qty, IvstActionCode.AdjustmentMinus, IvstException.CycleCount, "N");
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
            Query = $"select swm_to_mhe.container_id,swm_to_mhe.sku_id,pick_locn_dtl.locn_id,swm_to_mhe.qty from swm_to_mhe inner join trans_invn" +
                $" on trans_invn.sku_id = swm_to_mhe.sku_id inner join  pick_locn_dtl on swm_to_mhe.sku_id = pick_locn_dtl.sku_id  " +
                $"inner join case_hdr on swm_to_mhe.container_id = case_hdr.case_nbr and swm_to_mhe.source_msg_status = 'Ready' and swm_to_mhe.qty!= 0 and case_hdr.stat_code = 96"+
                $" and pick_locn_dtl.locn_id in (select lh.locn_id from locn_hdr lh inner join locn_grp lg on lg.locn_id = lh.locn_id inner join sys_code sc on sc.code_id = lg.grp_type and sc.code_type = '740' and sc.code_id = '18')";
            Command = new OracleCommand(Query, db);
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

        public void InsertingUnexpectedOverage(OracleConnection db, string actionCode, string exception, string InboundPallet)
        {
            Command = new OracleCommand(Query, db);
            var ivstmsg = CreateIvstMessage(IvstData.CaseNumber, IvstData.SkuId, IvstData.Qty, actionCode, exception,InboundPallet);
            EmsToWmsParameters = new EmsToWmsDto
            {
                Process = DefaultPossibleValue.MessageProcessor,
                Status = RecordStatus.Ready.ToString(),
                Transaction = TransactionCode.Ivst,
                MessageText = ivstmsg
            };
            
        }

        public void  InsertingInventoryShortage(OracleConnection db, string actionCode, string exception, string InboundPallet)
        {
            Command = new OracleCommand(Query, db);
            var ivstmsg = CreateIvstMessage(IvstData.CaseNumber, IvstData.SkuId, IvstData.Qty, actionCode, exception, InboundPallet);
            EmsToWmsParametersInventoryShortage = new EmsToWmsDto
            {
                Process = DefaultPossibleValue.MessageProcessor,
                Status = RecordStatus.Ready.ToString(),
                Transaction = TransactionCode.Ivst,
                MessageText = ivstmsg
            };       
        }
        public void InsertingDamage(OracleConnection db, string actionCode, string exception, string InboundPallet)
        {
            Command = new OracleCommand(Query, db);
            var ivstmsg = CreateIvstMessage(IvstData.CaseNumber, IvstData.SkuId, IvstData.Qty, actionCode, exception, InboundPallet);
            EmsToWmsParametersDamage = new EmsToWmsDto
            {
                Process = DefaultPossibleValue.MessageProcessor,
                Status = RecordStatus.Ready.ToString(),
                Transaction = TransactionCode.Ivst,
                MessageText = ivstmsg
            };           
        }

        public void InsertingWrongSku(OracleConnection db, string actionCode, string exception, string InboundPallet)
        {
            Command = new OracleCommand(Query, db);
            var ivstmsg = CreateIvstMessage(IvstData.CaseNumber, IvstData.SkuId, IvstData.Qty, actionCode, exception, InboundPallet);
            EmsToWmsParametersWrongSku = new EmsToWmsDto
            {
                Process = DefaultPossibleValue.MessageProcessor,
                Status = RecordStatus.Ready.ToString(),
                Transaction = TransactionCode.Ivst,
                MessageText = ivstmsg
            };           
        }

        public string CreateIvstMessage(string containerNbr, string skuId, string locationId, string actionCode, string adjustmentReasonCode,string InboundPallet)
        {
            IvstParameters = new IvstDto
            {
                ActionCode = actionCode,
                AdjustmentReasonCode = adjustmentReasonCode,
                ContainerId = containerNbr,
                Quantity = "1",
                Sku = skuId,
                Owner = "Wms",
                UserName = "Dematic",
                UnitOfMeasure = "Case",
                LotId = "1231",
                Po = "2445",
                InboundPallet = InboundPallet,
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

        public PickLocationDtlDto PickLocnData(OracleConnection db, string skuId)
        {
            var pickLocn = new PickLocationDtlDto();
            Query = $"select * from pick_locn_dtl where sku_id = '{skuId}' and locn_id = '{IvstData.LocnId}' order by mod_date_time desc";
            Command = new OracleCommand(Query, db);
            var pickLocnReader = Command.ExecuteReader();
            if (pickLocnReader.Read())
            {
                pickLocn.ActualInventoryQuantity = Convert.ToDecimal(pickLocnReader[FieldName.ActlInvnQty].ToString());
                pickLocn.ToBeFilledQty = Convert.ToDecimal(pickLocnReader[FieldName.ToBeFilledQty].ToString());
                pickLocn.LocationId = pickLocnReader[FieldName.LocnId].ToString();
            }
            return pickLocn;
        }

        public void GetDataAfterTrigger(long key)
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                SwmFromMhe = SwmFromMhe(db, key, TransactionCode.Ivst);
                Ivst = JsonConvert.DeserializeObject<IvstDto>(SwmFromMhe.MessageJson);
                TrnsInvAfterApi = FetchTransInvnentory(db, Ivst.Sku);
                PickLocnDtlAfterApi = PickLocnData(db, Ivst.Sku);
                UnitWeight = FetchUnitWeight(db, Ivst.Sku);
                var pixTrnAfterApi = PixTransactionTable(Ivst.AdjustmentReasonCode);
                Pixtran = GetPixtransaction(db, pixTrnAfterApi);
            }
        }

        public PixTransactionDto GetPixtransaction(OracleConnection db, string rsnCode)
        {
            var pixtran = new PixTransactionDto();
            Query = $"select * from Pix_tran where rsn_code='{rsnCode}'order by create_date_time desc";
            Command = new OracleCommand(Query, db);
            var rsn = Command.ExecuteReader();

            if (rsn.Read())
            {
                pixtran.ReasonCode = (rsn["RSN_CODE"].ToString());
            }
            return pixtran;
        }

        public string PixTransactionTable(string adjustmentReasonCode)
        {
            if (adjustmentReasonCode == "0001")
            {
                return "CC";
            }
            else if (adjustmentReasonCode == "0002")
            {
                return "CO";
            }
            else if (adjustmentReasonCode == "0003")
            {
                return "CS";
            }
            else if (adjustmentReasonCode == "0004")
            {
                return "DG";
            }
            else if (adjustmentReasonCode == "0005")
            {
                return "CC";
            }
            return "CC";
        }
    }
}
