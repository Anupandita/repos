using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData;
using Oracle.ManagedDataAccess.Client;
using Sfc.Wms.Interfaces.Builder.MessageBuilder;
using Sfc.Wms.Interfaces.ParserAndTranslator.Contracts.Constants;
using Sfc.Wms.Interfaces.ParserAndTranslator.Contracts.Dto;
using Sfc.Core.OnPrem.Result;
using Sfc.Wms.Interfaces.ParserAndTranslator.Contracts.Validation;
using Sfc.Wms.Interfaces.Asrs.Shamrock.Contracts.Dtos;
using Sfc.Wms.Interfaces.Asrs.Dematic.Contracts.Dtos;
using Newtonsoft.Json;
using Entities= Sfc.Wms.Data.Entities;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures
{
    
    public class DataBaseFixtureForMprq :CommonFunction
    {
        protected EmsToWmsDto EmsToWmsParameters;
        protected Mprq MprqData = new Mprq();
        protected MprqDto MprqParameters;
        protected string SqlStatements = "";
        protected MprqDto Mprq = new MprqDto();
        protected MpidDto Mpid = new MpidDto();
        protected new SwmFromMheDto SwmFromMhe = new SwmFromMheDto();
        protected new SwmToMheDto SwmToMhe = new SwmToMheDto();
        protected new string Query = "";
        protected  Entities.NextUpCounter NextUpCounter= new Entities.NextUpCounter();

        public void GetDataBeforeTrigger()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                var mprqResult = CreateMprqMessage();
                EmsToWmsParameters = new EmsToWmsDto
                {
                    Process = DefaultPossibleValue.MessageProcessor,
                    MessageKey = Convert.ToInt64(MprqData.MsgKey),
                    Status = DefaultValues.Status,
                    Transaction = TransactionCode.Mprq,
                    ResponseCode = (short)int.Parse(ReasonCode.Success),
                    MessageText = mprqResult,
                };
                MprqData.MsgKey = InsertEmsToWms(db, EmsToWmsParameters);
                NextUpCounter =Nxtupcnt(db);

            }
        }

        public Entities.NextUpCounter Nxtupcnt(OracleConnection db)
        {
            var nextup = new Entities.NextUpCounter();
            Query = $"select * from nxt_up_cnt where rec_type_id = :recTypeId";
            Command = new OracleCommand(Query, db);
            Command.Parameters.Add(new OracleParameter(Parameter.RecTypeId, Constants.RecTypeId));
            var nextUpCounterReader = Command.ExecuteReader();
            if (nextUpCounterReader.Read())
            {
                nextup.CurrentNumber = Convert.ToInt32(nextUpCounterReader[FieldName.Currentnumber]);
                nextup.PrefixField = nextUpCounterReader[FieldName.Prefixfield].ToString();           
            }
            return nextup;
        }

        public string CreateMprqMessage()
        {
            MprqParameters = new MprqDto
            {
                LocationId = Constants.SampleCurrentLocnId,
                TransactionCode = TransactionCode.Mprq,
                MessageLength = MessageLength.Mprq
            };
            var buildMessage = new MessageHeaderBuilder();
            var testResult = buildMessage.BuildMessage<MprqDto, MprqValidationRule>(MprqParameters, TransactionCode.Mprq);
            Assert.AreEqual(testResult.ResultType, ResultTypes.Ok);
            Assert.IsNotNull(testResult.Payload);
            return testResult.Payload;
        }


        public void GetDataAfterTrigger()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                SwmFromMhe = SwmFromMhe(db, MprqData.MsgKey, TransactionCode.Mprq);
                Mprq = JsonConvert.DeserializeObject<MprqDto>(SwmFromMhe.MessageJson);
                SwmToMhe = SwmToMhe(db, null,TransactionCode.Mpid,null);
                Mpid = JsonConvert.DeserializeObject<MpidDto>(SwmToMhe.MessageJson);
            }
        }
    }
}
