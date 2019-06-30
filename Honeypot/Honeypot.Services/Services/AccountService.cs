using System.Linq;
using Honeypot.Data;
using Honeypot.Models;
using Honeypot.Services.Contracts;

namespace Honeypot.Services
{
    public class AccountService : BaseService, IAccountService
    {
        public AccountService(HoneypotDbContext context)
            : base(context)
        {
        }

        public HoneypotUser GetByUsername(string username)
        {
            HoneypotUser user = this.context
                .Users
                .FirstOrDefault(x => x.UserName == username);

            return user;
        }
    }
}