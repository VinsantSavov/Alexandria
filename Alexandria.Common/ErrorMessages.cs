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
        public const string UserInvalidUser = "Invalid user!";

        // Create Genre
        public const string GenreNameLengthErrorMessage = "Genre name must be between {2} and {1} characters long!";
        public const string GenreDescriptionErrorMessage = "Genre description must be between {2} and {1} characters long!";
        public const string GenreNotExistingId = "Invalid genre!";

        // Create Review
        public const string ReviewDescriptionLengthErrorMessage = "Review content must be between {2} and {1} characters long!";
        public const string ReviewRequiredDescriptionErrorMessage = "Review content is required!";
        public const string ReviewNotExistingBookIdErrorMessage = "Invalid book!";
        public const string ReviewNotExistingReviewIdErrorMessage = "Invalid review!";

        // Create Book
        public const string BookTitleLengthErrorMessage = "Book title must be between {2} and {1} characters long!";
        public const string BookSummaryLengthErrorMessage = "Book summary must be between {2} and {1} characters long!";
        public const string BookPagesCountErrorMessage = "Book pages must be between {2} and {1} characters long!";
        public const string BookAmazonLinkLengthErrorMessage = "Book amazon link can not be more than {1} characters long!";
        public const string BookInvalidGenresIds = "Invalid genres!";
        public const string BookInvalidTagsIds = "Invalid tags!";
        public const string BookInvalidAwardsIds = "Invalid awards!";
        public const string BookInvalidAuthorId = "Invalid author!";
        public const string BookInvalidLanguageId = "Invalid edition language!";

        // Create Tag
        public const string TagNameLengthErrorMessage = "Tag name must be between {2} and {1} characters long!";

        // Create Message
        public const string MessageContentLengthErrorMessage = "Message can not be more than {1} characters long!";

        // Extensions
        public const string InvalidExtension = "File is not valid";
    }
}
