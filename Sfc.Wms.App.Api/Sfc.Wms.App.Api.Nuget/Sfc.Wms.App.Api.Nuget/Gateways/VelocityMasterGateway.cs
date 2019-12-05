using RestSharp;
using Sfc.Wms.App.Api.Contracts.Constants;
using Sfc.Wms.App.Api.Contracts.Entities;
using Sfc.Wms.App.Api.Contracts.Interfaces;
using System.Threading.Tasks;
using Sfc.Core.OnPrem.Result;
using Sfc.Core.RestResponse;
using Sfc.Wms.App.Api.Nuget.Builders;
using Sfc.Wms.App.Api.Nuget.Interfaces;

namespace Sfc.Wms.App.Api.Nuget.Gateways
{
    public class VelocityMasterGateway : SfcBaseGateway,IVelocityMasterGateway
    {
        private readonly string _endPoint;
        private readonly IResponseBuilder _responseBuilder;
        private readonly IRestClient _restClient;

        public VelocityMasterGateway(IResponseBuilder responseBuilders, IRestClient restClient)
        {
            _endPoint = Routes.Prefixes.VelocityMaster;
            _responseBuilder = responseBuilders;
            _restClient = restClient;
        }

        public async Task<BaseResult<string>> GetVelocityMasterDetailsAsync(VelocityMasterModel velocityMasterModel, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = GetVelocityMasterDetailsRequest(velocityMasterModel, token);
                var response = await _restClient.ExecuteTaskAsync<object>(request).ConfigureAwait(false);
                return _responseBuilder.GetResponseData<string>(response);
            }).ConfigureAwait(false);
        }

        private RestRequest GetVelocityMasterDetailsRequest(VelocityMasterModel velocityMasterModel, string token)
        {
            var resource = $"{_endPoint}/{Routes.Paths.SearchVelocityMaster}{Routes.Paths.QueryParamSymbol}pageNo={velocityMasterModel.pageNo}{Routes.Paths.QueryParamAnd}rowsPerPage={velocityMasterModel.rowsPerPage}{Routes.Paths.QueryParamAnd}totalRows={velocityMasterModel.totalRows}";

            resource = QueryStringBuilder.BuildQuery("item=", velocityMasterModel.item, resource, false);
            return GetRequest(token, resource);
        }
    }
}
