﻿using System.Threading.Tasks;
using Sfc.Wms.Interface.Asrs.Dtos;
using Sfc.Wms.Result;

namespace Sfc.App.Api.Nuget.Interfaces
{
    public interface IIvmtGateway
    {
        Task<BaseResult> CreateAsync(IvmtTriggerInputDto ivmtTriggerInput);
    }
}