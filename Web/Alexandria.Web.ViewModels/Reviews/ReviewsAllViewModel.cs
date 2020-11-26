namespace Alexandria.Web.ViewModels.Reviews
{
    using System.Collections.Generic;

    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;
    using AutoMapper;

    public class ReviewsAllViewModel : PagingViewModel, IMapFrom<Book>, IHaveCustomMappings
    {
        public string Title { get; set; }

        public int AuthorId { get; set; }

        public string AuthorFullName { get; set; }

        public ICollection<ReviewListingViewModel> AllReviews { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Book, ReviewsAllViewModel>()
                .ForMember(
                r => r.AuthorFullName,
                b => b.MapFrom(src => string.IsNullOrWhiteSpace(src.Author.SecondName) ? src.Author.FirstName + " " + src.Author.LastName : src.Author.FirstName + " " + src.Author.SecondName + " " + src.Author.LastName));
        }
    }
}
