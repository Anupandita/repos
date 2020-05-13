using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.TestData
{
    [TestClass]
    public class Ivst
    {
        public long MsgKey { get; set; }
        public long InvalidKey { get; set; }
        public long Key { get; set; }
        public string CaseNumber { get; set; }
        public string SkuId { get; set; }
        public string Qty { get; set; }
        public string LocnId { get; set; }
        public string CaseDtlQty { get; set; }
    }
    public class Contents
    {
        public const string ContentType = "application/json";
    }
}

