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

        public void GetCaseDetailsForInsertingCostMessage(OracleConnection db)
        {
            sqlStatements = $"select swm_to_mhe.container_id,swm_to_mhe.sku_id,pick_locn_dtl.locn_id,swm_to_mhe.qty from swm_to_mhe inner join trans_invn" +
                $" on trans_invn.sku_id = swm_to_mhe.sku_id inner join  pick_locn_dtl on swm_to_mhe.sku_id = pick_locn_dtl.sku_id  " +
                $"inner join case_hdr on swm_to_mhe.container_id = case_hdr.case_nbr and swm_to_mhe.source_msg_status = 'Ready' and swm_to_mhe.qty!= 0 and case_hdr.stat_code = 96";
            command = new OracleCommand(sqlStatements, db);
            var dr15 = command.ExecuteReader();
            if (dr15.Read())
            {
                costData.CaseNumber = dr15["CONTAINER_ID"].ToString();
                costData.SkuId = dr15["SKU_ID"].ToString();
                costData.Qty = dr15["QTY"].ToString();
                costData.LocnId = dr15["LOCN_ID"].ToString();
            }
            unitweight1 = FetchUnitWeight(db, costData.SkuId);            
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
                transaction = db.BeginTransaction();
                GetCaseDetailsForInsertingCostMessage(db);
                InsertCostMessageInToEmsTable(db);
                trnInvBeforeApi = FetchTransInvn(db, costData.SkuId);
                pickLcnDtlBeforeApi = PickLocnData(db, costData.SkuId);
            }
        }
         
        public PickLocationDtlDto PickLocnData(OracleConnection db,string skuId)
        {
            var pickLocn = new PickLocationDtlDto();
            sqlStatements = $"select tn.ACTL_INVN_UNITS,pl.ACTL_INVN_QTY,pl.TO_BE_FILLD_QTY,pl.LOCN_ID from trans_invn tn inner join  pick_locn_dtl pl on tn.sku_id = pl.sku_id and tn.trans_invn_type= 18 and tn.sku_id = '{skuId}' order by tn.mod_date_time desc";
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
         
        public void InsertCostMessageInToEmsTable(OracleConnection db)
        {
            var CostResult = CreateCostMessage(costData.CaseNumber, costData.SkuId, costData.Qty, costData.LocnId);
            costData.MsgKey = GetSeqNbr(db);
            emsToWmsParameters = new EmsToWmsDto
            {
                Process = "EMS",
                MessageKey = Convert.ToInt64(costData.MsgKey),
                Status = "Ready",
                Transaction = TransactionCode.Cost,
                ResponseCode = (short)int.Parse(DefaultPossibleValue.ReasonCode.Success),
            };
            var validsql = $"insert into emstowms values ('{emsToWmsParameters.Process}','{emsToWmsParameters.MessageKey}','{emsToWmsParameters.Status}','{emsToWmsParameters.Transaction}','{CostResult}','0','TestUser','22-JUL-19','22-JUL-19')";
            command = new OracleCommand(validsql, db);
            command.ExecuteNonQuery();
            transaction.Commit();
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
            transaction = db.BeginTransaction();
            command = new OracleCommand(sqlStatements, db);
            var invalidCaseNumber = CreateCostMessage("00000283000804736790", costData.SkuId, costData.Qty, costData.LocnId);
            costData.InvalidKey = GetSeqNbr(db);
            var invalidCaseSql = $"insert into emstowms values ('{emsToWmsParameters.Process}','{costData.InvalidKey}','{emsToWmsParameters.Status}','{emsToWmsParameters.Transaction}','{invalidCaseNumber}','0','TestUser','22-JUL-19','22-JUL-19')";
            command = new OracleCommand(invalidCaseSql, db);
            command.ExecuteNonQuery();
            transaction.Commit();
        }

        public void TransInvnDoesNotExistData(OracleConnection db)
        {
            sqlStatements = $"select cd.SKU_ID,ch.CASE_NBR,tn.ACTL_INVN_UNITS,ch.STAT_CODE from  CASE_HDR ch  inner join  case_dtl cd on cd.CASE_NBR = ch.CASE_NBR  inner join pick_locn_dtl on pick_locn_dtl.sku_id = cd.sku_id left join trans_invn tn on tn.SKU_ID = cd.SKU_ID and ch.STAT_CODE = 50 and tn.ACTL_INVN_UNITS >1 and trans_invn_type = '18' and tn.SKU_ID = null";
            command = new OracleCommand(sqlStatements, db);
            var dr17 = command.ExecuteReader();
            if (dr17.Read())
            {
                costDataForTransInvnNotExist.CaseNumber = dr17["CASE_NBR"].ToString();
                costDataForTransInvnNotExist.SkuId = dr17["SKU_ID"].ToString();
                costDataForTransInvnNotExist.Qty = dr17["ACTL_INVN_UNITS"].ToString();
            }
            InsertCostMsgForTransInvnNotExistData(db);
        }

        public void InsertCostMsgForTransInvnNotExistData(OracleConnection db)
        {
            transaction = db.BeginTransaction();
            costDataForTransInvnNotExist.MsgKey = GetSeqNbr(db);
            var transInvnNotExistMsg = CreateCostMessage(costDataForTransInvnNotExist.CaseNumber, costDataForTransInvnNotExist.SkuId, costDataForTransInvnNotExist.Qty, costData.LocnId);
            var transInvnNotExistSql = $"insert into emstowms values ('{emsToWmsParameters.Process}','{ costDataForTransInvnNotExist.MsgKey}','{emsToWmsParameters.Status}','{emsToWmsParameters.Transaction}','{transInvnNotExistMsg}','0','TestUser','22-JUL-19','22-JUL-19')";
            command = new OracleCommand(transInvnNotExistSql, db);
            command.ExecuteNonQuery();
            transaction.Commit();
        }

        public void PickLocnDoesNotExistData(OracleConnection db)
        {
            sqlStatements = $"select tn.ACTL_INVN_UNITS,cd.SKU_ID,ch.CASE_NBR,ch.STAT_CODE from  CASE_HDR ch  inner join  case_dtl cd on cd.CASE_NBR = ch.CASE_NBR  inner join trans_invn tn on tn.SKU_ID = cd.SKU_ID  left join pick_locn_dtl on pick_locn_dtl.sku_id = tn.sku_id  and ch.STAT_CODE = 96 and tn.ACTL_INVN_UNITS >1 and trans_invn_type = '18' and pick_locn_dtl.LOCN_ID = 0";
            command = new OracleCommand(sqlStatements, db);
            var dr18 = command.ExecuteReader();
            if (dr18.Read())
            {
                costDataForPickLocnNotExist.CaseNumber = dr18["CASE_NBR"].ToString();
                costDataForPickLocnNotExist.SkuId = dr18["SKU_ID"].ToString();
                costDataForPickLocnNotExist.Qty = dr18["ACTL_INVN_UNITS"].ToString();
            }
            InsertCostMsgForPickLocnDoesNotExistData(db);
        }

        public void InsertCostMsgForPickLocnDoesNotExistData(OracleConnection db)
        {
            transaction = db.BeginTransaction();
            costDataForPickLocnNotExist.MsgKey = GetSeqNbr(db);
            var pickLnInvnNotExistMsg = CreateCostMessage(costDataForPickLocnNotExist.CaseNumber, costDataForPickLocnNotExist.SkuId, costDataForPickLocnNotExist.Qty, costData.LocnId);
            var pickLnInvnNotExistSql = $"insert into emstowms values ('{emsToWmsParameters.Process}','{costDataForPickLocnNotExist.MsgKey}','{emsToWmsParameters.Status}','{emsToWmsParameters.Transaction}','{pickLnInvnNotExistMsg}','0','TestUser','22-JUL-19','22-JUL-19')";
            command = new OracleCommand(pickLnInvnNotExistSql, db);
            command.ExecuteNonQuery();
            transaction.Commit();
        }

        public Int64 GetSeqNbr(OracleConnection db)
        {
            sqlStatements = $"select WMSTOEMS_MSGKEY_SEQ.nextval from dual";
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
