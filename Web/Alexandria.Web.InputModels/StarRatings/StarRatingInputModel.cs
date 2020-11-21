namespace Alexandria.Web.InputModels.StarRatings
{
    using System.ComponentModel.DataAnnotations;

    using Alexandria.Common;

    public class StarRatingInputModel
    {
        [Range(GlobalConstants.RatingMinValue, GlobalConstants.RatingMaxValue)]
        public int Rate { get; set; }

        public int BookId { get; set; }
    }
}
