﻿using System.Linq;
using System.Threading.Tasks;
using Sfc.App.App.Interfaces;
using Sfc.Core.OnPrem.Result;
using Sfc.Core.OnPrem.Security.Contracts.Dtos;
using Sfc.Core.OnPrem.Security.Contracts.Interfaces;
using Sfc.Wms.Security.Token.Jwt.Jwt;

namespace Sfc.App.App.Gateways
{
    public class RbacGateway : IRbacGateway
    {
        private readonly IUserRbacService _userRbacService;

        public RbacGateway(IUserRbacService userRbacService)
        {
            _userRbacService = userRbacService;
        }

        public async Task<BaseResult<UserInfoDto>> SignInAsync(LoginCredentials loginCredentials)
        {
            var response = new BaseResult<UserInfoDto>
                {ResultType = ResultTypes.BadGateway};

            if (!ValidateLoginCredentials(loginCredentials)) return response;

            var result = await _userRbacService.SignInAsync(loginCredentials).ConfigureAwait(false);

            if (result.ResultType != ResultTypes.Ok) return GetBaseResult(result);

            var roles = await _userRbacService.GetRolesByUserNameAsync(loginCredentials.UserName)
                .ConfigureAwait(false);

            var userDetails = await _userRbacService.GetUserDetailsAsync(loginCredentials.UserName)
                .ConfigureAwait(false);

            userDetails.Payload.Token = JwtManager.GenerateToken(loginCredentials.UserName, result.Payload,
                roles.Payload.Select(el => el.RoleName).ToList());

            return userDetails;
        }

        #region Private Methods

        private BaseResult<UserInfoDto> GetBaseResult(BaseResult<int> result)
        {
            var response = new BaseResult<UserInfoDto> {ResultType = result.ResultType};
            if (result.ValidationMessages?.Count > 0)
                response.ValidationMessages.AddRange(result.ValidationMessages);
            return response;
        }

        private bool ValidateLoginCredentials(LoginCredentials loginCredentials)
        {
            return !(string.IsNullOrWhiteSpace(loginCredentials.UserName) ||
                     string.IsNullOrWhiteSpace(loginCredentials.Password));
        }

        #endregion
    }
}