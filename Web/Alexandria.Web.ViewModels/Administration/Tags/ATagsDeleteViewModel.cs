namespace Alexandria.Web.ViewModels.Administration.Tags
{
    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;

    public class ATagsDeleteViewModel : IMapFrom<Tag>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int BooksCount { get; set; }
    }
}
