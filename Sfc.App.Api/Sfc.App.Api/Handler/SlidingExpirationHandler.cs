using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Sfc.Wms.Security.Token.Jwt.Jwt;

namespace Sfc.App.Api.Handler
{
    public class SlidingExpirationHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized ||
                !(request.GetRequestContext().Principal is ClaimsPrincipal claimsPrincipal))
                return response;

            var token = JwtManager.GenerateToken(claimsPrincipal.Claims.ToArray());
            response.Headers.Add("Bearer", token);

            return response;
        }
    }
}