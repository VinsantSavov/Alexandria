namespace Alexandria.Web.ViewModels.Reviews
{
    using System.Collections.Generic;

    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;
    using AutoMapper;

    public class ReviewsAllViewModel : PagingViewModel, IMapFrom<Book>
    {
        public string Title { get; set; }

        public ReviewsBookAuthorViewModel Author { get; set; }

        public ICollection<ReviewListingViewModel> AllReviews { get; set; }
    }
}
