using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WanderXServer.BusinessObject;
using WanderXServer.BusinessObject.Enums;
using WanderXServer.DataAccessLayer;
using WanderXServer.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

builder.Services.AddControllers();
builder.Services.AddDbContext<WanderXDbContext>(options =>
{
    options.UseInMemoryDatabase("WanderX");
});
builder.Services.AddScoped<IAuthService, AuthService>();
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

SeedDevelopmentUsers(app.Services);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("WanderXClient");
app.UseAuthorization();
app.MapControllers();

app.Run();

static void SeedDevelopmentUsers(IServiceProvider services)
{
    using var scope = services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<WanderXDbContext>();

    if (dbContext.Users.Any())
    {
        return;
    }

    var admin = new ApplicationUser
    {
        FullName = "WanderX Admin",
        Email = "ad@ad.123",
        NormalizedEmail = AuthService.NormalizeEmail("ad@ad.123"),
        PhoneNumber = "+10000000000",
        Role = UserRole.Admin,
        IsEmailConfirmed = true,
        IsPhoneConfirmed = true
    };

    admin.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(admin, "123456");
    dbContext.Users.Add(admin);
    dbContext.SaveChanges();
}
