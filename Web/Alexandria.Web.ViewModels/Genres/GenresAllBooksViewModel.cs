namespace Alexandria.Web.ViewModels.Genres
{
    using System.Collections.Generic;

    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;

    public class GenresAllBooksViewModel : IMapFrom<Genre>
    {
        public string Name { get; set; }

        public IEnumerable<GenresBookDetailsViewModel> Books { get; set; }
    }
}
