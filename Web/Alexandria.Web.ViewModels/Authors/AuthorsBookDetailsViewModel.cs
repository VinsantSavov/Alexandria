namespace Alexandria.Web.ViewModels.Authors
{
    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;

    public class AuthorsBookDetailsViewModel : IMapFrom<Book>
    {
        public int Id { get; set; }

        public string Title { get; set; }
    }
}
