using Microsoft.AspNetCore.Components.WebAssembly.Http;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace WanderXClient.Services;

public class CsrfHeaderHandler : DelegatingHandler
{
    private static string? _csrfToken;
    private const string CsrfHeaderName = "X-XSRF-TOKEN";

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // 1. Instruct the browser to send credentials (cookies) in cross-origin environments
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

        if (RequiresCsrfToken(request.Method) && string.IsNullOrEmpty(_csrfToken))
        {
            await RefreshTokenAsync(request, cancellationToken);
        }

        // 2. Attach the CSRF token to the header if we have one stored
        if (!string.IsNullOrEmpty(_csrfToken))
        {
            request.Headers.Remove(CsrfHeaderName);
            request.Headers.Add(CsrfHeaderName, _csrfToken);
        }

        var response = await base.SendAsync(request, cancellationToken);

        // 3. Automatically capture the CSRF token from the response header if present
        CaptureToken(response);

        return response;
    }

    private async Task RefreshTokenAsync(HttpRequestMessage sourceRequest, CancellationToken cancellationToken)
    {
        if (InnerHandler is null)
        {
            return;
        }

        var csrfUri = sourceRequest.RequestUri?.IsAbsoluteUri == true
            ? new Uri(sourceRequest.RequestUri, "/api/auth/csrf")
            : new Uri("api/auth/csrf", UriKind.Relative);

        using var csrfRequest = new HttpRequestMessage(HttpMethod.Get, csrfUri);
        csrfRequest.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

        using var response = await base.SendAsync(csrfRequest, cancellationToken);
        CaptureToken(response);
    }

    private static void CaptureToken(HttpResponseMessage response)
    {
        if (response.Headers.TryGetValues(CsrfHeaderName, out var values))
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
    }

    private static bool RequiresCsrfToken(HttpMethod method)
    {
        return method != HttpMethod.Get &&
            method != HttpMethod.Head &&
            method != HttpMethod.Options &&
            method != HttpMethod.Trace;
    }
}
