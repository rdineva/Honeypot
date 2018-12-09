using AutoMapper;
using Honeypot.Data;
using Honeypot.Models;
using Honeypot.ViewModels.Author;
using Microsoft.AspNetCore.Mvc;

namespace Honeypot.Controllers
{
    public class AuthorController : Controller
    {
        private readonly IMapper mapper;
        private readonly HoneypotDbContext context;

        public AuthorController(IMapper mapper, HoneypotDbContext context)
        {
            this.mapper = mapper;
            this.context = context;
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Create(CreateViewModel viewModel)
        {
            var author = mapper.Map<Author>(viewModel);

            this.context.Authors.Add(author);
            this.context.SaveChanges();

            return RedirectToAction("Details", "Author", author.Id);
        }

        public IActionResult Details()
        {
            return this.View();
        }
    }
}