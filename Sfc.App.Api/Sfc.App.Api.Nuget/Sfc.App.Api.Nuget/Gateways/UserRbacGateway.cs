﻿using System.Threading.Tasks;
using RestSharp;
using Sfc.App.Api.Contracts.Constants;
using Sfc.App.Api.Nuget.Interfaces;
using Sfc.Wms.Result;
using Sfc.Wms.Security.Contracts.Dtos;

namespace Sfc.App.Api.Nuget.Gateways
{
    public class UserRbacGateway : SfcBaseGateway, IUserRbacGateway
    {
        public UserRbacGateway(IRestClient restClient) : base(restClient)
        {
        }

        public async Task<BaseResult<UserInfoDto>> SignInAsync(LoginCredentials loginCredentials)
        {
            var request = new RestRequest($"{Route.User}/{Route.Paths.UserLogin}", Method.POST);
            request.AddJsonBody(loginCredentials);
            var result = await RestClient
                .ExecuteTaskAsync<BaseResult<UserInfoDto>>(request)
                .ConfigureAwait(false);
            return GetBaseResult<UserInfoDto>(result);
        }
    }
}