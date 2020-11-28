namespace Alexandria.Web.ViewModels.Reviews
{
    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;

    public class ReviewsAuthorViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string ProfilePicture { get; set; }
    }
}
