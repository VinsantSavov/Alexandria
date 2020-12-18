namespace Alexandria.Web.ViewModels.Likes
{
    using System.ComponentModel.DataAnnotations;

    using Alexandria.Common;
    using Alexandria.Web.Infrastructure.Attributes;

    public class LikesCreateInputModel
    {
        public bool IsLiked { get; set; }

        [Required]
        [EnsureReviewIdExists(ErrorMessage = ErrorMessages.ReviewNotExistingReviewIdErrorMessage)]
        public int ReviewId { get; set; }
    }
}
