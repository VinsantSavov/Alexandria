namespace Alexandria.Web.ViewModels.Authors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;

    public class AuthorsDetailsViewModel : IMapFrom<Author>
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public string LastName { get; set; }

        public string FullName => this.SecondName == null ? this.FirstName + " " + this.LastName : this.FirstName + " " + this.SecondName + " " + this.LastName;

        public string ProfilePicture { get; set; }

        public string CountryName { get; set; }

        public string Biography { get; set; }

        public DateTime DateOfBirth { get; set; }

        public int BooksCount { get; set; }

        public double AverageRating => this.AllBooks.Average(b => b.AverageRating);

        public int RatingsCount => this.AllBooks.Sum(b => b.RatingsCount);

        public int ReviewsCount => this.AllBooks.Sum(b => b.ReviewsCount);

        public IEnumerable<AuthorsBookDetailsViewModel> AllBooks { get; set; }
    }
}
