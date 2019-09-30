using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wms.App.Contracts.Entities
{
    public class UserPreferencesModel
    {
        public List<UserPreferencesData> data { get; set; }
    }

    public class UserPreferencesData
    {
        public int ID { get; set; }
        public int USER_ID { get; set; }
        public int SETTING_ID { get; set; }
        public int ALLOWED_SETTING_VALUE_ID { get; set; }
        public string UNCONSTRAINED_VALUE { get; set; }
    }
}
