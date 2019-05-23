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
    public class AuthorController : BaseController
    {
        public AuthorController(HoneypotDbContext context, IMapper mapper)
            : base(context, mapper)
        {
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(CreateAuthorViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var authorExists = this.context.Authors
                    .Any(x => x.FirstName == viewModel.FirstName && x.LastName == viewModel.LastName);
                if (authorExists)
                {
                    return this.BadRequest("Author already exists!");
                }
                else
                {
                    var createdAuthor = OnPostCreateAuthor(viewModel);
                    return RedirectToAction("Details", "Author", new { id = createdAuthor.Id });
                }
            }

            return this.View(viewModel);
        }

        public Author OnPostCreateAuthor(CreateAuthorViewModel viewModel)
        {
            var author = this.mapper.Map<Author>(viewModel);
            this.context.Authors.Add(author);
            this.context.SaveChanges();
            return author;
        }

        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            var authorResult = this.context.Authors
                .Include(x => x.Books)
                .ThenInclude(x => x.Quotes)
                .FirstOrDefault(x => x.Id == id);

            if (authorResult == null)
            {
                return this.NotFound("No such author exists.");
            }

            var author = this.mapper.Map<AuthorDetailsViewModel>(authorResult);
            return this.View(author);
        }
    }
}