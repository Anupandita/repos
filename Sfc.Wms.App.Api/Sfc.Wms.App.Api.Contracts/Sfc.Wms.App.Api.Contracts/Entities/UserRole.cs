namespace Sfc.Wms.App.Api.Contracts.Entities
{
    public class UserRole
    {
        public int RoleId { get; set; }

        public string RoleName { get; set; }

        public string RoleDescription { get; set; }

        public string UpdatedBy { get; set; }

        public string CreatedBy { get; set; }
    }
}