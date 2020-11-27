namespace Alexandria.Web.ViewModels
{
    using System;
    using System.Linq;

    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;
    using AutoMapper;
    using Ganss.XSS;

    public class ReviewListingViewModel : IMapFrom<Review>, IHaveCustomMappings
    {
        private readonly HtmlSanitizer sanitizer;

        public ReviewListingViewModel()
        {
            this.sanitizer = new HtmlSanitizer();
        }

        public int Id { get; set; }

        public int? ParentId { get; set; }

        public string AuthorUsername { get; set; }

        public string AuthorProfilePicture { get; set; }

        public DateTime CreatedOn { get; set; }

        public string ReadingProgress { get; set; }

        public string Description { get; set; }

        public string SanitizedDescription => this.sanitizer.Sanitize(this.Description);

        public int Likes { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Review, ReviewListingViewModel>()
                         .ForMember(
                         dest => dest.Likes,
                         a => a.MapFrom(
                             src => src.Likes.Count(l => l.IsLiked)));
        }
    }
}
