using System.Collections.Generic;

namespace Sfc.Wms.App.Api.Contracts.Entities
{
    public class UserPreferencesData
    {
        public int ALLOWED_SETTING_VALUE_ID { get; set; }
        public int ID { get; set; }
        public int SETTING_ID { get; set; }
        public string UNCONSTRAINED_VALUE { get; set; }
        public int USER_ID { get; set; }
    }

    public class UserPreferencesModel
    {
        public List<UserPreferencesData> data { get; set; }
    }
}