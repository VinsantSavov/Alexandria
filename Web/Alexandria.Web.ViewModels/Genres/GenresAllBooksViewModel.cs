namespace Alexandria.Web.ViewModels.Genres
{
    using System.Collections.Generic;

    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;

    public class GenresAllBooksViewModel : IMapFrom<Genre>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<GenresBookDetailsViewModel> AllBooks { get; set; }

        public int PagesCount { get; set; }

        public int CurrentPage { get; set; }
    }
}
