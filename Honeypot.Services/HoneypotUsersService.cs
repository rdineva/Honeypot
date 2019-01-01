using System.Linq;
using Honeypot.Data;
using Honeypot.Models;

namespace Honeypot.Services
{
    public class HoneypotUsersService : BaseService
    {
        public HoneypotUsersService(HoneypotDbContext context) : base(context) { }

        public HoneypotUser GetByUsername(string username) =>
            this.context.Users.FirstOrDefault(x => x.UserName == username);
    }
}
