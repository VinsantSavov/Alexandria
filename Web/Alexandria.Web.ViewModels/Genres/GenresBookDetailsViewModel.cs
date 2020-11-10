namespace Alexandria.Web.ViewModels.Genres
{
    using System.Collections.Generic;

    public class GenresBookDetailsViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public IEnumerable<string> Tags { get; set; }
    }
}
