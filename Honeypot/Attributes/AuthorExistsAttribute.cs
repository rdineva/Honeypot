using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Honeypot.Constants;
using Honeypot.Models;
using Honeypot.Services.Contracts;

namespace Honeypot.Attributes
{
    public class AuthorExistsAttribute : ValidationAttribute
    {
        private IAuthorService authorService;

        public AuthorExistsAttribute()
            : base(AttributeConstants.InvalidField)
        {
        }

        public AuthorExistsAttribute(string message)
            : base(message)
        {
        }

        protected override ValidationResult IsValid(Object value, ValidationContext validationContext)
        {
            this.authorService = (IAuthorService)validationContext.GetService(typeof(IAuthorService));
            var isAuthorIdValid = int.TryParse(value.ToString(), NumberStyles.Integer, new NumberFormatInfo(), out int authorId);

            if (isAuthorIdValid && this.authorService.GeAuthorById(authorId) != null)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(string.Format(GeneralConstants.DoesntExist, typeof(Author).Name));
        }
    }
}
