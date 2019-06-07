using System;
using System.ComponentModel.DataAnnotations;
using Honeypot.Constants;
using Honeypot.Constants.ModelPropertyConstants;
using Honeypot.Models;
using Honeypot.Services.Contracts;

namespace Honeypot.Attributes
{
    public class AuthorNamesExistsAttribute : ValidationAttribute
    {
        private IAuthorService authorService;

        public bool ShouldAuthorExist { get; set; }

        public AuthorNamesExistsAttribute()
            : base(AttributeConstants.InvalidField)
        {
        }

        public AuthorNamesExistsAttribute(string message)
            : base(message)
        {
        }

        protected override ValidationResult IsValid(Object value, ValidationContext validationContext)
        {
            this.authorService = (IAuthorService)validationContext.GetService(typeof(IAuthorService));
            object instance = validationContext.ObjectInstance;
            Type type = instance.GetType();
            
            if (this.ShouldAuthorExist)
            {
                return AuthorShouldExist(type, instance);
            }
            else
            {
                return AuthorShouldNotExist(type, instance);
            }
        }

        public ValidationResult AuthorShouldExist(Type type, object instance)
        {
            var firstName = type.GetProperty(BookConstants.AuthorFirstName).GetValue(instance)?.ToString();
            var lastName = type.GetProperty(BookConstants.AuthorLastName).GetValue(instance)?.ToString();

            if (this.authorService.AuthorExists(firstName, lastName))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(string.Format(GeneralConstants.DoesntExist, typeof(Author).Name));
        }

        public ValidationResult AuthorShouldNotExist(Type type, object instance)
        {
            var firstName = type.GetProperty(AuthorConstants.FirstName).GetValue(instance)?.ToString();
            var lastName = type.GetProperty(AuthorConstants.LastName).GetValue(instance)?.ToString();

            if (!this.authorService.AuthorExists(firstName, lastName))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(string.Format(GeneralConstants.AlreadyExists, typeof(Author).Name));
        }
    }
}
