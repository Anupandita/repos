using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfc.Wms.App.Api.Contracts.Entities
{
    public class ItemAttributeParamModel : PaginationModel
    {
        public string inpt_itemid { get; set; }
        public string inpt_itemdesc { get; set; }
        public string inpt_vendoritemnbr { get; set; }
        public string inpt_tempzone { get; set; }
    }
}
