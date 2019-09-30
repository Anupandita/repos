using System.Collections.Generic;

namespace Wms.App.Contracts.Dto
{
    public class UserInfoDto : UserDto
    {
        public List<object> Menus = new List<object>();

        public List<object> PrinterValues = new List<object>();

        public List<object> PrinterList = new List<object>();

        public List<object> UserPreferences = new List<object>();

        public List<object> UserPermissions = new List<object>();

        public List<int> ActionIds = new List<int>();

        public string Token { get; set; }
    }
}