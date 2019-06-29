using System;
using System.ComponentModel.DataAnnotations;
using Honeypot.Constants;
using Honeypot.Services.Contracts;

namespace Honeypot.Attributes
{
    public class UniqueUsernameAttribute : ValidationAttribute
    {
        private IAccountService accountService;

        public UniqueUsernameAttribute()
            : base(AttributeConstants.UsernameTaken)
        {
        }

        public UniqueUsernameAttribute(string errorMessage)
            : base(errorMessage)
        {
        }

        protected override ValidationResult IsValid(Object value, ValidationContext validationContext)
        {
            this.accountService = (IAccountService)validationContext.GetService(typeof(IAccountService));
            var usernameExists = this.accountService.GetByUsername(value.ToString()) != null;
            if (usernameExists)
            {
                return new ValidationResult(AttributeConstants.UsernameTaken);
            }

            return ValidationResult.Success;
        }
    }
}
