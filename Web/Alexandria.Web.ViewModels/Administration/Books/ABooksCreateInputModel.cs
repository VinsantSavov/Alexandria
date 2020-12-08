namespace Alexandria.Web.ViewModels.Administration.Books
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Alexandria.Common;
    using Alexandria.Web.Infrastructure.Attributes;
    using Microsoft.AspNetCore.Http;

    public class ABooksCreateInputModel
    {
        public string Title { get; set; }

        [DataType(DataType.Upload)]
        [EnsureImageExtensionIsValid(ErrorMessage = ErrorMessages.InvalidExtension)]
        [Display(Name = GlobalConstants.BookPictureUrlDisplayNameConstant)]
        public IFormFile PictureURL { get; set; }

        public string Summary { get; set; }

        public DateTime PublishedOn { get; set; }

        public int Pages { get; set; }

        public string AmazonLink { get; set; }

        public IEnumerable<int> GenresIds { get; set; }

        public IEnumerable<int> TagsIds { get; set; }

        public IEnumerable<int> AwardsIds { get; set; }

        public IEnumerable<ABooksGenreViewModel> Genres { get; set; }

        public IEnumerable<ABooksTagViewModel> Tags { get; set; }

        public IEnumerable<ABooksAwardViewModel> Awards { get; set; }
    }
}
