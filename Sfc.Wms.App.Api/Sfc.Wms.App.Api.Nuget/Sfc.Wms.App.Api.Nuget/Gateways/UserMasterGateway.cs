using RestSharp;
using Sfc.Core.OnPrem.Result;
using Sfc.Core.RestResponse;
using Sfc.Wms.App.Api.Contracts.Constants;
using Sfc.Wms.App.Api.Nuget.Interfaces;
using Sfc.Wms.Configuration.UserMaster.Contracts.Dtos;
using System;
using System.Threading.Tasks;
using Sfc.Wms.App.Api.Contracts.Entities;

namespace Sfc.Wms.App.Api.Nuget.Gateways
{
    public class UserMasterGateway : SfcBaseGateway, IUserMasterGateway
    {
        private readonly string _endpoint;
        private readonly IResponseBuilder _responseBuilder;
        private readonly IRestClient _restCsharpClient;

        public UserMasterGateway(IResponseBuilder responseBuilder, IRestClient restClient) : base(restClient)
        {
            _endpoint = Routes.Prefixes.User;
            _responseBuilder = responseBuilder;
            restClient.BaseUrl = new Uri(ServiceUrl);
            _restCsharpClient = restClient;
        }

        public async Task<BaseResult> CreateUserPreferences(SwmUserSettingDto swmUserSettingDto, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var resourceUrl = $"{_endpoint}/{Routes.Paths.Preferences}";
                var request = PostRequest(resourceUrl, swmUserSettingDto, token, Constants.Authorization);
                var response = await _restCsharpClient.ExecuteTaskAsync<BaseResult>(request).ConfigureAwait(false);
                return _responseBuilder.GetBaseResult(response);
            }).ConfigureAwait(false);
        }

        public async Task<BaseResult> UpdateUserPreferences(UserPreferencesModel userPreferencesModel, string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var resourceUrl = $"{_endpoint}/{Routes.Paths.Preferences}";
                var request = PostRequest(resourceUrl, userPreferencesModel, token, Constants.Authorization);
                var response = await _restCsharpClient.ExecuteTaskAsync<BaseResult>(request).ConfigureAwait(false);
                return _responseBuilder.GetBaseResult(response);
            }).ConfigureAwait(false);
        }
    }
}
