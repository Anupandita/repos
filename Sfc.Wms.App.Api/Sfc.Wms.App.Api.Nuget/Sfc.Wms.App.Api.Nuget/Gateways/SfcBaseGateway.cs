using Polly;
using Polly.Retry;
using RestSharp;
using System;
using System.Configuration;
using Sfc.Wms.App.Api.Contracts.Constants;
using System.Net.Http;

namespace Sfc.Wms.App.Api.Nuget.Gateways
{
    public class SfcBaseGateway
    {
        protected  string _serviceBaseUrl;
        private readonly int _maxRetryAttempts;
        private readonly TimeSpan _pauseBetweenFailures;
        private readonly int _webRequestTimeoutInSecs;

        public SfcBaseGateway()
        {
            _serviceBaseUrl = ConfigurationManager.AppSettings["BaseUrl"];

            if (!int.TryParse(ConfigurationManager.AppSettings["MaxRetryAttempts"], out _maxRetryAttempts))
                _maxRetryAttempts = 3;

            if (!int.TryParse(ConfigurationManager.AppSettings["PauseBetweenFailures"], out var pauseBetweenFailuresInSec))
                pauseBetweenFailuresInSec = 2;

            _pauseBetweenFailures = TimeSpan.FromSeconds(pauseBetweenFailuresInSec);

            if (!int.TryParse(ConfigurationManager.AppSettings["WebRequestTimeoutInSec"], out _webRequestTimeoutInSecs))
                _webRequestTimeoutInSecs = 300;
        }

        public AsyncRetryPolicy Proxy()
        {
            return Policy
                 .Handle<HttpRequestException>()
                 .WaitAndRetryAsync(_maxRetryAttempts, i => _pauseBetweenFailures);
        }

        public RestRequest GetRequest(string token, string resource)
        {
            var request = new RestRequest(resource, Method.GET)
            {
                RequestFormat = DataFormat.Json
            };
            request.AddHeader("Token", token);
            request.Timeout = _webRequestTimeoutInSecs * 1000;
            return request;
        }

        public RestRequest PutRequest<TEntity>(string resource, TEntity body, string token) where TEntity : class
        {
            var request = new RestRequest(resource, Method.PUT)
            {
                RequestFormat = DataFormat.Json
            };
            request.AddHeader(Constants.Token, token);
            request.AddJsonBody(body);
            request.Timeout = _webRequestTimeoutInSecs * 1000;
            return request;
        }

        public RestRequest PostRequest<TEntity>(string resource, TEntity body, string token) where TEntity : class
        {
            var request = new RestRequest(resource, Method.POST)
            {
                RequestFormat = DataFormat.Json
            };
            request.AddHeader(Constants.Token, token);
            request.AddJsonBody(body);
            request.Timeout = _webRequestTimeoutInSecs * 1000;
            return request;
        }

        public RestRequest DeleteRequest(string resource, string token) 
        {
            var request = new RestRequest(resource, Method.DELETE)
            {
                RequestFormat = DataFormat.Json
            };
            request.AddHeader(Constants.Token, token);
            request.Timeout = _webRequestTimeoutInSecs * 1000;
            return request;
        }
    }
}