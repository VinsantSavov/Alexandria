namespace Alexandria.Web.ViewModels.Administration.Books
{
    using System;
    using System.Collections.Generic;

    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;
    using Alexandria.Web.ViewModels.Books;

    public class ABooksDetailsViewModel : IMapFrom<Book>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public ABooksAuthorViewModel Author { get; set; }

        public DateTime PublishedOn { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string Summary { get; set; }

        public int Pages { get; set; }

        public string PictureURL { get; set; }

        public string AmazonLink { get; set; }

        public string EditionLanguageName { get; set; }

        public int ReviewsCount { get; set; }

        public int RatingsCount { get; set; }

        public IEnumerable<BooksGenreViewModel> Genres { get; set; }

        public IEnumerable<BooksTagViewModel> Tags { get; set; }

        public IEnumerable<BooksLiteraryAwardViewModel> Awards { get; set; }
    }
}
