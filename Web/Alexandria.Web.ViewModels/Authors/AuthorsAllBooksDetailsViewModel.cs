namespace Alexandria.Web.ViewModels.Authors
{
    using System.Collections.Generic;
    using System.Linq;

    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;

    public class AuthorsAllBooksDetailsViewModel : IMapFrom<Author>
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public string LastName { get; set; }

        public string FullName => this.SecondName == null ? this.FirstName + " " + this.LastName : this.FirstName + " " + this.SecondName + " " + this.LastName;

        public string ProfilePicture { get; set; }

        public double AverageRating => this.Books.Average(b => b.AverageRating);

        public int RatingsCount => this.Books.Sum(b => b.RatingsCount);

        public int ReviewsCount => this.Books.Sum(b => b.ReviewsCount);

        public IEnumerable<AuthorsAllBooksBookViewModel> Books { get; set; }
    }
}
