using RestSharp;
using Wms.App.Contracts.Result;

namespace Wms.App.Contracts.Interfaces
{
    public interface IResponseBuilder
    {
        BaseResult<string> GetResponseData<TResult>(IRestResponse response);
        BaseResult<string> GetCResponseData<TResult>(IRestResponse response);

    }
}