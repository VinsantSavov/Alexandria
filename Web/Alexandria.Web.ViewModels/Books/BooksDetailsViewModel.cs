namespace Alexandria.Web.ViewModels.Books
{
    using System;
    using System.Collections.Generic;

    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;
    using AutoMapper;

    public class BooksDetailsViewModel : IMapFrom<Book>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public BooksAuthorViewModel Author { get; set; }

        public DateTime PublishedOn { get; set; }

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

        public IEnumerable<BooksReviewViewModel> Reviews { get; set; }
    }
}
