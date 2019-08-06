using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sfc.Wms.Asrs.Test.Integrated.TestData
{
    [TestClass]
    public class Cost
    {
        public long msgKey { get; set; }
        public string ValidCaseNumber { get; set; }
        public string ValidSkuId { get; set; }
        public string ValidQty { get; set; }
        public string ValidLocnId { get; set; }
        public Int64 ValidMsgKey;
        public Int64 InvalidMsgTextKey;
        public Int64 InvalidCaseNumberKey;
        public Int64 InvalidStsKey;
        public Int64 TransInvnNotExistKey;
        public string InvalidStsCase;
        public string InvalidStsSku;
        public string InvalidStsQty;
    }
}
