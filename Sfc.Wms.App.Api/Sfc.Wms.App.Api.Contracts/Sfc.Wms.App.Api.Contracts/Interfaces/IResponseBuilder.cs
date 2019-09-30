using RestSharp;
using Sfc.Wms.App.Api.Contracts.Result;

namespace Sfc.Wms.App.Api.Contracts.Interfaces
{
    public interface IResponseBuilder
    {
        BaseResult<string> GetResponseData<TResult>(IRestResponse response);
        BaseResult<string> GetCResponseData<TResult>(IRestResponse response);

    }
}