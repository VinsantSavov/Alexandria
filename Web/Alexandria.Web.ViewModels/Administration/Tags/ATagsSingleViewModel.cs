namespace Alexandria.Web.ViewModels.Administration.Tags
{
    using System;

    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;

    public class ATagsSingleViewModel : IMapFrom<Tag>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int BooksCount { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
