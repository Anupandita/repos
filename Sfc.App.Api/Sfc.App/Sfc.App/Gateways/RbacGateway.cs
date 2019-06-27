using System.Threading.Tasks;
using Sfc.App.Interfaces;
using Sfc.Wms.Result;
using Sfc.Wms.Security.Rbac.Contracts.Dtos;
using Sfc.Wms.Security.Rbac.Contracts.Dtos.UI;
using Sfc.Wms.Security.Rbac.Contracts.Interfaces;

namespace Sfc.App.Gateways
{
    public class RbacGateway : IRbacGateway
    {
        private readonly IUserRabcGateway _userRabcGateway;

        public RbacGateway(IUserRabcGateway userRabcGateway)
        {
            _userRabcGateway = userRabcGateway;
        }

        public async Task<BaseResult<UserDetailsDto>> SignInAsync(LoginCredentials loginCredentials)
        {
            var response = new BaseResult<UserDetailsDto>
                {ResultType = ResultTypes.Unauthorized, Payload = new UserDetailsDto()};

            if (!ValidateLoginCredentials(loginCredentials)) return response;

            response = await _userRabcGateway.SignInAsync(loginCredentials).ConfigureAwait(false);
            return response;
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