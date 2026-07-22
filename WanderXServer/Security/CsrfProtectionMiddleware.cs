using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;

namespace WanderXServer.Security;

public class CsrfProtectionMiddleware
{
    private readonly RequestDelegate _next;
    private const string CsrfCookieName = "XSRF-TOKEN";
    private const string CsrfHeaderName = "X-XSRF-TOKEN";

    public CsrfProtectionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var method = context.Request.Method;

        // Skip CORS preflight OPTIONS requests
        if (HttpMethods.IsOptions(method))
        {
            await _next(context);
            return;
        }

        // 1. For safe methods, set/refresh the CSRF cookie and response header
        if (HttpMethods.IsGet(method) || HttpMethods.IsHead(method))
        {
            string token;

            if (context.Request.Cookies.TryGetValue(CsrfCookieName, out var existingToken) && !string.IsNullOrEmpty(existingToken))
            {
                token = existingToken;
            }
            else
            {
                token = GenerateToken();
                context.Response.Cookies.Append(CsrfCookieName, token, new CookieOptions
                {
                    HttpOnly = false, // Must be readable by client-side JavaScript / Blazor WASM
                    Secure = context.Request.IsHttps,
                    SameSite = context.Request.IsHttps ? SameSiteMode.None : SameSiteMode.Lax,
                    Path = "/"
                });
            }

            // Expose the CSRF token in the response headers so that the client can read it even in cross-origin environments
            context.Response.Headers[CsrfHeaderName] = token;
        }
        // 2. For state-changing methods, validate the CSRF token from cookie and header
        else
        {
            var cookieToken = context.Request.Cookies[CsrfCookieName];
            var headerToken = context.Request.Headers[CsrfHeaderName].ToString();

            if (string.IsNullOrEmpty(cookieToken) || string.IsNullOrEmpty(headerToken) || cookieToken != headerToken)
            {
                System.Console.WriteLine($"[CsrfProtectionMiddleware] CSRF validation failed for {method} request to {context.Request.Path}");
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(new { detail = "Anti-CSRF token validation failed. Missing or mismatched CSRF token." });
                return;
            }
        }

        await _next(context);
    }

    private static string GenerateToken()
    {
        return Convert.ToHexString(RandomNumberGenerator.GetBytes(16)).ToLowerInvariant();
    }
}
