using System;

namespace Wms.App.Contracts.Entities
{
    public class ShipmentDetails
    {
        public string ShipmtNumber { get; set; }

        public long StatusFrom { get; set; }

        public long StatusTo { get; set; }

        public string PoNumber { get; set; }

        public string VendorName { get; set; }

        public DateTime ShippedDate { get; set; }

        public DateTime ExpectedDate { get; set; }

        public DateTime ScheduledDate { get; set; }
    }
}