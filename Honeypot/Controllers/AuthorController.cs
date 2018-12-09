using System.Linq;
using System.Threading;
using AutoMapper;
using Honeypot.Data;
using Honeypot.Models;
using Honeypot.ViewModels;
using Honeypot.ViewModels.Author;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        public IActionResult Details(int id)
        {
            var authorResut = this.context.Authors.FirstOrDefaultAsync(x => x.Id == id).Result;

            if (authorResut == null)
                return this.NotFound("No such author exists.");

            var author = mapper.Map<AuthorDetailsViewModel>(authorResut);

            author.Books = this.context.Books.Where(x => x.AuthorId == id).ToList();
            author.Quotes = this.context.Quotes.Where(x => x.AuthorId == id).ToList();

            return this.View(author);
        }
    }
}