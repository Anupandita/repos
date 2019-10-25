using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfc.Wms.App.Api.Contracts.Entities
{
    public class CommonModel
    {
        public string recType { get; set; }
        public string codeType { get; set; }
        public bool isNumber { get; set; }
        public string orderByColumn { get; set; }
        public string isWhseSysCode { get; set; }

    }
}
