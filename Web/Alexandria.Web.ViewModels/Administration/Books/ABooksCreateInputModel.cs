namespace Alexandria.Web.ViewModels.Administration.Books
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Alexandria.Common;
    using Alexandria.Web.Infrastructure.Attributes;
    using Microsoft.AspNetCore.Http;

    public class ABooksCreateInputModel
    {
        [Required]
        [StringLength(
            GlobalConstants.BookTitleMaxLength,
            ErrorMessage = ErrorMessages.BookTitleLengthErrorMessage,
            MinimumLength = GlobalConstants.BookTitleMinLength)]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.Upload)]
        [EnsureImageExtensionIsValid(ErrorMessage = ErrorMessages.InvalidExtension)]
        [Display(Name = GlobalConstants.BookPictureUrlDisplayNameConstant)]
        public IFormFile Cover { get; set; }

        [Required]
        [StringLength(
            GlobalConstants.BookSummaryMaxLength,
            ErrorMessage = ErrorMessages.BookSummaryLengthErrorMessage,
            MinimumLength = GlobalConstants.BookSummaryMinLength)]
        public string Summary { get; set; }

        [DataType(DataType.Date)]
        public DateTime PublishedOn { get; set; }

        [Range(
            GlobalConstants.BookMinCountPages,
            GlobalConstants.BookMaxCountPages,
            ErrorMessage = ErrorMessages.BookPagesCountErrorMessage)]
        public int Pages { get; set; }

        [MaxLength(
            GlobalConstants.BookAmazonLinkMaxLength,
            ErrorMessage = ErrorMessages.BookAmazonLinkLengthErrorMessage)]
        public string AmazonLink { get; set; }

        [EnsureGenresIdsExist(ErrorMessage = ErrorMessages.BookInvalidGenresIds)]
        [Display(Name = GlobalConstants.BookGenresDisplayNameConstant)]
        public IEnumerable<int> GenresIds { get; set; }

        [EnsureTagsIdsExist(ErrorMessage = ErrorMessages.BookInvalidTagsIds)]
        [Display(Name = GlobalConstants.BookTagsDisplayNameConstant)]
        public IEnumerable<int> TagsIds { get; set; }

        [EnsureAwardsIdsExist(ErrorMessage = ErrorMessages.BookInvalidAwardsIds)]
        [Display(Name = GlobalConstants.BookAwardsDisplayNameConstant)]
        public IEnumerable<int> AwardsIds { get; set; }

        [EnsureAuthorIdExist(ErrorMessage = ErrorMessages.BookInvalidAuthorId)]
        [Display(Name = GlobalConstants.BookAuthorsDisplayNameConstant)]
        public int AuthorId { get; set; }

        [EnsureEditionLanguageIdExist(ErrorMessage = ErrorMessages.BookInvalidLanguageId)]
        [Display(Name = GlobalConstants.BookLanguagesDisplayNameConstant)]
        public int EditionLanguageId { get; set; }

        public IEnumerable<ABooksAuthorViewModel> Authors { get; set; }

        public IEnumerable<ABooksGenreViewModel> Genres { get; set; }

        public IEnumerable<ABooksTagViewModel> Tags { get; set; }

        public IEnumerable<ABooksAwardViewModel> Awards { get; set; }

        public IEnumerable<ABooksEditionLanguageViewModel> Languages { get; set; }
    }
}
