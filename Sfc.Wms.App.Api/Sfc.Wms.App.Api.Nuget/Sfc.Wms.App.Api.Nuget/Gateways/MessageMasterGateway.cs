using Sfc.Core.OnPrem.Result;
using Sfc.Core.RestResponse;
using Sfc.Wms.App.Api.Contracts.Constants;
using Sfc.Wms.App.Api.Nuget.Interfaces;
using Sfc.Wms.Configuration.MessageMaster.Contracts.UoW.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp;

namespace Sfc.Wms.App.Api.Nuget.Gateways
{
    public class MessageMasterGateway : SfcBaseGateway, IMessageMasterGateway
    {
        private readonly string _endPoint;
        private readonly IResponseBuilder _responseBuilder;
        private readonly IRestClient _restCsharpClient;

        public MessageMasterGateway(IResponseBuilder responseBuilders, IRestClient restClient) : base(restClient)
        {
            _endPoint = Routes.Prefixes.MessageMaster;
            _responseBuilder = responseBuilders;
            _restCsharpClient = restClient;
        }

        public async Task<BaseResult<IEnumerable<MessageDetailDto>>> GetMessageDetailsAsync(string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var resource = $"{_endPoint}{Routes.Paths.QueryParamSeperator}{Routes.Paths.UiSpecificMessages}";
                var request = GetRequest(token, resource, Constants.Authorization);
                var response = await _restCsharpClient.ExecuteTaskAsync<BaseResult<IEnumerable<MessageDetailDto>>>(request)
                    .ConfigureAwait(false);
                return _responseBuilder.GetBaseResult<IEnumerable<MessageDetailDto>>(response);
            }).ConfigureAwait(false);
        }
    }

}
