namespace Sfc.Wms.Api.Asrs.Test.Integrated.TestData
{
    public class CartonView
    {
        public string CartonNbr { get; set; }
        public string WaveNbr { get; set; }
        public string StatusCode { get; set; }
        public string MiscInstrCode5 { get; set; }
        public string SkuId { get; set; }
        public string CwcZoneNbr { get; set; }
        public string PickTktCtrlNbr { get; set; }
        public string PickTktSeqNbr { get; set; }
        public string Whse { get; set; }
        public string Div { get; set; }
        public string PktQuantity { get; set; }
        public string SplInstrCode1 { get; set; }
        public string SplInstrCode5 { get; set; }
        public string Co { get; set; }
        public string PickTktQty { get; set; }
        public string TotalQty { get; set; }
        public string DestLocnId { get; set; }
        public string ShipWCtrlNbr { get; set; }
        public string LocnId { get; set; }
        public string TempZone { get; set; }

    }

    public class CartonHdr
    {
        public string CartonNbr { get; set; }
        public string StatCode { get; set; }
        public string WaveNbr { get; set; }
        public  string CurrentLocationId { get; set; }
        public  string DestinationLocnId { get; set; }
        public  string ModificationDateTime { get; set; }
        public  string UserId { get; set; }
        public string Pikr { get; set; }
        public string Pakr { get; set; }
        public string PickLocationId { get; set; }
        public string MiscInstrCode5 { get; set; }
    }
    public class PickTktHdr
    {
        public string CartonNbr { get; set; }
        public  string PickTktCtrlNbr { get; set; }
        public  string Whse { get; set; }
        public  string Co { get; set; }
        public  string Div { get; set; }
        public  string PktStatCode { get; set; }
        public  string ModDateTime { get; set; }
        public  string UserId { get; set; }
    }

    public class PickLocnDtl
    {
        public  string PktSeqNbr { get; set; }
        public  string SkuId { get; set; }
        public  string PktQty { get; set; }
        public  string ToBePickedQuantity { get; set; }
        public  string ModDateTime { get; set; }
        public  string UserId { get; set; }
        public string PickLocnDtlId { get; set; }
        public string LocnId { get; set; }
        public string ToBeFilledQty { get; set; }
        public string ActlInvnQty { get; set; }
    }

    public class PkLcnDtlExt 
    {
        public  string ActiveOrmtCount { get; set; }
        public  string SkuId { get; set; }
        public  string LocnId { get; set; }
        public  string UpdatedDateTime { get; set; }
        public  string UpdatedBy { get; set; }
    }
    public class CartonDtl 
    {
        public  string CartonNumber { get; set; }
        public  string UnitsPakd { get; set; }
        public  string ModificationDateTime { get; set; }
        public  string UserId { get; set; }
        public  string ToBePackdUnits { get; set; }
    }

    public class PickTktDtl
    {
        public  string UnitsPacked { get; set; }
        public  string VerfAsPakd { get; set; }
        public  string ModificationDateTime { get; set; }
        public  string PickTktCtrlNbr { get; set; }
        public  string CartonNumber { get; set; }
        public  string PickTicketSeqNbr { get; set; }
        public  string UserId { get; set; }
    }

    public class AllocInvnDtl
    { 
        public string QtyPulled { get; set; }
        public string CntrNbr { get; set; }
        public string ModificationDateTime { get; set; }
        public string UserId { get; set; }
        public string StatCode { get; set; }
    }

   
}


