using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.TestData
{
    public class Cost
    {
        public long MsgKey { get; set; }
        public long InvalidKey { get; set; }
        public string CaseNumber { get; set; }
        public string SkuId { get; set; }
        public string Qty { get; set; }
        public string LocnId { get; set; }
    }
    public class Content
    {
        public const string ContentType = "application/json";
    }
}
