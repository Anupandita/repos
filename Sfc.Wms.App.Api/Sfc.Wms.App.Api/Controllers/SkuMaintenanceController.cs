using Sfc.Core.BaseApiController;
using Sfc.Core.OnPrem.Result;
using Sfc.Wms.App.Api.Contracts.Constants;
using Sfc.Wms.Interfaces.Asrs.Contracts.Interfaces;
using SimpleInjector.Lifestyles;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace Sfc.Wms.App.Api.Controllers
{
    [RoutePrefix(Routes.Prefixes.DematicMessageSkmt)]
    public class SkuMaintenanceController : SfcBaseController
    {
        private readonly IWmsToEmsMessageProcessorService _wmsToEmsMessageProcessorService;

        public SkuMaintenanceController(IWmsToEmsMessageProcessorService wmsToEmsMessageProcessorService)
        {
            _wmsToEmsMessageProcessorService = wmsToEmsMessageProcessorService;
        }

        [HttpPost]
        [Route]
        [ResponseType(typeof(BaseResult))]
        [AllowAnonymous]
        public async Task<IHttpActionResult> CreateAsync(string skuId, string actionCode)
        {
            var result = await _wmsToEmsMessageProcessorService.GetSkmtMessageAsync(skuId, actionCode)
                .ConfigureAwait(false);

            return Content(Enum.TryParse(result.ResultType.ToString(), out HttpStatusCode statusCode)
                ? statusCode
                : HttpStatusCode.ExpectationFailed, result);
        }

        [HttpPost]
        [Route(Routes.Paths.SkmtWrapper)]
        [ResponseType(typeof(BaseResult))]
        [AllowAnonymous]
        public IHttpActionResult CreateAsync(string actionCode)
        {
            var start = new ParameterizedThreadStart(SKmtWrapper);
            var skmtThread = new Thread(start) {IsBackground = true};
            skmtThread.Start(actionCode);

            var result=new BaseResult()
            {   ResultType = ResultTypes.Ok,
                ValidationMessages = new List<ValidationMessage>()
                {
                    new ValidationMessage(nameof(SKmtWrapper),"Process Started")
                }

            };

            return Content(HttpStatusCode.OK, result);
        }

        private static void SKmtWrapper(object actionCode)
        {
            var container = DependencyConfig.Register();
            using (var scope = AsyncScopedLifestyle.BeginScope(container))
            {
                var wmsToEmsParallelProcessService = scope.GetInstance<IWmsToEmsParallelProcessService>();
                var result = wmsToEmsParallelProcessService
                    .GetSkmtParallelAsync(container, (string)actionCode).Result;
            }
        }
    }
}