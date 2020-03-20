using System.Collections.Generic;
using System.Threading.Tasks;
using Sfc.Core.OnPrem.Result;
using Sfc.Wms.Configuration.MessageLogger.Contracts.Dtos;

namespace Sfc.Wms.App.Api.Nuget.Interfaces
{
    public interface IMessageLoggerGateway
    {
        Task<BaseResult> BatchInsertAsync(IEnumerable<MessageLogDto> messageLogDtos,  string token);
    }
}