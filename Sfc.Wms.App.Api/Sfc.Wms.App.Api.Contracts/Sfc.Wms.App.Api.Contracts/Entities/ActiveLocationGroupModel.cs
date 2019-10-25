using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfc.Wms.App.Api.Contracts.Entities
{
    public class ActiveLocationGroupModel
    {
        public string locn_id { get; set; }
        public List<int> updated_grp_types { get; set; }
        public string grp_type { get; set; }
        public List<UpdatedDataModel> updated_data { get; set; }
    }
}
