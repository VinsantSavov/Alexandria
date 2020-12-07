namespace Alexandria.Common
{
    public static class ErrorMessages
    {
        // User
        public const string UserUsernameLengthErrorMessage = "The {0} must be at least {2} and at max {1} characters long.";
        public const string UserPasswordLengthErrorMessage = "The {0} must be at least {2} and at max {1} characters long.";
        public const string UserPasswordsNotMatching = "The password and confirmation password do not match.";
        public const string UserInvalidBiography = "Your biography can not be more than {2} characters!";
        public const string UserInvalidGender = "Invalid input!";

        // Create Genre
        public const string GenreNameLengthErrorMessage = "Genre name must be between {2} and {1} characters long!";
        public const string GenreDescriptionErrorMessage = "Genre description must be between {2} and {1} characters long!";
        public const string GenreNotExistingId = "Invalid genre!";

        // Create Review
        public const string ReviewDescriptionLengthErrorMessage = "Review content must be between {2} and {1} characters long!";
        public const string ReviewRequiredDescriptionErrorMessage = "Review content is required!";
        public const string ReviewNotExistingBookIdErrorMessage = "Invalid book!";
        public const string ReviewNotExistingReviewIdErrorMessage = "Invalid review!";

        // Extensions
        public const string InvalidExtension = "File is not valid";
    }
}
