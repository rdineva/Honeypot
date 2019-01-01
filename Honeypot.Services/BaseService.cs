using Honeypot.Data;

namespace Honeypot.Services
{
    public class BaseService
    {
        protected readonly HoneypotDbContext context;

        public BaseService(HoneypotDbContext context)
        {
            this.context = context;
        }
    }
}
