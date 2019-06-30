using System.Linq;
using Honeypot.Data;
using Honeypot.Models;
using Honeypot.Services.Contracts;
using Honeypot.Tests.Account;
using Xunit;

namespace Honeypot.Tests
{
    public class AccountServiceTests : IClassFixture<BaseTest>
    {
        private readonly IAccountService accountService;

        private readonly HoneypotDbContext context;

        public AccountServiceTests(BaseTest fixture)
        {
            this.accountService = fixture.Provider.GetService(typeof(IAccountService)) as IAccountService;
            this.context = fixture.Provider.GetService(typeof(HoneypotDbContext)) as HoneypotDbContext;
            this.SeedData();
        }

        private void SeedData()
        {
            this.DeleteUsersData();
            var user = new HoneypotUser()
            {
                UserName = TestsConstants.Username
            };

            this.context.Users.Add(user);
            this.context.SaveChanges();
        }

        private void DeleteUsersData()
        {
            var users = this.context.Users.ToList();
            this.context.Users.RemoveRange(users);
            this.context.SaveChanges();
        }

        [Fact]
        public void GetByUsername_ShouldReturnUser()
        {
            var user = this.accountService.GetByUsername(TestsConstants.Username);
            Assert.Equal(TestsConstants.Username, user.UserName);
        }
    }
}