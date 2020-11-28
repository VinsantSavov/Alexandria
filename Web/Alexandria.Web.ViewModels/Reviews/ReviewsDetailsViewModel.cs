namespace Alexandria.Web.ViewModels.Reviews
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using Alexandria.Common;
    using Alexandria.Data.Models;
    using Alexandria.Data.Models.Enums;
    using Alexandria.Services.Mapping;
    using AutoMapper;
    using Ganss.XSS;

    public class ReviewsDetailsViewModel : PagingViewModel, IMapFrom<Review>, IHaveCustomMappings
    {
        private readonly HtmlSanitizer sanitizer;

        public ReviewsDetailsViewModel()
        {
            this.sanitizer = new HtmlSanitizer();
        }

        // not mapped, used for input
        [IgnoreMap]
        [Display(Name = GlobalConstants.ReviewDescriptionDisplayNameConstant)]
        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }

        // mapped from review
        public string Content { get; set; }

        public string SanitizedContent => this.sanitizer.Sanitize(this.Content);

        public int Likes { get; set; }

        public bool UserLikedReview { get; set; }

        public ReadingProgress ReadingProgress { get; set; }

        public bool ThisEdition { get; set; }

        public ReviewsAuthorViewModel Author { get; set; }

        public ReviewsBookViewModel Book { get; set; }

        public IEnumerable<ReviewListingViewModel> Comments { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Review, ReviewsDetailsViewModel>()
                         .ForMember(
                         dest => dest.Content,
                         a => a.MapFrom(
                             src => src.Description))
                         .ForMember(
                         dest => dest.Likes,
                         a => a.MapFrom(
                             src => src.Likes.Count(l => l.IsLiked)));
        }
    }
}
