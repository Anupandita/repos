using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Sfc.Wms.Security.Token.Jwt.Jwt;

namespace Sfc.App.Api.DelegatingHandlers
{
    public class SlidingExpirationHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);

            if (DoesGenerateNewToken(request, response, out var claimsPrincipal))
                return response;

            var token = JwtManager.GenerateToken(claimsPrincipal.Claims.ToArray());
            response.Headers.Add(Constants.Authorization, $"{Constants.Bearer} {token}");

            return response;
        }

        private static bool DoesGenerateNewToken(HttpRequestMessage request, HttpResponseMessage response,
            out ClaimsPrincipal claimsPrincipal)
        {
            claimsPrincipal = (ClaimsPrincipal) request.GetRequestContext().Principal;
            return response.StatusCode == HttpStatusCode.Unauthorized ||
                   !request.Headers.TryGetValues(Constants.Authorization, out var values) ||
                   values == null || !values.Any() ||
                   claimsPrincipal == null || !claimsPrincipal.Claims.Any();
        }
    }
}