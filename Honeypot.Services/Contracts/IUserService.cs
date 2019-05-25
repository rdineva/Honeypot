using Honeypot.Models;

namespace Honeypot.Services.Contracts
{
    public interface IUserService
    {
        HoneypotUser GetByUsername(string username);
    }
}