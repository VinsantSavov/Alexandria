namespace Alexandria.Web.ViewModels.Reviews
{
    using System;

    using Alexandria.Data.Models;
    using Alexandria.Data.Models.Enums;
    using Alexandria.Services.Mapping;
    using AutoMapper;
    using Ganss.XSS;

    public class ReviewsDetailsViewModel : IMapFrom<Review>, IHaveCustomMappings
    {
        private readonly HtmlSanitizer sanitizer;

        public ReviewsDetailsViewModel()
        {
            this.sanitizer = new HtmlSanitizer();
        }

        public int Id { get; set; }

        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }

        public string AuthorUsername { get; set; }

        public string AuthorProfilePicture { get; set; }

        public string SanitizedDescription => this.sanitizer.Sanitize(this.Description);

        public int Likes { get; set; }

        public ReadingProgress ReadingProgress { get; set; }

        public bool ThisEdition { get; set; }

        public string BookId { get; set; }

        public string BookTitle { get; set; }

        public string BookPictureURL { get; set; }

        public int BookAuthorId { get; set; }

        public DateTime BookPublishedOn { get; set; }

        public string BookAuthorFullName { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Review, ReviewsDetailsViewModel>()
                         .ForMember(
                         dest => dest.BookAuthorFullName,
                         a => a.MapFrom(
                             src => string.IsNullOrWhiteSpace(src.Book.Author.SecondName)
                             ? src.Book.Author.FirstName + " " + src.Book.Author.LastName
                             : src.Book.Author.FirstName + " " + src.Book.Author.SecondName + " " + src.Book.Author.LastName));
        }
    }
}
