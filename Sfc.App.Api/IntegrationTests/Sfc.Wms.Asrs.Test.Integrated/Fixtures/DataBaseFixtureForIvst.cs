using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using Sfc.Wms.Asrs.Dematic.Contracts.Dtos;
using Sfc.Wms.Asrs.Shamrock.Contracts.Dtos;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData;
using Sfc.Wms.PickLocationDetail.Contracts.Dtos;
using Sfc.Wms.TransitionalInventory.Contracts.Dtos;
using System.Configuration;
using Sfc.Wms.Interfaces.ParserAndTranslator.Contracts.Dto;
using Sfc.Wms.Interfaces.ParserAndTranslator.Contracts.Constants;
using Sfc.Wms.Interfaces.ParserAndTranslator.Contracts.Validation;
using Sfc.Wms.Interfaces.Builder.MessageBuilder;
using Sfc.Wms.Asrs.Dematic.Contracts.EnumsAndConstants.Enums;
using System.Collections.Generic;
using Sfc.Core.OnPrem.Result;
using Sfc.Wms.Foundation.PixTransaction.Contracts.Dtos;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures
{
    [TestClass]
    public class DataBaseFixtureForIvst:CommonFunction
    {
        public decimal unitWeight;
        protected Ivst IvstData = new Ivst();
        protected Ivst Invshort = new Ivst();
        protected Ivst Damage = new Ivst();
        protected Ivst WrongSku = new Ivst();
        protected Ivst NoException = new Ivst();
        protected IvstDto Ivst = new IvstDto();
        protected string Query = "";
        protected SwmFromMheDto swmFromMhe = new SwmFromMheDto();
        protected string PixTrnAfterApi;
        protected EmsToWmsDto emsToWmsParameters;
        protected EmsToWmsDto emsToWmsParametersInventoryShortage;
        protected EmsToWmsDto emsToWmsParametersDamage;
        protected EmsToWmsDto emsToWmsParametersWrongSku;
        protected EmsToWmsDto emsToWmsParametersNoException;
        protected IvstDto IvstParameters;
       
        protected PickLocationDtlDto pickLocnDtlAfterApi = new PickLocationDtlDto();
        protected PickLocationDtlDto pickLcnDtlBeforeApi = new PickLocationDtlDto();
        public decimal unitweight1;
        protected OracleCommand oracleCommand;
        protected PixTransactionDto pixtran = new PixTransactionDto();
        protected TransitionalInventoryDto trnsInvBeforeApi = new TransitionalInventoryDto();
        protected TransitionalInventoryDto trnsInvAfterApi = new TransitionalInventoryDto();
        protected List<Scenarios> msgkeyList = new List<Scenarios>();



        public DataBaseFixtureForIvst()
        {
            // _dataTypeValidation = new DataTypeValidation();
        }



        public void GetDataBeforeApiTrigger()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                IvstData = GetCaseDetailsForInsertingIvstMessage(db);
                unitweight1 = FetchUnitWeight(db, IvstData.SkuId);
                trnsInvBeforeApi = FetchTransInvnentory(db, IvstData.SkuId);
                pickLcnDtlBeforeApi = PickLocnData(db, IvstData.SkuId);
            }
        }

        public void InsertIvstMessagetUnexpectedFunction()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();

                InsertingUnexpectedOverage(db, IvstActionCode.AdjustmentPlus, IvstException.UnexpectedOverage);
            }

        }

        public void InsertIvstMessageInventoryFunction()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();

                InsertingInventoryShortage(db, IvstActionCode.AdjustmentMinus, IvstException.InventoryShortage);
            }

        }

        public void InsertIvstMessageDamageFunction()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();

                InsertingDamage(db, IvstActionCode.AdjustmentMinus, IvstException.Damage);
            }

        }
        public void InsertIvstMessageWrongSkuFunction()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();

                InsertingWrongSku(db, IvstActionCode.AdjustmentMinus, IvstException.WrongSku);
            }

        }


        public Ivst GetCaseDetailsForInsertingIvstMessage(OracleConnection db)
        {
            var IvstDataDto = new Ivst();
            Query = $"select swm_to_mhe.container_id,swm_to_mhe.sku_id,pick_locn_dtl.locn_id,swm_to_mhe.qty from swm_to_mhe inner join trans_invn" +
                $" on trans_invn.sku_id = swm_to_mhe.sku_id inner join  pick_locn_dtl on swm_to_mhe.sku_id = pick_locn_dtl.sku_id  " +
                $"inner join case_hdr on swm_to_mhe.container_id = case_hdr.case_nbr and swm_to_mhe.source_msg_status = 'Ready' and swm_to_mhe.qty!= 0 and case_hdr.stat_code = 96";
            command = new OracleCommand(Query, db);
            var validData = command.ExecuteReader();
            if (validData.Read())
            {
                IvstDataDto.CaseNumber = validData[TestData.FieldName.ContainerId].ToString();
                IvstDataDto.SkuId = validData[TestData.FieldName.SkuId].ToString();
                IvstDataDto.Qty = validData[TestData.FieldName.Qty].ToString();
                IvstDataDto.LocnId = validData[TestData.FieldName.LocnId].ToString();
            }
            return IvstDataDto;
        }

        public void InsertingUnexpectedOverage(OracleConnection db, string ActionCode, string exception)
        {
            command = new OracleCommand(Query, db);
            var Ivstmsg = CreateIvstMessage(IvstData.CaseNumber, IvstData.SkuId, IvstData.Qty, ActionCode, exception);
            emsToWmsParameters = new EmsToWmsDto
            {
                Process = DefaultPossibleValue.MessageProcessor,
                Status = RecordStatus.Ready.ToString(),
                Transaction = TransactionCode.Ivst,
                MessageText = Ivstmsg
            };
            IvstData.Key = InsertEmsToWMS(db, emsToWmsParameters);
        }

        public void InsertingInventoryShortage(OracleConnection db, string ActionCode, string exception)
        {
            command = new OracleCommand(Query, db);
            var Ivstmsg = CreateIvstMessage(IvstData.CaseNumber, IvstData.SkuId, IvstData.Qty, ActionCode, exception);
            emsToWmsParametersInventoryShortage = new EmsToWmsDto
            {
                Process = DefaultPossibleValue.MessageProcessor,
                Status = RecordStatus.Ready.ToString(),
                Transaction = TransactionCode.Ivst,
                MessageText = Ivstmsg
            };
            Invshort.Key = InsertEmsToWMS(db, emsToWmsParametersInventoryShortage);
        }
        public void InsertingDamage(OracleConnection db, string ActionCode, string exception)
        {
            command = new OracleCommand(Query, db);
            var Ivstmsg = CreateIvstMessage(IvstData.CaseNumber, IvstData.SkuId, IvstData.Qty, ActionCode, exception);
            emsToWmsParametersDamage = new EmsToWmsDto
            {
                Process = DefaultPossibleValue.MessageProcessor,
                Status = RecordStatus.Ready.ToString(),
                Transaction = TransactionCode.Ivst,
                MessageText = Ivstmsg
            };
            Damage.Key = InsertEmsToWMS(db, emsToWmsParametersDamage);
        }

        public void InsertingWrongSku(OracleConnection db, string ActionCode, string exception)
        {
            command = new OracleCommand(Query, db);
            var Ivstmsg = CreateIvstMessage(IvstData.CaseNumber, IvstData.SkuId, IvstData.Qty, ActionCode, exception);
            emsToWmsParametersWrongSku = new EmsToWmsDto
            {
                Process = DefaultPossibleValue.MessageProcessor,
                Status = RecordStatus.Ready.ToString(),
                Transaction = TransactionCode.Ivst,
                MessageText = Ivstmsg
            };
            WrongSku.Key = InsertEmsToWMS(db, emsToWmsParametersWrongSku);
        }

        public string CreateIvstMessage(string containerNbr, string SkuId, string locationId, string ActionCode, string AdjustmentReasonCode)
        {
            IvstParameters = new IvstDto
            {
                ActionCode = ActionCode,
                AdjustmentReasonCode = AdjustmentReasonCode,
                ContainerId = containerNbr,
                Quantity = "1",
                Sku = SkuId,
                Owner = "Wms",
                UserName = "Prashant M G",
                UnitOfMeasure = "Case",
                LotId = "1231",
                Po = "2445",
                InboundPallet = ((char)InboundPallet.Yes).ToString(),
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
            command = new OracleCommand(Query, db);
            var pickLocnReader = command.ExecuteReader();
            if (pickLocnReader.Read())
            {
                pickLocn.ActualInventoryQuantity = Convert.ToDecimal(pickLocnReader[TestData.FieldName.ActlInvnQty].ToString());
                pickLocn.ToBeFilledQty = Convert.ToDecimal(pickLocnReader[TestData.FieldName.ToBeFilledQty].ToString());
                pickLocn.LocationId = pickLocnReader[TestData.FieldName.LocnId].ToString();
            }
            return pickLocn;
        }

        public SwmFromMheDto SwmFromMhe(OracleConnection db, string caseNbr, string trx, string skuId)
        {
            var swmFromMheData = new SwmFromMheDto();
            Query = $"select * from swm_from_mhe where container_id = '{caseNbr}' and source_msg_trans_code = '{trx}' and sku_id = '{skuId}' order by created_date_time desc";
            command = new OracleCommand(Query, db);
            var swmFromMheReader = command.ExecuteReader();
            if (swmFromMheReader.Read())
            {
                swmFromMheData.SourceMessageKey = Convert.ToInt16(swmFromMheReader[TestData.FieldName.SourceMsgKey].ToString());
                swmFromMheData.SourceMessageResponseCode = Convert.ToInt16(swmFromMheReader[TestData.FieldName.SourceMsgRsnCode].ToString());
                swmFromMheData.SourceMessageStatus = swmFromMheReader[TestData.FieldName.SourceMsgStatus].ToString();
                swmFromMheData.SourceMessageProcess = swmFromMheReader[TestData.FieldName.SourceMsgProcess].ToString();
                swmFromMheData.SourceMessageTransactionCode = swmFromMheReader[TestData.FieldName.SourceMsgTransCode].ToString();
                swmFromMheData.ContainerId = swmFromMheReader[TestData.FieldName.ContainerId].ToString();
                swmFromMheData.ContainerType = swmFromMheReader[TestData.FieldName.ContainerType].ToString();
                swmFromMheData.MessageJson = swmFromMheReader[TestData.FieldName.MsgJson].ToString();
                swmFromMheData.SourceMessageText = swmFromMheReader[TestData.FieldName.SourceMsgText].ToString();
                swmFromMheData.LocationId = swmFromMheReader[TestData.FieldName.LocnId].ToString();
            }
            return swmFromMheData;
        }

        public void GetDataAfterTrigger()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                swmFromMhe = SwmFromMhe(db, IvstData.CaseNumber, TransactionCode.Ivst, IvstData.SkuId);
                Ivst = JsonConvert.DeserializeObject<IvstDto>(swmFromMhe.MessageJson);
                trnsInvAfterApi = FetchTransInvnentory(db, Ivst.Sku);
                pickLocnDtlAfterApi = PickLocnData(db, Ivst.Sku);
                unitWeight = FetchUnitWeight(db, Ivst.Sku);
                var PixTrnAfterApi = PixTransactionTable(Ivst.AdjustmentReasonCode);
                pixtran = GetPixtransaction(db, PixTrnAfterApi);
            }
        }

        public PixTransactionDto GetPixtransaction(OracleConnection db, string rsnCode)
        {
            var pixtran = new PixTransactionDto();
            Query = $"select * from Pix_tran where rsn_code='{rsnCode}'";
            command = new OracleCommand(Query, db);
            var Rsn = command.ExecuteReader();

            if (Rsn.Read())
            {
                pixtran.ReasonCode = (Rsn["RSN_CODE"].ToString());
            }
            return pixtran;
        }


        public string PixTransactionTable(string AdjustmentReasonCode)
        {
            if (AdjustmentReasonCode == "0001")
            {
                return "CC";
            }
            else if (AdjustmentReasonCode == "0002")
            {
                return "CO";
            }
            else if (AdjustmentReasonCode == "0003")
            {
                return "CS";
            }
            else if (AdjustmentReasonCode == "0004")
            {
                return "DG";
            }
            else if (AdjustmentReasonCode == "0005")
            {
                return "CC";
            }
            return "CC";
        }
    }
}
