namespace Sfc.Wms.App.Api.Contracts.Dto
{
    public class UserDto
    {
        public long UserId { get; set; }

        public string UserName { get; set; }

        public int IsActive { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}