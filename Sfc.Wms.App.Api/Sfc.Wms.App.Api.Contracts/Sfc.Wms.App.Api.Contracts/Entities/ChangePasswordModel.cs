using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wms.App.Contracts.Entities
{
    public class ChangePasswordModel
    {
        public ChangePassword model { get; set; }
        public string userName { get; set; }
    }

    public class ChangePassword
    {
        public string currentPwd { get; set; }
        public string newPwd { get; set; }
    }
}
