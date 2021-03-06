using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.TestData
{
    [TestClass]
    public class IvmtParams
    {
        public string ActionCode { get; set; }
        public string CurrentLocationId { get; set; }
        public string ContainerId { get; set; }
        public string ContainerType { get; set; }
        public string ParentContainerId { get; set; }
        public string AttributeBitmap { get; set; }
        public string QuantityToInduct { get; set; }
    }
}
