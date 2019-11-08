namespace Sfc.Wms.App.Api.Contracts.Entities
{
    public class LpnDetailsUpdateModel
    {
        public string CASE_NBR { get; set; }
        public string CONS_CASE_PRTY { get; set; }
        public string CONS_PRTY_DATE { get; set; }
        public string CONS_SEQ { get; set; }
        public string DC_ORD_NBR { get; set; }
        public string EST_WT { get; set; }
        public string MFG_DATE { get; set; }
        public string PO_NBR { get; set; }
        public string VENDOR_ID { get; set; }
        public string VOL { get; set; }
        public string XPIRE_DATE { get; set; }
        public string SPL_INSTR_CODE_1 { get; set; }
        public string SPL_INSTR_CODE_2 { get; set; }
        public string SPL_INSTR_CODE_3 { get; set; }
        public string SPL_INSTR_CODE_4 { get; set; }
        public string SPL_INSTR_CODE_5 { get; set; }
        public string RCVD_SHPMT_NBR { get; set; }
        public string CUT_NBR { get; set; }
        public string ASSORT_NBR { get; set; }
        public bool ValidShipmentNumber { get; set; }
    }
}