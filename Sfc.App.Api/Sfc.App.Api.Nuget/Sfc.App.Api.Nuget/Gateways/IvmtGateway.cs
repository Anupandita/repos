using RestSharp;
using Sfc.Wms.Interface.Asrs.Constants;
using Sfc.Wms.Interface.Asrs.Dtos;
using Sfc.Wms.Result;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sfc.App.Api.Nuget.Interfaces;

namespace Sfc.App.Api.Nuget.Gateways
{
    public class IvmtGateway : SfcBaseGateway, IIvmtGateway
    {
        

        public IvmtGateway(IRestClient restClient) : base(restClient)
        {
        }

        public async Task<BaseResult> CreateAsync(IvmtTriggerInputDto ivmtTriggerInput)
        {
            var request = new RestRequest($"{Routes.DematicMessageIvmtPrefix}", Method.POST).AddJsonBody(ivmtTriggerInput);
            var result = await RestClient
                .ExecuteTaskAsync<BaseResult>(request).ConfigureAwait(false);

            return ToBaseResult(result);
        }

       
    }
}