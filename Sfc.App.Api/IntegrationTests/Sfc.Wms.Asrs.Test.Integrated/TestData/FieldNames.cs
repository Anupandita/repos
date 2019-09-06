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

    public class PickTicketHeader
    {
        public const string CartonNumber = "CARTON_NBR";
        public const string PickTktCtrlNbr = "PKT_CTRL_NBR";
        public const string Whse ="WHSE";
        public const string Co ="CO";
        public const string Div ="DIV";
        public const string PktStatCode = "PKT_STAT_CODE";
        public const string ModDateTime = "MOD_DATE_TIME";
        public const string UserId = "USER_ID";
    }

    public class PickLocationDetail
    {
        public const string PktSeqNbr ="PKT_SEQ_NBR";
        public const string SkuId ="SKU_ID";
        public const string PktQty ="PKT_QTY";
        public const string ToBePickedQuantity="TO_BE_PIKD_QTY";
        public const string ModDateTime = "MOD_DATE_TIME";
        public const string UserId = "USER_ID";
        public const string PickLocnDtlId = "PICK_LOCN_DTL_ID";
        public const string LocnId = "LOCN_ID";
        public const string ToBeFilledQty = "TO_BE_FILLD_QTY";
        public const string ActlInvnQty = "ACTL_INVN_QTY";
    }

    public class CartonHeader
    {
        public const string CartonNbr = "CARTON_NBR";
        public const string StatCode = "STAT_CODE";
        public const string WaveNbr = "WAVE_NBR";
        public const string CurrentLocationId = "CURR_LOCN_ID";
        public const string DestinationLocnId = "DEST_LOCN_ID";
        public const string ModificationDateTime = "MOD_DATE_TIME";
        public const string UserId = "USER_ID";
        public const string Pikr = "PIKR";
        public const string Pakr = "PAKR";
        public const string PickLocationId = "PICK_LOCN_ID";
        public const string MiscInstrCode5 = "MISC_INSTR_CODE_5";
    }
    
    public class ItemMaster
    {
        public const string SplInstrCode1 ="SPL_INSTR_CODE_1";
        public const string SplInstrCode5 ="SPL_INSTR_CODE_5";
    }

    public class PickLocnDtlExt
    {
        public const string ActiveOrmtCount  = "ACTIVE_ORMT_COUNT";
        public const string SkuId = "SKU_ID";
        public const string LocnId = "LOCN_ID";
        public const string UpdatedDateTime = "UPDATED_DATE_TIME";
        public const string UpdatedBy = "UPDATED_BY";
    }

    public class CartonDetail
    {
        public const string CartonNumber = "CARTON_NBR";
        public const string UnitsPakd = "UNITS_PAKD";
        public const string ModificationDateTime = "MOD_DATE_TIME";
        public const string UserId = "USER_ID";
        public const string ToBePackdUnits = "TO_BE_PAKD_UNITS";
    }

    public class PickTicketDetail
    {
        public const string UnitsPacked = "UNITS_PAKD";
        public const string VerfAsPakd = "VERF_AS_PAKD";
        public const string ModificationDateTime = "MOD_DATE_TIME";
        public const string PickTktCtrlNbr = "PKT_CTRL_NBR";
        public const string CartonNumber = "CARTON_NBR";
        public const string PickTicketSeqNbr = "PKT_SEQ_NBR";
        public const string UserId = "USER_ID";
    }

    public class AllocInvnDetail 
    {
        public const string QtyPulled = "QTY_PULLD";
        public const string CntrNbr = "CNTR_NBR";
        public const string ModificationDateTime = "MOD_DATE_TIME";
        public const string UserId = "USER_ID";
        public const string StatCode = "STAT_CODE";
    }

}
