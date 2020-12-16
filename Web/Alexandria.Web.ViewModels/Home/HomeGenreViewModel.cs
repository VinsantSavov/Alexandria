namespace Alexandria.Web.ViewModels.Home
{
    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;

    public class HomeGenreViewModel : IMapFrom<BookGenre>
    {
        public int GenreId { get; set; }

        public string GenreName { get; set; }
    }
}
