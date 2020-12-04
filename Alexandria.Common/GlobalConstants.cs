namespace Alexandria.Common
{
    public static class GlobalConstants
    {
        public const string SystemName = "Alexandria";

        public const string AdministratorRoleName = "Administrator";

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

        // Star Rating Input Model constants
        public const int RatingMinValue = 1;
        public const int RatingMaxValue = 5;

        // Review Input Model constants
        public const string ReviewDescriptionDisplayNameConstant = "What did you think?";
        public const string ReviewReadingProgressDisplayNameConstant = "What is your reading progress?";
        public const string ReviewThisEditionDisplayNameConstant = "Is this the edition you read?";
        public const int ReviewDescriptionMinLength = 20;

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
