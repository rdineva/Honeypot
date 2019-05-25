namespace Honeypot.ViewModels
{
    public class ErrorConstants
    {
        public const int MinNameLength = 5;

        public const int MaxNameLength = 25;

        public const int MinPasswordLength = 6;

        public const int MaxPasswordLength = 60;

        public const int MinBiographyLength = 15;

        public const int MaxBiographyLength = 500;

        public const int MinTitleLength = 3;

        public const int MaxTitleLength = 35;

        public const int MinSummaryLength = 10;

        public const int MaxSummaryLength = 400;

        public const int MinQuoteTextLength = 5;

        public const int MaxQuoteTextLength = 1000;

        public const string StringLengthError = "{0} must be between {1} and {2} characters long.";

        public const string PasswordsDontMatch = "Passwords don't match.";
    }
}
