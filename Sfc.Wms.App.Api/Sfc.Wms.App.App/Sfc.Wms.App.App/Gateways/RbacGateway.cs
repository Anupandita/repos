using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sfc.Core.OnPrem.Result;
using Sfc.Core.OnPrem.Security.Contracts.Dtos;
using Sfc.Core.OnPrem.Security.Contracts.Interfaces;
using Sfc.Wms.App.App.Interfaces;
using Sfc.Wms.Configuration.SystemCode.Contracts.Dtos;
using Sfc.Wms.Configuration.SystemCode.Contracts.Interfaces;
using Sfc.Wms.Framework.Security.Token.Jwt.Jwt;

namespace Sfc.Wms.App.App.Gateways
{
    public class RbacGateway : BaseResultBuilder, IRbacGateway
    {
        private readonly IUserRbacService _userRbacService;
        private readonly ISystemCodeService _systemCodeService; 

        public RbacGateway(IUserRbacService userRbacService, ISystemCodeService systemCodeService)
        {
            _userRbacService = userRbacService;
            _systemCodeService = systemCodeService;
        }

       
        public IEnumerable<SfcPrinterSelectList> GetPrinterValuesAsyc()
        {
            SfcPrinterSelectList printerList = new SfcPrinterSelectList();
            List<SfcPrinterSelectList> sfcPrinterSelectLists = new List<SfcPrinterSelectList>();
            IEnumerable<SysCodeDto> printerValues = (IEnumerable<SysCodeDto>)_systemCodeService.GetSystemCodeAsync("C", "205", "", "CodeDesc", false);
            foreach(var printer in printerValues)
            {
                printerList.Id = printer.CodeId;
                printerList.Description = printer.CodeDesc;
                printerList.DisplayName = printer.MiscFlag;
                sfcPrinterSelectLists.Add(printerList);
            }
            return (IEnumerable<SfcPrinterSelectList>)sfcPrinterSelectLists.ToList();
        }

        public async Task<BaseResult<UserInfoDto>> SignInAsync(LoginCredentials loginCredentials)
        {
            try
            {
                var response = new BaseResult<UserInfoDto>
                { ResultType = ResultTypes.BadGateway };

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
            catch (Exception exception)
            {
                return GetBaseResult<UserInfoDto>(exception, GetType().Namespace, null);
            }
        }

        #region Private Methods

        private BaseResult<UserInfoDto> GetBaseResult(BaseResult<int> result)
        {
            var response = new BaseResult<UserInfoDto> { ResultType = result.ResultType };
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