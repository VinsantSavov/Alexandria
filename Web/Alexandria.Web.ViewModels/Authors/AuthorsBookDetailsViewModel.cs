namespace Alexandria.Web.ViewModels.Authors
{
    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;

    public class AuthorsBookDetailsViewModel : IMapFrom<Book>
    {
        public int BookId { get; set; }

        public string BookPictureURL { get; set; }

        public string BookTitle { get; set; }
    }
}
