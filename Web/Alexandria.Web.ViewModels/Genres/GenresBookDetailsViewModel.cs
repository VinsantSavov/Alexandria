namespace Alexandria.Web.ViewModels.Genres
{
    using System.Collections.Generic;
    using System.Linq;

    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;
    using AutoMapper;

    public class GenresBookDetailsViewModel : IMapFrom<Book>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public string PictureURL { get; set; }

        public IEnumerable<string> Tags { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Book, GenresBookDetailsViewModel>()
                         .ForMember(
                            dest => dest.Tags,
                            b => b.MapFrom(src => src.Tags.Select(t => t.Tag.Name)))
                         .ForMember(
                            dest => dest.Author,
                            b => b.MapFrom(src => string.IsNullOrWhiteSpace(src.Author.SecondName) ? src.Author.FirstName + " " + src.Author.LastName : src.Author.FirstName + " " + src.Author.SecondName + " " + src.Author.LastName));
        }
    }
}
