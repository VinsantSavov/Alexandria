namespace Alexandria.Web.ViewModels.Administration.Genres
{
    using System.ComponentModel.DataAnnotations;

    using Alexandria.Common;
    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;
    using Alexandria.Web.Infrastructure.Attributes;

    public class GenresEditInputModel : IMapFrom<Genre>
    {
        [EnsureGenreIdExists(ErrorMessage = ErrorMessages.GenreNotExistingId)]
        public int Id { get; set; }

        [Required]
        [StringLength(
            GlobalConstants.GenreNameMaxLength,
            ErrorMessage = ErrorMessages.GenreNameLengthErrorMessage,
            MinimumLength = GlobalConstants.GenreNameMinLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(
            GlobalConstants.GenreDescriptionMaxLength,
            ErrorMessage = ErrorMessages.GenreDescriptionErrorMessage,
            MinimumLength = GlobalConstants.GenreDescriptionMinLength)]
        public string Description { get; set; }
    }
}
