﻿    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfc.Wms.App.Api.Contracts.Entities
{
    public class AddActiveLocationGroupModel
    {
        public string locn_id { get; set; }
        public List<UpdatedDataModel> data { get; set; }
    }
}