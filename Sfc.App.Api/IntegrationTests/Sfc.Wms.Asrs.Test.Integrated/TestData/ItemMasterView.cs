using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.TestData
{
    [TestClass]
    public class ItemMasterView
    {
        public string SkuId { get; set; }
        public string Div { get; set; }
        public string Skudesc { get; set; }
        public string StdCaseQty { get; set; }
        public string Tempzone { get; set; }
        public string Unitwieght { get; set; }

        public string Unitvolume { get; set; }
        public string Prodlifeinday { get; set; }
        public string Colordescription { get; set; }

        public string NestVolume { get; set; }
        public string Skubrcd { get; set; }
    }

    //public class Content
    //{
    //    public const string ContentType = "application/json";
    //}
}
