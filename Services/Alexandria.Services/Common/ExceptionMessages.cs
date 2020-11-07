namespace Alexandria.Services.Common
{
    public static class ExceptionMessages
    {
        // Star Ratings service
        public const string ExistingRating = "User has already rated this book";

        // Authors service
        public const string AuthorNotFound = "Author with {0} is not found.";

        // Awards service
        public const string AwardNotFound = "Award with {0} is not found.";

        // Books service
        public const string BookNotFound = "Book with {0} is not found.";

        // Genres service
        public const string GenreNotFound = "Genre with {0} is not found.";

        // Reviews service
        public const string ReviewNotFound = "Review with {0} is not found.";

        // Tags service
        public const string TagNotFound = "Tag with {0} is not found.";
    }
}
