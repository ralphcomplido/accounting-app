using LightNap.Core.Configuration;
using LightNap.Core.Data;
using LightNap.Core.Data.Entities;
using LightNap.Core.Data.Extensions;
using LightNap.Core.Identity.Dto.Request;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Data;

namespace LightNap.WebApi.Configuration
{
    /// <summary>
    /// Class responsible for seeding content in the application upon load.
    /// </summary>
    public partial class Seeder
    {
        private readonly RoleManager<ApplicationRole> RoleManager;
        private readonly ILogger<Seeder> Logger;
        private readonly UserManager<ApplicationUser> UserManager;
        private readonly ApplicationDbContext Db;
        private readonly IServiceProvider ServiceProvider;
        private readonly IOptions<List<AdministratorConfiguration>> AdministratorConfigurations;
        private readonly IOptions<ApplicationSettings> ApplicationSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="Seeder"/> class.
        /// </summary>
        /// <param name="serviceProvider">Service provider to pull dependencies from.</param>
        public Seeder(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            this.RoleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            this.Logger = serviceProvider.GetRequiredService<ILogger<Seeder>>();
            this.UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            this.Db = serviceProvider.GetRequiredService<ApplicationDbContext>();
            this.AdministratorConfigurations = serviceProvider.GetRequiredService<IOptions<List<AdministratorConfiguration>>>();
            this.ApplicationSettings = serviceProvider.GetRequiredService<IOptions<ApplicationSettings>>();
        }

        /// <summary>
        /// Run seeding functionality necessary every time an application loads, regardless of environment.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task SeedAsync()
        {
            await this.SeedRolesAsync();
            await this.SeedAdministratorsAsync();
            await this.SeedApplicationContentAsync();
            await this.SeedEnvironmentContentAsync();
        }

        /// <summary>
        /// Seeds the roles in the application.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task SeedRolesAsync()
        {
            foreach (ApplicationRole role in ApplicationRoles.All)
            {
                if (!await this.RoleManager.RoleExistsAsync(role.Name!))
                {
                    var result = await this.RoleManager.CreateAsync(role);
                    if (!result.Succeeded)
                    {
                        throw new ArgumentException($"Unable to create role '{role.Name}': {string.Join("; ", result.Errors.Select(error => error.Description))}");
                    }
                    this.Logger.LogInformation("Added role '{roleName}'", role.Name);
                }
            }

            var roleSet = new HashSet<string>(ApplicationRoles.All.Select(role => role.Name!), StringComparer.OrdinalIgnoreCase);

            foreach (var role in this.RoleManager.Roles.Where(role => role.Name != null && !roleSet.Contains(role.Name)))
            {
                var result = await this.RoleManager.DeleteAsync(role);
                if (!result.Succeeded)
                {
                    throw new ArgumentException($"Unable to remove role '{role.Name}': {string.Join("; ", result.Errors.Select(error => error.Description))}");
                }
                this.Logger.LogInformation("Removed role '{roleName}'", role.Name);
            }
        }

        /// <summary>
        /// Seeds the administrators in the application.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task SeedAdministratorsAsync()
        {
            if (this.AdministratorConfigurations.Value is null) { return; }

            foreach (var administrator in this.AdministratorConfigurations.Value)
            {
                ApplicationUser user = await this.GetOrCreateUserAsync(administrator.UserName, administrator.Email, administrator.Password);
                await this.AddUserToRole(user, ApplicationRoles.Administrator.Name!);
            }
        }

        /// <summary>
        /// Creates a new user in the application.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <param name="email">The email address.</param>
        /// <param name="password">The password.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task<ApplicationUser> GetOrCreateUserAsync(string userName, string email, string? password = null)
        {
            ApplicationUser? user = await this.UserManager.FindByEmailAsync(email);

            if (user is null)
            {
                bool passwordProvided = !string.IsNullOrWhiteSpace(password);
                string passwordToSet = passwordProvided ? password! : $"P@ssw0rd{Guid.NewGuid()}";

                var registerRequestDto = new RegisterRequestDto()
                {
                    ConfirmPassword = passwordToSet,
                    DeviceDetails = "Seeder",
                    Email = email,
                    Password = passwordToSet,
                    UserName = userName
                };

                user = registerRequestDto.ToCreate(this.ApplicationSettings.Value.RequireTwoFactorForNewUsers);

                var result = await this.UserManager.CreateAsync(user, passwordToSet);
                if (!result.Succeeded)
                {
                    throw new ArgumentException($"Unable to create user '{userName}' ('{email}'): {string.Join("; ", result.Errors.Select(error => error.Description))}");
                }

                this.Logger.LogInformation("Created user '{userName}' ('{email}')", userName, email);
            }

            return user;
        }

        /// <summary>
        /// Adds a user to a specified role if they're not already in it.
        /// </summary>
        /// <param name="user">The user to add to the role.</param>
        /// <param name="role">The role to add the user to.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task AddUserToRole(ApplicationUser user, string role)
        {
            if (!await this.UserManager.IsInRoleAsync(user, role))
            {
                var result = await this.UserManager.AddToRoleAsync(user, role);
                if (!result.Succeeded)
                {
                    throw new ArgumentException(
                        $"Unable to add user '{user.UserName}' ('{user.Email}') to role '{role}': {string.Join("; ", result.Errors.Select(error => error.Description))}");
                }
            }

            this.Logger.LogInformation("Added user '{userName}' ('{email}') to role '{roleName}'", user.UserName, user.Email, role);
        }

        /// <summary>
        /// Seeds content in the application. This method runs after baseline seeding (like roles and administrators) and provides an opportunity to
        /// seed any content required to be loaded regardless of environment.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        private Task SeedApplicationContentAsync()
        {
            // TODO: Add any seeding code you want run every time the app loads in any environment. For environment-specific seeding, see SeedEnvironmentContent().

            return Task.CompletedTask;
        }

        /// <summary>
        /// Seeds content in the application based on the implementation of a SeedEnvironmentContent partial method in the class. To use this, add a Seeder 
        /// partial class (like Seeder.Development.cs) that implements the private method SeedEnvironmentContent(). It runs after SeedApplicationContentAsync()
        /// and is always executed on load if it exists.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task SeedEnvironmentContentAsync()
        {
            this.SeedEnvironmentContent();
            return Task.CompletedTask;
        }

        /// <summary>
        /// Optional partial to implement in a new class (like Seeder.Development.cs) to seed environment-specific content.
        /// </summary>
        partial void SeedEnvironmentContent();
    }
}
