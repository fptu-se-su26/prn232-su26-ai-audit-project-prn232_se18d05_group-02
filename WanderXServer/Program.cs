using Microsoft.EntityFrameworkCore;
using WanderXServer.DataAccessLayer;
using WanderXServer.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

builder.Services.AddControllers();
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<WanderXDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.");
    options.UseSqlServer(connectionString);
});
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IGuideService, GuideService>();
builder.Services.AddScoped<IGuideTourService, GuideTourService>();
builder.Services.AddScoped<ITourService, TourService>();
builder.Services.AddScoped<ITourScheduleService, TourScheduleService>();
builder.Services.Configure<EmailOptions>(builder.Configuration.GetSection("Email"));
builder.Services.AddScoped<IEmailSender, SmtpEmailSender>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("WanderXClient", policy =>
    {
        policy
            .WithOrigins("http://localhost:5196", "https://localhost:7118")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.UseCors("WanderXClient");
app.UseStaticFiles();
app.UseAuthorization();
app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=AdminTourSchedules}/{action=Index}/{id?}");

app.Run();
