namespace Alexandria.Web.ViewModels.Books
{
    using System.Collections.Generic;

    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;
    using AutoMapper;

    public class BooksAuthorBooksViewModel : IMapFrom<Author>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public IEnumerable<BooksSingleAuthorBookViewModel> TopBooks { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Author, BooksAuthorBooksViewModel>()
                         .ForMember(
                         dest => dest.FullName,
                         a => a.MapFrom(
                             src => string.IsNullOrWhiteSpace(src.SecondName)
                             ? src.FirstName + " " + src.LastName
                             : src.FirstName + " " + src.SecondName + " " + src.LastName));
        }
    }
}
