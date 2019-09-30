using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wms.App.Contracts.Entities
{
    public class LpnParamModel : PaginationModel
    {
        public string lpnNumber { get; set; }

        public string asnId { get; set; }

        public string palletId { get; set; }

        public string ShipmtNumber { get; set; }

        public string skuId { get; set; }

        public string statusFrom { get; set; }

        public string statusTo { get; set; }

        public string zone { get; set; }

        public string aisle { get; set; }

        public string slot { get; set; }

        public DateTime createdDate { get; set; }

        public string lvl { get; set; }
    }
}
