using RestSharp;
using Sfc.Core.OnPrem.Result;
using Sfc.Core.OnPrem.Security.Contracts.Dtos;
using Sfc.Core.RestResponse;
using Sfc.Wms.App.Api.Contracts.Constants;
using Sfc.Wms.App.Api.Nuget.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfc.Wms.App.Api.Nuget.Gateways
{
    public class UserMasterGateway : SfcBaseGateway, IUserMasterGateway
    {
        private readonly string _endpoint;
        private readonly IResponseBuilder _responseBuilder;
        private readonly IRestCsharpClient _restCsharpClient;

        public UserMasterGateway(IResponseBuilder responseBuilder, IRestCsharpClient restClient) : base(restClient)
        {
            _endpoint = Routes.Prefixes.User;
            _responseBuilder = responseBuilder;
            _restCsharpClient = restClient;
        }

        public async Task<BaseResult<IEnumerable<PreferencesDto>>> UpdateUserPreferences(IEnumerable<PreferencesDto> preferencesDto, string token)
        {
            var retryPolicy = Proxy();
            _restCsharpClient.BaseUrl = new Uri(ServiceUrl);
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var resourceUrl = $"{_endpoint}/{Routes.Paths.Preferences}";
                var request = PostRequest(resourceUrl, preferencesDto, token, Constants.Authorization);
                var response = await _restCsharpClient.ExecuteTaskAsync<BaseResult<IEnumerable<PreferencesDto>>>(request).ConfigureAwait(false);
                return _responseBuilder.GetBaseResult<IEnumerable<PreferencesDto>> (response);
            }).ConfigureAwait(false);
        }
    }
}
