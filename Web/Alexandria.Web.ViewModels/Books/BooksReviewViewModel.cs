namespace Alexandria.Web.ViewModels.Books
{
    using System;

    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;

    public class BooksReviewViewModel : IMapFrom<Review>
    {
        public int Id { get; set; }

        public string AuthorUsername { get; set; }

        public string AuthorProfilePicture { get; set; }

        public DateTime CreatedOn { get; set; }

        public string ReadingProgress { get; set; }

        public string Description { get; set; }

        public string SanitizedDescription { get; set; }

        public int Likes { get; set; }
    }
}
