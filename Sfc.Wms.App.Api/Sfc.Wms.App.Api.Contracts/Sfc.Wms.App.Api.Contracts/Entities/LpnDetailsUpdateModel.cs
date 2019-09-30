using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wms.App.Contracts.Entities
{
    public class LpnDetailsUpdateModel
    {
        public string MFG_DATE { get; set; }
        public string XPIRE_DATE { get; set; }
        public string CONS_PRTY_DATE { get; set; }
        public string CONS_SEQ { get; set; }
        public string CONS_CASE_PRTY { get; set; }
        public string CASE_NBR { get; set; }
        public string EST_WT { get; set; }
        public string DC_ORD_NBR { get; set; }
        public string PO_NBR { get; set; }
        public string VOL { get; set; }
        public string VENDOR_ID { get; set; }
    }
}
