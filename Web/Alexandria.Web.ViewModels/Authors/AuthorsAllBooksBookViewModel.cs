namespace Alexandria.Web.ViewModels.Authors
{
    using System.Collections.Generic;
    using System.Linq;

    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;
    using AutoMapper;

    public class AuthorsAllBooksBookViewModel : IMapFrom<Book>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string PictureURL { get; set; }

        public string Title { get; set; }

        public string Summary { get; set; }

        public string ShortSummary => this.Summary.Length > 250 ? this.Summary.Substring(0, 250) + "..." : this.Summary + "...";

        public double AverageRating { get; set; }

        public int ReviewsCount { get; set; }

        public int RatingsCount { get; set; }

        public IEnumerable<AuthorsGenreViewModel> Genres { get; set; }

        public IEnumerable<AuthorsTagViewModel> Tags { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Book, AuthorsAllBooksBookViewModel>()
            .ForMember(
             dest => dest.AverageRating,
             a => a.MapFrom(src => src.Ratings.Count == 0 ? 0 : src.Ratings.Average(r => r.Rate)))
            .ForMember(
             dest => dest.ReviewsCount,
             a => a.MapFrom(src => src.Reviews.Count(r => !r.IsDeleted)));
        }
    }
}
