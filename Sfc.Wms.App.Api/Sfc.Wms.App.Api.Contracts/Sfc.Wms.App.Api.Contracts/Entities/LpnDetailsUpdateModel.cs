using System;

namespace Sfc.Wms.App.Api.Contracts.Entities
{
    public class LpnDetailsUpdateModel
    {
        public string AisleValue { get; set; }
        public string CaseNumber { get; set; }
        public string ConsolidateCaseParty { get; set; }
        public DateTime? ConsolidatePartyDate { get; set; }
        public string ConsolidateSequence { get; set; }
        public string DcOrderNumber { get; set; }
        public int EstWt { get; set; }
        public DateTime? ExpireDate { get; set; }
        public DateTime? ManufacturingDate { get; set; }
        public string PoNumber { get; set; }
        public string RcvdShipmentNumber { get; set; }
        public string SpecialInstructionCode1 { get; set; }
        public string SpecialInstructionCode2 { get; set; }
        public string SpecialInstructionCode3 { get; set; }
        public string SpecialInstructionCode4 { get; set; }
        public string SpecialInstructionCode5 { get; set; }
        public bool ValidShipmentNumber { get; set; }
        public string VendorId { get; set; }
        public int Volume { get; set; }
    }
}