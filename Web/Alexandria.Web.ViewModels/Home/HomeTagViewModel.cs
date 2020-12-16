namespace Alexandria.Web.ViewModels.Home
{
    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;

    public class HomeTagViewModel : IMapFrom<BookTag>
    {
        public int TagId { get; set; }

        public string TagName { get; set; }
    }
}
