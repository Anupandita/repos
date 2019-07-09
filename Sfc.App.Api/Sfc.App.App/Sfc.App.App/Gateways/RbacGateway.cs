﻿using System.Linq;
using System.Threading.Tasks;
using Sfc.App.App.Interfaces;
using Sfc.Wms.Result;
using Sfc.Wms.Security.Contracts.Dtos;
using Sfc.Wms.Security.Contracts.Interfaces;
using Sfc.Wms.Security.Token.Jwt.Jwt;

namespace Sfc.App.App.Gateways
{
    public class RbacGateway : IRbacGateway
    {
        private readonly IUserRabcService _userRabcService;

        public RbacGateway(IUserRabcService userRabcService)
        {
            _userRabcService = userRabcService;
        }

        public async Task<BaseResult<UserInfoDto>> SignInAsync(LoginCredentials loginCredentials)
        {
            var response = new BaseResult<UserInfoDto>
            { ResultType = ResultTypes.BadGateway };

            if (!ValidateLoginCredentials(loginCredentials)) return response;

            var result = await _userRabcService.SignInAsync(loginCredentials).ConfigureAwait(false);

            if (result.ResultType != ResultTypes.Ok)
                return new BaseResult<UserInfoDto>
                {
                    ResultType = result.ResultType,
                    ValidationMessages = result.ValidationMessages
                };

            var roles = await _userRabcService.GetRolesByUserNameAsync(loginCredentials.UserName)
                .ConfigureAwait(false);

            var userDetails = await _userRabcService.GetUserDetails(loginCredentials.UserName)
                .ConfigureAwait(false);

            userDetails.Payload.Token = JwtManager.GenerateToken(loginCredentials.UserName, result.Payload,
                roles.Payload.Select(el => el.RoleName).ToList());

            return userDetails;
        }

        #region Private Methods

        private bool ValidateLoginCredentials(LoginCredentials loginCredentials)
        {
            return !(string.IsNullOrWhiteSpace(loginCredentials.UserName) ||
                     string.IsNullOrWhiteSpace(loginCredentials.Password));
        }

        #endregion
    }
}