using Sfc.Core.ListManagement.Contracts.Models;

namespace Sfc.Wms.App.Api.Contracts.Entities
{
    public class ReturnReceivingSearchModel : PaginationModel
    {
        public string asn { get; set; }
        public string fromDate { get; set; }
        public string item { get; set; }
        public string toDate { get; set; }
        public string userRoute { get; set; }
    }
}