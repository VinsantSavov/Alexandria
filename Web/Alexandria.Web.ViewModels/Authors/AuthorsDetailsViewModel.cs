namespace Alexandria.Web.ViewModels.Authors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;
    using AutoMapper;

    public class AuthorsDetailsViewModel : IMapFrom<Author>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ProfilePicture { get; set; }

        public string Country { get; set; }

        public string Biography { get; set; }

        public DateTime BornOn { get; set; }

        public int BooksCount { get; set; }

        public double AverageRating { get; set; }

        public int RatingsCount { get; set; }

        public int ReviewsCount { get; set; }

        public IEnumerable<AuthorsBookDetailsViewModel> TopRatedBooks { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Author, AuthorsDetailsViewModel>()
                         .ForMember(
                          dest => dest.AverageRating,
                          a => a.MapFrom(src => src.Books.Average(b => b.Ratings.Sum(r => r.Rate))));
        }
    }
}
