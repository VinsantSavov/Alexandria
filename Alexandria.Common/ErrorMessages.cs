namespace Alexandria.Common
{
    public static class ErrorMessages
    {
        // User
        public const string UserUsernameLengthErrorMessage = "The {0} must be at least {2} and at max {1} characters long.";
        public const string UserPasswordLengthErrorMessage = "The {0} must be at least {2} and at max {1} characters long.";
        public const string UserPasswordsNotMatching = "The password and confirmation password do not match.";

        // Create Review
        public const string ReviewDescriptionLengthErrorMessage = "Review content must be between {2} and {1} characters long!";
        public const string ReviewRequiredDescriptionErrorMessage = "Review content is required!";
        public const string ReviewNotExistingBookIdErrorMessage = "Invalid book!";
    }
}
