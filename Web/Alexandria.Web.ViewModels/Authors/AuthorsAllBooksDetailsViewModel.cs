namespace Alexandria.Web.ViewModels.Authors
{
    using System.Collections.Generic;
    using System.Linq;

    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;

    public class AuthorsAllBooksDetailsViewModel : PagingViewModel, IMapFrom<Author>
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public string LastName { get; set; }

        public string FullName => string.IsNullOrWhiteSpace(this.SecondName) ? this.FirstName + " " + this.LastName : this.FirstName + " " + this.SecondName + " " + this.LastName;

        public string ProfilePicture { get; set; }

        public double AverageRating => this.AllBooks.Average(b => b.AverageRating);

        public int RatingsCount => this.AllBooks.Sum(b => b.RatingsCount);

        public int ReviewsCount => this.AllBooks.Sum(b => b.ReviewsCount);

        public IEnumerable<AuthorsAllBooksBookViewModel> AllBooks { get; set; }

        public override string GetId()
        {
            return this.Id.ToString();
        }
    }
}
