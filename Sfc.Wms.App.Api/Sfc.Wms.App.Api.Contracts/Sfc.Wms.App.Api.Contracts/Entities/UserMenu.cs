namespace Sfc.Wms.App.Api.Contracts.Entities
{
    public class UserMenu
    {
        public long MenuId { get; set; }

        public string MenuName { get; set; }

        public string MenuUrl { get; set; }

        public string ModuleName { get; set; }

        public int DispOrder { get; set; }

        public long ParentId { get; set; }

        public int IsActive { get; set; }

        public string MenuType { get; set; }

        public string UpdatedBy { get; set; }

        public string CreatedBy { get; set; }
    }
}