using System.Collections.Generic;
// ReSharper disable InconsistentNaming

namespace Sfc.Wms.App.Api.Contracts.Entities
{
    public class ActiveLocationGroupModel
    {
        public string grp_type { get; set; }
        public string locn_id { get; set; }
        public List<UpdatedDataModel> updated_data { get; set; }
        public List<int> updated_grp_types { get; set; }
    }
}