using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Grid.Generic.Api
{
    public class CookieMessageHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return base.SendAsync(request, cancellationToken);
        }
    }
}
