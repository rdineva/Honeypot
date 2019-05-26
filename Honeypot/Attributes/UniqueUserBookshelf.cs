﻿using System;
using System.ComponentModel.DataAnnotations;
using Honeypot.Services.Contracts;
using Microsoft.AspNetCore.Http;

namespace Honeypot.Attributes
{
    public class UniqueUserBookshelf : ValidationAttribute
    {
        private IBookshelfService bookshelfService;
        private IUserService userService;
        private IHttpContextAccessor httpContextAccessor;

        public UniqueUserBookshelf()
            : base("Invalid Field")
        {
        }

        public UniqueUserBookshelf(string errorMessage)
            : base(errorMessage)
        {
        }

        protected override ValidationResult IsValid(Object value, ValidationContext validationContext)
        {
            InitializeServices(validationContext);

            var user = this.userService.GetByUsername(httpContextAccessor.HttpContext.User.Identity.Name);
            var bookshelfTitleExists = this.bookshelfService.UserHasBookshelfTitled(value.ToString(), user.Id);
            if (bookshelfTitleExists)
            {
                return new ValidationResult("This user already has a bookshelf with this title.");
            }

            return ValidationResult.Success;
        }

        public void InitializeServices(ValidationContext validationContext)
        {
            this.bookshelfService = (IBookshelfService)validationContext.GetService(typeof(IBookshelfService));
            this.userService = (IUserService)validationContext.GetService(typeof(IUserService));
            this.httpContextAccessor = (IHttpContextAccessor)validationContext.GetService(typeof(IHttpContextAccessor));
        }
    }
}
