using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wms.App.Contracts.Entities
{
    public class ActiveItemModel : PaginationModel
    {
        public string grid_locn_id { get; set; }
        public string grid_seq_nbr { get; set; }
    }
}
