using Sfc.Core.OnPrem.Result;
using Sfc.Wms.Configuration.MessageLogger.Contracts.UoW.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfc.Wms.App.Api.Nuget.Interfaces
{
    public interface IMessageLoggerGateway
    {
        Task<BaseResult> BatchInsertAsync(IEnumerable<MessageLogDto> messageLogDtos, string token);
    }
}