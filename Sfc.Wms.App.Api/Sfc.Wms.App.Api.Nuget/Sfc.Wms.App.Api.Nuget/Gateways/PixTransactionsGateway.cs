using System.Threading.Tasks;
using RestSharp;
using Sfc.Core.OnPrem.Result;
using Sfc.Core.RestResponse;
using Sfc.Wms.App.Api.Contracts.Constants;
using Sfc.Wms.App.Api.Contracts.Entities;
using Sfc.Wms.App.Api.Nuget.Builders;
using Sfc.Wms.App.Api.Nuget.Interfaces;

namespace Sfc.Wms.App.Api.Nuget.Gateways
{
    public class PixTransactionsGateway : SfcBaseGateway, IPixTransactionsGateway
    {
        private readonly string _endPoint;
        private readonly IResponseBuilder _responseBuilder;
        private readonly IRestClient _restClient;

        public PixTransactionsGateway(IResponseBuilder responseBuilders, IRestClient restClient) : base(restClient)
        {
            _endPoint = Routes.Prefixes.PixTransactions;
            _responseBuilder = responseBuilders;
            _restClient = restClient;
        }

        public async Task<BaseResult<string>> GetPixTransactionsDetailsAsync(PixTranactionsModel pixTranactionsModel,
            string token)
        {
            var retryPolicy = Proxy();
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var request = GetPixTransactionsDetailsRequest(pixTranactionsModel, token);
                var response = await _restClient.ExecuteTaskAsync<object>(request).ConfigureAwait(false);
                return _responseBuilder.GetResponseData<string>(response);
            }).ConfigureAwait(false);
        }

        private RestRequest GetPixTransactionsDetailsRequest(PixTranactionsModel pixTranactionsModel, string token)
        {
            var resource =
                $"{_endPoint}/{Routes.Paths.SearchPixTransactions}{Routes.Paths.QueryParamSymbol}pageNo={pixTranactionsModel.pageNo}{Routes.Paths.QueryParamAnd}rowsPerPage={pixTranactionsModel.rowsPerPage}{Routes.Paths.QueryParamAnd}totalRows={pixTranactionsModel.totalRows}";
            resource = QueryStringBuilder.BuildQuery("inpt_itemid=", pixTranactionsModel.inpt_itemid, resource, false);
            resource = QueryStringBuilder.BuildQuery("inpt_start_date=", pixTranactionsModel.inpt_start_date, resource,
                false);
            resource = QueryStringBuilder.BuildQuery("inpt_end_date=", pixTranactionsModel.inpt_end_date, resource,
                false);
            resource = QueryStringBuilder.BuildQuery("inpt_nbr_days=", pixTranactionsModel.inpt_nbr_days, resource,
                false);
            return GetRequest(token, resource);
        }
    }
}