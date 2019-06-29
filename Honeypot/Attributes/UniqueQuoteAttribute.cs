using System;
using System.ComponentModel.DataAnnotations;
using Honeypot.Constants;
using Honeypot.Models;
using Honeypot.Services.Contracts;

namespace Honeypot.Attributes
{
    public class UniqueQuoteAttribute : ValidationAttribute
    {
        private IQuoteService quoteService;

        public UniqueQuoteAttribute()
            : base(AttributeConstants.InvalidField)
        {
        }

        public UniqueQuoteAttribute(string message)
            : base(message)
        {
        }

        protected override ValidationResult IsValid(Object value, ValidationContext validationContext)
        {
            this.quoteService = (IQuoteService)validationContext.GetService(typeof(IQuoteService));
            if (this.quoteService.QuoteExists(value?.ToString()))
            {
                return new ValidationResult(string.Format(GeneralConstants.AlreadyExists, typeof(Quote).Name));
            }

            return ValidationResult.Success;
        }
    }
}
