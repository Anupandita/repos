namespace Sfc.Wms.App.Api.Contracts.Entities
{
    public class UserMenu
    {
        public string CreatedBy { get; set; }
        public int DispOrder { get; set; }
        public int IsActive { get; set; }
        public long MenuId { get; set; }

        public string MenuName { get; set; }

        public string MenuType { get; set; }
        public string MenuUrl { get; set; }

        public string ModuleName { get; set; }
        public long ParentId { get; set; }
        public string UpdatedBy { get; set; }
    }
}