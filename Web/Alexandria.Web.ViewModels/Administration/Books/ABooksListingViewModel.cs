namespace Alexandria.Web.ViewModels.Administration.Books
{
    using System;

    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;
    using AutoMapper;

    public class ABooksListingViewModel : IMapFrom<Book>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public ABooksAuthorViewModel Author { get; set; }

        public DateTime PublishedOn { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public int Pages { get; set; }

        public string PictureURL { get; set; }

        [IgnoreMap]
        public bool AmazonLink { get; set; }

        public string EditionLanguageName { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Book, ABooksListingViewModel>()
                .ForMember(
                m => m.AmazonLink,
                b => b.MapFrom(src => src.AmazonLink == null ? false : true));
        }
    }
}
