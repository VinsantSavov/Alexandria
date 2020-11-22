namespace Alexandria.Web.ViewModels.Authors
{
    using System.Linq;

    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;
    using AutoMapper;

    public class AuthorsBookDetailsViewModel : IMapFrom<Book>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string PictureURL { get; set; }

        public string Title { get; set; }

        public int RatingsCount { get; set; }

        public double AverageRating { get; set; }

        public int ReviewsCount { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Book, AuthorsBookDetailsViewModel>()
                        .ForMember(
                         dest => dest.AverageRating,
                         a => a.MapFrom(src => src.Ratings.Count == 0 ? 0 : src.Ratings.Average(r => r.Rate)));
        }
    }
}
