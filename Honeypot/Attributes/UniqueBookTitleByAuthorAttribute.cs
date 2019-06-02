using System;
using System.ComponentModel.DataAnnotations;
using Honeypot.Constants;
using Honeypot.Models;
using Honeypot.Services.Contracts;

namespace Honeypot.Attributes
{
    public class UniqueBookTitleByAuthorAttribute : ValidationAttribute
    {
        private IBookService bookService;

        public UniqueBookTitleByAuthorAttribute()
        : base(AttributeConstants.InvalidField)
        {
        }

        public UniqueBookTitleByAuthorAttribute(string message)
        : base(message)
        {
        }

        protected override ValidationResult IsValid(Object value, ValidationContext validationContext)
        {
            this.bookService = (IBookService)validationContext.GetService(typeof(IBookService));
            object instance = validationContext.ObjectInstance;
            Type type = instance.GetType();
            var authorFirstName = type.GetProperty(BookConstants.AuthorFirstName).GetValue(instance).ToString();
            var authorLastName = type.GetProperty(BookConstants.AuthorLastName).GetValue(instance).ToString();
            var title = type.GetProperty(BookConstants.Title).GetValue(instance).ToString();

            if (!this.bookService.BookTitleExists(title, authorFirstName, authorLastName))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(string.Format(GeneralConstants.AlreadyExists, typeof(Book).Name));
        }
    }
}
