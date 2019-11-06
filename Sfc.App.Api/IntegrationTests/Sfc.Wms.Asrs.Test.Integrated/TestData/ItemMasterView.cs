using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.TestData
{
    [TestClass]
    public class ItemMasterView
    {
        public string SkuId { get; set; }
        public string div { get; set; }
        public string Skudesc { get; set; }
        public string StdCaseQty { get; set; }
        public string tempzone { get; set; }
        public string unitwieght { get; set; }

        public string unitvolume { get; set; }
        public string prodlifeinday { get; set; }
        public string colordescription { get; set; }

        public string NestVolume { get; set; }
        public string skubrcd { get; set; }
    }

    //public class Content
    //{
    //    public const string ContentType = "application/json";
    //}
}
