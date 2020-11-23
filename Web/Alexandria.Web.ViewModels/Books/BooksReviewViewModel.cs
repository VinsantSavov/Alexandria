namespace Alexandria.Web.ViewModels.Books
{
    using System;

    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;
    using Ganss.XSS;

    public class BooksReviewViewModel : IMapFrom<Review>
    {
        private readonly HtmlSanitizer sanitizer;

        public BooksReviewViewModel()
        {
            this.sanitizer = new HtmlSanitizer();
        }

        public int Id { get; set; }

        public string AuthorUsername { get; set; }

        public string AuthorProfilePicture { get; set; }

        public DateTime CreatedOn { get; set; }

        public string ReadingProgress { get; set; }

        public string Description { get; set; }

        public string SanitizedDescription => this.sanitizer.Sanitize(this.Description);

        public int Likes { get; set; }
    }
}
