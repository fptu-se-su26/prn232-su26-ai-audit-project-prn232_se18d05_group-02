using Microsoft.EntityFrameworkCore;
using WanderXServer.BusinessObject;

namespace WanderXServer.DataAccessLayer;

public class WanderXDbContext : DbContext
{
    public WanderXDbContext(DbContextOptions<WanderXDbContext> options) : base(options)
    {
    }

    public DbSet<ApplicationUser> Users => Set<ApplicationUser>();

    public DbSet<AuthVerificationCode> VerificationCodes => Set<AuthVerificationCode>();

    public DbSet<PasswordResetToken> PasswordResetTokens => Set<PasswordResetToken>();
}
