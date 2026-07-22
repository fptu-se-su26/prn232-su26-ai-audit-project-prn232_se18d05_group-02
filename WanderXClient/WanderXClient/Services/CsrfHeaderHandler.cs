using Microsoft.AspNetCore.Components.WebAssembly.Http;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace WanderXClient.Services;

public class CsrfHeaderHandler : DelegatingHandler
{
    private static string? _csrfToken;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // 1. Instruct the browser to send credentials (cookies) in cross-origin environments
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

        // 2. Attach the CSRF token to the header if we have one stored
        if (!string.IsNullOrEmpty(_csrfToken))
        {
            request.Headers.Add("X-XSRF-TOKEN", _csrfToken);
        }

        var response = await base.SendAsync(request, cancellationToken);

        // 3. Automatically capture the CSRF token from the response header if present
        if (response.Headers.TryGetValues("X-XSRF-TOKEN", out var values))
        {
            foreach (var val in values)
            {
                if (!string.IsNullOrEmpty(val))
                {
                    _csrfToken = val;
                    break;
                }
            }
        }

        return response;
    }
}
