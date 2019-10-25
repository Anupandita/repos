using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using Sfc.Wms.App.Api.Contracts.Enums;
using Sfc.Wms.App.Api.Contracts.Interfaces;
using Sfc.Wms.App.Api.Contracts.Result;

namespace Sfc.Wms.App.Api.Nuget.ResponseBuilders
{
    public class RestResponseBuilder : IResponseBuilder
    {
        private BaseResult<string> OkResult<TResult>(IRestResponse response)
        {
            try
            {
               // var payload = JsonConvert.DeserializeObject<TResult>(response.Content);
                var result = new BaseResult<string>
                {
                    ResultType = ResultTypes.Ok,
                    Payload = response.Content,
                    Authentication = response.Headers
                };
                return result;
            }
            catch
            {
                return BadRequestResult<string>(response);
            }
        }

        private BaseResult<string> OkResultForCResponse<TResult>(IRestResponse response)
        {
            try
            {
                var jsonSerializerSettings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };

                var serializedResponse = JsonConvert.DeserializeObject<BaseResult<TResult>>(response.Content);
                var result = new BaseResult<string>
                {
                    ResultType = ResultTypes.Ok,
                    Payload = JsonConvert.SerializeObject(serializedResponse.Payload , jsonSerializerSettings),
                    Authentication = response.Headers
                };
                return result;
            }
            catch
            {
                return BadRequestResult<string>(response);
            }
        }

        private BaseResult<string> BadRequestResult<TResult>(IRestResponse response)
        {
            var result = new BaseResult<string>
            {
                ResultType = ResultTypes.BadRequest,

                ValidationMessages = new List<ValidationMessage>
                {
                  new ValidationMessage(ResultTypes.BadRequest.ToString(), response.Content),
                }
            };

            return result;
        }

        private BaseResult<string> NotFoundResult<TResult>()
        {
            var result = new BaseResult<string>
            {
                ResultType = ResultTypes.NotFound,
            };

            return result;
        }

        private BaseResult<string> CreatedResult<TResult>(IRestResponse response)
        {
            var result = new BaseResult<string>
            {
                ResultType = ResultTypes.Created,
                Payload = response.Content
            };

            return result;
        }

        public BaseResult<string> GetResponseData<TResult>(IRestResponse response)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return OkResult<TResult>(response);

                case HttpStatusCode.BadRequest:
                    return BadRequestResult<TResult>(response);

                case HttpStatusCode.Created:
                    return CreatedResult<TResult>(response);

                case HttpStatusCode.InternalServerError:
                    var content = JsonConvert.DeserializeObject<dynamic>(response.Content);
                    throw new ApplicationException(content.ExceptionMessage);

                default:
                    return NotFoundResult<TResult>();
            }
        }

        public BaseResult<string> GetCResponseData<TResult>(IRestResponse response)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return OkResultForCResponse<TResult>(response);

                case HttpStatusCode.BadRequest:
                    return BadRequestResult<TResult>(response);

                case HttpStatusCode.Created:
                    return CreatedResult<TResult>(response);

                case HttpStatusCode.InternalServerError:
                    var content = JsonConvert.DeserializeObject<dynamic>(response.Content);
                    throw new ApplicationException(content.ExceptionMessage);

                default:
                    return NotFoundResult<TResult>();
            }
        }

    }
}