namespace Alexandria.Web.ViewModels.Administration.Books
{
    using System;

    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;

    public class ABooksDeleteViewModel : IMapFrom<Book>
    {
        public int Id { get; set; }

        public string PictureURL { get; set; }

        public string Title { get; set; }

        public string Summary { get; set; }

        public ABooksAuthorViewModel Author { get; set; }

        public DateTime PublishedOn { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
