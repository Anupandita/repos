using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData;
using Sfc.Wms.Interfaces.ParserAndTranslator.Contracts.Dto;
using Sfc.Wms.Interfaces.ParserAndTranslator.Contracts.Constants;
using Sfc.Wms.Interfaces.ParserAndTranslator.Contracts.Validation;
using Sfc.Wms.Interfaces.Builder.MessageBuilder;
using System.Collections.Generic;
using Sfc.Core.OnPrem.Result;
using Sfc.Wms.Foundation.PixTransaction.Contracts.Dtos;
using Sfc.Wms.Interfaces.Asrs.Shamrock.Contracts.Dtos;
using Sfc.Wms.Interfaces.Asrs.Dematic.Contracts.Dtos;
using Sfc.Wms.Foundation.TransitionalInventory.Contracts.Dtos;
using Sfc.Wms.Foundation.InboundLpn.Contracts.Enums;
using Sfc.Wms.Foundation.InboundLpn.Contracts.Dtos;
using System.Diagnostics;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures
{
    [TestClass]
    public class DataBaseFixtureForIvst:CommonFunction
    {
        protected decimal UnitWeight;
        protected Ivst IvstData = new Ivst();
        protected Ivst InvShortageInbound = new Ivst();
        protected Ivst InvShortageOutbound = new Ivst();
        protected Ivst MixedOutbound = new Ivst();
        protected Ivst NoExceptionInBound = new Ivst();
        protected Ivst DamageInbound = new Ivst();
        protected Ivst DamageOutbound = new Ivst();
        protected Ivst WrongSku = new Ivst();
        protected Ivst WrongSkuOutbound = new Ivst();
        protected Ivst CycleCountAdjustmentPlus = new Ivst();
        protected Ivst CycleCountAdjustmentMinus = new Ivst();
        protected Ivst NoException = new Ivst();
        protected IvstDto Ivst = new IvstDto();
        protected new string Query = "";
        protected new SwmFromMheDto SwmFromMhe = new SwmFromMheDto();
        protected string PixTrnAfterApi;
        protected EmsToWmsDto EmsToWmsParameters;
        protected EmsToWmsDto EmsToWmsParametersInventoryShortage;
        protected EmsToWmsDto EmsToWmsParametersMixedInventory;
        protected EmsToWmsDto EmsToWmsParametersOutBound;
        protected EmsToWmsDto EmsToWmsParametersDamage;
        protected EmsToWmsDto EmsToWmsParametersWrongSku;
        protected EmsToWmsDto EmsToWmsParametersNoException;
        protected EmsToWmsDto EmsToWmsParametersCycleCount;
        protected IvstDto IvstParameters;
        protected Foundation.Location.Contracts.Dtos.PickLocationDetailsDto PickLocnDtlAfterApi = new Foundation.Location.Contracts.Dtos.PickLocationDetailsDto();
        protected Foundation.Location.Contracts.Dtos.PickLocationDetailsDto PickLcnDtlBeforeApi = new Foundation.Location.Contracts.Dtos.PickLocationDetailsDto();
        protected decimal Unitweight1;
        protected OracleCommand OracleCommand;
        protected PixTransactionDto Pixtran = new PixTransactionDto();
        protected TransitionalInventoryDto TrnsInvBeforeApi = new TransitionalInventoryDto();
        protected TransitionalInventoryDto TrnsInvAfterApi = new TransitionalInventoryDto();
        protected TransitionalInventoryDto TransInvnNegativePick = new TransitionalInventoryDto();
        protected TransitionalInventoryDto TransInvnNegativePickBeforeApi = new TransitionalInventoryDto();
        protected List<Scenarios> MsgkeyList = new List<Scenarios>();
        protected CaseDetailDto caseDtlAfterApi = new CaseDetailDto();
        protected CaseDetailDto caseDtlQty = new CaseDetailDto();
        protected CaseHeaderDto caseHdrAfterApi = new CaseHeaderDto();
        protected CaseHeaderDto caseHdrStatCode = new CaseHeaderDto();
        protected List<Pb2CorbaHdrDtl> pb = new List<Pb2CorbaHdrDtl>();
      
        
        protected void InsertIvstMessageUnexpectedOverageWhenCaseHeaderStatusCodeIsLessthan90AndCaseDtlActlQtyIsGreaterThanZero()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                IvstData = GetCaseDetailsForInsertingIvstMessage(db, IvstQueries.CaseHdrStatCodeLesserThan90AndCaseDtlActlQtyGreaterThanZero);
                Unitweight1 = FetchUnitWeight(db, IvstData.SkuId);
                TrnsInvBeforeApi = FetchTransInvnentory(db, IvstData.SkuId);
                PickLcnDtlBeforeApi = GetPickLocationDetails(db, IvstData.SkuId, null);
                InsertingUnexpectedOverage(db, IvstActionCode.AdjustmentPlus, IvstException.UnexpectedOverage, Constants.InboundPalletY,Constants.IvstQuantity);
                IvstData.Key = InsertEmsToWms(db, EmsToWmsParameters);
            }
        }

        protected void
            IvstMessageUnexpectedOverageWhenCaseHeaderStatusCodeIsLessThanOrEqualTo90AndCaseDtlActlQtyIsEqualToZero()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                IvstData = GetCaseDetailsForInsertingIvstMessage(db, IvstQueries.CaseHdrStatCodeLesserThan90AndCaseDtlActlQtyEqualToZero);
                UpdatetheRecordForStatus96Tobellow90(db, IvstData.CaseNumber);
                Unitweight1 = FetchUnitWeight(db, IvstData.SkuId);
                TrnsInvBeforeApi = FetchTransInvnentory(db, IvstData.SkuId);
                PickLcnDtlBeforeApi = GetPickLocationDetails(db, IvstData.SkuId, null);
                InsertingUnexpectedOverage(db, IvstActionCode.AdjustmentPlus, IvstException.UnexpectedOverage, Constants.InboundPalletY,Constants.IvstQuantity);
                IvstData.Key = InsertEmsToWms(db, EmsToWmsParameters);
            }
        }


        protected void InsertIvstMessagetUnexpectedFunctionForstatus96AndNegativeTiAlreadyExists()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                IvstData = GetCaseDetailsForInsertingIvstMessage(db, IvstQueries.IvstQueryForTiNegativePickAlreadyExistsForStatus96);
                Unitweight1 = FetchUnitWeight(db, IvstData.SkuId);
                TrnsInvBeforeApi = FetchTransInvnentory(db, IvstData.SkuId);
                TransInvnNegativePickBeforeApi = FetchTransInvnentoryForNegative(db, IvstData.SkuId);
                PickLcnDtlBeforeApi = GetPickLocationDetails(db, IvstData.SkuId, null);
                InsertingUnexpectedOverage(db, IvstActionCode.AdjustmentPlus, IvstException.UnexpectedOverage, Constants.InboundPalletY,Constants.IvstQuantity);
                IvstData.Key = InsertEmsToWms(db, EmsToWmsParameters);
            }
        }

        protected void InsertIvstMessagetUnexpectedFunctionForstatus96AndNegativeTiNotExists()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                IvstData = GetCaseDetailsForInsertingIvstMessage(db, IvstQueries.IvstQueryForTiNegativePickNotExistsForStatus96);
                DeleteTheRecordForTransInvnTypeIs10(db, IvstData.SkuId);
                Unitweight1 = FetchUnitWeight(db, IvstData.SkuId);
                TrnsInvBeforeApi = FetchTransInvnentory(db, IvstData.SkuId);
                PickLcnDtlBeforeApi = GetPickLocationDetails(db, IvstData.SkuId, null);
                InsertingUnexpectedOverage(db, IvstActionCode.AdjustmentPlus, IvstException.UnexpectedOverage, Constants.InboundPalletY,Constants.IvstQuantity);
                IvstData.Key = InsertEmsToWms(db, EmsToWmsParameters);
            }
        }

        protected void InsertIvstMessageForInventoryShortageInboundPalletIsY()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                IvstData = GetCaseDetailsForInsertingIvstMessage(db, IvstQueries.IvstQueryForPickLocnQtyGreaterThanIvstQty);
                Unitweight1 = FetchUnitWeight(db, IvstData.SkuId);
                TrnsInvBeforeApi = FetchTransInvnentory(db, IvstData.SkuId);
                PickLcnDtlBeforeApi = GetPickLocationDetails(db, IvstData.SkuId, null);
                InsertingInventoryShortage(db, IvstActionCode.AdjustmentMinus, IvstException.InventoryShortage,Constants.InboundPalletY);
                InvShortageInbound.Key = InsertEmsToWms(db, EmsToWmsParametersInventoryShortage);
            }
        }

        protected void InsertIvstMessageForInventoryShortageInboundPalletIsN()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                IvstData = GetCaseDetailsForInsertingIvstMessage(db, IvstQueries.IvstQueryForPickLocnQtyGreaterThanIvstQty);
                Unitweight1 = FetchUnitWeight(db, IvstData.SkuId);
                TrnsInvBeforeApi = FetchTransInvnentory(db, IvstData.SkuId);
                PickLcnDtlBeforeApi = GetPickLocationDetails(db, IvstData.SkuId, null);
                var ivstResult = CreateIvstMessage(IvstData.CaseNumber, IvstData.SkuId, Constants.IvstQuantity, IvstActionCode.AdjustmentMinus, IvstException.InventoryShortage, Constants.InboundPalletN);
                EmsToWmsParametersInventoryShortage = new EmsToWmsDto
                {
                    Process = DefaultPossibleValue.MessageProcessor,
                    Status = RecordStatus.Ready.ToString(),
                    Transaction = TransactionCode.Ivst,
                    MessageText = ivstResult
                };
                InvShortageOutbound.Key = InsertEmsToWms(db, EmsToWmsParametersInventoryShortage);
            }
        }

        protected void InsertIvstMessageForInventoryShortageOutBoundAndPickLocnDtlQtyIsGreaterThanZeroButLesserThanIvstQty()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                IvstData = GetCaseDetailsForInsertingIvstMessage(db, IvstQueries.IvstQueryForTiNegativePickAlreadyExists);
                Unitweight1 = FetchUnitWeight(db, IvstData.SkuId);
                TrnsInvBeforeApi = FetchTransInvnentory(db, IvstData.SkuId);
                TransInvnNegativePickBeforeApi = FetchTransInvnentoryForNegative(db, IvstData.SkuId);
                PickLcnDtlBeforeApi = GetPickLocationDetails(db, IvstData.SkuId, null);
                var ivstResult = CreateIvstMessage(IvstData.CaseNumber, IvstData.SkuId, (PickLcnDtlBeforeApi.ActualInventoryQuantity + 1).ToString(), IvstActionCode.AdjustmentMinus, IvstException.InventoryShortage, Constants.InboundPalletN);
                EmsToWmsParametersInventoryShortage = new EmsToWmsDto
                {
                    Process = DefaultPossibleValue.MessageProcessor,
                    Status = RecordStatus.Ready.ToString(),
                    Transaction = TransactionCode.Ivst,
                    MessageText = ivstResult
                };
                InvShortageOutbound.Key = InsertEmsToWms(db, EmsToWmsParametersInventoryShortage);
            }
        }

        protected void InsertIvstMessageForInventoryShortageOutBoundAndPickLocnDtlQtyIsGreaterThanZeroButLesserThanIvstQtyForNegativePickDoesNotExists()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                IvstData = GetCaseDetailsForInsertingIvstMessage(db, IvstQueries.IvstQueryForTiNegativePickAlreadyExists);
                DeleteTheRecordForTransInvnTypeIs10(db, IvstData.SkuId);
                Unitweight1 = FetchUnitWeight(db, IvstData.SkuId);
                TrnsInvBeforeApi = FetchTransInvnentory(db, IvstData.SkuId);
                PickLcnDtlBeforeApi = GetPickLocationDetails(db, IvstData.SkuId, null);
                var ivstResult = CreateIvstMessage(IvstData.CaseNumber, IvstData.SkuId, (PickLcnDtlBeforeApi.ActualInventoryQuantity + 1).ToString(), IvstActionCode.AdjustmentMinus, IvstException.InventoryShortage, Constants.InboundPalletN);
                EmsToWmsParametersInventoryShortage = new EmsToWmsDto
                {
                    Process = DefaultPossibleValue.MessageProcessor,
                    Status = RecordStatus.Ready.ToString(),
                    Transaction = TransactionCode.Ivst,
                    MessageText = ivstResult
                };
                InvShortageOutbound.Key = InsertEmsToWms(db, EmsToWmsParametersInventoryShortage);
            }
        }       

        protected void InsertIvstMessageForInventoryShortageOutBoundAndPickLocnQtyLesserThanOrEqualToZero()
       {
           using (var db = GetOracleConnection())
           {
               db.Open();
               IvstData = GetCaseDetailsForInsertingIvstMessage(db, IvstQueries.IvstQueryForPickLocnQtyLesserThanOrEqualToZero);
               Unitweight1 = FetchUnitWeight(db, IvstData.SkuId);
               TrnsInvBeforeApi = FetchTransInvnentory(db, IvstData.SkuId);
                TransInvnNegativePickBeforeApi = FetchTransInvnentoryForNegative(db,IvstData.SkuId);
               PickLcnDtlBeforeApi = GetPickLocationDetails(db, IvstData.SkuId, null);
               var ivstResult = CreateIvstMessage(IvstData.CaseNumber, IvstData.SkuId, (PickLcnDtlBeforeApi.ActualInventoryQuantity + 1).ToString(), IvstActionCode.AdjustmentMinus, IvstException.InventoryShortage, Constants.InboundPalletN);
               EmsToWmsParametersInventoryShortage = new EmsToWmsDto
               {
                   Process = DefaultPossibleValue.MessageProcessor,
                   Status = RecordStatus.Ready.ToString(),
                   Transaction = TransactionCode.Ivst,
                   MessageText = ivstResult
               };
               InvShortageOutbound.Key = InsertEmsToWms(db, EmsToWmsParametersInventoryShortage);
           }
       }

        protected void InsertIvstMessageForMixedOrIncorrectInventory(String actionCode)
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                IvstData = GetCaseDetailsForInsertingIvstMessage(db, IvstQueries.IvstQueryForPickLocnQtyGreaterThanIvstQty);
                Unitweight1 = FetchUnitWeight(db, IvstData.SkuId);
                TrnsInvBeforeApi = FetchTransInvnentory(db, IvstData.SkuId);
                PickLcnDtlBeforeApi = GetPickLocationDetails(db, IvstData.SkuId, null);
                var ivstResult = CreateIvstMessage(IvstData.CaseNumber, IvstData.SkuId, "1",actionCode, "0009", Constants.InboundPalletN);
                EmsToWmsParametersMixedInventory = new EmsToWmsDto
                {
                    Process = "IVSTProcessor",
                    Status = RecordStatus.Ready.ToString(),
                    Transaction = TransactionCode.Ivst,
                    MessageText = ivstResult
                };
                MixedOutbound.Key = InsertEmsToWms(db, EmsToWmsParametersMixedInventory);
            }
        }

        protected void InsertIvstMessageForNoExceptionScenario()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                IvstData = GetCaseDetailsForInsertingIvstMessage(db, IvstQueries.IvstQueryForPickLocnQtyGreaterThanIvstQty);
                Unitweight1 = FetchUnitWeight(db, IvstData.SkuId);
                TrnsInvBeforeApi = FetchTransInvnentory(db, IvstData.SkuId);
                PickLcnDtlBeforeApi = GetPickLocationDetails(db, IvstData.SkuId, null);
                var ivstResult = CreateIvstMessage(IvstData.CaseNumber, IvstData.SkuId, IvstData.Qty, IvstActionCode.AdjustmentPlus, "0000", Constants.InboundPalletY);
                EmsToWmsParametersNoException = new EmsToWmsDto
                {
                    Process = "IVSTProcessor",
                    Status = RecordStatus.Ready.ToString(),
                    Transaction = TransactionCode.Ivst,
                    MessageText = ivstResult
                };
                NoExceptionInBound.Key = InsertEmsToWms(db, EmsToWmsParametersNoException);
            }
        }

        protected void InsertIvstMessageDamageForInboundPalletIsN()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                Command = new OracleCommand(Query, db);             
                IvstData = GetCaseDetailsForInsertingIvstMessage(db, IvstQueries.IvstQueryForPickLocnQtyGreaterThanIvstQty);
                Unitweight1 = FetchUnitWeight(db, IvstData.SkuId);
                TrnsInvBeforeApi = FetchTransInvnentory(db, IvstData.SkuId);
                PickLcnDtlBeforeApi = GetPickLocationDetails(db, IvstData.SkuId, null);
                var ivstResult = CreateIvstMessage(IvstData.CaseNumber, IvstData.SkuId, "1", IvstActionCode.AdjustmentMinus, IvstException.Damage, Constants.InboundPalletN);
                EmsToWmsParametersDamage = new EmsToWmsDto
                {
                    Process = DefaultPossibleValue.MessageProcessor,
                    Status = RecordStatus.Ready.ToString(),
                    Transaction = TransactionCode.Ivst,
                    MessageText = ivstResult
                };
                DamageOutbound.Key = InsertEmsToWms(db, EmsToWmsParametersDamage);               
            }
        }

        protected void InsertIvstMessageDamageForInboundPalletIsY()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                Command = new OracleCommand(Query, db);
                UpdatetheQuantity(db);
                IvstData = GetCaseDetailsForInsertingIvstMessage(db, IvstQueries.IvstQueryForPickLocnQtyGreaterThanIvstQty);
                Unitweight1 = FetchUnitWeight(db, IvstData.SkuId);
                TrnsInvBeforeApi = FetchTransInvnentory(db, IvstData.SkuId);
                PickLcnDtlBeforeApi = GetPickLocationDetails(db, IvstData.SkuId, null);
                InsertingDamageForInboundPalletY(db, IvstActionCode.AdjustmentMinus, IvstException.Damage,Constants.InboundPalletY,Constants.IvstQuantity);
                DamageInbound.Key = InsertEmsToWms(db, EmsToWmsParametersDamage);
            }
        }

        protected void InsertIvstMessageDamageForInboundPalletIsYScenario3()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                Command = new OracleCommand(Query, db);          
                IvstData = GetCaseDetailsForInsertingIvstMessage(db, IvstQueries.IvstQueryForPickLocnQtyGreaterThanIvstQty);
                Unitweight1 = FetchUnitWeight(db, IvstData.SkuId);
                TrnsInvBeforeApi = FetchTransInvnentory(db, IvstData.SkuId);
                PickLcnDtlBeforeApi = GetPickLocationDetails(db, IvstData.SkuId, null);
                InsertingDamageForInboundPalletY(db, IvstActionCode.AdjustmentMinus, IvstException.Damage, Constants.InboundPalletN, (PickLcnDtlBeforeApi.ActualInventoryQuantity + Convert.ToDecimal(Constants.IvstQuantity)).ToString());
                DamageOutbound.Key = InsertEmsToWms(db, EmsToWmsParametersDamage);
            }
        }

        protected void InsertIvstMessageWrongSkuFunction()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                UpdatetheQuantity(db);
                IvstData = GetCaseDetailsForInsertingIvstMessage(db, IvstQueries.IvstQueryForPickLocnQtyGreaterThanIvstQty);
                Unitweight1 = FetchUnitWeight(db, IvstData.SkuId);
                TrnsInvBeforeApi = FetchTransInvnentory(db, IvstData.SkuId);
                PickLcnDtlBeforeApi = GetPickLocationDetails(db, IvstData.SkuId, null);
                InsertingWrongSku(db, IvstActionCode.AdjustmentMinus, IvstException.WrongSku,Constants.InboundPalletY);
                WrongSku.Key = InsertEmsToWms(db, EmsToWmsParametersWrongSku);            
            }
        }

        protected void InsertIvstMessageForWrongSkuInboundPalletIsN()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                var ivstResult = CreateIvstMessage(IvstData.CaseNumber, IvstData.SkuId, "1", IvstActionCode.AdjustmentMinus, IvstException.WrongSku, Constants.InboundPalletN);
                EmsToWmsParametersWrongSku = new EmsToWmsDto
                {
                    Process = DefaultPossibleValue.MessageProcessor,
                    Status = RecordStatus.Ready.ToString(),
                    Transaction = TransactionCode.Ivst,
                    MessageText = ivstResult
                };
                WrongSkuOutbound.Key = InsertEmsToWms(db, EmsToWmsParametersWrongSku);
            }
        }

        protected void InsertIvstMessageForCycleCountWithAdjustmentPlus()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                IvstData = GetCaseDetailsForInsertingIvstMessage(db, IvstQueries.IvstQueryForPickLocnQtyGreaterThanIvstQty);
                Unitweight1 = FetchUnitWeight(db, IvstData.SkuId);
                TrnsInvBeforeApi = FetchTransInvnentory(db, IvstData.SkuId);
                PickLcnDtlBeforeApi = GetPickLocationDetails(db, IvstData.SkuId, null);
                var ivstResult = CreateIvstMessage(IvstData.CaseNumber, IvstData.SkuId, Constants.IvstQuantity, IvstActionCode.AdjustmentPlus, IvstException.CycleCount, Constants.InboundPalletN);
                EmsToWmsParametersCycleCount = new EmsToWmsDto
                {
                    Process = DefaultPossibleValue.MessageProcessor,
                    Status = RecordStatus.Ready.ToString(),
                    Transaction = TransactionCode.Ivst,
                    MessageText = ivstResult
                };
                CycleCountAdjustmentPlus.Key = InsertEmsToWms(db, EmsToWmsParametersCycleCount);
            }
        }

        protected void IvstMessageForCcAdjustmentMinusWherePickLocnDtlActlQtyIsGreaterThanIvstQty()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                IvstData = GetCaseDetailsForInsertingIvstMessage(db, IvstQueries.IvstQueryForPickLocnQtyGreaterThanIvstQty);
                Unitweight1 = FetchUnitWeight(db, IvstData.SkuId);
                TrnsInvBeforeApi = FetchTransInvnentory(db, IvstData.SkuId);
                PickLcnDtlBeforeApi = GetPickLocationDetails(db, IvstData.SkuId, null);
                var ivstResult = CreateIvstMessage(IvstData.CaseNumber, IvstData.SkuId, Constants.IvstQuantity, IvstActionCode.AdjustmentMinus, IvstException.CycleCount, Constants.InboundPalletN);
                EmsToWmsParametersCycleCount = new EmsToWmsDto
                {
                    Process = DefaultPossibleValue.MessageProcessor,
                    Status = RecordStatus.Ready.ToString(),
                    Transaction = TransactionCode.Ivst,
                    MessageText = ivstResult
                };
                CycleCountAdjustmentMinus.Key = InsertEmsToWms(db, EmsToWmsParametersCycleCount);
            }
        }

        protected void IvstMessageForCcAdjustmentMinusWherePickLocnDtlActlQtyIsGreaterThanZeroButLessThanIvstQty()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();

                IvstData = GetCaseDetailsForInsertingIvstMessage(db, IvstQueries.IvstQueryForTiNegativePickAlreadyExists);
                Unitweight1 = FetchUnitWeight(db, IvstData.SkuId);
                TrnsInvBeforeApi = FetchTransInvnentory(db, IvstData.SkuId);
                TransInvnNegativePickBeforeApi = FetchTransInvnentoryForNegative(db,IvstData.SkuId);
                PickLcnDtlBeforeApi = GetPickLocationDetails(db, IvstData.SkuId, null);
                var ivstResult = CreateIvstMessage(IvstData.CaseNumber, IvstData.SkuId, (PickLcnDtlBeforeApi.ActualInventoryQuantity + Convert.ToDecimal(Constants.IvstQuantity)).ToString(), IvstActionCode.AdjustmentMinus, IvstException.CycleCount, Constants.InboundPalletN);
                EmsToWmsParametersCycleCount = new EmsToWmsDto
                {
                    Process = DefaultPossibleValue.MessageProcessor,
                    Status = RecordStatus.Ready.ToString(),
                    Transaction = TransactionCode.Ivst,
                    MessageText = ivstResult
                };
                CycleCountAdjustmentMinus.Key = InsertEmsToWms(db, EmsToWmsParametersCycleCount);
            }
        }

        protected void
            IvstMessageForCcAdjustMentMinusWherePickLocnDtlActlQtyIsGreaterThanZeroButLessThanIvstQtyAndNegativeTiDoesNotExists()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                IvstData = GetCaseDetailsForInsertingIvstMessage(db, IvstQueries.IvstQueryForTiNegativePickNotExists);
                DeleteTheRecordForTransInvnTypeIs10(db, IvstData.SkuId);
                Unitweight1 = FetchUnitWeight(db, IvstData.SkuId);
                TrnsInvBeforeApi = FetchTransInvnentory(db, IvstData.SkuId);
                PickLcnDtlBeforeApi = GetPickLocationDetails(db, IvstData.SkuId, null);
                var ivstResult = CreateIvstMessage(IvstData.CaseNumber, IvstData.SkuId, (PickLcnDtlBeforeApi.ActualInventoryQuantity + Convert.ToDecimal(Constants.IvstQuantity)).ToString(), IvstActionCode.AdjustmentMinus, IvstException.CycleCount, Constants.InboundPalletN);
                EmsToWmsParametersCycleCount = new EmsToWmsDto
                {
                    Process = DefaultPossibleValue.MessageProcessor,
                    Status = RecordStatus.Ready.ToString(),
                    Transaction = TransactionCode.Ivst,
                    MessageText = ivstResult
                };
                CycleCountAdjustmentMinus.Key = InsertEmsToWms(db, EmsToWmsParametersCycleCount);
            }
        }

        protected void IvstMessageForCcAdjustmentMinusWherePickLocnQtyIsLessThanOrEqualToZeroAndLessThanIvstQuantity()
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                IvstData = GetCaseDetailsForInsertingIvstMessage(db, IvstQueries.IvstQueryForPickLocnQtyLesserThanOrEqualToZero);
                Unitweight1 = FetchUnitWeight(db, IvstData.SkuId);
                TrnsInvBeforeApi = FetchTransInvnentory(db, IvstData.SkuId);
                TransInvnNegativePickBeforeApi = FetchTransInvnentoryForNegative(db,IvstData.SkuId);
                PickLcnDtlBeforeApi = GetPickLocationDetails(db, IvstData.SkuId, null);
                var ivstResult = CreateIvstMessage(IvstData.CaseNumber, IvstData.SkuId, (PickLcnDtlBeforeApi.ActualInventoryQuantity + Convert.ToDecimal(Constants.IvstQuantity)).ToString(), IvstActionCode.AdjustmentMinus, IvstException.CycleCount, Constants.InboundPalletN);
                EmsToWmsParametersCycleCount = new EmsToWmsDto
                {
                    Process = DefaultPossibleValue.MessageProcessor,
                    Status = RecordStatus.Ready.ToString(),
                    Transaction = TransactionCode.Ivst,
                    MessageText = ivstResult
                };
                CycleCountAdjustmentMinus.Key = InsertEmsToWms(db, EmsToWmsParametersCycleCount);
            }
        }
        
        
        public Ivst GetCaseDetailsForInsertingIvstMessage(OracleConnection db,string query)
        {
            var ivstDataDto = new Ivst();
            Command = new OracleCommand(query, db);
            Command.Parameters.Add(new OracleParameter(Parameter.StatCode, Constants.StatusCodeConsumed));
            Command.Parameters.Add(new OracleParameter(Parameter.SysCodeType, Constants.SysCodeType));
            Command.Parameters.Add(new OracleParameter(Parameter.SysCodeId, Constants.SysCodeIdForActiveLocation));
            Command.Parameters.Add(new OracleParameter(Parameter.Ready, DefaultValues.Status));
            Command.Parameters.Add(new OracleParameter(Parameter.IvstQty, Constants.IvstQuantity));         
            var validData = Command.ExecuteReader();
            if (validData.Read())
            {
                ivstDataDto.CaseNumber = validData[FieldName.ContainerId].ToString();
                ivstDataDto.SkuId = validData[FieldName.SkuId].ToString();
                ivstDataDto.Qty = validData[FieldName.Qty].ToString();
                ivstDataDto.LocnId = validData[FieldName.LocnId].ToString();
                ivstDataDto.CaseDtlQty = validData["Actl_qty"].ToString();
            }
            return ivstDataDto;
        }

        protected void InsertingUnexpectedOverage(OracleConnection db, string actionCode, string exception, string inboundPallet,string quantity)
        {
            Command = new OracleCommand(Query, db);
            var ivstmsg = CreateIvstMessage(IvstData.CaseNumber, IvstData.SkuId, quantity, actionCode, exception,inboundPallet);
            EmsToWmsParameters = new EmsToWmsDto
            {
                Process = DefaultPossibleValue.MessageProcessor,
                Status = RecordStatus.Ready.ToString(),
                Transaction = TransactionCode.Ivst,
                MessageText = ivstmsg
            };            
        }

        protected void  InsertingInventoryShortage(OracleConnection db, string actionCode, string exception, string inboundPallet)
        {
            Command = new OracleCommand(Query, db);
            var ivstmsg = CreateIvstMessage(IvstData.CaseNumber, IvstData.SkuId, "1", actionCode, exception, inboundPallet);
            EmsToWmsParametersInventoryShortage = new EmsToWmsDto
            {
                Process = DefaultPossibleValue.MessageProcessor,
                Status = RecordStatus.Ready.ToString(),
                Transaction = TransactionCode.Ivst,
                MessageText = ivstmsg
            };       
        }
        protected void InsertingDamageForInboundPalletY(OracleConnection db, string actionCode, string exception, string inboundPallet, string quantity)
        {
           
            var ivstmsg = CreateIvstMessage(IvstData.CaseNumber, IvstData.SkuId, quantity, actionCode, exception, inboundPallet);
            EmsToWmsParametersDamage = new EmsToWmsDto
            {
                Process = DefaultPossibleValue.MessageProcessor,
                Status = RecordStatus.Ready.ToString(),
                Transaction = TransactionCode.Ivst,
                MessageText = ivstmsg
            };           
        }

        protected void InsertingWrongSku(OracleConnection db, string actionCode, string exception, string inboundPallet)
        {
            Command = new OracleCommand(Query, db);
            var ivstmsg = CreateIvstMessage(IvstData.CaseNumber, IvstData.SkuId,"1" , actionCode, exception, inboundPallet);
            EmsToWmsParametersWrongSku = new EmsToWmsDto
            {
                Process = DefaultPossibleValue.MessageProcessor,
                Status = RecordStatus.Ready.ToString(),
                Transaction = TransactionCode.Ivst,
                MessageText = ivstmsg
            };           
        }

        public string CreateIvstMessage(string containerNbr, string skuId, string quantity, string actionCode, string adjustmentReasonCode,string inboundPallet)
        {
            IvstParameters = new IvstDto
            {
                ActionCode = actionCode,
                AdjustmentReasonCode = adjustmentReasonCode,
                ContainerId = containerNbr,
                Quantity = quantity,
                Sku = skuId,
                Owner = Constants.Owner,
                UserName = Constants.UserName,
                UnitOfMeasure = DefaultValues.ContainerType,
                LotId = Constants.LotId,
                Po = Constants.Po,
                InboundPallet = inboundPallet,
                TransactionCode = TransactionCode.Ivst,
                MessageLength = MessageLength.Ivst,
                ReasonCode = ReasonCode.Success
            };
            var buildMessage = new MessageHeaderBuilder();
            var testResult = buildMessage.BuildMessage<IvstDto, IvstValidationRule>(IvstParameters, TransactionCode.Ivst);
            Assert.AreEqual(testResult.ResultType, ResultTypes.Ok);
            Assert.IsNotNull(testResult.Payload);
            return testResult.Payload;
        }

        protected void GetDataAfterTrigger(long key)
        {
            using (var db = GetOracleConnection())
            {
                db.Open();
                SwmFromMhe = SwmFromMhe(db, key, TransactionCode.Ivst);
                Ivst = JsonConvert.DeserializeObject<IvstDto>(SwmFromMhe.MessageJson);
                TrnsInvAfterApi = FetchTransInvnentory(db, Ivst.Sku);
                PickLocnDtlAfterApi = GetPickLocationDetails(db, Ivst.Sku,null);                  
                UnitWeight = FetchUnitWeight(db, Ivst.Sku);
                var pixTrnAfterApi = PixTransactionTable(Ivst.AdjustmentReasonCode);          
                Pixtran = GetPixtransaction(db, pixTrnAfterApi, SwmFromMhe.ContainerId);
                caseDtlQty = FetchCaseDetailQty(db, SwmFromMhe.ContainerId);
                caseHdrStatCode = CaseHeaderDetails(db, SwmFromMhe.ContainerId);
                TransInvnNegativePick = FetchTransInvnentoryForNegative(db, Ivst.Sku);        
                try
                {
                    Int64 maxId = FetchMaxIdFromPb2CorbaHdr(db);
                    pb = FetchPbHdrDetails(db, maxId);
                    caseDtlAfterApi = FetchCaseDetailQty(db, pb[0].ParmValue);
                    caseHdrAfterApi = CaseHeaderDetails(db, pb[0].ParmValue);
                }
                catch
                {
                    Debug.Print("No Data Found");
                }
            }
        }

        public PixTransactionDto GetPixtransaction(OracleConnection db, string rsnCode, string caseNbr)
        {
            var pixtran = new PixTransactionDto();
            Query = $"select * from Pix_tran where rsn_code= :reasonCode  and case_nbr = :caseNumber order by create_date_time desc";
            Command = new OracleCommand(Query, db);
            Command.Parameters.Add(new OracleParameter(Parameter.ReasonCode, rsnCode));
            Command.Parameters.Add(new OracleParameter(Parameter.CaseNumber, caseNbr));
            var rsn = Command.ExecuteReader();
            if (rsn.Read())
            {
                pixtran.ReasonCode = rsn["RSN_CODE"].ToString();
                pixtran.InventoryAdjustmentQuantity = Convert.ToDecimal(rsn["INVN_ADJMT_QTY"]);
                pixtran.InventoryAdjustmentType = rsn["INVN_ADJMT_TYPE"].ToString();
            }
            return pixtran;
        }

        protected void DeleteTheRecordForTransInvnTypeIs10(OracleConnection db,string skuId)
        {
           Transaction = db.BeginTransaction();           
           Query = $"Delete trans_invn where sku_id = '{skuId}' and trans_invn_type = '10'";
           Command = new OracleCommand(Query, db);
           Command.ExecuteNonQuery();
           Transaction.Commit();
        }

        protected void UpdatetheRecordForStatus96Tobellow90(OracleConnection db,string caseNbr)
        {
            Transaction = db.BeginTransaction();           
            Query = $"update case_hdr set stat_code = 80  where case_nbr = '{caseNbr}'";
            Command = new OracleCommand(Query, db);
            Command.ExecuteNonQuery();
            Transaction.Commit();
        }
        protected void UpdatetheQuantity(OracleConnection db)
        {
            Transaction = db.BeginTransaction();
            Query = $"update pick_locn_dtl set actl_invn_qty = 999 where sku_id = '2346701'";
            Command = new OracleCommand(Query, db);
            Command.ExecuteNonQuery();
            Transaction.Commit();
        }


        public string PixTransactionTable(string adjustmentReasonCode)
        {
            switch (adjustmentReasonCode)
            {
                case IvstException.CycleCount:
                    return Constants.PixRsnCodeForCycleCount;
                case IvstException.UnexpectedOverage:
                    return Constants.PixRsnCodeForUnExpectedOverage;
                case IvstException.InventoryShortage:
                    return Constants.PixRsnCodeForInventoryShortage;
                case IvstException.Damage:
                    return Constants.PixRsnCodeForDamage;
                case IvstException.WrongSku:
                    return Constants.PixRsnCodeForWrongSku;
                default:
                    return Constants.PixRsnCodeForCycleCount;
            }
        }
    }
}
