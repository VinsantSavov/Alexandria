namespace Alexandria.Common
{
    public static class GlobalConstants
    {
        public const string SystemName = "Alexandria";
        public const string SystemEmail = "alexandriasupport@gmail.com";

        public const string AdministrationArea = "Administration";
        public const string AdministratorRoleName = "Administrator";
        public const string AdministratorUsername = "Admin";
        public const string AdministratorEmail = "admin@gmail.com";
        public const string AdministratorPassword = "123456789";
        public const string AdministratorProfilePicture = "https://res.cloudinary.com/alexandrialib/image/upload/v1607190349/user-icon_126283-700_pfxixv.jpg";

        // Application user constants
        public const int UserProfilePictureLength = 500;
        public const int UserBiographyMaxLength = 1500;

        // Author constants
        public const int AuthorNamesMaxLength = 20;
        public const int AuthorBiographyMaxLength = 1500;
        public const int AuthorProfilePictureLength = 500;

        // Award constants
        public const int AwardNameMaxLength = 70;

        // Book constants
        public const int BookTitleMaxLength = 100;
        public const int BookSummaryMaxLength = 2000;
        public const int BookPictureUrlMaxLength = 500;
        public const int BookAmazonLinkMaxLength = 500;

        // Country constants
        public const int CountryNameMaxLength = 50;

        // EditionLanguage constants
        public const int EditionLanguageLength = 15;

        // Genre constants
        public const int GenreNameMaxLength = 30;
        public const int GenreDescriptionMaxLength = 1000;

        // Review constants
        public const int ReviewDescriptionMaxLength = 50000;

        // Tag constants
        public const int TagNameMaxLength = 20;

        // Genre Input Model constants
        public const int GenreNameMinLength = 3;
        public const int GenreDescriptionMinLength = 10;

        // Star Rating Input Model constants
        public const int RatingMinValue = 1;
        public const int RatingMaxValue = 5;

        // Review Input Model constants
        public const string ReviewDescriptionDisplayNameConstant = "What did you think?";
        public const string ReviewReadingProgressDisplayNameConstant = "What is your reading progress?";
        public const string ReviewThisEditionDisplayNameConstant = "Is this the edition you read?";
        public const int ReviewDescriptionMinLength = 20;

        // Book Input Model constants
        public const int BookTitleMinLength = 2;
        public const int BookSummaryMinLength = 20;
        public const int BookMinCountPages = 5;
        public const int BookMaxCountPages = 2000;
        public const string BookPictureUrlDisplayNameConstant = "Book Cover";
        public const string BookGenresDisplayNameConstant = "Genres";
        public const string BookTagsDisplayNameConstant = "Tags";
        public const string BookAwardsDisplayNameConstant = "Awards";
        public const string BookAuthorsDisplayNameConstant = "Author";
        public const string BookLanguagesDisplayNameConstant = "Edition Language";

        // User Register costants
        public const string UserEmailDisplayName = "Email";
        public const string UserPasswordDisplayName = "Password";
        public const string UserConfirmPasswordDisplayName = "Confirm password";
        public const int UserUsernameMaxLength = 25;
        public const int UserUsernameMinLength = 4;
        public const int UserPasswordMaxLength = 100;
        public const int UserPasswordMinLength = 6;

        // User Manage constants
        public const string PhoneNumberDisplayName = "Phone number";
        public const string ProfilePictureDisplayName = "Profile picture";

        // User Login constants
        public const string UserLoginRememberMe = "Remember me?";

        // Extensions
        public const string AllowableExtensions = "jpg,jpeg,png";

        // Redirects
        public const string RedirectToBooksDetails = "/Books/Details/{0}";
    }
}
