namespace Alexandria.Web.ViewModels.Administration.Books
{
    using Alexandria.Common;
    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;
    using Alexandria.Web.Infrastructure.Attributes;

    public class ABooksGenreViewModel : IMapFrom<Genre>
    {
        [EnsureGenreIdExists(ErrorMessage = ErrorMessages.GenreNotExistingId)]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
