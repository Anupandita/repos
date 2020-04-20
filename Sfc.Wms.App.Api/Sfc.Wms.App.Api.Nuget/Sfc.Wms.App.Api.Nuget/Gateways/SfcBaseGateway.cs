using Polly;
using Polly.Retry;
using RestSharp;
using Sfc.Core.OnPrem.Result;
using Sfc.Wms.App.Api.Contracts.Constants;
using Sfc.Wms.App.Api.Nuget.Extensions;
using Sfc.Wms.App.Api.Nuget.Serializer;
using System;
using System.Configuration;
using System.Net.Http;

namespace Sfc.Wms.App.Api.Nuget.Gateways
{
    public class SfcBaseGateway : BaseResultBuilder
    {
        private readonly int _maxRetryAttempts;
        private readonly NewtonsoftJsonSerializer _newtonsoftJsonSerializer;
        private readonly TimeSpan _pauseBetweenFailures;
        private readonly int _webRequestTimeoutInSecs;
        protected string ServiceBaseUrl;
        protected string ServiceUrl;

        protected SfcBaseGateway(IRestClient restClient)
        {
            ServiceBaseUrl = ConfigurationManager.AppSettings["BaseUrl"];
            ServiceUrl = ConfigurationManager.AppSettings["ServiceUrl"];

            if (!int.TryParse(ConfigurationManager.AppSettings["MaxRetryAttempts"], out _maxRetryAttempts))
                _maxRetryAttempts = 3;

            if (!int.TryParse(ConfigurationManager.AppSettings["PauseBetweenFailures"],
                out var pauseBetweenFailuresInSec))
                pauseBetweenFailuresInSec = 2;

            _pauseBetweenFailures = TimeSpan.FromSeconds(pauseBetweenFailuresInSec);

            if (!int.TryParse(ConfigurationManager.AppSettings["WebRequestTimeoutInSec"], out _webRequestTimeoutInSecs))
                _webRequestTimeoutInSecs = 300;

            _newtonsoftJsonSerializer = NewtonsoftJsonSerializer.Default;
            restClient.AddHandler("application/json", () => _newtonsoftJsonSerializer);
        }

        protected AsyncRetryPolicy Proxy()
        {
            return Policy
                .Handle<HttpRequestException>()
                .WaitAndRetryAsync(_maxRetryAttempts, i => _pauseBetweenFailures);
        }

        protected RestRequest GetRequest(string token, string resource)
        {
            var request = new RestRequest(resource, Method.GET)
            {
                RequestFormat = DataFormat.Json,
                JsonSerializer = _newtonsoftJsonSerializer
            };
            request.AddHeader(Constants.Token, token);
            request.Timeout = _webRequestTimeoutInSecs * 1000;
            return request;
        }

        protected RestRequest PutRequest<TEntity>(string resource, TEntity body, string token) where TEntity : class
        {
            var request = new RestRequest(resource, Method.PUT)
            {
                RequestFormat = DataFormat.Json,
                JsonSerializer = _newtonsoftJsonSerializer
            };
            request.AddHeader(Constants.Token, token);
            request.AddJsonBody(body);
            request.Timeout = _webRequestTimeoutInSecs * 1000;
            return request;
        }

        protected RestRequest PostRequest<TEntity>(string resource, TEntity body, string token) where TEntity : class
        {
            var request = new RestRequest(resource, Method.POST)
            {
                RequestFormat = DataFormat.Json,
                JsonSerializer = _newtonsoftJsonSerializer
            };
            request.AddHeader(Constants.Token, token);
            request.AddJsonBody(body);
            request.Timeout = _webRequestTimeoutInSecs * 1000;
            return request;
        }

        protected RestRequest DeleteRequest(string resource, string token)
        {
            var request = new RestRequest(resource, Method.DELETE)
            {
                RequestFormat = DataFormat.Json,
                JsonSerializer = _newtonsoftJsonSerializer
            };
            request.AddHeader(Constants.Token, token);
            request.Timeout = _webRequestTimeoutInSecs * 1000;
            return request;
        }

        protected RestRequest GetRequest(string token, string resource, string header)
        {
            var request = new RestRequest(resource, Method.GET)
            {
                RequestFormat = DataFormat.Json,
                JsonSerializer = _newtonsoftJsonSerializer
            };
            request.AddHeader(header, token);
            request.Timeout = _webRequestTimeoutInSecs * 1000;
            return request;
        }

        protected RestRequest PutRequest<TEntity>(string resource, TEntity body, string token, string header)
            where TEntity : class
        {
            var request = new RestRequest(resource, Method.PUT)
            {
                RequestFormat = DataFormat.Json,
                JsonSerializer = _newtonsoftJsonSerializer
            };
            request.AddHeader(header, token);
            request.AddJsonBody(body);
            request.Timeout = _webRequestTimeoutInSecs * 1000;
            return request;
        }

        protected RestRequest PostRequest<TEntity>(string resource, TEntity body, string token, string header)
            where TEntity : class
        {
            var request = new RestRequest(resource, Method.POST)
            {
                RequestFormat = DataFormat.Json,
                JsonSerializer = _newtonsoftJsonSerializer
            };
            request.AddHeader(header, token);
            request.AddJsonBody(body);
            request.Timeout = _webRequestTimeoutInSecs * 1000;
            return request;
        }

        protected RestRequest DeleteRequest(string resource, string token, string header)
        {
            var request = new RestRequest(resource, Method.DELETE)
            {
                RequestFormat = DataFormat.Json,
                JsonSerializer = _newtonsoftJsonSerializer
            };
            request.AddHeader(header, token);
            request.Timeout = _webRequestTimeoutInSecs * 1000;
            return request;
        }

        protected RestRequest GetRequest<TEntity>(string resource, TEntity query, string token, string header) where TEntity : class
        {
            var formUrl = new FormUrlEncodedContent(query.ToKeyValue());
            var queryString = formUrl.ReadAsStringAsync().GetAwaiter().GetResult();
            var url = string.Concat(resource, "?", queryString);

            var request = new RestRequest(url, Method.GET)
            {
                RequestFormat = DataFormat.Json,
                JsonSerializer = _newtonsoftJsonSerializer
            };
            request.AddHeader(header, token);
            request.Timeout = _webRequestTimeoutInSecs * 1000;
            return request;
        }
    }
}