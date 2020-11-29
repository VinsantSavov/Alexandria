namespace Alexandria.Web.ViewModels.Reviews
{
    using System;
    using System.Linq;

    using Alexandria.Data.Models;
    using Alexandria.Data.Models.Enums;
    using Alexandria.Services.Mapping;
    using AutoMapper;
    using Ganss.XSS;

    public class ReviewsDeleteViewModel : IMapFrom<Review>, IHaveCustomMappings
    {
        private readonly HtmlSanitizer sanitizer;

        public ReviewsDeleteViewModel()
        {
            this.sanitizer = new HtmlSanitizer();
        }

        public int Id { get; set; }

        public string Description { get; set; }

        public string SanitizedDescription => this.sanitizer.Sanitize(this.Description);

        public DateTime CreatedOn { get; set; }

        public ReadingProgress ReadingProgress { get; set; }

        public bool ThisEdition { get; set; }

        public int Likes { get; set; }

        public ReviewsAuthorViewModel Author { get; set; }

        public ReviewsBookViewModel Book { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Review, ReviewsDeleteViewModel>()
                         .ForMember(
                         dest => dest.Likes,
                         a => a.MapFrom(
                                src => src.Likes.Count(l => l.IsLiked)));
        }
    }
}
