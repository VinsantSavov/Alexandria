namespace Alexandria.Web.ViewModels.Books
{
    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;
    using AutoMapper;

    public class BooksGenreViewModel : IMapFrom<BookGenre>, IHaveCustomMappings
    {
        public int GenreId { get; set; }

        public string Name { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<BookGenre, BooksGenreViewModel>()
                .ForMember(
                bg => bg.Name,
                src => src.MapFrom(b => b.Genre.Name))
                .ForMember(
                bg => bg.GenreId,
                src => src.MapFrom(b => b.Genre.Id));
        }
    }
}
