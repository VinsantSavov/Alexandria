﻿namespace Alexandria.Web.ViewModels.Reviews
{
    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;
    using AutoMapper;

    public class ReviewsBookAuthorViewModel : IMapFrom<Author>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Author, ReviewsBookAuthorViewModel>()
                         .ForMember(
                         dest => dest.FullName,
                         a => a.MapFrom(
                             src => string.IsNullOrWhiteSpace(src.SecondName)
                             ? src.FirstName + " " + src.LastName
                             : src.FirstName + " " + src.SecondName + " " + src.LastName));
        }
    }
}
