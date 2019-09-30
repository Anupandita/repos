using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wms.App.Contracts.Entities
{
    public class LockUnlockModel
    {
        public string lock_code { get; set; }
        public string grid_locn_id { get; set; }
        public string all_items { get; set; }
        public string sku_id { get; set; }
    }
}
