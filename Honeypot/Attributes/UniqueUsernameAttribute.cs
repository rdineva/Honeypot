using System;
using System.ComponentModel.DataAnnotations;
using Honeypot.Services.Contracts;

namespace Honeypot.Attributes
{
    public class UniqueUsernameAttribute : ValidationAttribute
    {
        private IUserService userService;

        public UniqueUsernameAttribute()
            : base("Invalid Field")
        {
        }

        public UniqueUsernameAttribute(string errorMessage)
            : base(errorMessage)
        {
        }

        protected override ValidationResult IsValid(Object value, ValidationContext validationContext)
        {
            this.userService = (IUserService)validationContext.GetService(typeof(IUserService));
            var usernameExists = this.userService.GetByUsername(value.ToString()) != null;

            if (usernameExists)
            {
                return new ValidationResult("This username is already taken.");
            }

            return ValidationResult.Success;
        }
    }
}
