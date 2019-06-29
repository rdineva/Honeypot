using System;
using AutoMapper;
using Honeypot.Data;
using Honeypot.Models;
using Honeypot.Services;
using Honeypot.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Honeypot.Tests
{
    public class BaseTest
    {
        private readonly IMapper mapper;

        private readonly DbContextOptionsBuilder<HoneypotDbContext> options;

        public IServiceCollection ServiceCollection { get; private set; }

        public IServiceProvider Provider { get; private set; }

        public BaseTest()
        {
            this.options = new DbContextOptionsBuilder<HoneypotDbContext>()
                .UseInMemoryDatabase(databaseName: "HoneypotInMemoryDb");
            this.mapper = new AutoMapperConfiguration().Configure().CreateMapper();
            this.ServiceCollection = new ServiceCollection();
            this.ConfigureServices();
            this.Provider = ServiceCollection.BuildServiceProvider();
        }

        private void ConfigureServices()
        {
            AddIdentityConfiguration();

            this.ServiceCollection
                .AddDbContext<HoneypotDbContext>(options => 
                    options.UseInMemoryDatabase(databaseName: "HoneypotInMemoryDb"));

            AddServices();
        }

        private void AddIdentityConfiguration()
        {
            this.ServiceCollection.AddIdentity<HoneypotUser, IdentityRole>(options =>
                {
                    options.SignIn.RequireConfirmedEmail = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredUniqueChars = 0;
                    options.Password.RequiredLength = 4;
                })
                .AddEntityFrameworkStores<HoneypotDbContext>()
                .AddDefaultTokenProviders();
        }

        private void AddServices()
        {
            this.ServiceCollection.AddScoped<IAccountService, AccountService>();
            this.ServiceCollection.AddScoped<IQuoteService, QuoteService>();
            this.ServiceCollection.AddScoped<IBookshelfService, BookshelfService>();
            this.ServiceCollection.AddScoped<IAuthorService, AuthorService>();
            this.ServiceCollection.AddScoped<IBookService, BookService>();
            this.ServiceCollection.AddScoped<IRatingService, RatingService>();
            this.ServiceCollection.AddSingleton(this.mapper);
        }
    }
}