namespace Alexandria.Web.ViewModels.Reviews
{
    using System.Collections.Generic;

    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;
    using AutoMapper;

    public class ReviewsAllViewModel : PagingViewModel, IMapFrom<Book>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public ReviewsBookAuthorViewModel Author { get; set; }

        public ICollection<ReviewListingViewModel> AllReviews { get; set; }

        public override string GetId()
        {
            return this.Id.ToString();
        }
    }
}
