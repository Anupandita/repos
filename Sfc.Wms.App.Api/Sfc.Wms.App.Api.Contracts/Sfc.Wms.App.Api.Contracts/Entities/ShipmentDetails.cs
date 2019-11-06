using System;

namespace Sfc.Wms.App.Api.Contracts.Entities
{
    public class ShipmentDetails
    {
        public DateTime ExpectedDate { get; set; }
        public string PoNumber { get; set; }
        public DateTime ScheduledDate { get; set; }
        public string ShipmtNumber { get; set; }

        public DateTime ShippedDate { get; set; }
        public long StatusFrom { get; set; }

        public long StatusTo { get; set; }
        public string VendorName { get; set; }
    }
}