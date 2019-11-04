namespace Sfc.Wms.App.Api.Contracts.Entities
{
    public class MenuModel
    {
        public string createdBy { get; set; }
        public string dispOrder { get; set; }
        public string isActive { get; set; }
        public string menuId { get; set; }
        public string menuName { get; set; }
        public string menuType { get; set; }
        public string menuUrl { get; set; }
        public string moduleName { get; set; }
        public string parentDispOrder { get; set; }
        public string parentId { get; set; }
        public bool selectItem { get; set; }
        public string updatedBy { get; set; }
    }
}