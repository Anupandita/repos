using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sfc.Wms.App.Api.Contracts.Entities
{
    public class ReturnReceivingInsertModel : PaginationModel
    {
        public string shpmtNbr { get; set;}
        public string workstationId { get; set;}
        public string procedureCall { get; set; }
    }
}