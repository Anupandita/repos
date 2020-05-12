using Sfc.Core.OnPrem.Result;
using Sfc.Core.RestResponse;
using Sfc.Wms.App.Api.Contracts.Constants;
using Sfc.Wms.App.Api.Nuget.Interfaces;
using Sfc.Wms.Configuration.MessageLogger.Contracts.UoW.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfc.Wms.App.Api.Nuget.Gateways
{
    public class MessageLoggerGateway : SfcBaseGateway, IMessageLoggerGateway
    {
        private readonly string _endPoint;
        private readonly IResponseBuilder _responseBuilder;
        private readonly IRestCsharpClient _restCsharpClient;

        public MessageLoggerGateway(IResponseBuilder responseBuilders, IRestCsharpClient restClient) : base(restClient)
        {
            _endPoint = Routes.Prefixes.MessageLogger;
            _responseBuilder = responseBuilders;
            _restCsharpClient = restClient;
        }

        public async Task<BaseResult> BatchInsertAsync(IEnumerable<MessageLogDto> messageLogDtos, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = PostRequest(_endPoint, messageLogDtos, token, Constants.Authorization);
                var response = await _restCsharpClient.ExecuteTaskAsync<BaseResult>(request)
                    .ConfigureAwait(false);
                return _responseBuilder.GetBaseResult(response);
            }).ConfigureAwait(false);
        }
    }
}