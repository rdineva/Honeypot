using Honeypot.Data;

namespace Honeypot.Services
{
    public class BaseService
    {
        protected readonly HoneypotDbContext Context;

        protected BaseService(HoneypotDbContext context)
        {
            this.Context = context;
        }
    }
}