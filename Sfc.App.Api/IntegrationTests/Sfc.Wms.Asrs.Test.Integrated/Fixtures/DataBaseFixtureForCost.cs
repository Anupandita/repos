﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using Sfc.Wms.Asrs.Dematic.Contracts.Dtos;
using Sfc.Wms.Asrs.Shamrock.Contracts.Dtos;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData;
using Sfc.Wms.Builder.MessageBuilder;
using Sfc.Wms.InboundLpn.Contracts.Dtos;
using Sfc.Wms.ParserAndTranslator.Contracts.Constants;
using Sfc.Wms.ParserAndTranslator.Contracts.Dto;
using Sfc.Wms.ParserAndTranslator.Contracts.Interfaces;
using Sfc.Wms.ParserAndTranslator.Contracts.Validation;
using Sfc.Wms.PickLocationDetail.Contracts.Dtos;
using Sfc.Wms.Result;
using System;
using System.Configuration;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures
{
    public class DataBaseFixtureForCost :CommonFunction
    {       
        protected SwmFromMheDto swmFromMhe = new SwmFromMheDto();
        protected CaseViewDto trnInvBeforeApi = new CaseViewDto();
        protected CaseViewDto trnInvAfterApi  = new CaseViewDto();
        protected CostDto CostParameters;
        protected Cost costData = new Cost();
        protected Cost costDataForTransInvnNotExist = new Cost();
        protected Cost costDataForPickLocnNotExist = new Cost();
        protected EmsToWmsDto emsToWmsParameters;
        private readonly IHaveDataTypeValidation _dataTypeValidation;
        protected PickLocationDtlDto pickLocnDtlAfterApi = new PickLocationDtlDto();
        protected PickLocationDtlDto pickLcnDtlBeforeApi = new PickLocationDtlDto();
        protected CaseHeaderDto caseHeaderDto = new CaseHeaderDto();
        protected CostDto cost = new CostDto();  
        protected string sqlStatements = "";
        protected OracleCommand oracleCommand;
        public decimal unitweight1;
       

        public DataBaseFixtureForCost()
        {
            _dataTypeValidation = new DataTypeValidation();
        }

        public Cost GetCaseDetailsForInsertingCostMessage(OracleConnection db)
        {
            var CostDataDto = new Cost();
            sqlStatements = $"select swm_to_mhe.container_id,swm_to_mhe.sku_id,pick_locn_dtl.locn_id,swm_to_mhe.qty from swm_to_mhe inner join trans_invn" +
                $" on trans_invn.sku_id = swm_to_mhe.sku_id inner join  pick_locn_dtl on swm_to_mhe.sku_id = pick_locn_dtl.sku_id  " +
                $"inner join case_hdr on swm_to_mhe.container_id = case_hdr.case_nbr and swm_to_mhe.source_msg_status = 'Ready' and swm_to_mhe.qty!= 0 and case_hdr.stat_code = 96";
            command = new OracleCommand(sqlStatements, db);
            var validData = command.ExecuteReader();
            if (validData.Read())
            {
                CostDataDto.CaseNumber = validData[TestData.SwmToMhe.ContainerId].ToString();
                CostDataDto.SkuId = validData[TestData.SwmToMhe.SkuId].ToString();
                CostDataDto.Qty = validData[TestData.SwmToMhe.Qty].ToString();
                CostDataDto.LocnId = validData[TestData.PickLocationDetail.LocnId].ToString();
            }
            return CostDataDto;                 
        }

        public void GetDataBeforeTrigger()
        {           
            using (var db = GetOracleConnection())
            {
                db.Open();
                costData = GetCaseDetailsForInsertingCostMessage(db);
                unitweight1 = FetchUnitWeight(db, costData.SkuId);
                var CostResult = CreateCostMessage(costData.CaseNumber, costData.SkuId, costData.Qty, costData.LocnId);
                emsToWmsParameters = new EmsToWmsDto
                {
                    Process = DefaultValues.Process,
                    MessageKey = Convert.ToInt64(costData.MsgKey),
                    Status = DefaultValues.Status,
                    Transaction = TransactionCode.Cost,
                    ResponseCode = (short)int.Parse(ReasonCode.Success),
                    MessageText = CostResult,
                };
                costData.MsgKey = InsertEmsToWMS(db,emsToWmsParameters);
                trnInvBeforeApi = FetchTransInvn(db, costData.SkuId);
                pickLcnDtlBeforeApi = PickLocnData(db, costData.SkuId,costData.SkuId);
            }
        }

        protected SwmToMheDto SwmToMhe(OracleConnection db, string caseNbr, string trx, string skuId)
        {
            var swmtomhedata = new SwmToMheDto();
            var t = $"and sku_id = '{skuId}' ";
            var query = $"select * from SWM_TO_MHE where container_id = '{caseNbr}' and source_msg_trans_code = '{trx}' ";
            var orderBy = "order by created_date_time desc";
            if (trx == TransactionCode.Ivmt)
            {
                query = query + t + orderBy;
            }
            else
            {
                query = query + orderBy;
            }
            command = new OracleCommand(query, db);
            var swmToMheReader = command.ExecuteReader();
            if (swmToMheReader.Read())
            {
                swmtomhedata.SourceMessageKey = Convert.ToInt16(swmToMheReader[TestData.SwmToMhe.SourceMsgKey].ToString());
                swmtomhedata.SourceMessageResponseCode = Convert.ToInt16(swmToMheReader[TestData.SwmToMhe.SourceMsgRsnCode].ToString());
                swmtomhedata.SourceMessageStatus = swmToMheReader[TestData.SwmToMhe.SourceMsgStatus].ToString();
                swmtomhedata.ContainerId = swmToMheReader[TestData.SwmToMhe.ContainerId].ToString();
                swmtomhedata.ContainerType = swmToMheReader[TestData.SwmToMhe.ContainerType].ToString();
                swmtomhedata.MessageJson = swmToMheReader[TestData.SwmToMhe.MsgJson].ToString();
                swmtomhedata.LocationId = swmToMheReader[TestData.SwmToMhe.LocnId].ToString();
                swmtomhedata.SourceMessageText = swmToMheReader[TestData.SwmToMhe.SourceMsgText].ToString();
            }
            return swmtomhedata;
        }

        public string CreateCostMessage(string containerNbr, string skuId, string qty, string locationId)
        {
            CostParameters = new CostDto
            {
                ActionCode = DefaultValues.ActionCodeCost,
                ContainerReasonCodeMap = ReasonCode.Success,
                ContainerId = containerNbr,
                ContainerType = DefaultValues.ContainerType,
                PhysicalContainerId = "",
                CurrentLocationId = locationId,
                StorageClassAttribute1 = skuId,
                StorageClassAttribute2 = "1",
                StorageClassAttribute3 = "",
                StorageClassAttribute4 = "",
                StorageClassAttribute5 = "",
                PalletLpn = containerNbr,
                TransactionCode = TransactionCode.Cost,
                MessageLength = MessageLength.Cost,
                ReasonCode = ReasonCode.Success
            };
            GenericMessageBuilder gm = new GenericMessageBuilder(_dataTypeValidation);
            var testResult = gm.BuildMessage<CostDto, CostValidationRule>(CostParameters, TransactionCode.Cost);         
            Assert.AreEqual(testResult.ResultType, ResultTypes.Ok);
            Assert.IsNotNull(testResult.Payload);
            return testResult.Payload;
        }

        
        public void GetDataForNegativeCases()
        {     
            using (var db = GetOracleConnection())
            {
                db.Open();
                InvalidCaseData(db);
                TransInvnDoesNotExistData(db);
                PickLocnDoesNotExistData(db);
            }
        }
        
        public void InvalidCaseData(OracleConnection db)
        {   
            command = new OracleCommand(sqlStatements, db);
            var costmsg = CreateCostMessage(DefaultValues.InvalidCase, costData.SkuId, costData.Qty, costData.LocnId);
             var emsToWms = new EmsToWmsDto
            {
                Process = DefaultValues.Process,
                Status = DefaultValues.Status,
                Transaction = TransactionCode.Cost,
                ResponseCode = (short)int.Parse(ReasonCode.Success),
                MessageText = costmsg
            };
            costData.InvalidKey = InsertEmsToWMS(db, emsToWms);
        }

        public void TransInvnDoesNotExistData(OracleConnection db)
        {
            costDataForTransInvnNotExist = FetchCaseNumberWithoutTransInventry(db);
            var costmsg = CreateCostMessage(costDataForTransInvnNotExist.CaseNumber, costDataForTransInvnNotExist.SkuId, costDataForTransInvnNotExist.Qty, costDataForTransInvnNotExist.LocnId);
            var emsToWms = new EmsToWmsDto
            {
                Process = DefaultValues.Process,
                Status = DefaultValues.Status,
                Transaction = TransactionCode.Cost,
                ResponseCode = (short)int.Parse(ReasonCode.Success),
                MessageText = costmsg
            };
            costDataForTransInvnNotExist.MsgKey = InsertEmsToWMS(db, emsToWms);
        }

        public Cost FetchCaseNumberWithoutTransInventry (OracleConnection db)
        {
            var CostTransData = new Cost();
            sqlStatements = $"select cd.SKU_ID,ch.CASE_NBR,tn.ACTL_INVN_UNITS,ch.STAT_CODE,pick_locn_dtl.locn_id from  CASE_HDR ch  inner join  case_dtl cd on cd.CASE_NBR = ch.CASE_NBR  inner join pick_locn_dtl on pick_locn_dtl.sku_id = cd.sku_id left join trans_invn tn on tn.SKU_ID = cd.SKU_ID and ch.STAT_CODE = 50 and tn.ACTL_INVN_UNITS >1 and trans_invn_type = '18' and tn.SKU_ID = null";
            command = new OracleCommand(sqlStatements, db);
            var Reader = command.ExecuteReader();
            if (Reader.Read())
            {
                CostTransData.CaseNumber = Reader[CaseHeader.CaseNumber].ToString();
                CostTransData.SkuId = Reader[CaseDetail.SkuId].ToString();
                CostTransData.Qty = Reader[TransInventory.ActualInventoryUnits].ToString();
                CostTransData.LocnId = Reader[TestData.PickLocationDetail.LocnId].ToString();
            }
            return CostTransData;
        }

        public void PickLocnDoesNotExistData(OracleConnection db)
        {
            costDataForPickLocnNotExist = FetchPickLocnDoesNotExistData(db);
            var pickLnInvnNotExistMsg = CreateCostMessage(costDataForPickLocnNotExist.CaseNumber, costDataForPickLocnNotExist.SkuId, costDataForPickLocnNotExist.Qty, costData.LocnId);
            var emsToWms = new EmsToWmsDto
            {
                Process = DefaultValues.Process,
                Status = DefaultValues.Status,
                Transaction = TransactionCode.Cost,
                ResponseCode = (short)int.Parse(ReasonCode.Success),
                MessageText = pickLnInvnNotExistMsg
            };
            costDataForPickLocnNotExist.MsgKey = InsertEmsToWMS(db, emsToWms);
        }

        public Cost FetchPickLocnDoesNotExistData(OracleConnection db)
        {
            var CostTransData = new Cost();
            sqlStatements = $"select tn.ACTL_INVN_UNITS,cd.SKU_ID,ch.CASE_NBR,ch.STAT_CODE from  CASE_HDR ch  inner join  case_dtl cd on cd.CASE_NBR = ch.CASE_NBR  inner join trans_invn tn on tn.SKU_ID = cd.SKU_ID  left join pick_locn_dtl on pick_locn_dtl.sku_id = tn.sku_id  and ch.STAT_CODE = 96 and tn.ACTL_INVN_UNITS >1 and trans_invn_type = '18' and pick_locn_dtl.LOCN_ID = 0";
            command = new OracleCommand(sqlStatements, db);
            var Reader = command.ExecuteReader();
            if (Reader.Read())
            {
                CostTransData.CaseNumber = Reader[CaseHeader.CaseNumber].ToString();
                CostTransData.SkuId = Reader[CaseDetail.SkuId].ToString();
                CostTransData.Qty = Reader[TransInventory.ActualInventoryUnits].ToString();
            }
            return CostTransData;
        }
     
       
        public void GetDataAfterTrigger()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                swmFromMhe = SwmFromMhe(db, costData.MsgKey, TransactionCode.Cost);
                cost = JsonConvert.DeserializeObject<CostDto>(swmFromMhe.MessageJson);
                trnInvAfterApi = FetchTransInvn(db, cost.StorageClassAttribute1);
                pickLocnDtlAfterApi = PickLocnData(db, cost.StorageClassAttribute1,cost.CurrentLocationId);
            }
        }     
    }
}
