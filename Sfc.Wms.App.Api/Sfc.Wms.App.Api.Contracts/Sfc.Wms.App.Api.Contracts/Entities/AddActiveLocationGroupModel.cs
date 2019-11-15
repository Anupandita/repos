using System.Collections.Generic;

namespace Sfc.Wms.App.Api.Contracts.Entities
{
    public class AddActiveLocationGroupModel
    {
        public List<UpdatedDataModel> data { get; set; }
        public string locn_id { get; set; }
    }
}