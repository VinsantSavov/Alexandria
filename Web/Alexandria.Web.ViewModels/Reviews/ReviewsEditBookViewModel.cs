namespace Alexandria.Web.ViewModels.Reviews
{
    using System.ComponentModel.DataAnnotations;

    using Alexandria.Common;
    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;
    using Alexandria.Web.Infrastructure.Attributes;

    public class ReviewsEditBookViewModel : IMapFrom<Book>
    {
        [Required]
        [EnsureBookIdExists(ErrorMessage = ErrorMessages.ReviewNotExistingBookIdErrorMessage)]
        public int Id { get; set; }

        public string Title { get; set; }

        public string PictureURL { get; set; }

        public ReviewsBookAuthorViewModel Author { get; set; }
    }
}
