using System.Threading.Tasks;
using RestSharp;
using Sfc.Core.OnPrem.Result;
using Sfc.Core.RestResponse;
using Sfc.Wms.App.Api.Contracts.Constants;
using Sfc.Wms.App.Api.Contracts.Entities;
using Sfc.Wms.App.Api.Nuget.Builders;
using Sfc.Wms.App.Api.Nuget.Interfaces;

namespace Sfc.Wms.App.Api.Nuget.Gateways
{
    public class ItemAttributeGateway : SfcBaseGateway, IItemAttributeGateway
    {
        private readonly string _endPoint;
        private readonly IResponseBuilder _responseBuilder;
        private readonly IRestClient _restClient;

        public ItemAttributeGateway(IResponseBuilder responseBuilders, IRestClient restClient) : base(restClient)
        {
            _endPoint = Routes.Prefixes.ItemAttribute;
            _responseBuilder = responseBuilders;
            _restClient = restClient;
        }

        public async Task<BaseResult<string>> GetItemAttributeDetailsAsync(
            ItemAttributeParamModel itemAttributeParamModel, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = GetItemAttributeDetailsRequest(itemAttributeParamModel, token);
                var response = await _restClient.ExecuteTaskAsync<object>(request).ConfigureAwait(false);
                return _responseBuilder.GetResponseData<string>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<string>> GetDrillDownItemAttributeAsync(string item, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = GetDrillDownItemAttributeRequest(item, token);
                var response = await _restClient.ExecuteTaskAsync<object>(request).ConfigureAwait(false);
                return _responseBuilder.GetResponseData<string>(response);
            }).ConfigureAwait(false);
        }

        private RestRequest GetItemAttributeDetailsRequest(ItemAttributeParamModel itemAttributeParamModel,
            string token)
        {
            var resource =
                $"{_endPoint}/{Routes.Paths.SearchItemAttribute}{Routes.Paths.QueryParamSymbol}pageNo={itemAttributeParamModel.pageNo}{Routes.Paths.QueryParamAnd}rowsPerPage={itemAttributeParamModel.rowsPerPage}{Routes.Paths.QueryParamAnd}totalRows={itemAttributeParamModel.totalRows}";

            resource = QueryStringBuilder.BuildQuery("inpt_itemid=", itemAttributeParamModel.inpt_itemid, resource,
                false);
            resource = QueryStringBuilder.BuildQuery("inpt_itemdesc=", itemAttributeParamModel.inpt_itemdesc, resource,
                false);
            resource = QueryStringBuilder.BuildQuery("inpt_vendoritemnbr=", itemAttributeParamModel.inpt_vendoritemnbr,
                resource, false);
            resource = QueryStringBuilder.BuildQuery("inpt_tempzone=", itemAttributeParamModel.inpt_tempzone, resource,
                false);

            return GetRequest(token, resource);
        }

        private RestRequest GetDrillDownItemAttributeRequest(string item, string token)
        {
            var resource =
                $"{_endPoint}/{Routes.Paths.DrillDownItemAttribute}{Routes.Paths.QueryParamSymbol}item={item}";
            return GetRequest(token, resource);
        }
    }
}