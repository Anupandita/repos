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
using Sfc.Wms.ParserAndTranslator.Contracts.Constants;
using Sfc.Wms.ParserAndTranslator.Contracts.Dto;
using Sfc.Wms.ParserAndTranslator.Contracts.Validation;
using Sfc.Wms.Result;
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
        protected CostDto CostParams;
        protected EmsToWmsDto emsToWmsParameters;
        protected EmsToWmsDto emsToWms = new EmsToWmsDto();
        private readonly IHaveDataTypeValidation _dataTypeValidation;
        protected PickLocationDtlDto pickLocnDtl = new PickLocationDtlDto();
        protected PickLocationDtlDto pickLcnDtl = new PickLocationDtlDto();
        protected CaseHeaderDto caseHdr = new CaseHeaderDto();
        protected CostDto cost = new CostDto();
        public decimal unitWeight;
        protected string sql1 = "";
        protected string msgkey;
        public Int64 validMsgKey;
        public Int64 invalidMsgTextKey;
        public Int64 invalidCaseNumberKey;
        public Int64 invalidStsKey;
        public Int64 transInvnNotExistKey;
        public string validCaseNumber;
        public string validSkuId;
        public string validQty;
        public string invalidStsCase;
        public string invalidStsSku;
        public string invalidStsQty;


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
                sql1 = $"select tn.SKU_ID,tn.ACTL_INVN_UNITS,cd.SKU_ID,ch.CASE_NBR,ch.STAT_CODE from  TRANS_INVN tn INNER JOIN CASE_DTL cd  on tn.SKU_ID = cd.SKU_ID INNER JOIN CASE_HDR ch on cd.CASE_NBR = ch.CASE_NBR and ch.STAT_CODE = 96 and tn.ACTL_INVN_UNITS >1 and trans_invn_type = '18'";
                command = new OracleCommand(sql1, db);
                var dr15 = command.ExecuteReader();
                if (dr15.Read())
                {
                    validCaseNumber = dr15["CASE_NBR"].ToString();
                    validSkuId = dr15["SKU_ID"].ToString();
                    validQty = dr15["ACTL_INVN_UNITS"].ToString();
                }

                sql1 = $"select unit_wt from item_master where sku_id = '{validSkuId}'";
                command = new OracleCommand(sql1, db);
                var dr12 = command.ExecuteReader();
                if (dr12.Read())
                {
                    unitWeight = Convert.ToDecimal(dr12["UNIT_WT"].ToString());
                }

                var CostResult = CreateCostMessage(validCaseNumber,validSkuId,validQty);  
                
                sql1 = $"select WMSTOEMS_MSGKEY_SEQ.nextval from dual";
                command = new OracleCommand(sql1, db);
                validMsgKey = Convert.ToInt64(command.ExecuteScalar().ToString());
                emsToWmsParameters = new EmsToWmsDto
                {
                    Process = "EMS",
                    MessageKey = Convert.ToInt64(validMsgKey),
                    Status = "Ready",
                    Transaction = DefaultPossibleValue.TransactionCode.Cost,
                    // ResponseCode = DefaultPossibleValue.ReasonCode.Success,
                    AddWho = "Dematic",
                    AddDate = DateTime.Now,
                    ProcessedDate = DateTime.Now
                };

                var validsql = $"insert into emstowms values ('{emsToWmsParameters.Process}','{emsToWmsParameters.MessageKey}','{emsToWmsParameters.Status}','{emsToWmsParameters.Transaction}','{CostResult}','0','{emsToWmsParameters.AddWho}','22-JUL-19','22-JUL-19')";
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
                    emsToWms.AddWho = dr["ADDWHO"].ToString();
                }

                sql1 = $"select * from TRANS_INVN  where sku_id = '{validSkuId}' and trans_invn_type = '18'";
                command = new OracleCommand(sql1, db);
                var dr3 = command.ExecuteReader();
                if (dr3.Read())
                {
                    trn.ActualInventoryUnits = Convert.ToDecimal(dr3["ACTL_INVN_UNITS"].ToString());
                    Assert.AreNotEqual(0, trn.ActualInventoryUnits);
                }

                try
                {
                    sql1 = $"select * from PICK_LOCN_DTL where sku_id = '{validSkuId}'";
                    command = new OracleCommand(sql1, db);
                    var dr4 = command.ExecuteReader();
                    if (dr4.Read())
                    {
                        pickLcnDtl.ActualQty = Convert.ToDecimal(dr4["ACT_QTY"].ToString());
                        pickLcnDtl.ToBeFilledQty = Convert.ToDecimal(dr4["TO_BE_FILLED_QTY"].ToString());
                        pickLcnDtl.UserId = dr4["USER_ID"].ToString();
                        pickLcnDtl.LocationId = dr4["LOCN_ID"].ToString();
                        pickLcnDtl.UpdatedDateTime = Convert.ToDateTime(dr4["MOD_DATE_TIME"].ToString());
                    }
                }
                catch
                {
                    Debug.Print("There are no records");
                }

            }
        }

        public string CreateCostMessage(string containerNbr, string skuId, string qty)
        {
            CostParameters = new CostDto
            {
                ActionCode = "Arrival",
                ContainerReasonCodeMap = DefaultPossibleValue.ReasonCode.Success,
                ContainerId = containerNbr,
                ContainerType = "Case",
                PhysicalContainerId = containerNbr,
                CurrentLocationId = "123",
                StorageClassAttribute1 = skuId,
                StorageClassAttribute2 = qty,
                StorageClassAttribute3 = "",
                StorageClassAttribute4 = "",
                StorageClassAttribute5 = "",
                PalletLpn = "08019279187",
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
               
                var invalidCaseNumber = CreateCostMessage("00000283000804736790",validSkuId,validQty);
                invalidCaseNumberKey = GetSeqNbr(command, db);
                var invalidCaseSql = $"insert into emstowms values ('{emsToWmsParameters.Process}','{invalidCaseNumberKey}','{emsToWmsParameters.Status}','{emsToWmsParameters.Transaction}','{invalidCaseNumber}','0','{emsToWmsParameters.AddWho}','22-JUL-19','22-JUL-19')";
                command = new OracleCommand(invalidCaseSql, db);
                command.ExecuteNonQuery();
                //transaction.Commit();
                invalidStsKey = GetSeqNbr(command, db);

                sql1 = $"select tn.SKU_ID,tn.ACTL_INVN_UNITS,cd.SKU_ID,ch.CASE_NBR,ch.STAT_CODE from  TRANS_INVN tn INNER JOIN CASE_DTL cd  on tn.SKU_ID = cd.SKU_ID INNER JOIN CASE_HDR ch on cd.CASE_NBR = ch.CASE_NBR and ch.STAT_CODE = 50 and tn.ACTL_INVN_UNITS >1 and trans_invn_type = '18'";
                command = new OracleCommand(sql1, db);
                var dr16 = command.ExecuteReader();
                if (dr16.Read())
                {
                    invalidStsCase = dr16["CASE_NBR"].ToString();
                    invalidStsSku  = dr16["SKU_ID"].ToString();
                    invalidStsQty = dr16["ACTL_INVN_UNITS"].ToString();
                }

                var invalidstatusCostMsg = CreateCostMessage(invalidStsCase,invalidStsSku,invalidStsQty);
                var invalidStsSql = $"insert into emstowms values ('{emsToWmsParameters.Process}','{invalidStsKey}','{emsToWmsParameters.Status}','{emsToWmsParameters.Transaction}','{invalidstatusCostMsg}','0','{emsToWmsParameters.AddWho}','22-JUL-19','22-JUL-19')";
                command = new OracleCommand(invalidStsSql, db);
                command.ExecuteNonQuery();
                // transaction.Commit();

                transInvnNotExistKey = GetSeqNbr(command, db);

                var transInvnNotExistMsg = CreateCostMessage("00100283000803374979", "3970291","27");
                var transInvnNotExistSql = $"insert into emstowms values ('{emsToWmsParameters.Process}','{transInvnNotExistKey}','{emsToWmsParameters.Status}','{emsToWmsParameters.Transaction}','{transInvnNotExistMsg}','0','{emsToWmsParameters.AddWho}','22-JUL-19','22-JUL-19')";
                command = new OracleCommand(transInvnNotExistSql, db);
                command.ExecuteNonQuery();
                // transaction.Commit();
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
                sql1 = $"select * from swm_from_mhe  where source_msg_key = '{emsToWmsParameters.MessageKey}'";
                command = new OracleCommand(sql1, db);
                var dr1 = command.ExecuteReader();
                if (dr1.Read())
                {
                    swmFromMhe.SourceMessageKey = Convert.ToInt16(dr1["SOURCE_MSG_KEY"].ToString());
                    swmFromMhe.SourceMessageResponseCode = Convert.ToInt16(dr1["SOURCE_MSG_RSN_CODE"].ToString());
                    swmFromMhe.SourceMessageStatus = dr1["SOURCE_MSG_STATUS"].ToString();
                    swmFromMhe.CreatedDateTime = Convert.ToDateTime(dr1["CREATED_DATE_TIME"].ToString());
                    swmFromMhe.SourceMessageProcess = dr1["SOURCE_MSG_PROCESS"].ToString();
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

                sql1 = $"select * from TRANS_INVN  where sku_id = '{cost.StorageClassAttribute1} and trans_invn_type = '18'";
                command = new OracleCommand(sql1, db);
                var dr3 = command.ExecuteReader();
                if (dr3.Read())
                {
                    transInvn.ActualInventoryUnits = Convert.ToDecimal(dr3["ACTL_INVN_UNITS"].ToString());
                    transInvn.ActualWeight = Convert.ToDecimal(dr3["ACTL_WT"].ToString());
                    transInvn.TransitionInventoryType = Convert.ToInt16(dr3["TRANS_INVN_TYPE"].ToString());
                }

                sql1 = $"select * from PICK_LOCN_DTL where sku_id = '{cost.StorageClassAttribute1}'";
                command = new OracleCommand(sql1, db);
                var dr4 = command.ExecuteReader();
                if (dr4.Read())
                {
                    pickLocnDtl.ActualQty = Convert.ToDecimal(dr4["ACT_QTY"].ToString());
                    pickLocnDtl.ToBeFilledQty = Convert.ToDecimal(dr4["TO_BE_FILLED_QTY"].ToString());
                    pickLocnDtl.UserId = dr4["USER_ID"].ToString();
                    pickLocnDtl.LocationId = dr4["LOCN_ID"].ToString();
                    pickLocnDtl.UpdatedDateTime = Convert.ToDateTime(dr4["MOD_DATE_TIME"].ToString());
                }
            }
        }     
    }
}
