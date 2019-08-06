using System;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using Sfc.Wms.Asrs.Dematic.Contracts.Dtos;
using Sfc.Wms.Asrs.Shamrock.Contracts.Dtos;
using Sfc.Wms.Amh.Dematic.Contracts.Dtos;
using Sfc.Wms.Builder.MessageBuilder;
using Sfc.Wms.TransitionalInventory.Contracts.Dtos;
using Sfc.Wms.ParserAndTranslator.Contracts.Interfaces;
using Sfc.Wms.ParserAndTranslator.Contracts.Dto;
using Sfc.Wms.ParserAndTranslator.Contracts.Validation;
using Sfc.Wms.Result;
using Sfc.Wms.Asrs.Test.Integrated.TestData;
using DefaultPossibleValue = Sfc.Wms.DematicMessage.Contracts.Constants;
using System.Diagnostics;


namespace Sfc.Wms.Asrs.Test.Integrated.Fixtures
{
    public class DataBaseFixtureForCost
    {
        protected WmsToEmsDto wte = new WmsToEmsDto();
        protected SwmToMheDto swmToMhe = new SwmToMheDto();
        protected SwmFromMheDto swmFromMhe = new SwmFromMheDto();
        protected IvmtDto ivmt = new IvmtDto();
        protected TransitionalInventoryDto trn = new TransitionalInventoryDto();
        protected TransitionalInventoryDto transInvn = new TransitionalInventoryDto();
        protected AllocationInventoryDetailDto ai = new AllocationInventoryDetailDto();
        protected CostDto CostParameters;
        protected Cost costData = new Cost();
        protected EmsToWmsDto emsToWmsParameters;
        protected EmsToWmsDto emsToWms = new EmsToWmsDto();
        private readonly IHaveDataTypeValidation _dataTypeValidation;
        protected PickLocationDtlDto pickLocnDtl = new PickLocationDtlDto();
        protected PickLocationDtlDto pickLcnDtl = new PickLocationDtlDto();
        protected CaseHeaderDto caseHdr = new CaseHeaderDto();
        protected CostDto cost = new CostDto();
        public decimal unitWeight;
        protected string sql1 = "";
             
        public DataBaseFixtureForCost()
        {
            _dataTypeValidation = new DataTypeValidation();
        }

        public void GetDataBeforeTrigger()
        {
            OracleCommand command;
            OracleConnection db;
            OracleTransaction transaction;
            using (db = new OracleConnection
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["SfcRbacContextModel"].ConnectionString
            })
            {
                db.Open();
                transaction = db.BeginTransaction();
                sql1 = $"select swm_to_mhe.container_id,swm_to_mhe.sku_id,pick_locn_dtl.locn_id,swm_to_mhe.qty from swm_to_mhe inner join trans_invn on trans_invn.sku_id = swm_to_mhe.sku_id inner join  pick_locn_dtl on swm_to_mhe.sku_id = pick_locn_dtl.sku_id  inner join case_hdr on swm_to_mhe.container_id = case_hdr.case_nbr and swm_to_mhe.source_msg_status = 'Ready' and swm_to_mhe.qty!= 0 and case_hdr.stat_code = 96";
                command = new OracleCommand(sql1, db);
                var dr15 = command.ExecuteReader();
                if (dr15.Read())
                {
                    costData.ValidCaseNumber= dr15["CONTAINER_ID"].ToString();
                    costData.ValidSkuId = dr15["SKU_ID"].ToString();
                    costData.ValidQty = dr15["QTY"].ToString();
                    costData.ValidLocnId = dr15["LOCN_ID"].ToString();
                }


                sql1 = $"select unit_wt from item_master where sku_id = '{costData.ValidSkuId}'";
                command = new OracleCommand(sql1, db);
                var dr12 = command.ExecuteReader();
                if (dr12.Read())
                {
                    unitWeight = Convert.ToDecimal(dr12["UNIT_WT"].ToString());
                }

                var CostResult = CreateCostMessage(costData.ValidCaseNumber, costData.ValidSkuId, costData.ValidQty, costData.ValidLocnId);  
                
                sql1 = $"select WMSTOEMS_MSGKEY_SEQ.nextval from dual";
                command = new OracleCommand(sql1, db);
                costData.ValidMsgKey = Convert.ToInt64(command.ExecuteScalar().ToString());
                emsToWmsParameters = new EmsToWmsDto
                {
                    Process = "EMS",
                    MessageKey = Convert.ToInt64(costData.ValidMsgKey),
                    Status = "Ready",
                    Transaction = DefaultPossibleValue.TransactionCode.Cost,
                   // ResponseCode = DefaultPossibleValue.ReasonCode.Success,                    
                };

                var validsql = $"insert into emstowms values ('{emsToWmsParameters.Process}','{emsToWmsParameters.MessageKey}','{emsToWmsParameters.Status}','{emsToWmsParameters.Transaction}','{CostResult}','0','TestUser','22-JUL-19','22-JUL-19')";
                command = new OracleCommand(validsql, db);
                command.ExecuteNonQuery();
                transaction.Commit();

                sql1 = $"select * from emstowms where msgkey = '{emsToWmsParameters.MessageKey}'";
                command = new OracleCommand(sql1, db);
                var dr = command.ExecuteReader();
                if (dr.Read())
                {
                    emsToWms.MessageKey = Convert.ToInt64(dr["MSGKEY"].ToString());
                    emsToWms.Process = dr["PRC"].ToString();
                    //emsToWms.ResponseCode = dr["RSN"].ToString();
                    emsToWms.Status = dr["STS"].ToString();
                    emsToWms.Transaction = dr["TRX"].ToString();                   
                }
                
                sql1 = $"select tn.ACTL_INVN_UNITS,pl.ACTL_INVN_QTY,pl.TO_BE_FILLD_QTY,pl.LOCN_ID from trans_invn tn inner join  pick_locn_dtl pl on tn.sku_id = pl.sku_id and tn.trans_invn_type= 18 and tn.sku_id = '{costData.ValidSkuId}' order by tn.mod_date_time desc";
                command = new OracleCommand(sql1, db);
                var dr3 = command.ExecuteReader();
                if (dr3.Read())
                {
                    trn.ActualInventoryUnits = Convert.ToDecimal(dr3["ACTL_INVN_UNITS"].ToString());
                    Assert.AreNotEqual(0, trn.ActualInventoryUnits);
                    pickLcnDtl.ActualQty = Convert.ToDecimal(dr3["ACTL_INVN_QTY"].ToString());
                    pickLcnDtl.ToBeFilledQty = Convert.ToDecimal(dr3["TO_BE_FILLD_QTY"].ToString());
                    pickLcnDtl.LocationId = dr3["LOCN_ID"].ToString();
                }
            }
        }

        public string CreateCostMessage(string containerNbr, string skuId, string qty, string locnId)
        {
            CostParameters = new CostDto
            {
                ActionCode = "Arrival",
                ContainerReasonCodeMap = DefaultPossibleValue.ReasonCode.Success,
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
                TransactionCode = DefaultPossibleValue.TransactionCode.Cost,
                MessageLength = DefaultPossibleValue.MessageLength.Cost,
                ReasonCode = DefaultPossibleValue.ReasonCode.Success
            };
            GenericMessageBuilder gm = new GenericMessageBuilder(_dataTypeValidation);
            var testResult = gm.BuildMessage<CostDto, CostValidationRule>(CostParameters, DefaultPossibleValue.TransactionCode.Cost);         
            Assert.AreEqual(testResult.ResultType, ResultTypes.Ok);
            Assert.IsNotNull(testResult.Payload);
            return testResult.Payload;
        }
        
        public void NegativeCases()
        {
            OracleCommand command;
            OracleConnection db;
            OracleTransaction transaction;
            using (db = new OracleConnection
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["SfcRbacContextModel"].ConnectionString

            })
            {
                db.Open();                
                transaction = db.BeginTransaction();
                sql1 = $"select tn.SKU_ID,tn.ACTL_INVN_UNITS,cd.SKU_ID,ch.CASE_NBR,ch.STAT_CODE from  TRANS_INVN tn INNER JOIN CASE_DTL cd  on tn.SKU_ID = cd.SKU_ID INNER JOIN CASE_HDR ch on cd.CASE_NBR = ch.CASE_NBR and ch.STAT_CODE = 96 and tn.ACTL_INVN_UNITS >1 and trans_invn_type = '18'";
                command = new OracleCommand(sql1, db);
               
                var invalidCaseNumber = CreateCostMessage("00000283000804736790", costData.ValidSkuId, costData.ValidQty, costData.ValidLocnId);
                costData.InvalidCaseNumberKey = GetSeqNbr(command, db);
                var invalidCaseSql = $"insert into emstowms values ('{emsToWmsParameters.Process}','{costData.InvalidCaseNumberKey}','{emsToWmsParameters.Status}','{emsToWmsParameters.Transaction}','{invalidCaseNumber}','0','TestUser','22-JUL-19','22-JUL-19')";
                command = new OracleCommand(invalidCaseSql, db);
                command.ExecuteNonQuery();
                transaction.Commit();
                costData.InvalidStsKey = GetSeqNbr(command, db);

                sql1 = $"select tn.SKU_ID,tn.ACTL_INVN_UNITS,cd.SKU_ID,ch.CASE_NBR,ch.STAT_CODE from  TRANS_INVN tn INNER JOIN CASE_DTL cd  on tn.SKU_ID = cd.SKU_ID INNER JOIN CASE_HDR ch on cd.CASE_NBR = ch.CASE_NBR and ch.STAT_CODE = 50 and tn.ACTL_INVN_UNITS >1 and trans_invn_type = '18'";
                command = new OracleCommand(sql1, db);
                var dr16 = command.ExecuteReader();
                if (dr16.Read())
                {
                    costData.InvalidStsCase = dr16["CASE_NBR"].ToString();
                    costData.InvalidStsSku = dr16["SKU_ID"].ToString();
                    costData.InvalidStsQty = dr16["ACTL_INVN_UNITS"].ToString();
                }
                transaction = db.BeginTransaction();
                var invalidstatusCostMsg = CreateCostMessage(costData.InvalidStsCase, costData.InvalidStsSku, costData.InvalidStsQty, costData.ValidLocnId);
                var invalidStsSql = $"insert into emstowms values ('{emsToWmsParameters.Process}','{costData.InvalidStsKey}','{emsToWmsParameters.Status}','{emsToWmsParameters.Transaction}','{invalidstatusCostMsg}','0','TestUser','22-JUL-19','22-JUL-19')";
                command = new OracleCommand(invalidStsSql, db);
                command.ExecuteNonQuery();
                transaction.Commit();

                transaction = db.BeginTransaction();
                costData.TransInvnNotExistKey = GetSeqNbr(command, db);
                var transInvnNotExistMsg = CreateCostMessage("00100283000803374979", "3970291","27", costData.ValidLocnId);
                var transInvnNotExistSql = $"insert into emstowms values ('{emsToWmsParameters.Process}','{costData.TransInvnNotExistKey}','{emsToWmsParameters.Status}','{emsToWmsParameters.Transaction}','{transInvnNotExistMsg}','0','TestUser','22-JUL-19','22-JUL-19')";
                command = new OracleCommand(transInvnNotExistSql, db);
                command.ExecuteNonQuery();
                transaction.Commit();
            }
        }
    
        public Int64 GetSeqNbr(OracleCommand command, OracleConnection db)
        {
            sql1 = $"select WMSTOEMS_MSGKEY_SEQ.nextval from dual";
            command = new OracleCommand(sql1, db);
            var key = Convert.ToInt64(command.ExecuteScalar().ToString());
            return key;
        }

        public void GetDataAfterTrigger()
        {
            OracleCommand command;
            OracleConnection db;
            using (db = new OracleConnection
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["SfcRbacContextModel"].ConnectionString
            })
            {
                db.Open();
                sql1 = $"select * from swm_from_mhe  order by created_date_time desc";
                command = new OracleCommand(sql1, db);
                var dr1 = command.ExecuteReader();
                if (dr1.Read())
                {
                    swmFromMhe.SourceMessageKey = Convert.ToInt16(dr1["SOURCE_MSG_KEY"].ToString());
                    swmFromMhe.SourceMessageResponseCode = Convert.ToInt16(dr1["SOURCE_MSG_RSN_CODE"].ToString());
                    swmFromMhe.SourceMessageStatus = dr1["SOURCE_MSG_STATUS"].ToString();                   
                    swmFromMhe.SourceMessageProcess = dr1["SOURCE_MSG_PROCESS"].ToString();
                    swmFromMhe.SourceMessageTransactionCode = dr1["SOURCE_MSG_TRANS_CODE"].ToString();
                    swmFromMhe.ContainerId = dr1["CONTAINER_ID"].ToString();
                    swmFromMhe.ContainerType = dr1["CONTAINER_TYPE"].ToString();
                    swmFromMhe.MessageJson = dr1["MSG_JSON"].ToString();
                    swmFromMhe.LocationId = dr1["LOCN_ID"].ToString();
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

                sql1 = $"select tn.ACTL_INVN_UNITS,tn.Actl_wt,pl.ACTL_INVN_QTY,pl.TO_BE_FILLD_QTY,pl.LOCN_ID from trans_invn tn inner join  pick_locn_dtl pl on tn.sku_id = pl.sku_id and tn.trans_invn_type= 18 and tn.sku_id = '{cost.StorageClassAttribute1}' order by tn.mod_date_time desc";
                command = new OracleCommand(sql1, db);
                var dr3 = command.ExecuteReader();
                if (dr3.Read())
                {
                    transInvn.ActualInventoryUnits = Convert.ToDecimal(dr3["ACTL_INVN_UNITS"].ToString());
                    transInvn.ActualWeight = Convert.ToDecimal(dr3["ACTL_WT"].ToString());
                    pickLocnDtl.ActualQty = Convert.ToDecimal(dr3["ACTL_INVN_QTY"].ToString());
                    pickLocnDtl.ToBeFilledQty = Convert.ToDecimal(dr3["TO_BE_FILLD_QTY"].ToString());
                    pickLocnDtl.LocationId = dr3["LOCN_ID"].ToString();
                }
            }
        }     
    }
}
