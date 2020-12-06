namespace Alexandria.Web.ViewModels.Genres
{
    using System.Linq;

    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;
    using AutoMapper;

    public class GenresRandomViewModel : IMapFrom<Genre>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int BooksCount { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Genre, GenresRandomViewModel>()
                .ForMember(
                g => g.BooksCount,
                bg => bg.MapFrom(src => src.Books
                        .Where(b => !b.Book.IsDeleted)
                        .Count()));
        }
    }
}
