using Sfc.Core.BaseApiController;
using Sfc.Core.OnPrem.Result;
using Sfc.Wms.App.Api.Contracts.Constants;
using Sfc.Wms.Configuration.MessageMaster.Contracts.Constants;
using Sfc.Wms.Configuration.MessageMaster.Contracts.Dtos;
using Sfc.Wms.Configuration.MessageMaster.Contracts.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace Sfc.Wms.App.Api.Controllers
{
    [Authorize, RoutePrefix(Routes.Prefixes.MessageMaster)]
    public class MessageMasterController : SfcBaseController
    {
        private readonly IMessageMasterService _messageMasterService;

        public MessageMasterController(IMessageMasterService messageMasterService)
        {
            _messageMasterService = messageMasterService;
        }

        [HttpGet]
        [Route(Routes.Paths.UiSpecificMessages)]
        [ResponseType(typeof(BaseResult<IEnumerable<MessageDetailDto>>))]
        public async Task<IHttpActionResult> GetUiSpecificMessageDetails()
        {
            var response = await _messageMasterService.GetMessageDetailsAsync(el => el.PrintIndicator == DefaultedValues.UiSpecificPrintIndicator).
                ConfigureAwait(false);
            return ResponseHandler(response);
        }
    }
}