using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using Sfc.Wms.Asrs.Dematic.Contracts.Dtos;
using Sfc.Wms.Asrs.Shamrock.Contracts.Dtos;
using Sfc.Wms.Asrs.Test.Integrated.TestData;
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
using DefaultPossibleValue = Sfc.Wms.ParserAndTranslator.Contracts.Constants;

namespace Sfc.Wms.Asrs.Test.Integrated.Fixtures
{
    public class DataBaseFixtureForCost :DataBaseFixture
    {       
        protected SwmFromMheDto swmFromMhe = new SwmFromMheDto();
        protected CaseDto trnInvBeforeApi = new CaseDto();
        protected CaseDto trnInvAfterApi  = new CaseDto();
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
        protected OracleTransaction transaction;
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
            var dr15 = command.ExecuteReader();
            if (dr15.Read())
            {
                CostDataDto.CaseNumber = dr15["CONTAINER_ID"].ToString();
                CostDataDto.SkuId = dr15["SKU_ID"].ToString();
                CostDataDto.Qty = dr15["QTY"].ToString();
                CostDataDto.LocnId = dr15["LOCN_ID"].ToString();
            }
            return CostDataDto;                 
        }

        public void GetDataBeforeTrigger()
        {           
            OracleConnection db;        
            using (db = new OracleConnection
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["SfcRbacContextModel"].ConnectionString
            })
            {
                db.Open();
                costData = GetCaseDetailsForInsertingCostMessage(db);
                unitweight1 = FetchUnitWeight(db, costData.SkuId);
                var CostResult = CreateCostMessage(costData.CaseNumber, costData.SkuId, costData.Qty, costData.LocnId);
                emsToWmsParameters = new EmsToWmsDto
                {
                    Process = "EMS",
                    MessageKey = Convert.ToInt64(costData.MsgKey),
                    Status = "Ready",
                    Transaction = TransactionCode.Cost,
                    ResponseCode = (short)int.Parse(ReasonCode.Success),
                    MessageText = CostResult,
                };
                costData.MsgKey = InsertEmsToWMS(db,emsToWmsParameters);
                trnInvBeforeApi = FetchTransInvn(db, costData.SkuId);
                pickLcnDtlBeforeApi = PickLocnData(db, costData.SkuId);
            }
        }
         
        public PickLocationDtlDto PickLocnData(OracleConnection db,string skuId)
        {
            var pickLocn = new PickLocationDtlDto();
            sqlStatements = $"select * from pick_locn_dtl where sku_id = '{skuId}' and locn_id = '{costData.LocnId}' order by mod_date_time desc";
            command = new OracleCommand(sqlStatements, db);
            var dr3 = command.ExecuteReader();
            if (dr3.Read())
            {
                pickLocn.ActualInventoryQuantity = Convert.ToDecimal(dr3["ACTL_INVN_QTY"].ToString());
                pickLocn.ToBeFilledQty = Convert.ToDecimal(dr3["TO_BE_FILLD_QTY"].ToString());
                pickLocn.LocationId = dr3["LOCN_ID"].ToString();
            }
            return pickLocn;
        }

        public long InsertEmsToWMS (OracleConnection db, EmsToWmsDto emsToWmsDto)
        {
            transaction = db.BeginTransaction();
            var MsgKey = GetSeqNbrEmsToWms(db);
            command.Parameters.Add(new OracleParameter("dateParam", OracleDbType.Date)).Value = DateTime.Now;
            var validsql = $"insert into emstowms values ('{emsToWmsDto.Process}','{MsgKey}','{emsToWmsDto.Status}','{emsToWmsDto.Transaction}','{emsToWmsDto.MessageText}','{emsToWmsDto.ResponseCode}','TestUser','{DateTime.Now.ToString("dd-MMM-yy")}','{DateTime.Now.ToString("dd-MMM-yy")}')";
            command = new OracleCommand(validsql, db);
            command.ExecuteNonQuery();
            transaction.Commit();
            return MsgKey;
        }

        public string CreateCostMessage(string containerNbr, string skuId, string qty, string locationId)
        {
            CostParameters = new CostDto
            {
                ActionCode = "Arrival",
                ContainerReasonCodeMap = ReasonCode.Success,
                ContainerId = containerNbr,
                ContainerType = "Case",
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
            OracleConnection db;       
            using (db = new OracleConnection
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["SfcRbacContextModel"].ConnectionString
            })
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
            var costmsg = CreateCostMessage("00000283000804736790", costData.SkuId, costData.Qty, costData.LocnId);
             var emsToWms = new EmsToWmsDto
            {
                Process = "EMS",
                Status = "Ready",
                Transaction = TransactionCode.Cost,
                ResponseCode = (short)int.Parse(ReasonCode.Success),
                MessageText = costmsg
            };
            costData.InvalidKey = InsertEmsToWMS(db, emsToWms);
        }

        public void TransInvnDoesNotExistData(OracleConnection db)
        {
            costDataForTransInvnNotExist = FetchCaseNumberWithoutTransInventry(db);
            var costmsg = CreateCostMessage(costDataForTransInvnNotExist.CaseNumber, costDataForTransInvnNotExist.SkuId, costDataForTransInvnNotExist.Qty, costData.LocnId);
            var emsToWms = new EmsToWmsDto
            {
                Process = "EMS",
                Status = "Ready",
                Transaction = TransactionCode.Cost,
                ResponseCode = (short)int.Parse(ReasonCode.Success),
                MessageText = costmsg
            };
            costDataForTransInvnNotExist.InvalidKey = InsertEmsToWMS(db, emsToWms);
        }

        public Cost FetchCaseNumberWithoutTransInventry (OracleConnection db)
        {
            var CostTransData = new Cost();
            sqlStatements = $"select cd.SKU_ID,ch.CASE_NBR,tn.ACTL_INVN_UNITS,ch.STAT_CODE from  CASE_HDR ch  inner join  case_dtl cd on cd.CASE_NBR = ch.CASE_NBR  inner join pick_locn_dtl on pick_locn_dtl.sku_id = cd.sku_id left join trans_invn tn on tn.SKU_ID = cd.SKU_ID and ch.STAT_CODE = 50 and tn.ACTL_INVN_UNITS >1 and trans_invn_type = '18' and tn.SKU_ID = null";
            command = new OracleCommand(sqlStatements, db);
            var dr17 = command.ExecuteReader();
            if (dr17.Read())
            {
                CostTransData.CaseNumber = dr17["CASE_NBR"].ToString();
                CostTransData.SkuId = dr17["SKU_ID"].ToString();
                CostTransData.Qty = dr17["ACTL_INVN_UNITS"].ToString();
            }
            return CostTransData;
        }

        public void PickLocnDoesNotExistData(OracleConnection db)
        {
            costDataForPickLocnNotExist = FetchPickLocnDoesNotExistData(db);
            var pickLnInvnNotExistMsg = CreateCostMessage(costDataForPickLocnNotExist.CaseNumber, costDataForPickLocnNotExist.SkuId, costDataForPickLocnNotExist.Qty, costData.LocnId);
            var emsToWms = new EmsToWmsDto
            {
                Process = "EMS",
                Status = "Ready",
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
            var dr18 = command.ExecuteReader();
            if (dr18.Read())
            {
                CostTransData.CaseNumber = dr18["CASE_NBR"].ToString();
                CostTransData.SkuId = dr18["SKU_ID"].ToString();
                CostTransData.Qty = dr18["ACTL_INVN_UNITS"].ToString();
            }
            return CostTransData;
        }

        public Int64 GetSeqNbrEmsToWms(OracleConnection db)
        {
            sqlStatements = $"select EMSTOWMS_MSGKEY_SEQ.nextval from dual";
            command = new OracleCommand(sqlStatements, db);
            var key = Convert.ToInt64(command.ExecuteScalar().ToString());
            return key;
        }

        public SwmFromMheDto SwmFromMhe(OracleConnection db, string caseNbr, string trx, string skuId)
        {
            var swmFromMheData = new SwmFromMheDto();
            sqlStatements = $"select * from swm_from_mhe where container_id = '{caseNbr}' and source_msg_trans_code = '{trx}' and sku_id = '{skuId}' order by created_date_time desc";
            command = new OracleCommand(sqlStatements, db);
            var dr1 = command.ExecuteReader();
            if (dr1.Read())
            {
                swmFromMheData.SourceMessageKey = Convert.ToInt16(dr1["SOURCE_MSG_KEY"].ToString());
                swmFromMheData.SourceMessageResponseCode = Convert.ToInt16(dr1["SOURCE_MSG_RSN_CODE"].ToString());
                swmFromMheData.SourceMessageStatus = dr1["SOURCE_MSG_STATUS"].ToString();
                swmFromMheData.SourceMessageProcess = dr1["SOURCE_MSG_PROCESS"].ToString();
                swmFromMheData.SourceMessageTransactionCode = dr1["SOURCE_MSG_TRANS_CODE"].ToString();
                swmFromMheData.ContainerId = dr1["CONTAINER_ID"].ToString();
                swmFromMheData.ContainerType = dr1["CONTAINER_TYPE"].ToString();
                swmFromMheData.MessageJson = dr1["MSG_JSON"].ToString();
                swmFromMheData.SourceMessageText = dr1["SOURCE_MSG_TEXT"].ToString();
                swmFromMheData.LocationId = dr1["LOCN_ID"].ToString();
            }
            return swmFromMheData;
        }
        
       
        public void GetDataAfterTrigger()
        {
            OracleConnection db;
            using (db = new OracleConnection
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["SfcRbacContextModel"].ConnectionString
            })
            {
                db.Open();
                swmFromMhe = SwmFromMhe(db, costData.CaseNumber, TransactionCode.Cost, costData.SkuId);
                cost = JsonConvert.DeserializeObject<CostDto>(swmFromMhe.MessageJson);
                trnInvAfterApi = FetchTransInvn(db, cost.StorageClassAttribute1);
                pickLocnDtlAfterApi = PickLocnData(db, cost.StorageClassAttribute1);
            }
        }     
    }
}
