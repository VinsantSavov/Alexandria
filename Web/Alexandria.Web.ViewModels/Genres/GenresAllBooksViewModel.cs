namespace Alexandria.Web.ViewModels.Genres
{
    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;

    public class GenresAllBooksViewModel : IMapFrom<Genre>
    {
        public string Name { get; set; }

        public IEnumerbale<GenresBookDetailsViewModel> Books { get; set; }
    }
}
