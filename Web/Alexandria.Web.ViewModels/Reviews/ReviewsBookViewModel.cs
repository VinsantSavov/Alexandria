namespace Alexandria.Web.ViewModels.Reviews
{
    using System;

    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;

    public class ReviewsBookViewModel : IMapFrom<Book>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string PictureURL { get; set; }

        public DateTime PublishedOn { get; set; }

        public ReviewsBookAuthorViewModel Author { get; set; }
    }
}
