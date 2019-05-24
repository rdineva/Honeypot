using System.Linq;
using Honeypot.Data;
using Honeypot.Models;

namespace Honeypot.Services
{
    public class UserService : BaseService
    {
        public UserService(HoneypotDbContext context)
            : base(context)
        {
        }

        public HoneypotUser GetByUsername(string username)
        {
            HoneypotUser user = this.Context.Users.FirstOrDefault(x => x.UserName == username);
            return user;
        }
    }
}