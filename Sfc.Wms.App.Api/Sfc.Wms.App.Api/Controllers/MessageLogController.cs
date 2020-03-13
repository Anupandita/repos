using Sfc.Core.BaseApiController;
using Sfc.Core.OnPrem.Result;
using Sfc.Wms.App.Api.Contracts.Constants;
using Sfc.Wms.Configuration.MessageLogger.Contracts.Dtos;
using Sfc.Wms.Configuration.MessageLogger.Contracts.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace Sfc.Wms.App.Api.Controllers
{
    [Authorize, RoutePrefix(Routes.Prefixes.MessageLogger)]
    public class MessageLogController : SfcBaseController
    {
        private readonly IMessageLogService _messageLogService;

        public MessageLogController(IMessageLogService messageLogService)
        {
            _messageLogService = messageLogService;
        }

        [HttpPost]
        [Route(Routes.Paths.IsDbTransactionAllowed)]
        [ResponseType(typeof(BaseResult))]
        public async Task<IHttpActionResult> BatchInsertAsync(IEnumerable<MessageLogDto> messageLogDtos, bool isDbTransactionAllowed = false)
        {
            var response = await _messageLogService.BatchInsertAsync(messageLogDtos, isDbTransactionAllowed);
            return ResponseHandler(response);
        }
    }
}