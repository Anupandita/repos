using Sfc.Wms.Asrs.App.Interfaces;
using Sfc.Wms.Asrs.Shamrock.Contracts.EnumsAndConstants.Constants;
using Sfc.Wms.Result;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Sfc.Wms.Asrs.Shamrock.Contracts.Dtos;
using Sfc.Wms.Data.Entities;

namespace Sfc.Wms.Asrs.Api.Controllers.Shamrock
{
    [RoutePrefix(Routes.AllocationInventoryDetailPrefix)]
    public class AllocationInventoryDtlController : SfcBaseApiController
    {
        private readonly IShamrockService<AllocationInventoryDetailDto, AllocationInventoryDetail> _shamrockService;

        public AllocationInventoryDtlController(IShamrockService<AllocationInventoryDetailDto, AllocationInventoryDetail> shamrockService)
        {
            _shamrockService = shamrockService;
        }

        [HttpGet]
        [ResponseType(typeof(BaseResult))]
        [Route(Routes.GetAllocationInventoryDetail)]
        public async Task<IHttpActionResult> GetInventorDetailAsync(string containerNumber, string skuId, decimal quantity)
        {
            var response = await _shamrockService
                .GetAsync(e => e.ContainerNumber == containerNumber
                               && e.SkuId == skuId && e.InventoryNeedType == 1
                               && e.QuantityAllocated == quantity)
                .ConfigureAwait(false);
            return ResponseHandler(response);
        }
    }
}