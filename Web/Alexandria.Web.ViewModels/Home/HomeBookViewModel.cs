namespace Alexandria.Web.ViewModels.Home
{
    using System.Collections.Generic;
    using System.Linq;

    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;

    public class HomeBookViewModel : IMapFrom<Book>
    {
        public int Id { get; set; }

        public string PictureUrl { get; set; }

        public string Title { get; set; }

        public IEnumerable<HomeGenreViewModel> Genres { get; set; }

        public IEnumerable<HomeTagViewModel> Tags { get; set; }

        public string Genre => !this.Genres.Any() ? "No genre" : this.Genres.OrderBy(g => g.GenreName).FirstOrDefault().GenreName;

        public string Tag => !this.Tags.Any() ? "No tag" : this.Tags.OrderBy(t => t.TagName).FirstOrDefault().TagName;
    }
}
