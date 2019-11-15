namespace Sfc.Wms.App.Api.Contracts.Entities
{
    public class UserRole
    {
        public string CreatedBy { get; set; }
        public string RoleDescription { get; set; }
        public int RoleId { get; set; }

        public string RoleName { get; set; }
        public string UpdatedBy { get; set; }
    }
}