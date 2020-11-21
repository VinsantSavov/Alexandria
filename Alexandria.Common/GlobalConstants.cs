﻿namespace Alexandria.Common
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
    }
}
