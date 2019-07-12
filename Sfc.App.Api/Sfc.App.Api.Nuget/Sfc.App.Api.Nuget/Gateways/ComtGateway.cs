using RestSharp;
using Sfc.App.Api.Nuget.Interfaces;
using Sfc.Wms.Interface.Asrs.Constants;
using Sfc.Wms.Interface.Asrs.Dtos;
using Sfc.Wms.Result;
using System.Threading.Tasks;

namespace Sfc.App.Api.Nuget.Gateways
{
    public class ComtGateway : SfcBaseGateway, IComtGateway
    {
        public ComtGateway(IRestClient restClient) : base(restClient)
        {
        }

        public async Task<BaseResult> CreateAsync(ComtTriggerInputDto comtTriggerInput)
        {
            var request = new RestRequest($"{Routes.DematicMessageComtPrefix}", Method.POST).AddJsonBody(comtTriggerInput);
            var result = await RestClient
                .ExecuteTaskAsync<BaseResult>(request).ConfigureAwait(false);

            return ToBaseResult(result);
        }
    }
}