using AutoMapper;
using Honeypot.Data;
using Honeypot.Models;
using Honeypot.ViewModels.Quote;
using Microsoft.AspNetCore.Mvc;

namespace Honeypot.Controllers
{
    public class QuoteController : Controller
    {
        private readonly IMapper mapper;
        private readonly HoneypotDbContext context;

        public QuoteController(IMapper mapper, HoneypotDbContext context)
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
            var quote = mapper.Map<Quote>(viewModel);

            this.context.Quotes.Add(quote);
            this.context.SaveChanges();

            return this.RedirectToAction("Details", "Quote", quote.Id);
        }

        public IActionResult Details()
        {
            return this.View();
        }
    }
}