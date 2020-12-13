namespace Alexandria.Web.ViewModels.Administration.Tags
{
    using System.ComponentModel.DataAnnotations;

    using Alexandria.Common;
    using Alexandria.Web.Infrastructure.Attributes;

    public class ATagsCreateInputModel
    {
        [Required]
        [StringLength(
            GlobalConstants.TagNameMaxLength,
            ErrorMessage = ErrorMessages.TagNameLengthErrorMessage,
            MinimumLength = GlobalConstants.TagNameMinLength)]
        [EnsureTagNameIsFree(ErrorMessage = ErrorMessages.TagNameUnavailable)]
        public string Name { get; set; }
    }
}
