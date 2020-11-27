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

        // mapped from review
        public string Content { get; set; }

        // not mapped, used for input
        [IgnoreMap]
        [Display(Name = GlobalConstants.ReviewDescriptionDisplayNameConstant)]
        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }

        public string AuthorUsername { get; set; }

        public string AuthorProfilePicture { get; set; }

        public string SanitizedContent => this.sanitizer.Sanitize(this.Content);

        public int Likes { get; set; }

        public ReadingProgress ReadingProgress { get; set; }

        public bool ThisEdition { get; set; }

        public int BookId { get; set; }

        public string BookTitle { get; set; }

        public string BookPictureURL { get; set; }

        public int BookAuthorId { get; set; }

        public DateTime BookPublishedOn { get; set; }

        public string BookAuthorFullName { get; set; }

        public IEnumerable<ReviewListingViewModel> Comments { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Review, ReviewsDetailsViewModel>()
                         .ForMember(
                         dest => dest.BookAuthorFullName,
                         a => a.MapFrom(
                             src => string.IsNullOrWhiteSpace(src.Book.Author.SecondName)
                             ? src.Book.Author.FirstName + " " + src.Book.Author.LastName
                             : src.Book.Author.FirstName + " " + src.Book.Author.SecondName + " " + src.Book.Author.LastName))
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
