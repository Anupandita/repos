using AutoMapper;
using Sfc.Core.OnPrem.Result;
using Sfc.Core.OnPrem.Security.Contracts.Dtos;
using Sfc.Wms.App.Api.Interfaces;
using Sfc.Wms.App.App.Interfaces;
using Sfc.Wms.Configuration.SystemCode.Contracts.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfc.Wms.App.Api.Services
{
    public class RbacService : IRbacService
    {
        private readonly IMapper _mapper;
        private readonly IRbacGateway _rbacGateway;

        public RbacService(IMapper mapper, IRbacGateway rbacGateway)
        {
            _mapper = mapper;
            _rbacGateway = rbacGateway;
        }

        public async Task<BaseResult<UserInfoDto>> SignInAsync(LoginCredentials loginCredentials)
        {
            var result = await _rbacGateway.SignInAsync(loginCredentials).ConfigureAwait(false);

            return new BaseResult<UserInfoDto>
            {
                ValidationMessages = result.ValidationMessages,
                ResultType = result.ResultType,
                Payload = result.ResultType == ResultTypes.Ok ? result.Payload : null
            };
        }

        public async Task<BaseResult<UserInfoDto>> GetPrinterValuesAsyc(UserInfoDto userInfoDto)
        {
            var result = await _rbacGateway.GetPrinterValuesAsyc().ConfigureAwait(false);
            userInfoDto.PrinterList = _mapper.Map<IEnumerable<SysCodeDto>, IEnumerable<SfcPrinterSelectList>>(result.Payload);
            return new BaseResult<UserInfoDto>
            {
                ValidationMessages = result.ValidationMessages,
                ResultType = result.ResultType,
                Payload = result.ResultType == ResultTypes.Ok ? userInfoDto : null
            };
        }
    }
}