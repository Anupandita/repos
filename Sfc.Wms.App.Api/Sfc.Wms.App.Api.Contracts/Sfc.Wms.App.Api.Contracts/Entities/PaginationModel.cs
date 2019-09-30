using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wms.App.Contracts.Entities
{
    public class PaginationModel
    {
        public int totalRows { get; set; }
        public int pageNo { get; set; }
        public int rowsPerPage { get; set; }
    }
}
