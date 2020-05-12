using Sfc.Core.BaseApiController;
using Sfc.Core.OnPrem.Result;
using Sfc.Wms.App.Api.Contracts.Constants;
using Sfc.Wms.Configuration.MessageLogger.Contracts.UoW.Dtos;
using Sfc.Wms.Configuration.MessageLogger.Contracts.UoW.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace Sfc.Wms.App.Api.Controllers
{
    [Authorize]
    public class MessageLogController : SfcBaseController
    {
        private readonly IMessageLogService _messageLogService;

        public MessageLogController(IMessageLogService messageLogService)
        {
            _messageLogService = messageLogService;
        }

        [HttpPost]
        [Route(Routes.Prefixes.MessageLogger)]
        [ResponseType(typeof(BaseResult))]
        public async Task<IHttpActionResult> BatchInsertAsync(IEnumerable<MessageLogDto> messageLogDtos)
        {
            var response = await _messageLogService.InsertRangeAsync(messageLogDtos)
                .ConfigureAwait(false);
            return ResponseHandler(response);
        }
    }
}