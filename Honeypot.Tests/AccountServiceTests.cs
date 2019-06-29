using System.Linq;
using AutoMapper;
using Honeypot.Data;
using Honeypot.Models;
using Honeypot.Services.Contracts;
using Honeypot.Tests.Account;
using Xunit;

namespace Honeypot.Tests
{
    public class AccountServiceTests : IClassFixture<BaseTest>
    {
        private readonly IMapper mapper;

        private readonly IAccountService accountService;

        private readonly HoneypotDbContext context;

        public AccountServiceTests(BaseTest fixture)
        {
            this.mapper = fixture.Provider.GetService(typeof(IMapper)) as IMapper;
            this.accountService = fixture.Provider.GetService(typeof(IAccountService)) as IAccountService;
            this.context = fixture.Provider.GetService(typeof(HoneypotDbContext)) as HoneypotDbContext;
            this.SeedData();
        }

        private void SeedData()
        {
            this.DeleteUsersData();
            var user = new HoneypotUser()
            {
                UserName = TestConstants.Username
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
            var user = this.accountService.GetByUsername(TestConstants.Username);
            Assert.Equal(user.UserName, TestConstants.Username);
        }
    }
}