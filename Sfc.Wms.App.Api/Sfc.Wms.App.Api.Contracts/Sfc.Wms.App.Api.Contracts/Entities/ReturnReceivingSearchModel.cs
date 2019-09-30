using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sfc.Wms.App.Api.Contracts.Entities
{
    public class ReturnReceivingSearchModel : PaginationModel
    {
        public string item { get; set;}
        public string asn { get; set;}
        public string userRoute { get; set; }
        public string fromDate { get; set; }
        public string toDate { get; set; }
    }
}