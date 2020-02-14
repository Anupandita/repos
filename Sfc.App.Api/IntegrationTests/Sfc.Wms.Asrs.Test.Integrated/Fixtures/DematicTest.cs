using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using Sfc.Wms.Interfaces.Asrs.Dematic.Contracts.Dtos;
using Sfc.Wms.Interfaces.Asrs.Shamrock.Contracts.Dtos;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData;
using Sfc.Wms.Foundation.InboundLpn.Contracts.Dtos;
using Sfc.Wms.Interfaces.ParserAndTranslator.Contracts.Constants;
using Sfc.Wms.Interfaces.ParserAndTranslator.Contracts.Dto;
using Sfc.Core.OnPrem.Result;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Sfc.Core.OnPrem.ParserAndTranslator.Constants;
using Sfc.Wms.Foundation.Location.Contracts.Dtos;
using Sfc.Wms.Interfaces.Parser.Parsers;
using Sfc.Wms.Foundation.Tasks.Contracts.Dtos;
using Sfc.Wms.Foundation.TransitionalInventory.Contracts.Dtos;
using Sfc.Wms.Interfaces.Builder.MessageBuilder;
using Sfc.Wms.Interfaces.ParserAndTranslator.Contracts.Validation;
using Sfc.Core.OnPrem.ParserAndTranslator.Dtos;
using Sfc.Wms.Interfaces.ParserAndTranslator.Contracts.Interfaces;
using Sfc.Wms.Data.Context;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures
{   
    public class DematicTest : DataBaseFixture
    {
        protected WmsToEmsDto wmsToEms = new WmsToEmsDto();
        protected CostDto CostParameters;
        protected Cost CostData = new Cost();
        protected EmsToWmsDto EmsToWmsParameters;
        protected BaseResult<MessageHeaderDto> testResult;
        public WmsToEmsDto FetchDataFromWmsToEms(OracleConnection db,string trx)
        {
            var wmsToEmsData  = new WmsToEmsDto();
            var q = "Select * from wmstoems where trx = '{trx}' and STS = 'Ready' Order by adddate desc";
            Command = new OracleCommand(q, db);
            var wmsToEmsReader = Command.ExecuteReader();
            if (wmsToEmsReader.Read())
            {
                wmsToEmsData.Process = wmsToEmsReader["PRC"].ToString();
                wmsToEmsData.MessageKey = Convert.ToInt64(wmsToEmsReader["MSGKEY"].ToString());
                wmsToEmsData.Status = wmsToEmsReader["STS"].ToString();
                wmsToEmsData.Transaction = wmsToEmsReader["TRX"].ToString();
                wmsToEmsData.MessageText = wmsToEmsReader["MSGTEXT"].ToString();
                wmsToEmsData.ResponseCode = Convert.ToInt16(wmsToEmsReader["RSNRCODE"]);
            }
            return wmsToEmsData;
        }

        public void  DematicTestProcessFlow()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                wmsToEms = FetchDataFromWmsToEms(db,TransactionCode.Ivmt);
                UpdatetheStatusInWmsToEms(db, wmsToEms.MessageKey);
                testResult = ParserTestforMsgText(wmsToEms.Transaction, wmsToEms.MessageText);
                IvmtDto ivmtDto =(IvmtDto)testResult.Payload;
                var costResult = CreateCostMessage(ivmtDto.ContainerId, ivmtDto.Sku, ivmtDto.Quantity, "56789");
                EmsToWmsParameters = new EmsToWmsDto
                {
                    Process = DefaultPossibleValue.MessageProcessor,
                    MessageKey = Convert.ToInt64(CostData.MsgKey),
                    Status = DefaultValues.Status,
                    Transaction = TransactionCode.Cost,
                    ResponseCode = (short)int.Parse(ReasonCode.Success),
                    MessageText = costResult,
                };
            }
        }
      
        
        public void UpdatetheStatusInWmsToEms(OracleConnection db, Int64 msgKey)
        {
            Transaction = db.BeginTransaction();
            Query = $"update wmstoems set sts = 'Processed' where STS = 'Ready' and TRX = 'COMT' and msgKey = '{msgKey}'";
            Command = new OracleCommand(Query, db);
            Command.ExecuteNonQuery();
            Transaction.Commit();
        }

        public string CreateCostMessage(string containerNbr, string skuId, string qty, string locationId)
        {
            CostParameters = new CostDto
            {
                ActionCode = DefaultValues.ActionCodeCost,
                ContainerReasonCodeMap = ReasonCode.Success,
                ContainerId = Constants.InvalidContainerId,
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
    }
}
