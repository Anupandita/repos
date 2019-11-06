using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.TestData
{
    [TestClass]
    public class OrstTestData
    {
        public string OrderId  { get; set; }
        public string SkuId { get; set; }
        public short Quantity { get; set; }
        public string CurrentLocationId { get; set; }
        public string LocnId { get; set; }
        public string MessageJson { get; set; }
        public string DestLocnId { get; set; }
        public string ShipWCtrlNbr { get; set; }
    }
}
