namespace Alexandria.Web.ViewModels.Genres
{
    using System.Collections.Generic;

    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;

    public class GenresDetailsViewModel : IMapFrom<Genre>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public IEnumerable<GenresBookDetailsViewModel> NewReleasedBooks { get; set; }

        public IEnumerable<GenresBookDetailsViewModel> TopRatedBooks { get; set; }
    }
}
