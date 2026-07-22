using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WanderXServer.DataAccessLayer;
using WanderXServer.Services;
using WanderXServer.Security;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Register controllers with global XssSanitizationFilter
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<XssSanitizationFilter>();
});

builder.Services.AddDbContext<WanderXDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.");
    options.UseSqlServer(connectionString);
});

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IGuideService, GuideService>();
builder.Services.AddScoped<IGuideTourService, GuideTourService>();
builder.Services.AddScoped<UserSpecialRequestService>();
builder.Services.AddScoped<TourReviewService>();
builder.Services.AddScoped<ITourService, TourService>();
builder.Services.AddScoped<ITourScheduleService, TourScheduleService>();
builder.Services.AddScoped<IBookedTourService, BookedTourService>();
builder.Services.AddScoped<ITourPricingService, TourPricingService>();
builder.Services.Configure<EmailOptions>(builder.Configuration.GetSection("Email"));
builder.Services.AddScoped<IEmailSender, SmtpEmailSender>();

// Configure CORS dynamically from appsettings.json, allow credentials, and expose XSRF header
builder.Services.AddCors(options =>
{
    options.AddPolicy("WanderXClient", policy =>
    {
        var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
            ?? new[] { "http://localhost:5196", "https://localhost:7118" };
        policy
            .WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .WithExposedHeaders("X-XSRF-TOKEN");
    });
});

// Configure .NET 8 Rate Limiting
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    // Global sliding window rate limiter
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
    {
        var ip = httpContext.Connection.RemoteIpAddress?.ToString()
            ?? httpContext.Request.Headers.Host.ToString()
            ?? "unknown";

        return RateLimitPartition.GetSlidingWindowLimiter(ip, _ => new SlidingWindowRateLimiterOptions
        {
            PermitLimit = 60,
            Window = TimeSpan.FromMinutes(1),
            SegmentsPerWindow = 4,
            QueueLimit = 0
        });
    });

    // Auth specific limiter (5 requests per minute per IP for brute-force prevention)
    options.AddFixedWindowLimiter("AuthLimiter", opt =>
    {
        opt.Window = TimeSpan.FromMinutes(1);
        opt.PermitLimit = 5;
        opt.QueueLimit = 0;
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var jwtKey = builder.Configuration["Jwt:Key"] ?? "YourSuperSecretKeyThatIsAtLeast32CharactersLong!";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "WanderX";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "WanderXClient";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    try
    {
        scope.ServiceProvider.GetRequiredService<WanderXDbContext>().SeedDevelopmentData();
    }
    catch (Exception exception)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogWarning(exception, "Database initialization was skipped because the database is unavailable.");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Security Middlewares Pipeline (Order is critical)
app.UseCors("WanderXClient");
app.UseRateLimiter();
app.UseMiddleware<CsrfProtectionMiddleware>();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=AdminTourSchedules}/{action=Index}/{id?}");

app.Run();
