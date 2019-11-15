namespace Sfc.Wms.App.Api.Contracts.Entities
{
    public class ChangePassword
    {
        public string currentPwd { get; set; }
        public string newPwd { get; set; }
    }

    public class ChangePasswordModel
    {
        public ChangePassword model { get; set; }
        public string userName { get; set; }
    }
}