namespace Alexandria.Web.ViewModels.Authors
{
    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;

    public class AuthorsGenreViewModel : IMapFrom<BookGenre>
    {
        public int GenreId { get; set; }

        public string GenreName { get; set; }
    }
}
