﻿namespace Alexandria.Web.ViewModels
{
    using System;

    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;
    using Ganss.XSS;

    public class ReviewListingViewModel : IMapFrom<Review>
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
    }
}