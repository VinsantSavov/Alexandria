namespace Alexandria.Web.ViewModels.Books
{
    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;
    using AutoMapper;

    public class BooksTagViewModel : IMapFrom<BookTag>, IHaveCustomMappings
    {
        public int TagId { get; set; }

        public string Name { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<BookTag, BooksTagViewModel>()
                .ForMember(
                bt => bt.Name,
                src => src.MapFrom(b => b.Tag.Name))
                .ForMember(
                bt => bt.TagId,
                src => src.MapFrom(b => b.TagId));
        }
    }
}
