﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfc.Wms.App.Api.Contracts.Entities
{
    public class UserRoleModel
    {

        public string userName { get; set; }
        public Role roles { get; set; }
    }

    public class Role
    {

        public string roleId { get; set; }
    }
}