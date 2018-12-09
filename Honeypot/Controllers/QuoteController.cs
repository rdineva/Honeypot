using Microsoft.AspNetCore.Mvc;

namespace Honeypot.Controllers
{
    public class QuoteController : Controller
    {
        public IActionResult Create()
        {
            return this.View();
        }
    }
}