using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wms.App.Contracts.Entities
{
    public class LpnCommentsModel
    {
        public string caseNbr { get; set; }
        public string seqNbr { get; set; }
        public string cmntType { get; set; }
        public string cmntCode { get; set; }
        public string cmnt { get; set; }
    }
}
