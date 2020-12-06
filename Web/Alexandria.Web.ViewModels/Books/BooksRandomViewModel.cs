namespace Alexandria.Web.ViewModels.Books
{
    using System.Collections.Generic;

    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;

    public class BooksRandomViewModel : IMapFrom<Book>
    {
        public int Id { get; set; }

        public string PictureUrl { get; set; }

        public string Title { get; set; }

        public BooksAuthorViewModel Author { get; set; }

        public IEnumerable<BooksGenreViewModel> Genres { get; set; }
    }
}
