using Honeypot.Data;

namespace Honeypot.Services
{
    public abstract class BaseService
    {
        protected readonly HoneypotDbContext context;

        protected BaseService(HoneypotDbContext context)
        {
            this.context = context;
        }
    }
}