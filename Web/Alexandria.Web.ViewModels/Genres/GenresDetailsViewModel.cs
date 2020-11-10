namespace Alexandria.Web.ViewModels.Genres
{
    using System.Collections.Generic;

    public class GenresDetailsViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public IEnumerable<GenresBookDetailsViewModel> NewReleasedBooks { get; set; }

        public IEnumerable<GenresBookDetailsViewModel> TopRatedBooks { get; set; }
    }
}
