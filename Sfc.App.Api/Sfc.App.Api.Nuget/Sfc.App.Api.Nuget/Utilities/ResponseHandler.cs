using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using RestSharp;
using Sfc.Wms.Result;

namespace Sfc.App.Api.Nuget.Utilities
{
    public abstract class ResponseHandler
    {
        protected BaseResult<TResult> GetBaseResult<TResult>(IRestResponse response)
        {
            switch (response.ResponseStatus)
            {
                case ResponseStatus.Completed:
                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.OK:
                        case HttpStatusCode.Created:
                            return GetDeserializedBaseResult<TResult>(response);
                        case HttpStatusCode.NotFound: return NotFoundResult<TResult>();
                        default:
                            return BadRequestResult<TResult>(response);
                    }
                default:
                    return NotCompletedResult<TResult>(response);
            }
        }

        protected BaseResult GetBaseResult(IRestResponse response)
        {
            return JsonConvert.DeserializeObject<BaseResult>(response.Content);
        }


        private List<ValidationMessage> GetValidationMessages(IRestResponse response)
        {
            return new List<ValidationMessage>
            {
                new ValidationMessage
                {
                    FieldName = nameof(response.ResponseStatus), Message = response.ResponseStatus.ToString()
                },
                new ValidationMessage
                {
                    FieldName = nameof(response.Request.Resource), Message = response.Request.Resource
                },
                new ValidationMessage
                {
                    FieldName = nameof(response.Content), Message = response.Content
                },
                new ValidationMessage
                {
                    FieldName = nameof(response.ErrorMessage), Message = response.ErrorMessage
                }
            };
        }

        private BaseResult<TResult> GetDeserializedBaseResult<TResult>(IRestResponse response)
        {
            return JsonConvert.DeserializeObject<BaseResult<TResult>>(response.Content);
        }

        private BaseResult<TResult> NotCompletedResult<TResult>(IRestResponse response)
        {
            return new BaseResult<TResult>
            {
                ResultType = ResultTypes.NotCompleted,
                ValidationMessages = GetValidationMessages(response)
            };
        }

        private BaseResult<TResult> BadRequestResult<TResult>(IRestResponse response)
        {
            return new BaseResult<TResult>
            {
                ResultType = ResultTypes.BadRequest,
                ValidationMessages = GetValidationMessages(response)
            };
        }

        private BaseResult<TResult> NotFoundResult<TResult>()
        {
            return new BaseResult<TResult>
            {
                ResultType = ResultTypes.NotFound
            };
        }
    }
}