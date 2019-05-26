using System;
using System.Security.Principal;
using System.Threading.Tasks;
using AutoMapper;
using Honeypot.Data;
using Honeypot.Models;
using Honeypot.Services;
using Honeypot.Services.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Honeypot
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            var config = new AutoMapperConfiguration().Configure();
            IMapper iMapper = config.CreateMapper();

            Mapper.Initialize(cfg => { });
            Mapper.Configuration.CompileMappings();

            services.AddDbContext<HoneypotDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<HoneypotUser, IdentityRole>(options =>
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

            //services.AddAutoMapper();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IQuoteService, QuoteService>();
            services.AddScoped<IBookshelfService, BookshelfService>();
            services.AddScoped<IAuthorService, AuthorService>();
            services.AddScoped<IBookService, BookService>();
            services.AddSingleton(iMapper);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider services)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            CreateUserRoles(services).Wait();
        }

        private async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            IdentityResult roleResult;
            var roleAdminExists = await roleManager.RoleExistsAsync(Role.Admin);
            var roleUserExists = await roleManager.RoleExistsAsync(Role.User);

            if (!roleAdminExists)
            {
                roleResult = await roleManager.CreateAsync(new IdentityRole(Role.Admin));
            }

            if (!roleUserExists)
            {
                roleResult = await roleManager.CreateAsync(new IdentityRole(Role.User));
            }
        }
    }
}
