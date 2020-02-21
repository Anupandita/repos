using Sfc.Core.OnPrem.Result;
using Sfc.Core.RestResponse;
using Sfc.Wms.App.Api.Contracts.Constants;
using Sfc.Wms.App.Api.Nuget.Interfaces;
using Sfc.Wms.Configuration.ItemMasters.Contracts.Dtos;
using System;
using System.Threading.Tasks;

namespace Sfc.Wms.App.Api.Nuget.Gateways
{
    public class ItemAttributeGateway : SfcBaseGateway, IItemAttributeGateway
    {
        private readonly string _endPoint;
        private readonly IResponseBuilder _responseBuilder;
        private readonly IRestCsharpClient _restCsharpClient;
        private const string Authorization = "Authorization";


        public ItemAttributeGateway(IResponseBuilder responseBuilders, IRestCsharpClient restClient) : base(restClient)
        {
            _endPoint = Routes.Prefixes.ItemAttribute;
            _responseBuilder = responseBuilders;
            restClient.BaseUrl = new Uri(ServiceUrl);
            _restCsharpClient = restClient;
        }

        public async Task<BaseResult<ItemAttributeSearchResultDto>> SearchAsync(
            ItemAttributeSearchInputDto itemAttributeSearchInputDto, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var resource = $"{_endPoint}/{Routes.Paths.Search}";
                var request = GetRequest(resource, itemAttributeSearchInputDto, token, Authorization);

                var response = await _restCsharpClient.ExecuteTaskAsync<BaseResult<ItemAttributeSearchResultDto>>(request)
                    .ConfigureAwait(false);

                return _responseBuilder.GetBaseResult<ItemAttributeSearchResultDto>(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult<ItemAttributeDetailsDto>> AttributeDrillDownAsync(string item, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var resource = $"{_endPoint}/{Routes.Paths.DrillDownItemAttribute}/{item}";
                var request = GetRequest(token, resource, Authorization);

                var response = await _restCsharpClient.ExecuteTaskAsync<BaseResult<ItemAttributeDetailsDto>>(request)
                    .ConfigureAwait(false);

                return _responseBuilder.GetBaseResult<ItemAttributeDetailsDto>(response);
            }).ConfigureAwait(false);
        }
    }
}