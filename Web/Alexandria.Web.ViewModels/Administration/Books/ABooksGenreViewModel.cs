namespace Alexandria.Web.ViewModels.Administration.Books
{
    using Alexandria.Common;
    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;
    using Alexandria.Web.Infrastructure.Attributes;
    using AutoMapper;

    public class ABooksGenreViewModel : IMapFrom<Genre>, IHaveCustomMappings
    {
        [EnsureGenreIdExists(ErrorMessage = ErrorMessages.GenreNotExistingId)]
        public int GenreId { get; set; }

        public string GenreName { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Genre, ABooksGenreViewModel>()
                .ForMember(
                m => m.GenreId,
                g => g.MapFrom(src => src.Id))
                .ForMember(
                m => m.GenreName,
                g => g.MapFrom(src => src.Name));
        }
    }
}
