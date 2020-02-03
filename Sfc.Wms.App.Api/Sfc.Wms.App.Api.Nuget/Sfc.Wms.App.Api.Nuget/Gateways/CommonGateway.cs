using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp;
using Sfc.Core.OnPrem.Result;
using Sfc.Core.RestResponse;
using Sfc.Wms.App.Api.Contracts.Constants;
using Sfc.Wms.App.Api.Contracts.Dto;
using Sfc.Wms.App.Api.Nuget.Interfaces;
using Sfc.Wms.Configuration.SystemCode.Contracts.Dtos;

namespace Sfc.Wms.App.Api.Nuget.Gateways
{
    public class CommonGateway : SfcBaseGateway, ICommonGateway
    {
        private readonly string _endPoint;
        private readonly IResponseBuilder _responseBuilder;
        private readonly IRestCsharpClient _restCsharpClient;

        public CommonGateway(IResponseBuilder responseBuilders, IRestCsharpClient restClient) : base(restClient)
        {
            _endPoint = Routes.Prefixes.Common;
            _responseBuilder = responseBuilders;
            _restCsharpClient = restClient;
        }

        public async Task<BaseResult<IEnumerable<SysCodeDto>>> GetCodeIdsAsync(SystemCodeInputDto systemCodeInputDto,string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var resource = $"{_endPoint}{Routes.Paths.QueryParamSeperator}{Routes.Paths.CodeIds}";
                var request = GetRequest(resource, systemCodeInputDto, token, Constants.Authorization);
                var response = await _restCsharpClient.ExecuteTaskAsync< BaseResult<IEnumerable<SysCodeDto>>>(request)
                    .ConfigureAwait(false);
                 return _responseBuilder.GetBaseResult<IEnumerable<SysCodeDto>>(response);
            }).ConfigureAwait(false);
        }
    }
}