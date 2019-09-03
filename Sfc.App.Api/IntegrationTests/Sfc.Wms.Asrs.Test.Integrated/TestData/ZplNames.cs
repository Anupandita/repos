using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.TestData
{
    public class ZplDto
    {      
        public string CartonTotalQtyDesc { get; set; }
        public string PalletId { get; set; }
        public string Flags1 { get; set; }
        public string Flags2 { get; set; }
        public string CartonTotalQty { get; set; }
        public string Level { get; set; }
        public string Bay { get; set; }
        public string Aisle { get; set; }
        public string Area { get; set; }
        public string ReverseCode1 { get; set; }
        public string ReverseCode2 { get; set;}
        public string ShipTo { get; set; }
        public string ShipToName { get; set; }
        public string Line { get; set; }
        public string PktSeqNbr { get; set; }
        public string ActlDockActlDoor { get; set; }
        public string TempZone { get; set; }
        public string ShpmtNbr { get; set; }
        public string CartonNbrBc { get; set; }
        public string CaseCount { get; set; }
        public string ShipDateTime { get; set; }
        public string CustDept { get; set; }
        public string Style { get; set; }
        public string WaveNbr { get; set; }
        public string NestVolDfltUom { get; set; }
        public string VendorItemNbr { get; set; }
        public string SkuDesc { get; set; }
        public string XofY { get; set; }
        public string Quant{ get; set; }
    }
    
    public class ZplFieldNames
    {
        public const string CartonTotalQtyDesc = "carton_total_qty_desc";
        public const string PalletId = "pallet_id";
        public const string Flags1 = "flags_1";
        public const string Flags2 = "flags_2";
        public const string CartonTotalQty = "carton_total_qty";
        public const string Level = "Level";
        public const string Bay = "Bay";
        public const string Aisle = "Aisle";
        public const string Area = "Area";
        public const string ReverseCode1 = "REVERSECODE1";
        public const string ReverseCode2 = "REVERSECODE2";
        public const string ShipTo = "shipto";
        public const string ShipToName = "shipto_name";
        public const string Line = "Line";
        public const string PktSeqNbr = "pkt_seq_nbr";
        public const string ActlDockActlDoor = "actl_dock_actl_door";
        public const string TempZone = "temp_zone";
        public const string ShpmtNbr = "shpmt_nbr";
        public const string CartonNbrBc = "carton_nbr_bc";
        public const string CaseCount = "case_count";
        public const string ShipDateTime = "ship_date_time";
        public const string CustDept = "cust_dept";
        public const string Style = "style";
        public const string WaveNbr = "wave_nbr";
        public const string NestVolDfltUom = "nest_vol_dflt_uom";
        public const string VendorItemNbr = "vendor_item_nbr";
        public const string SkuDesc = "sku_desc";
        public const string XofY = "XofY";
        public const string Quant = "carton_nbr_bc";
    }

}
