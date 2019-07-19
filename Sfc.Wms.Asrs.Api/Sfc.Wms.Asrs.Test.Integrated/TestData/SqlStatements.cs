using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SFC.Service.ServiceName.Tests.Integrated.TestData
{
    public class LoginTestData
    {
        public const string SingleSKUCaseNo = "00100283000800983082";
        public const string MultiSKU = "select CASE_NBR,CASE_SEQ_NBR,SKU_ID from CASE_DTL where CASE_NBR = '" + SingleSKUCaseNo + "'";
        public const string SingleSKU = "select CASE_NBR,CASE_SEQ_NBR,SKU_ID from CASE_DTL where CASE_NBR = '00100283000800983082'";
        public const string LocnId = "select LOCN_ID from CASE_HDR where CASE_NBR = '00100283000800983082'";
        public const string ContainerId = "select CASE_NBR from CASE_HDR where CASE_NBR = '00100283000800983082'";
        public const string ActlWt = "select ACTL_WT from CASE_HDR where CASE_NBR = '00100283000800983082'";
        public const string SkuId = "select SKU_ID from CASE_DTL where CASE_NBR = '00100283000800983082'";
        public const string ActlQty = "select ACTL_QTY from CASE_DTL where CASE_NBR = '00100283000800983082'";
        public const string MfgDate = "select MFG_DATE from CASE_HDR where CASE_NBR = '00100283000800983082'";
        public const string ShipByDate = "select SHIP_BY_DATE from CASE_HDR where CASE_NBR = '00100283000800983082'";
        public const string XpireDate = "select XPIRE_DATE from CASE_HDR where CASE_NBR = '00100283000800983082'";
        public const string BatchNbr = "select BATCH_NBR from CASE_DTL where CASE_NBR = '00100283000800983082'";
        public const string PoNbr = "select po_nbr from CASE_HDR  where CASE_NBR = '00100283000800983082'";
        public const string TransInvnSku = "select SKU_ID from TRANS_INVN where SKU_ID = '4160731'";
        public const string TransInvnLocn = "select LOCN_ID from TRANS_INVN where SKU_ID = '4160731'";
        public const string TransInvnType = "select TRANS_INVN_TYPE from TRANS_INVN where SKU_ID = '4160731'";
        public const string ActlInvnUnits = "select ACTL_INVN_UNITS from TRANS_INVN where SKU_ID = '4160731'";
        public const string TransInvnActlWt = "select ACTL_WT from TRANS_INVN where SKU_ID = '4160731'";
        public const string TotalAllocQty = "select TOTAL_ALLOC_QTY from CASE_DTL";
        public const string StatCode = "select STAT_CODE from CASE_HDR";
        public const string PrevLocnId = "select PREV_LOCN_ID from CASE_HDR";
        public const string ModDateTime = "select MOD_DATE_TIME from CASE_HDR";
        public const string UserId = "select USER_ID from CASE_HDR";
        public const string TaskDesc = "select TASK_DESC from TASK_HDR";
        public const string TaskHdrSku = "select SKU_ID from TASK_HDR";

    }
}
