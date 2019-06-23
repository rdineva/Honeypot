using AutoMapper;
using Honeypot.Data;
using Honeypot.Models;
using Honeypot.Services.Contracts;
using Honeypot.ViewModels.Author;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Honeypot.Controllers
{
    [Authorize]
    public class AuthorController : BaseController
    {
        private readonly IAuthorService authorService;

        public AuthorController(HoneypotDbContext context, IMapper mapper, IAuthorService authorService)
            : base(context, mapper)
        {
            this.authorService = authorService;
        }

        [Authorize(Roles = Role.Admin)]
        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        [Authorize(Roles = Role.Admin)]
        public IActionResult Create(CreateAuthorViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var createdAuthor = OnPostCreateAuthor(viewModel);
                return RedirectToAction("Details", "Author", new { id = createdAuthor.Id });
            }

            return this.View(viewModel);
        }

        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            var authorResult = this.authorService.GeAuthorById(id);
            if (authorResult == null)
            {
                return this.RedirectToAction("/", "Home");
            }

            var author = this.mapper.Map<AuthorDetailsViewModel>(authorResult);
            return this.View(author);
        }

        private Author OnPostCreateAuthor(CreateAuthorViewModel viewModel)
        {
            var author = this.mapper.Map<Author>(viewModel);
            this.context.Authors.Add(author);
            this.context.SaveChanges();
            return author;
        }
    }
}