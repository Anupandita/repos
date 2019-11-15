// ReSharper disable InconsistentNaming

using Sfc.Core.ListManagement.Contracts.Models;

namespace Sfc.Wms.App.Api.Contracts.Entities
{
    public class ActiveItemModel : PaginationModel
    {
        public string grid_locn_id { get; set; }
        public string grid_seq_nbr { get; set; }
    }
}