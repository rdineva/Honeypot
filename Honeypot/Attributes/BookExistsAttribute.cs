using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Honeypot.Constants;
using Honeypot.Models;
using Honeypot.Services.Contracts;

namespace Honeypot.Attributes
{
    public class BookExistsAttribute : ValidationAttribute
    {
        private IBookService bookService;

        public BookExistsAttribute()
            : base(AttributeConstants.InvalidField)
        {
        }

        public BookExistsAttribute(string message)
            : base(message)
        {
        }

        protected override ValidationResult IsValid(Object value, ValidationContext validationContext)
        {
            this.bookService = (IBookService)validationContext.GetService(typeof(IBookService));
            var isBookIdValid = int.TryParse(value.ToString(), NumberStyles.Integer, new NumberFormatInfo(), out int bookId);

            if (isBookIdValid && this.bookService.GeBookById(bookId) != null)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(string.Format(GeneralConstants.DoesntExist, typeof(Book).Name));
        }
    }
}
