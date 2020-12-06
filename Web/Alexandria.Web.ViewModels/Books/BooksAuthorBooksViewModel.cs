namespace Alexandria.Web.ViewModels.Books
{
    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;

    public class BooksAuthorBooksViewModel : IMapFrom<Book>
    {
        public int Id { get; set; }

        public string PictureUrl { get; set; }
    }
}
