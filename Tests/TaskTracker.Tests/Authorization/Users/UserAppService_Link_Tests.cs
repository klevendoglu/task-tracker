using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Abp.Domain.Uow;
using Abp.Runtime.Security;
using TaskTracker.Authorization.Users;
using TaskTracker.Authorization.Users.Dto;
using TaskTracker.MultiTenancy;
using Shouldly;
using Xunit;

namespace TaskTracker.Tests.Authorization.Users
{
    public class UserAppService_Link_Tests : UserAppServiceTestBase
    {
        private readonly IUserLinkAppService _userLinkAppService;
        private readonly IUserLinkManager _userLinkManager;
        private readonly UserManager _userManager;
        private readonly TenantManager _tenantManager;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public UserAppService_Link_Tests()
        {
            _userLinkAppService = Resolve<IUserLinkAppService>();
            _userLinkManager = Resolve<IUserLinkManager>();
            _userManager = Resolve<UserManager>();
            _tenantManager = Resolve<TenantManager>();
            _unitOfWorkManager = Resolve<IUnitOfWorkManager>();
        }

        [Fact]
        public async Task Should_Link_User_To_Host_Admin()
        {
            LoginAsHostAdmin();
            await LinkUserAndTestAsync(string.Empty);
        }

        [Fact]
        public async Task Should_Link_User_To_Default_Tenant_Admin()
        {
            LoginAsDefaultTenantAdmin();
            await LinkUserAndTestAsync(Tenant.DefaultTenantName);
        }

        [Fact]
        public async Task Should_Link_User_To_Different_Tenant_User()
        {
            //Arrange
            LoginAsHostAdmin();
            await CreateTestTenantAndTestUser();

            //Act
            LoginAsDefaultTenantAdmin();
            await _userLinkAppService.LinkToUser(new LinkToUserInput
            {
                TenancyName = "Test",
                UsernameOrEmailAddress = "test",
                Password = "123qwe"
            });

            //Assert
            await UsingDbContextAsync(async context =>
            {
                var linkedUser = await context.Users.FirstOrDefaultAsync(u => u.UserName == "test");
                var linkedUserAccount = await _userLinkManager.GetUserAccountAsync(linkedUser.ToUserIdentifier());

                var currentUser = await context.Users.FirstOrDefaultAsync(u => u.Id == AbpSession.UserId);
                var currentUserAccount = await _userLinkManager.GetUserAccountAsync(currentUser.ToUserIdentifier());

                currentUserAccount.UserLinkId.ShouldBe(currentUser.Id);
                linkedUserAccount.UserLinkId.ShouldBe(currentUser.Id);
            });
        }

        [Fact]
        public async Task Should_Link_User_To_Already_Linked_User()
        {
            //Arrange
            LoginAsHostAdmin();
            await CreateTestTenantAndTestUser();

            LoginAsDefaultTenantAdmin();
            await CreateTestUsersForAccountLinkingAsync();

            var linkToTestTenantUserInput = new LinkToUserInput
            {
                TenancyName = "Test",
                UsernameOrEmailAddress = "test",
                Password = "123qwe"
            };

            //Act
            //Link Default\admin -> Test\test
            await _userLinkAppService.LinkToUser(linkToTestTenantUserInput);

            LoginAsTenant(Tenant.DefaultTenantName, "jnash");
            //Link Default\jnash->Test\test
            await _userLinkAppService.LinkToUser(linkToTestTenantUserInput);

            //Assert
            await UsingDbContextAsync(async context =>
            {
                var defaultTenantAdmin = await context.Users.FirstOrDefaultAsync(u => u.Id == AbpSession.UserId);
                var defaultTenantAdminAccount = await _userLinkManager.GetUserAccountAsync(defaultTenantAdmin.ToUserIdentifier());

                var jnash = await context.Users.FirstOrDefaultAsync(u => u.UserName == "jnash");
                var jnashAccount = await _userLinkManager.GetUserAccountAsync(jnash.ToUserIdentifier());

                var testTenantUser = await context.Users.FirstOrDefaultAsync(u => u.UserName == "test");
                var testTenantUserAccount = await _userLinkManager.GetUserAccountAsync(testTenantUser.ToUserIdentifier());

                jnashAccount.UserLinkId.ShouldBe(jnash.Id);
                defaultTenantAdminAccount.UserLinkId.ShouldBe(jnash.Id);
                testTenantUserAccount.UserLinkId.ShouldBe(jnash.Id);
            });
        }

        private async Task CreateTestTenantAndTestUser()
        {
            var testTenant = new Tenant("Test", "test")
            {
                ConnectionString = SimpleStringCipher.Instance.Encrypt("Server=localhost; Database=TaskTrackerTest_" + Guid.NewGuid().ToString("N") + "; Trusted_Connection=True;")
            };

            await _tenantManager.CreateAsync(testTenant);

            using (var uow = _unitOfWorkManager.Begin())
            {
                using (_unitOfWorkManager.Current.SetTenantId(testTenant.Id))
                {
                    await _userManager.CreateAsync(new User
                    {
                        EmailAddress = "test@test.com",
                        IsEmailConfirmed = true,
                        Name = "Test",
                        Surname = "User",
                        UserName = "test",
                        Password = "AM4OLBpptxBYmM79lGOX9egzZk3vIQU3d/gFCJzaBjAPXzYIK3tQ2N7X4fcrHtElTw==", //123qwe
                        TenantId = testTenant.Id,
                    });
                }

                await uow.CompleteAsync();
            }
        }

        private async Task LinkUserAndTestAsync(string tenancyName)
        {
            //Arrange
            await CreateTestUsersForAccountLinkingAsync();

            //Act
            await _userLinkAppService.LinkToUser(new LinkToUserInput
            {
                TenancyName = tenancyName,
                UsernameOrEmailAddress = "jnash",
                Password = "123qwe"
            });

            //Assert
            await UsingDbContextAsync(async context =>
            {
                var linkedUser = await context.Users.FirstOrDefaultAsync(u => u.UserName == "jnash");
                var linkedUserAccount = await _userLinkManager.GetUserAccountAsync(linkedUser.ToUserIdentifier());

                var currentUser = await context.Users.FirstOrDefaultAsync(u => u.Id == AbpSession.UserId);
                var currentUserAccount = await _userLinkManager.GetUserAccountAsync(currentUser.ToUserIdentifier());

                linkedUserAccount.UserLinkId.HasValue.ShouldBe(true);
                linkedUserAccount.UserLinkId.Value.ShouldBe(currentUser.Id);

                currentUserAccount.UserLinkId.HasValue.ShouldBe(true);
                currentUserAccount.UserLinkId.Value.ShouldBe(currentUser.Id);
            });
        }

        private async Task CreateTestUsersForAccountLinkingAsync()
        {
            await _userManager.CreateAsync(CreateUserEntity("jnash", "John", "Nash", "jnsh2000@testdomain.com"));
            await _userManager.CreateAsync(CreateUserEntity("adams_d", "Douglas", "Adams", "adams_d@gmail.com"));
            await _userManager.CreateAsync(CreateUserEntity("artdent", "Arthur", "Dent", "ArthurDent@yahoo.com"));
        }
    }
}
