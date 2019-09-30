using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wms.App.Contracts.Entities
{
    public class ActiveLocationModel : PaginationModel
    {
        public string user_inpt_zone { set; get; }
        public string user_inpt_grp_type { set; get; }
        public string user_inpt_locn_grp { set; get; }
        public string user_inpt_aisle { set; get; }
        public string user_inpt_slot { set; get; }
        public string user_inpt_lvl { set; get; }
        public string user_inpt_sku { set; get; }
    }
}
