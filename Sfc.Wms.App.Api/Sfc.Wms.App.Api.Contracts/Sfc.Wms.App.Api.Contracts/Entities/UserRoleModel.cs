namespace Sfc.Wms.App.Api.Contracts.Entities
{
    public class Role
    {
        public string roleId { get; set; }
    }

    public class UserRoleModel
    {
        public Role roles { get; set; }
        public string userName { get; set; }
    }
}