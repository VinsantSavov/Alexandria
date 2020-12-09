namespace Alexandria.Web.ViewModels.Administration.Books
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Alexandria.Common;
    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;
    using Alexandria.Web.Infrastructure.Attributes;

    public class ABooksAddTagsInputModel : IMapFrom<Book>
    {
        [EnsureBookIdExists(ErrorMessage = ErrorMessages.ReviewNotExistingBookIdErrorMessage)]
        public int Id { get; set; }

        public string PictureURL { get; set; }

        public string Title { get; set; }

        public ABooksAuthorViewModel Author { get; set; }

        [EnsureTagsIdsExist(ErrorMessage = ErrorMessages.BookInvalidTagsIds)]
        [Display(Name = GlobalConstants.BookTagsDisplayNameConstant)]
        public IEnumerable<int> TagsIds { get; set; }

        public IEnumerable<ABooksTagViewModel> AllTags { get; set; }
    }
}
