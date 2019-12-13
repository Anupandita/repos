using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData;
using Sfc.Wms.Interfaces.Builder.MessageBuilder;
using Sfc.Wms.Interfaces.ParserAndTranslator.Contracts.Constants;
using Sfc.Wms.Interfaces.ParserAndTranslator.Contracts.Dto;
using Sfc.Wms.Foundation.Location.Contracts.Dtos;
using Sfc.Core.OnPrem.Result;
using System;
using Sfc.Wms.Interfaces.ParserAndTranslator.Contracts.Validation;
using Sfc.Wms.Interfaces.Asrs.Shamrock.Contracts.Dtos;
using Sfc.Wms.Interfaces.Asrs.Dematic.Contracts.Dtos;
using Sfc.Wms.Foundation.InboundLpn.Contracts.Dtos;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures
{
    public class DataBaseFixtureForCost : CommonFunction
    {
        protected SwmFromMheDto SwmFromMheDto = new SwmFromMheDto();
        protected CaseViewDto TrnInvBeforeApi = new CaseViewDto();
        protected CaseViewDto TrnInvAfterApi = new CaseViewDto();
        protected CostDto CostParameters;
        protected Cost CostData = new Cost();
        protected Cost CostDataForTransInvnNotExist = new Cost();
        protected Cost CostDataForPickLocnNotExist = new Cost();
        protected EmsToWmsDto EmsToWmsParameters;
        protected PickLocationDetailsDto PickLocnDtlAfterApi = new PickLocationDetailsDto();
        protected PickLocationDetailsDto PickLcnDtlBeforeApi = new PickLocationDetailsDto();
        protected CaseHeaderDto CaseHeaderDto = new CaseHeaderDto();
        protected CostDto Cost = new CostDto();
        protected string SqlStatements = "";
        protected OracleCommand OracleCommand;
        public decimal UnitWeight1;


        public Cost GetCaseDetailsForInsertingCostMessage(OracleConnection db)
        {
            var costDataDto = new Cost();
            SqlStatements = $"select swm_to_mhe.container_id,swm_to_mhe.sku_id,pick_locn_dtl.locn_id,swm_to_mhe.qty from swm_to_mhe inner join trans_invn" +
                $" on trans_invn.sku_id = swm_to_mhe.sku_id inner join  pick_locn_dtl on swm_to_mhe.sku_id = pick_locn_dtl.sku_id  " +
                $"inner join case_hdr on swm_to_mhe.container_id = case_hdr.case_nbr and swm_to_mhe.source_msg_status = 'Ready' and swm_to_mhe.qty!= :qty and case_hdr.stat_code = :statCode"+
                $" and pick_locn_dtl.locn_id in (select lh.locn_id from locn_hdr lh inner join locn_grp lg on lg.locn_id = lh.locn_id " +
                $"inner join sys_code sc on sc.code_id = lg.grp_type and sc.code_type = :sysCodeType and sc.code_id = :codeId)";
            Command = new OracleCommand(SqlStatements, db)
            {
                BindByName = true
            };
            Command.Parameters.Add(new OracleParameter("qty", Constants.NumZero));
            Command.Parameters.Add(new OracleParameter("statCode", Constants.StatusCodeConsumed));
            Command.Parameters.Add(new OracleParameter("sysCodeType", Constants.SysCodeType));
            Command.Parameters.Add(new OracleParameter("codeId", Constants.SysCodeIdForActiveLocation));
            var validData = Command.ExecuteReader();
            if (validData.Read())
            {
                costDataDto.CaseNumber = validData[TestData.SwmToMhe.ContainerId].ToString();
                costDataDto.SkuId = validData[TestData.SwmToMhe.SkuId].ToString();
                costDataDto.Qty = validData[TestData.SwmToMhe.Qty].ToString();
                costDataDto.LocnId = validData[PickLocationDetail.LocnId].ToString();
            }
            return costDataDto;
        }

        public void GetValidData()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                CostData = GetCaseDetailsForInsertingCostMessage(db);
                UnitWeight1 = FetchUnitWeight(db, CostData.SkuId);
            }
        }

        public void GetDataBeforeTrigger()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                var costResult = CreateCostMessage(CostData.CaseNumber, CostData.SkuId, CostData.Qty, CostData.LocnId);
                EmsToWmsParameters = new EmsToWmsDto
                {
                    Process = DefaultPossibleValue.MessageProcessor,
                    MessageKey = Convert.ToInt64(CostData.MsgKey),
                    Status = DefaultValues.Status,
                    Transaction = TransactionCode.Cost,
                    ResponseCode = (short)int.Parse(ReasonCode.Success),
                    MessageText = costResult,
                };
                CostData.MsgKey = InsertEmsToWms(db, EmsToWmsParameters);
                TrnInvBeforeApi = FetchTransInvn(db, CostData.SkuId);
                PickLcnDtlBeforeApi = GetPickLocationDetails(db, CostData.SkuId, CostData.LocnId);
            }
        }

        public string CreateCostMessage(string containerNbr, string skuId, string qty, string locationId)
        {
            CostParameters = new CostDto
            {
                ActionCode = DefaultValues.ActionCodeCost,
                ContainerReasonCodeMap = ReasonCode.Success,
                ContainerId =  Constants.InvalidContainerId,
                ContainerType = DefaultValues.ContainerType,
                PhysicalContainerId = "",
                CurrentLocationId = Constants.SampleCurrentLocnId,
                StorageClassAttribute1 = skuId,
                StorageClassAttribute2 = Constants.QtyToSend,
                StorageClassAttribute3 = "",
                StorageClassAttribute4 = "",
                StorageClassAttribute5 = "",
                PalletLpn = containerNbr,
                TransactionCode = TransactionCode.Cost,
                MessageLength = MessageLength.Cost,
                ReasonCode = ReasonCode.Success
            };
            var buildMessage = new MessageHeaderBuilder();
            var testResult = buildMessage.BuildMessage<CostDto, CostValidationRule>(CostParameters, TransactionCode.Cost);
            Assert.AreEqual(testResult.ResultType, ResultTypes.Ok);
            Assert.IsNotNull(testResult.Payload);
            return testResult.Payload;
        }

        public void InsertCostMessageForPickLocnDoesNotExist()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();              
                PickLocnDoesNotExistData(db);
            }
        }

        public void InsertCostMessageForInValidCase()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                InvalidCaseData(db);
            }
        }

        public void InsertCostMessageForTransInvnDoesNotExist()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                TransInvnDoesNotExistData(db);
            }
        }     

        public void InvalidCaseData(OracleConnection db)
        {
            Command = new OracleCommand(SqlStatements, db);
            var costmsg = CreateCostMessage(DefaultValues.InvalidCase, CostData.SkuId, CostData.Qty, CostData.LocnId);
            var emsToWms = new EmsToWmsDto
            {
                Process = DefaultValues.Process,
                Status = DefaultValues.Status,
                Transaction = TransactionCode.Cost,
                ResponseCode = (short)int.Parse(ReasonCode.Success),
                MessageText = costmsg
            };
            CostData.InvalidKey = InsertEmsToWms(db, emsToWms);
        }

        public void TransInvnDoesNotExistData(OracleConnection db)
        {
           
            CostDataForTransInvnNotExist = FetchCaseNumberWithoutTransInventry(db);
            var costmsg = CreateCostMessage(CostDataForTransInvnNotExist.CaseNumber, CostDataForTransInvnNotExist.SkuId, CostDataForTransInvnNotExist.Qty, CostDataForTransInvnNotExist.LocnId);
            var emsToWms = new EmsToWmsDto
            {
                Process = DefaultValues.Process,
                Status = DefaultValues.Status,
                Transaction = TransactionCode.Cost,
                ResponseCode = (short)int.Parse(ReasonCode.Success),
                MessageText = costmsg
            };
            CostDataForTransInvnNotExist.MsgKey = InsertEmsToWms(db, emsToWms);
        }

        public Cost FetchCaseNumberWithoutTransInventry (OracleConnection db)
        {
            var costTransData = new Cost();
            SqlStatements = $"select cd.SKU_ID,ch.CASE_NBR,tn.ACTL_INVN_UNITS,ch.STAT_CODE,pick_locn_dtl.locn_id from CASE_HDR ch  " +
                $"inner join  case_dtl cd on cd.CASE_NBR = ch.CASE_NBR  " +
                $"inner join pick_locn_dtl on pick_locn_dtl.sku_id = cd.sku_id " +
                $"left join trans_invn tn on tn.SKU_ID = cd.SKU_ID and ch.STAT_CODE = :statCode" +
                $"and tn.ACTL_INVN_UNITS > :qty " +
                $"and trans_invn_type = :transInvnType and tn.SKU_ID = null";
            Command = new OracleCommand(SqlStatements, db)
            {
                BindByName = true
            };
            Command.Parameters.Add(new OracleParameter("qty", Constants.MinQuantity));
            Command.Parameters.Add(new OracleParameter("statCode", Constants.ReceivedCaseFromVendorStatCode));
            Command.Parameters.Add(new OracleParameter("transInvnType", Constants.TransInvnType));
            var reader = Command.ExecuteReader();
            if (reader.Read())
            {
                costTransData.CaseNumber = reader[CaseHeader.CaseNumber].ToString();
                costTransData.SkuId = reader[CaseDetail.SkuId].ToString();
                costTransData.Qty = reader[TransInventory.ActualInventoryUnits].ToString();
                costTransData.LocnId = reader[PickLocationDetail.LocnId].ToString();
            }
            return costTransData;
        }

        public void PickLocnDoesNotExistData(OracleConnection db)
        {
            CostDataForPickLocnNotExist = FetchPickLocnDoesNotExistData(db);
            var pickLnInvnNotExistMsg = CreateCostMessage(CostDataForPickLocnNotExist.CaseNumber, CostDataForPickLocnNotExist.SkuId, CostDataForPickLocnNotExist.Qty, CostData.LocnId);
            var emsToWms = new EmsToWmsDto
            {
                Process = DefaultValues.Process,
                Status = DefaultValues.Status,
                Transaction = TransactionCode.Cost,
                ResponseCode = (short)int.Parse(ReasonCode.Success),
                MessageText = pickLnInvnNotExistMsg
            };
            CostDataForPickLocnNotExist.MsgKey = InsertEmsToWms(db, emsToWms);
        }

        public Cost FetchPickLocnDoesNotExistData(OracleConnection db)
        {
            var costTransData = new Cost();
            SqlStatements = $"select tn.ACTL_INVN_UNITS,cd.SKU_ID,ch.CASE_NBR,ch.STAT_CODE from  CASE_HDR ch  inner join  case_dtl cd on cd.CASE_NBR = ch.CASE_NBR  inner join trans_invn tn on tn.SKU_ID = cd.SKU_ID  " +
                $"left join pick_locn_dtl on pick_locn_dtl.sku_id = tn.sku_id  and ch.STAT_CODE = :statCode and tn.ACTL_INVN_UNITS > :qty and " +
                $"trans_invn_type = :transInvnType and pick_locn_dtl.LOCN_ID = :locnId";
            Command = new OracleCommand(SqlStatements, db)
            {
                BindByName = true
            };
            Command.Parameters.Add(new OracleParameter("statCode", Constants.StatusCodeConsumed));
            Command.Parameters.Add(new OracleParameter("qty", Constants.MinQuantity));
            Command.Parameters.Add(new OracleParameter("transInvnType", Constants.MinQuantity));
            Command.Parameters.Add(new OracleParameter("locnId", Constants.NumZero));
            var reader = Command.ExecuteReader();
            if (reader.Read())
            {
                costTransData.CaseNumber = reader[CaseHeader.CaseNumber].ToString();
                costTransData.SkuId = reader[CaseDetail.SkuId].ToString();
                costTransData.Qty = reader[TransInventory.ActualInventoryUnits].ToString();
            }
            return costTransData;
        }
           
        public void GetDataAfterTrigger()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                SwmFromMheDto = SwmFromMhe(db, CostData.MsgKey, TransactionCode.Cost);
                Cost = JsonConvert.DeserializeObject<CostDto>(SwmFromMheDto.MessageJson);
                TrnInvAfterApi = FetchTransInvn(db, Cost.StorageClassAttribute1);
                PickLocnDtlAfterApi = GetPickLocationDetails(db, Cost.StorageClassAttribute1,Cost.CurrentLocationId);
            }
        }     
    }
}
