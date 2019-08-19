using System;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using Sfc.Wms.Asrs.Dematic.Contracts.Dtos;
using Sfc.Wms.Asrs.Shamrock.Contracts.Dtos;
using Sfc.Wms.Builder.MessageBuilder;
using Sfc.Wms.TransitionalInventory.Contracts.Dtos;
using Sfc.Wms.ParserAndTranslator.Contracts.Interfaces;
using Sfc.Wms.ParserAndTranslator.Contracts.Dto;
using Sfc.Wms.ParserAndTranslator.Contracts.Validation;
using Sfc.Wms.Result;
using Sfc.Wms.Asrs.Test.Integrated.TestData;
using DefaultPossibleValue = Sfc.Wms.ParserAndTranslator.Contracts.Constants;
using System.Diagnostics;
using Sfc.Wms.PickLocationDetail.Contracts.Dtos;
using Sfc.Wms.InboundLpn.Contracts.Dtos;
using Sfc.Wms.ParserAndTranslator.Contracts.Constants;

namespace Sfc.Wms.Asrs.Test.Integrated.Fixtures
{
    public class DataBaseFixtureForCost :DataBaseFixture
    {       
        protected SwmFromMheDto swmFromMhe = new SwmFromMheDto();
        protected TransitionalInventoryDto trn3 = new TransitionalInventoryDto();
        protected TransitionalInventoryDto transInvn  = new TransitionalInventoryDto();
        protected CostDto CostParameters;
        protected Cost costData = new Cost();
        protected EmsToWmsDto emsToWmsParameters;
        protected EmsToWmsDto emsToWms = new EmsToWmsDto();
        private readonly IHaveDataTypeValidation _dataTypeValidation;
        protected PickLocationDtlDto pickLocnDtl = new PickLocationDtlDto();
        protected PickLocationDtlDto pickLcnDtl = new PickLocationDtlDto();
        protected CaseHeaderDto caseHdr1 = new CaseHeaderDto();
        protected CostDto cost = new CostDto();  
        protected string sql1 = "";
        protected OracleTransaction transaction;
        protected OracleCommand command;
        public decimal unitweight1;

        public DataBaseFixtureForCost()
        {
            _dataTypeValidation = new DataTypeValidation();
        }

        public void GetCaseDetailsForInsertingCostMessage(OracleConnection db)
        {
            sql1 = $"select swm_to_mhe.container_id,swm_to_mhe.sku_id,pick_locn_dtl.locn_id,swm_to_mhe.qty from swm_to_mhe inner join trans_invn" +
                $" on trans_invn.sku_id = swm_to_mhe.sku_id inner join  pick_locn_dtl on swm_to_mhe.sku_id = pick_locn_dtl.sku_id  " +
                $"inner join case_hdr on swm_to_mhe.container_id = case_hdr.case_nbr and swm_to_mhe.source_msg_status = 'Ready' and swm_to_mhe.qty!= 0 and case_hdr.stat_code = 96";
            command = new OracleCommand(sql1, db);
            var dr15 = command.ExecuteReader();
            if (dr15.Read())
            {
                costData.ValidCaseNumber = dr15["CONTAINER_ID"].ToString();
                costData.ValidSkuId = dr15["SKU_ID"].ToString();
                costData.ValidQty = dr15["QTY"].ToString();
                costData.ValidLocnId = dr15["LOCN_ID"].ToString();
            }
            unitweight1 = FetchUnitWeight(db, costData.ValidSkuId);            
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

                sql1 = $"select * from emstowms where msgkey = '{emsToWmsParameters.MessageKey}'";
                command = new OracleCommand(sql1, db);
                var dr = command.ExecuteReader();
                if (dr.Read())
                {
                    emsToWms.MessageKey = Convert.ToInt64(dr["MSGKEY"].ToString());
                    emsToWms.Process = dr["PRC"].ToString();
                    emsToWms.ResponseCode = (short)int.Parse(dr["RSNRCODE"].ToString());
                    emsToWms.Status = dr["STS"].ToString();
                    emsToWms.Transaction = dr["TRX"].ToString();                   
                }
                
                sql1 = $"select tn.ACTL_INVN_UNITS,pl.ACTL_INVN_QTY,pl.TO_BE_FILLD_QTY,pl.LOCN_ID from trans_invn tn inner join  pick_locn_dtl pl on tn.sku_id = pl.sku_id and tn.trans_invn_type= 18 and tn.sku_id = '{costData.ValidSkuId}' order by tn.mod_date_time desc";
                command = new OracleCommand(sql1, db);
                var dr3 = command.ExecuteReader();
                if (dr3.Read())
                {
                    trn3.ActualInventoryUnits = Convert.ToDecimal(dr3["ACTL_INVN_UNITS"].ToString());
                    Assert.AreNotEqual(0, trn3.ActualInventoryUnits);
                    pickLcnDtl.ActualInventoryQuantity = Convert.ToDecimal(dr3["ACTL_INVN_QTY"].ToString());
                    pickLcnDtl.ToBeFilledQty = Convert.ToDecimal(dr3["TO_BE_FILLD_QTY"].ToString());
                    pickLcnDtl.LocationId = dr3["LOCN_ID"].ToString();
                }
            }
        }
         
        public void InsertCostMessageInToEmsTable(OracleConnection db)
        {
            var CostResult = CreateCostMessage(costData.ValidCaseNumber, costData.ValidSkuId, costData.ValidQty, costData.ValidLocnId);
            sql1 = $"select WMSTOEMS_MSGKEY_SEQ.nextval from dual";
            command = new OracleCommand(sql1, db);
            costData.ValidMsgKey = Convert.ToInt64(command.ExecuteScalar().ToString());
            emsToWmsParameters = new EmsToWmsDto
            {
                Process = "EMS",
                MessageKey = Convert.ToInt64(costData.ValidMsgKey),
                Status = "Ready",
                Transaction = TransactionCode.Cost,
                ResponseCode = (short)int.Parse(DefaultPossibleValue.ReasonCode.Success),
            };
            var validsql = $"insert into emstowms values ('{emsToWmsParameters.Process}','{emsToWmsParameters.MessageKey}','{emsToWmsParameters.Status}','{emsToWmsParameters.Transaction}','{CostResult}','0','TestUser','22-JUL-19','22-JUL-19')";
            command = new OracleCommand(validsql, db);
            command.ExecuteNonQuery();
            transaction.Commit();
        }

        public string CreateCostMessage(string containerNbr, string skuId, string qty, string locnId)
        {
            CostParameters = new CostDto
            {
                ActionCode = "Arrival",
                ContainerReasonCodeMap = ReasonCode.Success,
                ContainerId = "00100283000815502000",
                ContainerType = "Case",
                PhysicalContainerId = "",
                CurrentLocationId = locnId,
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
            var testResult = gm.BuildMessage<CostDto, CostValidationRule>(CostParameters, DefaultPossibleValue.TransactionCode.Cost);         
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
            command = new OracleCommand(sql1, db);
            var invalidCaseNumber = CreateCostMessage("00000283000804736790", costData.ValidSkuId, costData.ValidQty, costData.ValidLocnId);
            costData.InvalidCaseNumberKey = GetSeqNbr(db);
            var invalidCaseSql = $"insert into emstowms values ('{emsToWmsParameters.Process}','{costData.InvalidCaseNumberKey}','{emsToWmsParameters.Status}','{emsToWmsParameters.Transaction}','{invalidCaseNumber}','0','TestUser','22-JUL-19','22-JUL-19')";
            command = new OracleCommand(invalidCaseSql, db);
            command.ExecuteNonQuery();
            transaction.Commit();
        }

        public void TransInvnDoesNotExistData(OracleConnection db)
        {
            sql1 = $"select cd.SKU_ID,ch.CASE_NBR,tn.ACTL_INVN_UNITS,ch.STAT_CODE from  CASE_HDR ch  inner join  case_dtl cd on cd.CASE_NBR = ch.CASE_NBR  inner join pick_locn_dtl on pick_locn_dtl.sku_id = cd.sku_id left join trans_invn tn on tn.SKU_ID = cd.SKU_ID and ch.STAT_CODE = 50 and tn.ACTL_INVN_UNITS >1 and trans_invn_type = '18' and tn.SKU_ID = null";
            command = new OracleCommand(sql1, db);
            var dr17 = command.ExecuteReader();
            if (dr17.Read())
            {
                costData.TransInvnNotExistCase = dr17["CASE_NBR"].ToString();
                costData.TransInvnNotExistSku = dr17["SKU_ID"].ToString();
                costData.TransInvnNotExistQty = dr17["ACTL_INVN_UNITS"].ToString();
            }
            transaction = db.BeginTransaction();
            costData.TransInvnNotExistKey = GetSeqNbr(db);
            var transInvnNotExistMsg = CreateCostMessage(costData.TransInvnNotExistCase, costData.TransInvnNotExistSku, costData.TransInvnNotExistQty, costData.ValidLocnId);
            var transInvnNotExistSql = $"insert into emstowms values ('{emsToWmsParameters.Process}','{costData.TransInvnNotExistKey}','{emsToWmsParameters.Status}','{emsToWmsParameters.Transaction}','{transInvnNotExistMsg}','0','TestUser','22-JUL-19','22-JUL-19')";
            command = new OracleCommand(transInvnNotExistSql, db);
            command.ExecuteNonQuery();
            transaction.Commit();
        }

        public void PickLocnDoesNotExistData(OracleConnection db)
        {
            sql1 = $"select tn.ACTL_INVN_UNITS,cd.SKU_ID,ch.CASE_NBR,ch.STAT_CODE from  CASE_HDR ch  inner join  case_dtl cd on cd.CASE_NBR = ch.CASE_NBR  inner join trans_invn tn on tn.SKU_ID = cd.SKU_ID  left join pick_locn_dtl on pick_locn_dtl.sku_id = tn.sku_id  and ch.STAT_CODE = 96 and tn.ACTL_INVN_UNITS >1 and trans_invn_type = '18' and pick_locn_dtl.LOCN_ID = 0";
            command = new OracleCommand(sql1, db);
            var dr18 = command.ExecuteReader();
            if (dr18.Read())
            {
                costData.PickLocnDtlNotExistCase = dr18["CASE_NBR"].ToString();
                costData.PickLocnDtlNotExistSku = dr18["SKU_ID"].ToString();
                costData.PickLocnDtlNotExistQty = dr18["ACTL_INVN_UNITS"].ToString();
            }
            transaction = db.BeginTransaction();
            costData.PickLocationNotExistKey = GetSeqNbr(db);

            var pickLnInvnNotExistMsg = CreateCostMessage(costData.PickLocnDtlNotExistCase, costData.PickLocnDtlNotExistSku, costData.PickLocnDtlNotExistQty, costData.ValidLocnId);
            var pickLnInvnNotExistSql = $"insert into emstowms values ('{emsToWmsParameters.Process}','{costData.PickLocationNotExistKey}','{emsToWmsParameters.Status}','{emsToWmsParameters.Transaction}','{pickLnInvnNotExistMsg}','0','TestUser','22-JUL-19','22-JUL-19')";
            command = new OracleCommand(pickLnInvnNotExistSql, db);
            command.ExecuteNonQuery();
            transaction.Commit();
        }

        public Int64 GetSeqNbr(OracleConnection db)
        {
            sql1 = $"select WMSTOEMS_MSGKEY_SEQ.nextval from dual";
            command = new OracleCommand(sql1, db);
            var key = Convert.ToInt64(command.ExecuteScalar().ToString());
            return key;
        }

        public SwmFromMheDto SwmFromMhe(OracleConnection db, string caseNbr, string trx, string skuId)
        {
            var swmFromMheData = new SwmFromMheDto();
            sql1 = $"select * from swm_from_mhe where container_id = '{caseNbr}' and source_msg_trans_code = '{trx}' and sku_id = '{skuId}' order by created_date_time desc";
            command = new OracleCommand(sql1, db);
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
        public void SwmFromMheForCostData(OracleConnection db, string caseNbr, string trx, string skuId)
        {
            swmFromMhe = SwmFromMhe(db, caseNbr, trx, skuId);
            var msgDto = JsonConvert.DeserializeObject<CostDto>(swmFromMhe.MessageJson);
            cost.TransactionCode = msgDto.TransactionCode;
            cost.ActionCode = msgDto.ActionCode;
            cost.MessageLength = msgDto.MessageLength;
            cost.ContainerReasonCodeMap = msgDto.ContainerReasonCodeMap;
            cost.ContainerId = msgDto.ContainerId;
            cost.StorageClassAttribute1 = msgDto.StorageClassAttribute1;
            cost.StorageClassAttribute2 = msgDto.StorageClassAttribute2;
            cost.CurrentLocationId = msgDto.CurrentLocationId;
            cost.ContainerType = msgDto.ContainerType;
            cost.PalletLpn = msgDto.PalletLpn;
        }

        public void TransInvnPickLocnDataAfterTrigger(OracleConnection db)
        {
            sql1 = $"select tn.ACTL_INVN_UNITS,tn.Actl_wt,pl.ACTL_INVN_QTY,pl.TO_BE_FILLD_QTY,pl.LOCN_ID from trans_invn tn inner join  pick_locn_dtl pl on tn.sku_id = pl.sku_id and tn.trans_invn_type= 18 and tn.sku_id = '{cost.StorageClassAttribute1}' order by tn.mod_date_time desc";
            command = new OracleCommand(sql1, db);
            var dr3 = command.ExecuteReader();
            if (dr3.Read())
            {
                transInvn.ActualInventoryUnits = Convert.ToDecimal(dr3["ACTL_INVN_UNITS"].ToString());
                transInvn.ActualWeight = Convert.ToDecimal(dr3["ACTL_WT"].ToString());
                pickLocnDtl.ActualInventoryQuantity = Convert.ToDecimal(dr3["ACTL_INVN_QTY"].ToString());
                pickLocnDtl.ToBeFilledQty = Convert.ToDecimal(dr3["TO_BE_FILLD_QTY"].ToString());
                pickLocnDtl.LocationId = dr3["LOCN_ID"].ToString();
            }
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
                SwmFromMheForCostData(db, costData.ValidCaseNumber, TransactionCode.Cost, costData.ValidSkuId);
                TransInvnPickLocnDataAfterTrigger(db);
            }
        }     
    }
}
