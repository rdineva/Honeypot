using System.Linq;
using Honeypot.Data;
using Honeypot.Models;

namespace Honeypot.Services
{
    public class HoneypotUsersService
    {
        private readonly HoneypotDbContext context;

        public HoneypotUsersService(HoneypotDbContext context)
        {
            this.context = context;
        }

        public HoneypotUser GetByUsername(string username) =>
            this.context.Users.FirstOrDefault(x => x.UserName == username);
    }
}
