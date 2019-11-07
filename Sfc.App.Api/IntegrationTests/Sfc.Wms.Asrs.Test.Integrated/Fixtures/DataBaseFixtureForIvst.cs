﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData;
using System.Configuration;
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
        protected decimal unitWeight;
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
        protected decimal unitweight1;
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
                $"inner join case_hdr on swm_to_mhe.container_id = case_hdr.case_nbr and swm_to_mhe.source_msg_status = 'Ready' and swm_to_mhe.qty!= 0 and case_hdr.stat_code = 96"+
                $" and pick_locn_dtl.locn_id in (select lh.locn_id from locn_hdr lh inner join locn_grp lg on lg.locn_id = lh.locn_id inner join sys_code sc on sc.code_id = lg.grp_type and sc.code_type = '740' and sc.code_id = '18')";
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
                UserName = "Dematic",
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

        public void GetDataAfterTrigger(long key)
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                swmFromMhe = SwmFromMhe(db, key, TransactionCode.Ivst);
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