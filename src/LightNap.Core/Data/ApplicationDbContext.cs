using LightNap.Core.Data.Comparers;
using LightNap.Core.Data.Converters;
using LightNap.Core.Data.Entities;
using LightNap.Core.Profile.Dto.Response;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LightNap.Core.Data
{
    /// <summary>
    /// Represents the application database context.
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        /// <summary>
        /// Notifications in the DB.
        /// </summary>
        public DbSet<Notification> Notifications { get; set; } = null!;

        /// <summary>
        /// Refresh tokens in the DB.
        /// </summary>
        public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;

        /// <summary>
        /// Chart of Accounts in the DB.
        /// </summary>
        public DbSet<Account> Accounts { get; set; } = null!;


        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
        /// </summary>
        /// <param name="options">The DbContext options.</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
        /// </summary>
        public ApplicationDbContext() { }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Notification>()
                .Property(n => n.Data)
                    .HasConversion(new DictionaryStringObjectConverter())
                    .Metadata.SetValueComparer(new DictionaryStringObjectValueComparer());

            builder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId)
                .IsRequired();

            builder.Entity<RefreshToken>()
                .HasOne(rt => rt.User)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(rt => rt.UserId)
                .IsRequired();

            builder.Entity<ApplicationUser>()
                .Property(u => u.BrowserSettings)
                .Metadata.SetValueComparer(new BrowserSettingsValueComparer());

            builder.Entity<Account>().HasData(
                new Account
                {
                    Id = 1,
                    Name = "Cash",
                    Type = "Asset",
                    Description = "Cash on hand",
                    Balance = 1000.00m
                },
                new Account
                {
                    Id = 2,
                    Name = "Accounts Payable",
                    Type = "Liability",
                    Description = "Outstanding bills",
                    Balance = 500.00m
                },
                new Account
                {
                    Id = 3,
                    Name = "Service Revenue",
                    Type = "Income",
                    Description = "Consulting services",
                    Balance = 7500.00m
                }
            );

        }


        /// <inheritdoc />
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            // Make sure all DateTime properties are stored as UTC.
            configurationBuilder.Properties<DateTime>().HaveConversion<UtcValueConverter>();

            // Storing this as a JSON string.
            configurationBuilder.Properties<BrowserSettingsDto>()
                .HaveConversion<BrowserSettingsValueConverter>();
        }
    }
}
