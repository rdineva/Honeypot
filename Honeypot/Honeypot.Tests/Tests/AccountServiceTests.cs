using Honeypot.Services.Contracts;
using Honeypot.Tests.Account;
using Honeypot.Tests.Tests;
using Xunit;

namespace Honeypot.Tests
{
    public class AccountServiceTests : BaseTest
    {
        private readonly IAccountService accountService;

        public AccountServiceTests(BaseTestFixture fixture) : base(fixture)
        {
            this.accountService = fixture.Provider.GetService(typeof(IAccountService)) as IAccountService;
            this.SeedData();
        }

        private void SeedData()
        {
            this.DeleteUsersData();
            this.CreateUserData();
        }

        [Fact]
        public void GetByUsername_ShouldReturnUser()
        {
            var user = this.accountService.GetByUsername(TestsConstants.Username);
            Assert.Equal(TestsConstants.Username, user.UserName);
        }
    }
}