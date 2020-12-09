namespace Alexandria.Web.ViewModels.Administration.Books
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Alexandria.Common;
    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;
    using Alexandria.Web.Infrastructure.Attributes;
    using Microsoft.AspNetCore.Http;

    public class ABooksEditInputModel : IMapFrom<Book>
    {
        [EnsureBookIdExists(ErrorMessage = ErrorMessages.ReviewNotExistingBookIdErrorMessage)]
        public int Id { get; set; }

        [Required]
        [StringLength(
            GlobalConstants.BookTitleMaxLength,
            ErrorMessage = ErrorMessages.BookTitleLengthErrorMessage,
            MinimumLength = GlobalConstants.BookTitleMinLength)]
        public string Title { get; set; }

        public string PictureURL { get; set; }

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

        public ABooksAuthorViewModel Author { get; set; }
    }
}
