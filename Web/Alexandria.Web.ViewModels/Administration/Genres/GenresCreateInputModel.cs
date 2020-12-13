namespace Alexandria.Web.ViewModels.Administration.Genres
{
    using System.ComponentModel.DataAnnotations;

    using Alexandria.Common;
    using Alexandria.Web.Infrastructure.Attributes;

    public class GenresCreateInputModel
    {
        [Required]
        [StringLength(
            GlobalConstants.GenreNameMaxLength,
            ErrorMessage = ErrorMessages.GenreNameLengthErrorMessage,
            MinimumLength = GlobalConstants.GenreNameMinLength)]
        [EnsureGenreNameIsFree(ErrorMessage = ErrorMessages.GenreNameUnavailable)]
        public string Name { get; set; }

        [Required]
        [StringLength(
            GlobalConstants.GenreDescriptionMaxLength,
            ErrorMessage = ErrorMessages.GenreDescriptionErrorMessage,
            MinimumLength = GlobalConstants.GenreDescriptionMinLength)]
        public string Description { get; set; }
    }
}
