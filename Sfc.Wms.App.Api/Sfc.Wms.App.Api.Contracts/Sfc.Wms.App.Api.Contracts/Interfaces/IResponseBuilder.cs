using RestSharp;
using Sfc.Core.OnPrem.Result;

namespace Sfc.Wms.App.Api.Contracts.Interfaces
{
    public interface IResponseBuilder
    {
        BaseResult<string> GetResponseData<TResult>(IRestResponse response);
        BaseResult<string> GetCResponseData<TResult>(IRestResponse response);

    }
}