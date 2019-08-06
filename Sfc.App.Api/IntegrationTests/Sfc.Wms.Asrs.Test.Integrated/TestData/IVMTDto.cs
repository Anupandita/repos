using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfc.Wms.Asrs.Test.Integrated
{
    public class IVMTDto
    {
        public string CaseNumber { get; set; }
        public string LocationId { get; set; }
        public short StatCode { get; set; }
        public string SkuId { get; set; }
        public int ActlQty { get; set; }
        public int TotalAllocQty { get; set; }
        public int ActualInventoryUnits { get; set; }
        public int ActualWeight { get; set; }
    }
}
