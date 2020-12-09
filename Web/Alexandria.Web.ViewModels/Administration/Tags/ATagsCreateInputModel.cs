namespace Alexandria.Web.ViewModels.Administration.Tags
{
    using System.ComponentModel.DataAnnotations;

    using Alexandria.Common;

    public class ATagsCreateInputModel
    {
        [StringLength(
            GlobalConstants.TagNameMaxLength,
            ErrorMessage = ErrorMessages.TagNameLengthErrorMessage,
            MinimumLength = GlobalConstants.TagNameMinLength)]
        public string Name { get; set; }
    }
}
