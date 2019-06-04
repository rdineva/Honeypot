using System.Linq;
using Honeypot.Data;
using Honeypot.Models;
using Honeypot.Services.Contracts;

namespace Honeypot.Services
{
    public class UserService : BaseService, IUserService
    {
        public UserService(HoneypotDbContext context)
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