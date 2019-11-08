﻿using Sfc.Core.ListManagement.Contracts.Models;
using System;

namespace Sfc.Wms.App.Api.Contracts.Entities
{
    public class LpnParamModel : PaginationOptions
    {
        public string Aisle { get; set; }
        public string AsnId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LpnNumber { get; set; }
        public string Level { get; set; }
        public string PalletId { get; set; }
        public string ShipmtNumber { get; set; }
        public string SkuId { get; set; }
        public string Slot { get; set; }
        public string StatusFrom { get; set; }
        public string StatusTo { get; set; }
        public string Zone { get; set; }
    }
}