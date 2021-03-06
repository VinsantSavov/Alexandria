﻿namespace Alexandria.Web.ViewModels.Users
{
    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;
    using AutoMapper;

    public class UsersBookAuthorViewModel : IMapFrom<Author>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Author, UsersBookAuthorViewModel>()
                         .ForMember(
                         dest => dest.FullName,
                         a => a.MapFrom(
                             src => string.IsNullOrWhiteSpace(src.SecondName)
                             ? src.FirstName + " " + src.LastName
                             : src.FirstName + " " + src.SecondName + " " + src.LastName));
        }
    }
}
