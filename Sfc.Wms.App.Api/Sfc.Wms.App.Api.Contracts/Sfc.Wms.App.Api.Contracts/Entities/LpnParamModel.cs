using System;

namespace Sfc.Wms.App.Api.Contracts.Entities
{
    public class LpnParamModel : PaginationModel
    {
        public string aisle { get; set; }
        public string asnId { get; set; }
        public DateTime createdDate { get; set; }
        public string lpnNumber { get; set; }
        public string lvl { get; set; }
        public string palletId { get; set; }

        public string ShipmtNumber { get; set; }

        public string skuId { get; set; }

        public string slot { get; set; }
        public string statusFrom { get; set; }

        public string statusTo { get; set; }

        public string zone { get; set; }
    }
}