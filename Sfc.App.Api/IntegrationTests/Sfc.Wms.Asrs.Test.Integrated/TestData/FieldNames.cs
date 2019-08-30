using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.TestData
{
  public class CaseHeader 
    {
        public const string CaseNumber = "CASE_NBR";
        public const string LocationId = "LOCN_ID";
        public const string StatusCode = "STAT_CODE";
    }
    public class CaseDetail 
    {
        public const string SkuId = "SKU_ID";
        public const string TotalAllocQty = "TOTAL_ALLOC_QTY";

    }

  public class TransInventory
    {
        public const string ActualInventoryUnits = "ACTL_INVN_UNITS";
        public const string ActlWt = "ACTL_WT";
    }
  public class PickLocationDtl
    {
        public const string LocnId = "LOCN_ID";
        public const string ToBeFilledQty = "TO_BE_FILLD_QTY";
        public const string ActlInvnQty = "ACTL_INVN_QTY";
    }
   
  public class WmsToEms
    {
        public const string Status = "STS";
        public const string ReasonCode = "RSNRCODE";
        public const string MsgKey = "MSGKEY";
        public const string Trx = "TRX";
        public const string MsgTxt = "MSGTEXT";
    }
  public class SwmToMhe
    {
        public const string SourceMsgKey = "SOURCE_MSG_KEY";
        public const string SourceMsgRsnCode = "SOURCE_MSG_RSN_CODE";
        public const string SourceMsgStatus  = "SOURCE_MSG_STATUS";
        public const string ContainerId = "CONTAINER_ID";
        public const string ContainerType = "CONTAINER_TYPE";
        public const string MsgJson = "MSG_JSON";
        public const string LocnId = "LOCN_ID";
        public const string SkuId = "SKU_ID";
        public const string Qty = "QTY";
        public const string SourceMsgText = "SOURCE_MSG_TEXT";
        public const string SourceMsgProcess = "SOURCE_MSG_PROCESS";
        public const string SourceMsgTransCode = "SOURCE_MSG_TRANS_CODE";
    }

    public class SwmFromMhe
    {
        public const string SourceMsgKey = "SOURCE_MSG_KEY";
        public const string SourceMsgRsnCode = "SOURCE_MSG_RSN_CODE";
        public const string SourceMsgStatus = "SOURCE_MSG_STATUS";
        public const string ContainerId = "CONTAINER_ID";
        public const string ContainerType = "CONTAINER_TYPE";
        public const string MsgJson = "MSG_JSON";
        public const string LocnId = "LOCN_ID";
        public const string SkuId = "SKU_ID";
        public const string Qty = "QTY";
        public const string SourceMsgText = "SOURCE_MSG_TEXT";
        public const string SourceMsgProcess = "SOURCE_MSG_PROCESS";
        public const string SourceMsgTransCode = "SOURCE_MSG_TRANS_CODE";
    }

    public class TaskHeader
    {
        public const string StatCode = "STAT_CODE";
    }

    public class PickHeader
    {
        public const string PickTktCtrlNbr = "PKT_CTRL_NBR";
        public const string Whse ="WHSE";
        public const string Co ="CO";
        public const string Div ="DIV";

    }

    public class PickDetail
    {
        public const string PktSeqNbr ="PKT_SEQ_NBR";
        public const string SkuId ="SKU_ID";
        public const string PktQty ="PKT_QTY";
    }

    public class CartonHeader
    {
        public const string cartonNbr = "CARTON_NBR";
        public const string statCode = "STAT_CODE";
        public const string waveNbr = "WAVE_NBR";
    }
    
    public class ItemMaster
    {
        public const string SplInstrCode1 ="SPL_INSTR_CODE_1";
        public const string SplInstrCode5 ="SPL_INSTR_CODE_5";
    }
}
