﻿using System;
using System.Threading.Tasks;
using Sfc.Wms.App.Api.Contracts.Result;
using Sfc.Wms.App.Api.Contracts.Entities;

namespace Sfc.Wms.App.Api.Contracts.Interfaces
{
    public interface IReturnReceivingGateway
    {
        Task<BaseResult<string>> GetReturnReceiving(ReturnReceivingSearchModel returnReceivingSearchModel, string token);
        Task<BaseResult<string>> InsertReturnReceiving(ReturnReceivingInsertModel returnReceivingInsertModel, string token);
    }
}
