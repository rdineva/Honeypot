using System.Linq;
using AutoMapper;
using Honeypot.Data;
using Honeypot.Models;
using Honeypot.ViewModels.Author;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Honeypot.Controllers
{
    [Authorize]
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
            if (!ModelState.IsValid)
            {
                return this.View(viewModel);
            }

            var author = new Author()
            {
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                Biography = viewModel.Biography
            };

            if (this.context.Authors.Any(x => x.FirstName == author.FirstName && x.LastName == author.LastName))
                return this.BadRequest("Author already exists!");

            this.context.Authors.Add(author);
            this.context.SaveChanges();

            return RedirectToAction("Details", "Author", new { id = author.Id });
        }

        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            var authorResut = this.context.Authors.FirstOrDefaultAsync(x => x.Id == id).Result;

            if (authorResut == null)
                return this.NotFound("No such author exists.");

            var author = new AuthorDetailsViewModel()
            {
                FirstName = authorResut.FirstName,
                LastName = authorResut.LastName,
                Biography = authorResut.Biography,
                Books = this.context.Books.Where(x => x.AuthorId == id).ToList(),
                Quotes = this.context.Quotes.Where(x => x.AuthorId == id).ToList()
            };

            return this.View(author);
        }

        public IActionResult Rate()
        {
            //TODO: add rate
            return null;
        }
    }
}