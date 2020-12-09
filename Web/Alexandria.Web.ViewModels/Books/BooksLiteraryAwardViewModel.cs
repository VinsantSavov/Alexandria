namespace Alexandria.Web.ViewModels.Books
{
    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;

    public class BooksLiteraryAwardViewModel : IMapFrom<BookAward>
    {
        public int AwardId { get; set; }

        public string AwardName { get; set; }
    }
}
