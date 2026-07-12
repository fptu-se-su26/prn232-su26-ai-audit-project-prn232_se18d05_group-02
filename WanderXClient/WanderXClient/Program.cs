using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WanderXClient;
using WanderXClient.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<AuthSessionService>();
builder.Services.AddScoped(_ =>
{
    var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? "http://localhost:5009/";
    return new AuthApiClient(new HttpClient { BaseAddress = new Uri(apiBaseUrl) });
});
builder.Services.AddScoped(_ =>
{
    var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? "http://localhost:5009/";
    return new GuideApiClient(new HttpClient { BaseAddress = new Uri(apiBaseUrl) });
});
builder.Services.AddScoped(_ =>
{
    var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? "http://localhost:5009/";
    return new GuideTourApiClient(new HttpClient { BaseAddress = new Uri(apiBaseUrl) });
});
builder.Services.AddScoped(sp =>
{
    var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? "http://localhost:5009/";
    var httpClient = new HttpClient { BaseAddress = new Uri(apiBaseUrl) };
    var sessionService = sp.GetRequiredService<AuthSessionService>();
    return new UserApiClient(httpClient, sessionService);
});

await builder.Build().RunAsync();
