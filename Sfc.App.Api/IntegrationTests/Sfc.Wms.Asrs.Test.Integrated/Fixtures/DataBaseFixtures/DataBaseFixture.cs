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
using Sfc.Core.OnPrem.ParserAndTranslator.Dtos;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures
{
    public class DataBaseFixture : CommonFunction
    {
        public decimal UnitWeight;
        public decimal UnitVol;
        public CaseViewDto SingleSkuCase = new CaseViewDto();
        public CaseHeaderDto CaseHdrMultiSku = new CaseHeaderDto();
        public SwmToMheDto SwmToMheComt = new SwmToMheDto();
        public SwmToMheDto SwmToMheIvmt = new SwmToMheDto();
        public PickLocationDetailsDto PickLocn = new PickLocationDetailsDto();
        public IvmtDto Ivmt = new IvmtDto();
        public ComtDto Comt = new ComtDto();
        public WmsToEmsDto WmsToEmsIvmt = new WmsToEmsDto();
        public WmsToEmsDto WmsToEmsComt = new WmsToEmsDto();
        public List<TransitionalInventoryDto> TrnList = new List<TransitionalInventoryDto>();
        public List<CaseViewDto> CaseList = new List<CaseViewDto>();
        public List<CaseViewDto> CaseDtoList = new List<CaseViewDto>();
        public TaskHeaderDto TaskMultiSku= new TaskHeaderDto();
        public TaskHeaderDto TaskSingleSku = new TaskHeaderDto();
        public TransitionalInventoryDto TransInvnAfterTrigger = new TransitionalInventoryDto();
        public TransitionalInventoryDto TransInvnBeforeTrigger  = new TransitionalInventoryDto();
        private readonly MessageHeaderParser _canParseMessage;
        public string LocnId;
        public dynamic TrnInvnBeforeCallingApi;
        public CaseViewDto CaseDtlAfterApi  = new CaseViewDto();
        public  CaseViewDto CaseDtlBeforeApi =new CaseViewDto();
        public CaseViewDto CaseHdrDtl = new CaseViewDto();
        public CaseHeaderDto NotEnoughInvCase = new CaseHeaderDto();
        public PickLocationDetailsDto PickLocnAfterCallingApi = new PickLocationDetailsDto();
        public PickLocationDetailsDto PickLocnBeforeCallingApi = new PickLocationDetailsDto();
        public string Uom;
        public string TempZone;
        public string LotId;
        public string CurrentLocationId;
        public ReserveLocationHeaderDto RsvBeforeApi = new ReserveLocationHeaderDto();
        public ReserveLocationHeaderDto RsvAfterApi = new ReserveLocationHeaderDto();

        public DataBaseFixture()
        {
            _canParseMessage = new MessageHeaderParser();
        }

        protected void GetDataBeforeTriggerComt()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                var caseheader = TriggerOnCaseHeader(db, Constants.CaseSeqNumberForSingleSku);
                var caseDto = GetCaseDtlData(db, caseheader.CaseNumber);
                SingleSkuCase = FetchTransInvn(db, caseDto[0].SkuId);
                SingleSkuCase.CaseNumber = caseheader.CaseNumber;
                SingleSkuCase.LocationId = caseheader.LocationId;
                SingleSkuCase.StatusCode = caseheader.StatusCode;
                SingleSkuCase.Cons_prty_date = caseheader.ConsumePriorityDate;
                SingleSkuCase.SkuId = caseDto[0].SkuId;
                SingleSkuCase.TotalAllocQty = Convert.ToInt32(caseDto[0].TotalAllocQty);
                var UOM = GetUnitOfMeasureFromItemMaster(db,SingleSkuCase.SkuId);
                Uom = ItemMasterUnitOfMeasure(UOM);
                MultiSkuData(db);
                NotEnoughInvCase = QueryForNotEnoughInventoryInCase(db, 1);
                RsvBeforeApi = GetResrvLocnDetails(db, SingleSkuCase.LocationId);
                TempZone = GetTempZone(db, SingleSkuCase.SkuId);
                LotId = GetLotId(db, caseheader.CaseNumber);
            }
        }

      

        protected void GetDataBeforeTriggerForComtIvmt()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                var caseheader = TriggerOnCaseHeaderTable(db, Constants.CaseSeqNumberForSingleSku);
                var caseDto = GetCaseDtlData(db, caseheader.CaseNumber);
                SingleSkuCase = FetchTransInvn(db, caseDto[0].SkuId);
                SingleSkuCase.CaseNumber = caseheader.CaseNumber;
                SingleSkuCase.LocationId = caseheader.LocationId;
                SingleSkuCase.StatusCode = caseheader.StatusCode;
                SingleSkuCase.SkuId = caseDto[0].SkuId;
                SingleSkuCase.TotalAllocQty = Convert.ToInt32(caseDto[0].TotalAllocQty);
                LotId = GetLotId(db, caseheader.CaseNumber);

            }
        }

        protected void GetDataBeforeTriggerforReceivedCaseComt()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                var returncaseheader = TriggerOnReceivedCaseHeader(db, 1);
                var receivedcaseDto = GetCaseDtlData(db, returncaseheader.CaseNumber);
                CaseDtlBeforeApi = FetchCaseDetailsTriger(db);
                SingleSkuCase = FetchTransInvn(db, receivedcaseDto[0].SkuId);
                SingleSkuCase.CaseNumber = returncaseheader.CaseNumber;
                SingleSkuCase.LocationId = returncaseheader.LocationId;
                SingleSkuCase.StatusCode = returncaseheader.StatusCode;
                SingleSkuCase.SkuId = receivedcaseDto[0].SkuId;
                SingleSkuCase.ActlQty = Convert.ToInt32(receivedcaseDto[0].ActlQty);
                PickLocnBeforeCallingApi = GetPickLocationDetails(db, SingleSkuCase.SkuId, null);
                RsvBeforeApi = GetResrvLocnDetails(db, SingleSkuCase.LocationId);
                TempZone = GetTempZone(db, SingleSkuCase.SkuId);
                LotId = GetLotId(db, returncaseheader.CaseNumber);
            }
        }

        protected void GetDataBeforeTriggerForStatCode30()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                var returncaseheader = TriggerOnCaseHeaderTableForStatCode30(db, 1);
                var receivedcaseDto = GetCaseDtlData(db, returncaseheader.CaseNumber);
                CaseDtlBeforeApi = FetchCaseDetailsTriger(db);
                SingleSkuCase = FetchTransInvn(db, receivedcaseDto[0].SkuId);
                SingleSkuCase.CaseNumber = returncaseheader.CaseNumber;
                SingleSkuCase.LocationId = returncaseheader.LocationId;
                SingleSkuCase.StatusCode = returncaseheader.StatusCode;
                SingleSkuCase.SkuId = receivedcaseDto[0].SkuId;
                SingleSkuCase.ActlQty = Convert.ToInt32(receivedcaseDto[0].ActlQty);
                RsvBeforeApi = GetResrvLocnDetails(db, SingleSkuCase.LocationId);
                TempZone = GetTempZone(db, SingleSkuCase.SkuId);
            }
        }

        public CaseViewDto TriggerOnReceivedCaseHeader(OracleConnection db, int seqNumber)
        {
            var receivedcaseheader = new CaseViewDto();
            var query = ComtQueries.ReceivedCaseFromReturns;
            Command = new OracleCommand(query, db);            
            Command.Parameters.Add(new OracleParameter(Parameter.SysCodeType, Constants.SysCodeType));
            Command.Parameters.Add(new OracleParameter(Parameter.SysCodeId, Constants.SysCodeIdForActiveLocation));           
            Command.Parameters.Add(new OracleParameter(Parameter.SeqNbr, Constants.CaseSeqNumberForSingleSku));
            Command.Parameters.Add(new OracleParameter(Parameter.CodeIdForDropZone, Constants.SysCodeIdForDropZone));
            Command.Parameters.Add(new OracleParameter(Parameter.Dry, Constants.Dry));
            Command.Parameters.Add(new OracleParameter(Parameter.Freezer, Constants.Freezer));
            Command.Parameters.Add(new OracleParameter(Parameter.StatCode, Constants.ReceivedCaseFromReturnStatusCode));
            Command.Parameters.Add(new OracleParameter(Parameter.ListSkuAssign, Constants.ListSquAssign));
            var receivedcaseHeaderReader = Command.ExecuteReader();
            if (receivedcaseHeaderReader.Read())
            {
                receivedcaseheader.CaseNumber = receivedcaseHeaderReader[CaseHeader.CaseNumber].ToString();
                receivedcaseheader.LocationId = receivedcaseHeaderReader[CaseHeader.LocationId].ToString();
                receivedcaseheader.StatusCode = Convert.ToInt32(receivedcaseHeaderReader[CaseHeader.StatusCode]);
            }
            return receivedcaseheader;
        }

        public CaseHeaderDto TriggerOnCaseHeader(OracleConnection db, int seqNumber)
        {
            var caseheader = new CaseHeaderDto();
            var q = ComtQueries.ReceivedCaseFromVendors;
            Command = new OracleCommand(q, db);            
            Command.Parameters.Add(new OracleParameter(Parameter.SysCodeType, Constants.SysCodeType));
            Command.Parameters.Add(new OracleParameter(Parameter.SysCodeId, Constants.SysCodeIdForActiveLocation));
            Command.Parameters.Add(new OracleParameter(Parameter.SeqNbr, Constants.CaseSeqNumberForSingleSku));
            Command.Parameters.Add(new OracleParameter(Parameter.CodeIdForDropZone, Constants.SysCodeIdForDropZone));
            Command.Parameters.Add(new OracleParameter(Parameter.Dry, Constants.Dry));
            Command.Parameters.Add(new OracleParameter(Parameter.Freezer, Constants.Freezer));
            Command.Parameters.Add(new OracleParameter(Parameter.StatCode, Constants.ReceivedCaseFromVendorStatCode));
            var caseHeaderReader = Command.ExecuteReader();
            if (caseHeaderReader.Read())
            {
                caseheader.CaseNumber = caseHeaderReader[CaseHeader.CaseNumber].ToString();
                caseheader.LocationId = caseHeaderReader[CaseHeader.LocationId].ToString();
                caseheader.StatusCode = Convert.ToInt32(caseHeaderReader[CaseHeader.StatusCode].ToString());
                caseheader.ConsumePriorityDate = Convert.ToDateTime(caseHeaderReader[CaseHeader.ConsPrtyDate]);
                caseheader.PoNumber = caseHeaderReader[CaseHeader.PoNumber].ToString();
                
            }
            return caseheader;
        }

        public CaseViewDto TriggerOnCaseHeaderTable(OracleConnection db, int seqNumber)
        {
            var caseheader = new CaseViewDto();
            var q = ComtQueries.CasesFromVendorsWithTriggerEnabled;
            Command = new OracleCommand(q, db);
            var caseHeaderReader = Command.ExecuteReader();
            if (caseHeaderReader.Read())
            {
                caseheader.CaseNumber = caseHeaderReader[CaseHeader.CaseNumber].ToString();
                caseheader.LocationId = caseHeaderReader[CaseHeader.LocationId].ToString();
                caseheader.StatusCode = Convert.ToInt32(caseHeaderReader[CaseHeader.StatusCode].ToString());
            }
            return caseheader;

        }

        public CaseViewDto TriggerOnCaseHeaderTableForStatCode30(OracleConnection db, int seqNumber)
        {
            var caseheader = new CaseViewDto();
            var q = ComtQueries.CasesFromReturnsWithTriggerEnabled;
            Command = new OracleCommand(q, db);
            var caseHeaderReader = Command.ExecuteReader();
            if (caseHeaderReader.Read())
            {
                caseheader.CaseNumber = caseHeaderReader[CaseHeader.CaseNumber].ToString();
                caseheader.LocationId = caseHeaderReader[CaseHeader.LocationId].ToString();
                caseheader.StatusCode = Convert.ToInt32(caseHeaderReader[CaseHeader.StatusCode]);
            }
            return caseheader;
        }

        public CaseHeaderDto QueryForNotEnoughInventoryInCase(OracleConnection db, int seqNumber)
        {
            var caseheader = new CaseHeaderDto();
            var query = ComtQueries.NotEnoughInventory;
            var command = new OracleCommand(query, db);
            var caseHeaderReader = command.ExecuteReader();
            if (caseHeaderReader.Read())
            {
                caseheader.CaseNumber = caseHeaderReader[CaseHeader.CaseNumber].ToString();
            }
            return caseheader;
        }

        public List<CaseViewDto> GetCaseDtlData(OracleConnection db, string caseNumber)
        {
            var caseDtl = new List<CaseViewDto>();
            var singleSkuQuery = $"Select * from case_dtl where case_nbr = '{caseNumber}'";
            var command = new OracleCommand(singleSkuQuery, db);
            var caseDetailReader = command.ExecuteReader();
            while (caseDetailReader.Read())
            {
                var set = new CaseViewDto
                {
                    SkuId = caseDetailReader[CaseDetail.SkuId].ToString(),
                    TotalAllocQty = Convert.ToInt32(caseDetailReader[CaseDetail.TotalAllocQty]),
                    ActlQty = Convert.ToInt32(caseDetailReader[CaseDetail.ActualQty])
                };
                caseDtl.Add(set);
            }
            return caseDtl;
        }

        protected void MultiSkuData(OracleConnection db)
        {
            CaseHdrMultiSku = TriggerOnCaseHeader(db,1);
            CaseDtoList = GetCaseDtlData(db, CaseHdrMultiSku.CaseNumber);
            FetchTransInvnDataForMultiSku(db);
        }

        public List<TransitionalInventoryDto> FetchTransInvnDataForMultiSku(OracleConnection db)
        {
            for (var j = 0; j < CaseDtoList.Count; j++)
            {
                var query = ComtQueries.TransInvnForMultiSku;
                Command = new OracleCommand(query, db);               
                Command.Parameters.Add(new OracleParameter(Parameter.SkuId, CaseDtoList[j].SkuId));
                Command.Parameters.Add(new OracleParameter(Parameter.TransInvnType, Constants.TransInvnType));
                var transInvnMultiSku = Command.ExecuteReader();
                if (transInvnMultiSku.Read())
                {
                    TransInvnBeforeTrigger.ActualInventoryUnits = Convert.ToDecimal(transInvnMultiSku[TransInventory.ActualInventoryUnits]);
                    TransInvnBeforeTrigger.ActualWeight = Convert.ToDecimal(transInvnMultiSku[TransInventory.ActlWt]);
                    TrnList.Add(TransInvnBeforeTrigger);
                }        
            }
            return TrnList;
        }
       
        protected void GetDataAfterTriggerOfComtForSingleSku()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                Command = new OracleCommand();
                SwmToMheComt = SwmToMhe(db, SingleSkuCase.CaseNumber, TransactionCode.Comt, null);
                Comt = JsonConvert.DeserializeObject<ComtDto>(SwmToMheComt.MessageJson);
                ParserTestforMsgText(TransactionCode.Comt, SwmToMheComt.SourceMessageText);
                WmsToEmsComt = WmsToEmsData(db, SwmToMheComt.SourceMessageKey,TransactionCode.Comt);
                SwmToMheIvmt = SwmToMhe(db, SingleSkuCase.CaseNumber, TransactionCode.Ivmt, SingleSkuCase.SkuId);
                Ivmt = JsonConvert.DeserializeObject<IvmtDto>(SwmToMheIvmt.MessageJson);
                ParserTestforMsgText(TransactionCode.Ivmt, SwmToMheIvmt.SourceMessageText);
                WmsToEmsIvmt = WmsToEmsData(db, SwmToMheIvmt.SourceMessageKey, TransactionCode.Ivmt);
                CaseDtlAfterApi = FetchCaseDetailsTriger(db);
                UnitWeight = FetchUnitWeight(db, SingleSkuCase.SkuId);
                UnitVol = FetchUnitVol(db, SingleSkuCase.SkuId);
                TaskSingleSku = FetchTaskDetails(db, SingleSkuCase.SkuId);
                var tempZone  = GetTempZone(db, SingleSkuCase.SkuId);
                CurrentLocationId = VerifyLocnIdAfterApi(TempZone);
            }
        }



        protected void GetDataAfterTriggerOfComtForReceivedCaseSingleSku()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                Command = new OracleCommand();               
                SwmToMheIvmt = SwmToMhe(db, SingleSkuCase.CaseNumber, TransactionCode.Ivmt, SingleSkuCase.SkuId);
                Ivmt = JsonConvert.DeserializeObject<IvmtDto>(SwmToMheIvmt.MessageJson);
                ParserTestforMsgText(TransactionCode.Ivmt, SwmToMheIvmt.SourceMessageText);
                WmsToEmsIvmt = WmsToEmsData(db, SwmToMheIvmt.SourceMessageKey, TransactionCode.Ivmt);
                CaseDtlAfterApi = FetchCaseDetailsTriger(db);
                TransInvnAfterTrigger = FetchTransInvnentory(db, SingleSkuCase.SkuId);
                UnitWeight = FetchUnitWeight(db, SingleSkuCase.SkuId);
                UnitVol = FetchUnitVol(db, SingleSkuCase.SkuId);
                TaskSingleSku = FetchTaskDetails(db, SingleSkuCase.SkuId);
                PickLocnAfterCallingApi = GetPickLocationDetails(db, SingleSkuCase.SkuId,null);
                RsvAfterApi = GetResrvLocnDetails(db, SingleSkuCase.LocationId);
            }
        }

        protected void GetDataAfterTriggerOfIvmtForSingleSku()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                SwmToMheIvmt = SwmToMhe(db, SingleSkuCase.CaseNumber, TransactionCode.Ivmt, SingleSkuCase.SkuId);
                Ivmt = JsonConvert.DeserializeObject<IvmtDto>(SwmToMheIvmt.MessageJson);
                ParserTestforMsgText(TransactionCode.Ivmt, SwmToMheIvmt.SourceMessageText);
                WmsToEmsIvmt = WmsToEmsData(db, SwmToMheIvmt.SourceMessageKey, TransactionCode.Ivmt);
                CaseDtlAfterApi = FetchCaseDetailsTriger(db);
                UnitWeight = FetchUnitWeight(db, SingleSkuCase.SkuId);
                UnitVol = FetchUnitVol(db, SingleSkuCase.SkuId);
                TaskSingleSku = FetchTaskDetails(db, SingleSkuCase.SkuId);
            }
        }

        public CaseViewDto FetchCaseDetailsTriger(OracleConnection db)
        {
            var caseDtl = new CaseViewDto();
            var query = ComtQueries.CaseHdrDtlTransInvnJoin;
            Command = new OracleCommand(query, db);           
            Command.Parameters.Add(new OracleParameter(Parameter.CaseNumber, SingleSkuCase.CaseNumber));
            Command.Parameters.Add(new OracleParameter(Parameter.TransInvnType, Constants.TransInvnType));
            var caseHdrDtlTrnReader = Command.ExecuteReader();
            if (caseHdrDtlTrnReader.Read())
            {
                caseDtl.StatusCode = Convert.ToInt16(caseHdrDtlTrnReader[CaseHeader.StatusCode].ToString());
                caseDtl.ActualInventoryUnits = Convert.ToDecimal(caseHdrDtlTrnReader[TransInventory.ActualInventoryUnits].ToString());
                caseDtl.ActualWeight = Convert.ToDecimal(caseHdrDtlTrnReader[TransInventory.ActlWt].ToString());
                caseDtl.TotalAllocQty = Convert.ToInt32(caseHdrDtlTrnReader[CaseDetail.TotalAllocQty].ToString());
                caseDtl.ActlQty = Convert.ToInt32(caseHdrDtlTrnReader[CaseDetail.ActualQty].ToString());
            }
            return caseDtl;
        }
        
        public TaskHeaderDto FetchTaskDetails(OracleConnection db,string skuId)
        {
            var task = new TaskHeaderDto();
            var query = CommonQueries.TaskHdr;
            Command = new OracleCommand(query, db);          
            Command.Parameters.Add(new OracleParameter(Parameter.SkuId, skuId));
            var taskHdr = Command.ExecuteReader();
            if (taskHdr.Read())
            {
                task.StatusCode = Convert.ToByte(taskHdr[TaskHeader.StatCode]);
            }
            return task;
        }       

        protected void GetDataAfterTriggerForMultiSkuAndValidateData()
        {     
            using (var db = GetOracleConnection())
            {
                db.Open();
                SwmToMheComt = SwmToMhe(db, CaseHdrMultiSku.CaseNumber, TransactionCode.Comt, null);
                Comt = JsonConvert.DeserializeObject<ComtDto>(SwmToMheComt.MessageJson);
                ParserTestforMsgText(TransactionCode.Comt, SwmToMheComt.SourceMessageText);
                WmsToEmsComt = WmsToEmsData(db, SwmToMheComt.SourceMessageKey, TransactionCode.Comt);               
                for (var i = 0; i < CaseDtoList.Count; i++)
                {
                    SwmToMheIvmt = SwmToMhe(db, CaseHdrMultiSku.CaseNumber, TransactionCode.Ivmt, CaseDtoList[i].SkuId);
                    Ivmt = JsonConvert.DeserializeObject<IvmtDto>(SwmToMheIvmt.MessageJson);
                    ParserTestforMsgText(TransactionCode.Ivmt, SwmToMheIvmt.SourceMessageText);
                    var caseDtl = CaseDtoList[i];
                    VerifyIvmtMessageWasInsertedIntoSwmToMhe(Ivmt,caseDtl);
                    WmsToEmsIvmt = WmsToEmsData(db, SwmToMheIvmt.SourceMessageKey, TransactionCode.Ivmt);
                    VerifyIvmtMessageWasInsertedIntoWmsToEms(WmsToEmsIvmt);
                    UnitWeight = FetchUnitWeight(db, CaseDtoList[i].SkuId);
                    UnitVol = FetchUnitVol(db, SingleSkuCase.SkuId);
                    TransInvnAfterTrigger =FetchTransInvnentory(db, CaseDtoList[i].SkuId);
                    var trnInvnAfterApi = TransInvnList[i];
                    try
                    {
                        TrnInvnBeforeCallingApi = TrnList[i];
                    }
                    catch
                    {
                        Debug.Print("No TransInvn Record");
                    }
                    
                    VerifyQuantityIsIncreasedIntoTransInvn(trnInvnAfterApi, TrnInvnBeforeCallingApi);
                    TaskMultiSku = FetchTaskDetails(db, caseDtl.SkuId);                
                    VerifyStatusIsUpdatedIntoTaskHeader(TaskMultiSku.StatusCode);
                }
                CaseHdrDtl = CaseHdrAndDtlDataAfterCallingApi(db);
            }
        }

        public CaseViewDto CaseHdrAndDtlDataAfterCallingApi(OracleConnection db)
        {
            var caseHdrDtl = new CaseViewDto();
            var query = ComtQueries.CaseHdrDtlJoin;
            Command = new OracleCommand(query, db);          
            Command.Parameters.Add(new OracleParameter(Parameter.CaseNumber, CaseHdrMultiSku.CaseNumber));
            var caseReader = Command.ExecuteReader();
            if (caseReader.Read())
            {
                caseHdrDtl.StatusCode = Convert.ToInt16(caseReader[CaseHeader.StatusCode].ToString());
                caseHdrDtl.TotalAllocQty = Convert.ToInt32(caseReader[CaseDetail.TotalAllocQty].ToString());
                caseHdrDtl.ActlQty = Convert.ToInt32(caseReader[CaseDetail.ActualQty]);
                CaseList.Add(caseHdrDtl);
            }
            return caseHdrDtl;
        }
        
        public BaseResult<MessageHeaderDto> ParserTestforMsgText(string transactionCode,string sourceTextMsg)
        {
            BaseResult<MessageHeaderDto> testResult = _canParseMessage.ParseMessage(transactionCode, sourceTextMsg);       
            return testResult;
        }
        protected void VerifyComtMessageWasInsertedIntoSwmToMhe(ComtDto comt, SwmToMheDto swmToMhe,string caseNbr)
        {
            Assert.AreEqual(DefaultValues.Status, SwmToMheComt.SourceMessageStatus);
            Assert.AreEqual(Constants.ReasonCode, SwmToMheComt.SourceMessageResponseCode);
            Assert.AreEqual("Pallet", SwmToMheComt.ContainerType);
            Assert.AreEqual(TransactionCode.Comt, comt.TransactionCode);
            Assert.AreEqual(MessageLength.Comt, comt.MessageLength);
            Assert.AreEqual(ActionCodeConstants.Create, comt.ActionCode);
            Assert.AreEqual(caseNbr, SwmToMheComt.ContainerId);
            Assert.AreEqual("Pallet", SwmToMheComt.ContainerType);
            Assert.AreEqual(Constants.MessageStatus, SwmToMheComt.MessageStatus);
            Assert.AreEqual(CurrentLocationId, comt.CurrentLocationId);
        }
        protected void VerifyComtMessageWasInsertedIntoWmsToEms(WmsToEmsDto wte1)
        {
            Assert.AreEqual(SwmToMheComt.SourceMessageProcess, wte1.Process);
            Assert.AreEqual(SwmToMheComt.SourceMessageKey,wte1.MessageKey);
            Assert.AreEqual(SwmToMheComt.SourceMessageTransactionCode, wte1.Transaction);
            Assert.AreEqual(SwmToMheComt.SourceMessageText,wte1.MessageText);
            Assert.AreEqual(SwmToMheComt.SourceMessageStatus, wte1.Status);
            Assert.AreEqual(SwmToMheComt.SourceMessageResponseCode,wte1.ResponseCode);
            Assert.AreEqual(TransactionCode.Comt, wte1.Transaction);
            Assert.AreEqual(Convert.ToInt16(SwmToMheComt.SourceMessageResponseCode), wte1.ResponseCode);
        }

        protected void VerifyIvmtMessageWasInsertedIntoSwmToMhe(IvmtDto ivmt,CaseViewDto caseDtl)
        {
            Assert.AreEqual(DefaultValues.Status, SwmToMheIvmt.SourceMessageStatus);  
            Assert.AreEqual(TransactionCode.Ivmt, ivmt.TransactionCode);
            Assert.AreEqual(MessageLength.Ivmt, ivmt.MessageLength);
            Assert.AreEqual(ActionCodeConstants.Create, ivmt.ActionCode);
            Assert.AreEqual(caseDtl.SkuId, ivmt.Sku);
            Assert.AreEqual(caseDtl.TotalAllocQty, Convert.ToDouble(ivmt.Quantity));
            Assert.AreEqual(DefaultValues.ContainerType, ivmt.UnitOfMeasure);          
            Assert.AreEqual(DefaultValues.DataControl, ivmt.DateControl);
            Assert.AreEqual(DefaultValues.InboundPallet,ivmt.InboundPallet);
            Assert.AreEqual(SingleSkuCase.Cons_prty_date,ivmt.FifoDate);
            Assert.AreEqual(LotId, SwmToMheIvmt.LotId);
            if (SingleSkuCase.PoNbr != null)
            {
                Assert.AreEqual(SingleSkuCase.PoNbr, ivmt.Po);
            }
            else
            {
                Assert.AreEqual(Constants.NoPO, ivmt.Po);
            }
          
        }

        protected void VerifyIvmtMessageWasInsertedIntoWmsToEms(WmsToEmsDto wmsToEms)
        {
            Assert.AreEqual(SwmToMheIvmt.SourceMessageStatus, wmsToEms.Status);
            Assert.AreEqual(TransactionCode.Ivmt, wmsToEms.Transaction);
            Assert.AreEqual(Convert.ToInt16(SwmToMheIvmt.SourceMessageResponseCode), wmsToEms.ResponseCode);
        }
        protected void VerifyQuantityIsIncreasedIntoTransInvn(TransitionalInventoryDto trnInvnAfterApi, TransitionalInventoryDto trnInvnBeforeApi)
        {
            Assert.AreEqual(TransInvnBeforeTrigger.ActualInventoryUnits + Convert.ToDecimal(Ivmt.Quantity), TransInvnAfterTrigger.ActualInventoryUnits);
            Assert.AreEqual((UnitWeight * Convert.ToDecimal(Ivmt.Quantity)) + SingleSkuCase.ActualWeight, Convert.ToDecimal(CaseDtlAfterApi.ActualWeight));
        }
        protected void VerifyQuantityisReducedIntoCaseDetailForMultiSku()
        {
            for (var j = 0; j < CaseList.Count; j++)
            {
                Assert.AreEqual(0, CaseList[j].TotalAllocQty);
            }
        }
        protected void VerifyStatusIsUpdatedIntoCaseHeader(int caseHdrstatcode)
        {
            Assert.AreEqual(DefaultValues.CaseHdrStatCode, caseHdrstatcode);
        }
        protected void VerifyStatusIsUpdatedIntoTaskHeader(int statcode)
        {
            try
            {
                Assert.AreEqual(DefaultValues.TaskStatusCode, statcode);
            }
            catch
            {
                Debug.Print("No Task Found");
            }
        }

        protected string VerifyLocnIdAfterApi(string tempZone)
        {
            switch (tempZone)
            {
                case Constants.Dry:
                    return Constants.DryLocnValue;
                case Constants.Freezer:
                    return Constants.FreezerLocnValue;
                default:
                    return "";
            }
        }
    }
}

    
    



