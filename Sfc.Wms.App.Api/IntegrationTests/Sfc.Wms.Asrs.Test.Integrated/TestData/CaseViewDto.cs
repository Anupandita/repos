using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.TestData
{
    public class CaseViewDto
    {
        public string CaseNumber { get; set; }
        public string LocationId { get; set; }
        public int StatusCode { get; set; }
        public string SkuId { get; set; }
        public int ActlQty { get; set; }
        public int TotalAllocQty { get; set; }
        public decimal ActualInventoryUnits { get; set; }
        public decimal ActualWeight { get; set; }      
        public string TempZone { get; set; }
    }
}
