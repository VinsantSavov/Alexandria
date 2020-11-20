namespace Alexandria.Web.ViewModels.Authors
{
    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;

    public class AuthorsTagViewModel : IMapFrom<BookTag>
    {
        public int TagId { get; set; }

        public string TagName { get; set; }
    }
}
