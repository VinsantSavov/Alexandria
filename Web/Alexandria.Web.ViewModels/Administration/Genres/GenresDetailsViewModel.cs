namespace Alexandria.Web.ViewModels.Administration.Genres
{
    using System;

    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;

    public class GenresDetailsViewModel : IMapFrom<Genre>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }
    }
}
