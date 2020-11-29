namespace Alexandria.Web.ViewModels.Reviews
{
    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;

    public class ReviewsDeleteConfirmedViewModel : IMapFrom<Review>
    {
        public string AuthorId { get; set; }

        public int BookId { get; set; }
    }
}
