using System;
using System.ComponentModel.DataAnnotations;
using Honeypot.Constants;
using Honeypot.Services.Contracts;
using Microsoft.AspNetCore.Http;

namespace Honeypot.Attributes
{
    public class UniqueUserBookshelfAttribute : ValidationAttribute
    {
        private IBookshelfService bookshelfService;
        private IAccountService accountService;
        private IHttpContextAccessor httpContextAccessor;

        public UniqueUserBookshelfAttribute()
            : base(AttributeConstants.UsernameTaken)
        {
        }

        public UniqueUserBookshelfAttribute(string errorMessage)
            : base(errorMessage)
        {
        }

        protected override ValidationResult IsValid(Object value, ValidationContext validationContext)
        {
            InitializeServices(validationContext);
            var user = this.accountService.GetByUsername(httpContextAccessor.HttpContext.User.Identity.Name);
            var bookshelfTitleExists = this.bookshelfService.UserHasBookshelfTitled(value?.ToString(), user.Id);
            if (bookshelfTitleExists)
            {
                return new ValidationResult(AttributeConstants.BookshelfNameExists);
            }

            return ValidationResult.Success;
        }

        public void InitializeServices(ValidationContext validationContext)
        {
            this.bookshelfService = (IBookshelfService)validationContext.GetService(typeof(IBookshelfService));
            this.accountService = (IAccountService)validationContext.GetService(typeof(IAccountService));
            this.httpContextAccessor = (IHttpContextAccessor)validationContext.GetService(typeof(IHttpContextAccessor));
        }
    }
}