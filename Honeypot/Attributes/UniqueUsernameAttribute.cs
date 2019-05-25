using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using Honeypot.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Honeypot.Attributes
{
    public class UniqueUsernameAttribute : ValidationAttribute
    {
        private HoneypotDbContext context;
        private UserManager<HoneypotDbContext> userManager;

        public UniqueUsernameAttribute()
            : base("Invalid Field")
        {
        }

        public UniqueUsernameAttribute(string errorMessage)
            :base(errorMessage)
        {
        }

        public override bool IsValid(object value)
        {
            var currentUser = this.userManager.GetUserAsync(ClaimsPrincipal.Current).Result;
            if (this.context.Users.Include(x => x.CustomBookshelves).Any(x => x.UserName == value.ToString()))
            {
                return true;
            }

            return false;
        }

        protected override ValidationResult IsValid(Object value, ValidationContext validationContext)
        {
            if (IsValid(value))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("User already has bookshelf with this title.");
        }
    }
}
