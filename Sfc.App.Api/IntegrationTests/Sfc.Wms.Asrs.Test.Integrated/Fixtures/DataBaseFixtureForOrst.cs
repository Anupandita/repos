using System;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData;
using Sfc.Wms.Asrs.Dematic.Contracts.Dtos;
using Sfc.Wms.Asrs.Shamrock.Contracts.Dtos;
using Sfc.Wms.Builder.MessageBuilder;
using Sfc.Wms.ParserAndTranslator.Contracts.Constants;
using Sfc.Wms.ParserAndTranslator.Contracts.Dto;
using Sfc.Wms.ParserAndTranslator.Contracts.Interfaces;
using Sfc.Wms.ParserAndTranslator.Contracts.Validation;
using Sfc.Wms.Result;


namespace Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures
{
    public class DataBaseFixtureForOrst : DataBaseFixtureForCost
    {
        protected SwmToMheDto case1 = new SwmToMheDto();
        protected SwmToMheDto case2 = new SwmToMheDto();
        protected SwmToMheDto case3 = new SwmToMheDto();
        protected SwmToMheDto case4 = new SwmToMheDto();
        protected OrstDto OrstParameters;
        private readonly IHaveDataTypeValidation _dataTypeValidation;
        protected OrmtDto OrmtCase1 = new OrmtDto();
        protected OrmtDto OrmtCase2 = new OrmtDto();
        protected OrmtDto OrmtCase3 = new OrmtDto();
        protected OrmtDto OrmtCase4 = new OrmtDto();
        // case1 = allocated status
        // case2 = completed status
        //case3 = Deallocate status
        


        public DataBaseFixtureForOrst()
        {
            _dataTypeValidation = new DataTypeValidation();
        }

        public SwmToMheDto GetCartonDetailsForInsertingOrstMessage(OracleConnection db,int statusCode)
        {
            var swmToMheView = new SwmToMheDto();
            var sqlStatement = $"select swm_to_mhe.container_id,swm_to_mhe.sku_id,swm_to_mhe.qty,swm_to_mhe.msg_json from swm_to_mhe inner join carton_hdr on swm_to_mhe.container_id = carton_hdr.carton_nbr and swm_to_mhe.source_msg_status = 'Ready' and swm_to_mhe.qty!= 0 and carton_hdr.stat_code = {statusCode}";
            command = new OracleCommand(sqlStatement, db);
            var swmToMheReader = command.ExecuteReader();
            if(swmToMheReader.Read())
            {
                swmToMheView.ContainerId = swmToMheReader["CONTAINER_ID"].ToString();
                swmToMheView.SkuId = swmToMheReader["SKU_ID"].ToString();
                swmToMheView.Quantity = Convert.ToInt16(swmToMheReader["QTY"].ToString());
                swmToMheView.MessageJson = swmToMheReader["MSG_JSON"].ToString();         
            }
            return swmToMheView;
        }
          
        public void GetDataBeforeTriggerOrst()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                case1 = GetCartonDetailsForInsertingOrstMessage(db, 12);
                OrmtCase1 = JsonConvert.DeserializeObject<OrmtDto>(case1.MessageJson);
                case2 = GetCartonDetailsForInsertingOrstMessage(db, 15);
                OrmtCase2 = JsonConvert.DeserializeObject<OrmtDto>(case2.MessageJson);
                case3 = GetCartonDetailsForInsertingOrstMessage(db, 30);
                OrmtCase3 = JsonConvert.DeserializeObject<OrmtDto>(case3.MessageJson);
                case4 = GetCartonDetailsForInsertingOrstMessage(db, 30);
                OrmtCase4 = JsonConvert.DeserializeObject<OrmtDto>(case4.MessageJson);
            }
        }

        public string CreateOrstMessage(string actionCode,string containerId,string skuId,string qty,string waveNbr)
        {
            OrstParameters = new OrstDto
            {
                TransactionCode = TransactionCode.Orst,
                MessageLength = MessageLength.Orst,
                ActionCode = actionCode,
                OrderId = containerId,
                OrderLineId = "1",
                CompletionTime = DateTime.Now.ToString("dd-MMM-yy"),
                Sku = skuId,
                Owner = "",
                UnitOfMeasure = "Case",
                ParentContainerId = containerId,
                QuantityOrdered = qty,
                QuantityDelivered = "1",
                DestinationLocationId = OrmtCase1.DestinationLocationId,
                CurrentLocationId = "asrsLocation",
                Priority = "",
                OrderReasonCodeMap = "",
                WaveId = waveNbr
            };
            GenericMessageBuilder gm = new GenericMessageBuilder(_dataTypeValidation);
            var testResult = gm.BuildMessage<OrstDto, OrstValidationRule>(OrstParameters, TransactionCode.Orst);
            Assert.AreEqual(testResult.ResultType, ResultTypes.Ok);
            Assert.IsNotNull(testResult.Payload);
            return testResult.Payload;
        }

        public void OrstMessageCreatedForAllocatedStatus(OracleConnection db)
        {
            var orstmsg = CreateOrstMessage("Allocated",case1.ContainerId,case1.SkuId, OrmtCase1.Quantity, OrmtCase1.WaveId);
            var emsToWms = new EmsToWmsDto
            {
                Process = DefaultValues.Process,
                Status = DefaultValues.Status,
                Transaction = TransactionCode.Cost,
                ResponseCode = (short)int.Parse(ReasonCode.Success),
                MessageText = orstmsg
            };
            var msgKey = InsertEmsToWMS(db, emsToWms);
        }

        public void OrstMessageCreatedForCompletedStatus(OracleConnection db)
        {
            var orstmsg = CreateOrstMessage("Completed", case2.ContainerId, case2.SkuId, OrmtCase2.Quantity, OrmtCase2.WaveId);
            var emsToWms = new EmsToWmsDto
            {
                Process = DefaultValues.Process,
                Status = DefaultValues.Status,
                Transaction = TransactionCode.Cost,
                ResponseCode = (short)int.Parse(ReasonCode.Success),
                MessageText = orstmsg
            };
            var msgKey = InsertEmsToWMS(db, emsToWms);
        }

        public void OrstMessageCreatedForDeallocateStatus(OracleConnection db)
        {
            var orstmsg = CreateOrstMessage("Deallocate", case3.ContainerId, case3.SkuId, OrmtCase3.Quantity, OrmtCase3.WaveId);
            var emsToWms = new EmsToWmsDto
            {
                Process = DefaultValues.Process,
                Status = DefaultValues.Status,
                Transaction = TransactionCode.Cost,
                ResponseCode = (short)int.Parse(ReasonCode.Success),
                MessageText = orstmsg
            };
            var msgKey = InsertEmsToWMS(db, emsToWms);
        }

        public void OrstMessageCreatedForCancelledStatus(OracleConnection db)
        {
            var orstmsg = CreateOrstMessage("Deallocate", case4.ContainerId, case4.SkuId, OrmtCase4.Quantity, OrmtCase4.WaveId);
            var emsToWms = new EmsToWmsDto
            {
                Process = DefaultValues.Process,
                Status = DefaultValues.Status,
                Transaction = TransactionCode.Cost,
                ResponseCode = (short)int.Parse(ReasonCode.Success),
                MessageText = orstmsg
            };
            var msgKey = InsertEmsToWMS(db, emsToWms);
        }

        












    }
}
