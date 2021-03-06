﻿namespace Alexandria.Web.ViewModels.Books
{
    using System.Collections.Generic;
    using System.Linq;

    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;
    using AutoMapper;

    public class BooksSingleViewModel : IMapFrom<Book>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string PictureURL { get; set; }

        public string Title { get; set; }

        public string Summary { get; set; }

        public string ShortSummary => this.Summary.Length > 250 ? this.Summary.Substring(0, 250) + "..." : this.Summary + "...";

        public double AverageRating { get; set; }

        public int ReviewsCount { get; set; }

        public IEnumerable<BooksGenreViewModel> Genres { get; set; }

        public IEnumerable<BooksTagViewModel> Tags { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Book, BooksSingleViewModel>()
                .ForMember(
                         bm => bm.AverageRating,
                         src => src.MapFrom(b => b.Ratings.Count == 0 ? 0 : b.Ratings.Average(r => r.Rate)))
                .ForMember(
                         dest => dest.ReviewsCount,
                         a => a.MapFrom(src => src.Reviews.Count(r => !r.IsDeleted)));
        }
    }
}
