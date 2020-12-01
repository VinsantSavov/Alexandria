﻿namespace Alexandria.Web.ViewModels.Users
{
    using System;
    using System.Linq;

    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;
    using Ganss.XSS;

    public class UsersReviewViewModel : IMapFrom<Review>
    {
        private readonly HtmlSanitizer sanitizer;

        public UsersReviewViewModel()
        {
            this.sanitizer = new HtmlSanitizer();
        }

        public int Id { get; set; }

        public string Description { get; set; }

        public string SanitizedDescription => this.sanitizer.Sanitize(this.Description);

        public string ShortSanitizedDescription => this.SanitizedDescription.Count() > 200 ? this.SanitizedDescription.Substring(0, 200) + "..." : this.SanitizedDescription;

        public DateTime CreatedOn { get; set; }

        public UsersBookViewModel Book { get; set; }
    }
}
