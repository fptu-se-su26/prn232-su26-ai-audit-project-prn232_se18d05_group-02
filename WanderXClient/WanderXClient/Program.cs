using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WanderXClient;
using WanderXClient.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<AuthSessionService>();

// Register the custom CsrfHeaderHandler
builder.Services.AddTransient<CsrfHeaderHandler>();

builder.Services.AddScoped(sp =>
{
    var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? "http://localhost:5009/";
    var handler = sp.GetRequiredService<CsrfHeaderHandler>();
    handler.InnerHandler = new HttpClientHandler();
    return new AuthApiClient(new HttpClient(handler) { BaseAddress = new Uri(apiBaseUrl) });
});
builder.Services.AddScoped(sp =>
{
    var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? "http://localhost:5009/";
    var handler = sp.GetRequiredService<CsrfHeaderHandler>();
    handler.InnerHandler = new HttpClientHandler();
    var httpClient = new HttpClient(handler) { BaseAddress = new Uri(apiBaseUrl) };
    var sessionService = sp.GetRequiredService<AuthSessionService>();
    return new UserApiClient(httpClient, sessionService);
});
builder.Services.AddScoped(sp =>
{
    var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? "http://localhost:5009/";
    var handler = sp.GetRequiredService<CsrfHeaderHandler>();
    handler.InnerHandler = new HttpClientHandler();
    return new TourApiClient(new HttpClient(handler) { BaseAddress = new Uri(apiBaseUrl) });
});
builder.Services.AddScoped(sp =>
{
    var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? "http://localhost:5009/";
    var handler = sp.GetRequiredService<CsrfHeaderHandler>();
    handler.InnerHandler = new HttpClientHandler();
    return new TourScheduleApiClient(new HttpClient(handler) { BaseAddress = new Uri(apiBaseUrl) });
});
builder.Services.AddScoped(sp =>
{
    var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? "http://localhost:5009/";
    var handler = sp.GetRequiredService<CsrfHeaderHandler>();
    handler.InnerHandler = new HttpClientHandler();
    return new BookedTourApiClient(new HttpClient(handler) { BaseAddress = new Uri(apiBaseUrl) });
});
builder.Services.AddScoped(sp =>
{
    var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? "http://localhost:5009/";
    var handler = sp.GetRequiredService<CsrfHeaderHandler>();
    handler.InnerHandler = new HttpClientHandler();
    return new TourPricingApiClient(new HttpClient(handler) { BaseAddress = new Uri(apiBaseUrl) });
});

builder.Services.AddScoped(sp =>
{
    var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? "http://localhost:5009/";
    var handler = sp.GetRequiredService<CsrfHeaderHandler>();
    handler.InnerHandler = new HttpClientHandler();
    return new GuideApiClient(new HttpClient(handler) { BaseAddress = new Uri(apiBaseUrl) });
});

builder.Services.AddScoped(sp =>
{
    var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? "http://localhost:5009/";
    var handler = sp.GetRequiredService<CsrfHeaderHandler>();
    handler.InnerHandler = new HttpClientHandler();
    return new GuideTourApiClient(new HttpClient(handler) { BaseAddress = new Uri(apiBaseUrl) });
});

var host = builder.Build();

// Retrieve CSRF token from server response headers and store it on startup
var authApi = host.Services.GetRequiredService<AuthApiClient>();
await authApi.InitializeCsrfAsync();

await host.RunAsync();

