using Honeypot.Data;
using Microsoft.AspNetCore.Mvc;

namespace Honeypot.Controllers
{
    public class BaseController : Controller
    {        
        private readonly HoneypotDbContext context;

        public BaseController(HoneypotDbContext context)
        {
            this.context = context;
        }
    }
}