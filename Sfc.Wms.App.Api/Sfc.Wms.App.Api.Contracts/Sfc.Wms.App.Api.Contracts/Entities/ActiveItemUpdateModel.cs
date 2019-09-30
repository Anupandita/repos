using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wms.App.Contracts.Entities
{
    public class ActiveItemUpdateModel
    {
        public string min_invn_qty { get; set; }
        public string max_invn_qty { get; set; }
        public string min_invn_cases { get; set; }
        public string max_invn_cases { get; set; }
        public string locn_id { get; set; }
        public string seq_nbr { get; set; }
    }
}
