using System;
using System.ComponentModel.DataAnnotations;

namespace Honeypot.Attributes
{
    public class NotNullOrWhiteSpaceAttribute : ValidationAttribute
    {
        public NotNullOrWhiteSpaceAttribute()
            : base("Invalid Field")
        {
        }

        public NotNullOrWhiteSpaceAttribute(string message)
            : base(message)
        {
        }

        public override bool IsValid(object value)
        {
            if (string.IsNullOrWhiteSpace(value?.ToString()))
            {
                return false;
            }

            return true;
        }

        protected override ValidationResult IsValid(Object value, ValidationContext validationContext)
        {
            if (IsValid(value))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("Value cannot be empty or white space.");
        }
    }
}
