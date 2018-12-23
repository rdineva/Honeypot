using Microsoft.AspNetCore.Mvc;

namespace Honeypot.Controllers
{
    public class ReviewController : Controller
    {
        public IActionResult Details()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        //public IActionResult Details()
        //{
        //    return View();
        //}

        //public IActionResult Create()
        //{
        //    return View();
        //}
    }
}