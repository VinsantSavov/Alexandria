namespace Alexandria.Web.ViewModels.StarRatings
{
    using System.ComponentModel.DataAnnotations;

    using Alexandria.Common;
    using Alexandria.Web.Infrastructure.Attributes;

    public class StarRatingInputModel
    {
        [Range(GlobalConstants.RatingMinValue, GlobalConstants.RatingMaxValue)]
        public int Rate { get; set; }

        [EnsureBookIdExists(ErrorMessage = ErrorMessages.ReviewNotExistingBookIdErrorMessage)]
        public int BookId { get; set; }
    }
}
