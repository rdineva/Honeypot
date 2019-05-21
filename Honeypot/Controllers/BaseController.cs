using AutoMapper;
using Honeypot.Data;
using Microsoft.AspNetCore.Mvc;

namespace Honeypot.Controllers
{
    public class BaseController : Controller
    {        
        protected readonly HoneypotDbContext context;
        protected readonly IMapper mapper;

        public BaseController(HoneypotDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
    }
}