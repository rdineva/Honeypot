using Honeypot.Models;

namespace Honeypot.Services.Contracts
{
    public interface IAccountService
    {
        HoneypotUser GetByUsername(string username);
    }
}