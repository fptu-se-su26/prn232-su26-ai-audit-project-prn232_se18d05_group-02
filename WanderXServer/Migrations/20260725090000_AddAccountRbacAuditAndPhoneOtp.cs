using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using WanderXServer.DataAccessLayer;
#nullable disable
namespace WanderXServer.Migrations;
[DbContext(typeof(WanderXDbContext))]
[Migration("20260725090000_AddAccountRbacAuditAndPhoneOtp")]
public partial class AddAccountRbacAuditAndPhoneOtp : Migration
{
 protected override void Up(MigrationBuilder migrationBuilder)
 {
  migrationBuilder.Sql("""
DELETE FROM [VerificationCodes] WHERE [Purpose] = 1;
IF COL_LENGTH('Users', 'AccountStatus') IS NULL ALTER TABLE [Users] ADD [AccountStatus] nvarchar(32) NOT NULL CONSTRAINT [DF_Users_AccountStatus] DEFAULT 'Active';
IF COL_LENGTH('Users', 'LockoutEnd') IS NULL ALTER TABLE [Users] ADD [LockoutEnd] datetime2 NULL;
IF COL_LENGTH('Users', 'LockReason') IS NULL ALTER TABLE [Users] ADD [LockReason] nvarchar(500) NULL;
IF COL_LENGTH('Users', 'TokenVersion') IS NULL ALTER TABLE [Users] ADD [TokenVersion] int NOT NULL CONSTRAINT [DF_Users_TokenVersion] DEFAULT 0;
IF OBJECT_ID(N'[AccountAuditLogs]', N'U') IS NULL BEGIN CREATE TABLE [AccountAuditLogs]([Id] uniqueidentifier NOT NULL PRIMARY KEY,[ActorUserId] uniqueidentifier NOT NULL,[TargetUserId] uniqueidentifier NOT NULL,[Action] nvarchar(40) NOT NULL,[OldValue] nvarchar(500) NULL,[NewValue] nvarchar(500) NULL,[Reason] nvarchar(500) NOT NULL,[CreatedAt] datetime2 NOT NULL); CREATE INDEX [IX_AccountAuditLogs_TargetUserId_CreatedAt] ON [AccountAuditLogs]([TargetUserId],[CreatedAt]); END;
IF OBJECT_ID(N'[PhoneVerifications]', N'U') IS NULL BEGIN CREATE TABLE [PhoneVerifications]([Id] uniqueidentifier NOT NULL PRIMARY KEY,[UserId] uniqueidentifier NOT NULL,[PhoneNumber] nvarchar(20) NOT NULL,[OtpHash] nvarchar(128) NOT NULL,[ExpiresAt] datetime2 NOT NULL,[AttemptCount] int NOT NULL,[MaxAttempts] int NOT NULL,[SentCount] int NOT NULL,[LastSentAt] datetime2 NOT NULL,[Status] nvarchar(20) NOT NULL,[ProviderMessageId] nvarchar(120) NULL,[CreatedAt] datetime2 NOT NULL,[VerifiedAt] datetime2 NULL); CREATE INDEX [IX_PhoneVerifications_UserId_PhoneNumber_CreatedAt] ON [PhoneVerifications]([UserId],[PhoneNumber],[CreatedAt]); END;
""");
 }
 protected override void Down(MigrationBuilder migrationBuilder){migrationBuilder.Sql("DROP TABLE IF EXISTS [PhoneVerifications]; DROP TABLE IF EXISTS [AccountAuditLogs]; IF COL_LENGTH('Users','TokenVersion') IS NOT NULL ALTER TABLE [Users] DROP CONSTRAINT [DF_Users_TokenVersion], COLUMN [TokenVersion]; IF COL_LENGTH('Users','AccountStatus') IS NOT NULL ALTER TABLE [Users] DROP CONSTRAINT [DF_Users_AccountStatus], COLUMN [AccountStatus]; IF COL_LENGTH('Users','LockoutEnd') IS NOT NULL ALTER TABLE [Users] DROP COLUMN [LockoutEnd]; IF COL_LENGTH('Users','LockReason') IS NOT NULL ALTER TABLE [Users] DROP COLUMN [LockReason];");}
}