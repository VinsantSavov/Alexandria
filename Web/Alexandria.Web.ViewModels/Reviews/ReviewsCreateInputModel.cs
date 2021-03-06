﻿namespace Alexandria.Web.ViewModels.Reviews
{
    using System.ComponentModel.DataAnnotations;

    using Alexandria.Common;
    using Alexandria.Data.Models;
    using Alexandria.Data.Models.Enums;
    using Alexandria.Services.Mapping;
    using Alexandria.Web.Infrastructure.Attributes;

    public class ReviewsCreateInputModel : IMapFrom<Book>
    {
        [Required]
        [EnsureBookIdExists(ErrorMessage = ErrorMessages.ReviewNotExistingBookIdErrorMessage)]
        public int Id { get; set; }

        [EnsureReviewIdExists(ErrorMessage = ErrorMessages.ReviewNotExistingReviewIdErrorMessage)]
        public int? ReviewId { get; set; }

        public string Title { get; set; }

        public string PictureURL { get; set; }

        public ReviewsBookAuthorViewModel Author { get; set; }

        [Display(Name = GlobalConstants.ReviewDescriptionDisplayNameConstant)]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = ErrorMessages.ReviewRequiredDescriptionErrorMessage)]
        [StringLength(
            GlobalConstants.ReviewDescriptionMaxLength,
            ErrorMessage = ErrorMessages.ReviewDescriptionLengthErrorMessage,
            MinimumLength = GlobalConstants.ReviewDescriptionMinLength)]
        public string Description { get; set; }

        [Required]
        [Display(Name = GlobalConstants.ReviewReadingProgressDisplayNameConstant)]
        [EnumDataType(typeof(ReadingProgress))]
        public ReadingProgress ReadingProgress { get; set; }

        [Required]
        [Display(Name = GlobalConstants.ReviewThisEditionDisplayNameConstant)]
        public bool ThisEdition { get; set; }
    }
}
