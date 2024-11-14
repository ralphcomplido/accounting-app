using LightNap.Core.Administrator.Dto.Request;
using LightNap.Core.Administrator.Services;
using LightNap.Core.Api;
using LightNap.Core.Data;
using LightNap.Core.Data.Entities;
using LightNap.Core.Extensions;
using LightNap.Core.Tests.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Data;

namespace LightNap.Core.Tests.Services
{
    [TestClass]
    public class AdministratorServiceTests
    {
        // These will be initialized during TestInitialize.
#pragma warning disable CS8618
        private RoleManager<ApplicationRole> _roleManager;
        private UserManager<ApplicationUser> _userManager;
        private ApplicationDbContext _dbContext;
        private AdministratorService _administratorService;
#pragma warning restore CS8618

        [TestInitialize]
        public void TestInitialize()
        {
            var services = new ServiceCollection();
            services.AddLogging()
                .AddLightNapInMemoryDatabase()
                .AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            var serviceProvider = services.BuildServiceProvider();
            this._dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
            this._userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            this._roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            this._administratorService = new AdministratorService(this._userManager, this._dbContext);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            this._dbContext.Database.EnsureDeleted();
            this._dbContext.Dispose();
        }

        [TestMethod]
        public async Task GetUserAsync_UserExists_ReturnsUser()
        {
            // Arrange
            var userId = "test-user-id";
            await TestHelper.CreateTestUserAsync(this._userManager, userId);

            // Act
            var result = await this._administratorService.GetUserAsync(userId);

            // Assert
            TestHelper.AssertSuccess(result);
            Assert.AreEqual(userId, result.Result!.Id);
        }

        [TestMethod]
        public async Task GetUserAsync_UserDoesNotExist_ReturnsNull()
        {
            // Arrange
            var userId = "non-existent-user-id";

            // Act
            var result = await this._administratorService.GetUserAsync(userId);

            // Assert
            TestHelper.AssertSuccess(result, true);
        }

        [TestMethod]
        public async Task UpdateUserAsync_UserExists_UpdatesUser()
        {
            // Arrange
            var userId = "test-user-id";
            UpdateAdminUserDto updateDto = new();
            await TestHelper.CreateTestUserAsync(this._userManager, userId);

            // Act
            var result = await this._administratorService.UpdateUserAsync(userId, updateDto);

            // Assert
            TestHelper.AssertSuccess(result);
            Assert.AreEqual(userId, result.Result!.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(UserFriendlyApiException))]
        public async Task UpdateUserAsync_UserDoesNotExist_ThrowsError()
        {
            // Arrange
            var userId = "non-existent-user-id";
            var updateDto = new UpdateAdminUserDto();

            // Act
            await this._administratorService.UpdateUserAsync(userId, updateDto);
        }

        [TestMethod]
        public async Task DeleteUserAsync_UserExists_DeletesUser()
        {
            // Arrange
            var userId = "test-user-id";
            await TestHelper.CreateTestUserAsync(this._userManager, userId);

            // Act
            var result = await this._administratorService.DeleteUserAsync(userId);

            // Assert
            TestHelper.AssertSuccess(result);
        }

        [TestMethod]
        [ExpectedException(typeof(UserFriendlyApiException))]
        public async Task DeleteUserAsync_UserDoesNotExist_ThrowsError()
        {
            // Arrange
            var userId = "non-existent-user-id";

            // Act
            await this._administratorService.DeleteUserAsync(userId);
        }

        [TestMethod]
        public async Task AddUserToRoleAsync_UserAndRoleExist_AddsUserToRole()
        {
            // Arrange
            var userId = "test-user-id";
            var role = "test-role";
            await TestHelper.CreateTestUserAsync(this._userManager, userId);
            await TestHelper.CreateTestRoleAsync(this._roleManager, role);

            // Act
            var result = await this._administratorService.AddUserToRoleAsync(role, userId);

            // Assert
            TestHelper.AssertSuccess(result);
        }

        [TestMethod]
        [ExpectedException(typeof(UserFriendlyApiException))]
        public async Task AddUserToRoleAsync_UserDoesNotExist_ThrowsError()
        {
            // Arrange
            var userId = "non-existent-user-id";
            var role = "test-role";

            // Act
            await this._administratorService.AddUserToRoleAsync(role, userId);
        }

        [TestMethod]
        public async Task SearchUsersAsync_ValidRequest_ReturnsPagedResponse()
        {
            // Arrange
            var requestDto = new SearchAdminUsersRequestDto { Email = "example" };
            List<ApplicationUser> users =
            [
                new("testuser1", "test1@example.com", true) { Id = "test-user-id1" },
                new("testuser2", "test2@exNOTample.com", true) { Id = "test-user-id2" },
                new("testuser3", "test3@example.com", true) { Id = "test-user-id3" }
            ];

            await Task.WhenAll(users.Select(user => this._userManager.CreateAsync(user)));

            // Act
            var result = await this._administratorService.SearchUsersAsync(requestDto);

            // Assert
            TestHelper.AssertSuccess(result);
            Assert.AreEqual(2, result.Result!.TotalCount);
        }

        [TestMethod]
        public void GetRoles_ReturnsRoles()
        {
            // Arrange
            var roles = ApplicationRoles.All;

            // Act
            var result = this._administratorService.GetRoles();

            // Assert
            TestHelper.AssertSuccess(result);
            Assert.AreEqual(roles.Count, result.Result!.Count);

            for (int i = 0; i < roles.Count; i++)
            {
                Assert.AreEqual(roles[i].Name, result.Result[i].Name);
                Assert.AreEqual(roles[i].DisplayName, result.Result[i].DisplayName);
                Assert.AreEqual(roles[i].Description, result.Result[i].Description);
            }
        }

        [TestMethod]
        public async Task GetRolesForUserAsync_UserExists_ReturnsRoles()
        {
            // Arrange
            var userId = "test-user-id";
            List<string> roles = ["Admin", "User"];
            var user = await TestHelper.CreateTestUserAsync(this._userManager, userId);

            await TestHelper.CreateTestRoleAsync(this._roleManager, roles[0]);
            await TestHelper.CreateTestRoleAsync(this._roleManager, roles[1]);
            await this._userManager.AddToRolesAsync(user, roles);

            // Act
            var result = await this._administratorService.GetRolesForUserAsync(userId);

            // Assert
            TestHelper.AssertSuccess(result);
            Assert.AreEqual(2, result.Result!.Count);
        }

        [TestMethod]
        public async Task GetUsersInRoleAsync_RoleExists_ReturnsUsers()
        {
            // Arrange
            var role = "test-role";
            await TestHelper.CreateTestRoleAsync(this._roleManager, role);
            var user1 = await TestHelper.CreateTestUserAsync(this._userManager, "test-user-id-1");
            var user2 = await TestHelper.CreateTestUserAsync(this._userManager, "test-user-id-2");
            await this._userManager.AddToRoleAsync(user1, role);
            await this._userManager.AddToRoleAsync(user2, role);

            // Act
            var result = await this._administratorService.GetUsersInRoleAsync(role);

            // Assert
            TestHelper.AssertSuccess(result);
            Assert.AreEqual(2, result.Result!.Count);
        }

        [TestMethod]
        public async Task RemoveUserFromRoleAsync_UserAndRoleExist_RemovesUserFromRole()
        {
            // Arrange
            var userId = "test-user-id";
            var role = "test-role";
            await TestHelper.CreateTestRoleAsync(this._roleManager, role);
            var user = await TestHelper.CreateTestUserAsync(this._userManager, userId);
            await this._userManager.AddToRoleAsync(user, role);

            // Act
            var result = await this._administratorService.RemoveUserFromRoleAsync(role, userId);

            // Assert
            TestHelper.AssertSuccess(result);
            Assert.IsTrue(result.Result);
        }

        [TestMethod]
        public async Task LockUserAsync_UserExists_LocksUser()
        {
            // Arrange
            var userId = "test-user-id";
            await TestHelper.CreateTestUserAsync(this._userManager, userId);

            // Act
            var result = await this._administratorService.LockUserAccountAsync(userId);

            // Assert
            TestHelper.AssertSuccess(result);
            Assert.IsTrue(result.Result);
        }

        [TestMethod]
        [ExpectedException(typeof(UserFriendlyApiException))]
        public async Task LockUserAsync_UserDoesNotExist_ThrowsError()
        {
            // Arrange
            var userId = "non-existent-user-id";

            // Act
            await this._administratorService.LockUserAccountAsync(userId);
        }

        [TestMethod]
        public async Task UnlockUserAsync_UserExists_UnlocksUser()
        {
            // Arrange
            var userId = "test-user-id";
            await TestHelper.CreateTestUserAsync(this._userManager, userId);
            await this._administratorService.LockUserAccountAsync(userId);

            // Act
            var result = await this._administratorService.UnlockUserAccountAsync(userId);

            // Assert
            TestHelper.AssertSuccess(result);
            Assert.IsTrue(result.Result);
        }

        [TestMethod]
        [ExpectedException(typeof(UserFriendlyApiException))]
        public async Task UnlockUserAsync_UserDoesNotExist_ThrowsError()
        {
            // Arrange
            var userId = "non-existent-user-id";

            // Act
            await this._administratorService.UnlockUserAccountAsync(userId);
        }


    }
}
