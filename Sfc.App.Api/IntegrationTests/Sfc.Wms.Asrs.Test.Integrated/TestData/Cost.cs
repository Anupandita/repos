using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sfc.Wms.Asrs.Test.Integrated.TestData
{
    [TestClass]
    public class Cost
    {
        public long MsgKey { get; set; }
        public long InvalidKey { get; set; }
        public string CaseNumber { get; set; }
        public string SkuId { get; set; }
        public string Qty { get; set; }
        public string LocnId { get; set; }
    }

    public class EmsToWms
    {
        
    }

}
