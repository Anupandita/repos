using Sfc.Core.ListManagement.Contracts.Models;

namespace Sfc.Wms.App.Api.Contracts.Entities
{
    public class ItemAttributeParamModel : PaginationModel
    {
        public string inpt_itemdesc { get; set; }
        public string inpt_itemid { get; set; }
        public string inpt_tempzone { get; set; }
        public string inpt_vendoritemnbr { get; set; }
    }
}