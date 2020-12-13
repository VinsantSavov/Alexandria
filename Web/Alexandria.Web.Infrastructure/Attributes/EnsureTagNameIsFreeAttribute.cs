namespace Alexandria.Web.Infrastructure.Attributes
{
    using System.ComponentModel.DataAnnotations;

    using Alexandria.Services.Tags;

    public class EnsureTagNameIsFreeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var tagsService = (ITagsService)validationContext.GetService(typeof(ITagsService));

                var doExist = tagsService.DoesTagNameExistAsync(value as string).GetAwaiter().GetResult();

                if (!doExist)
                {
                    return ValidationResult.Success;
                }
            }

            return new ValidationResult(this.ErrorMessage);
        }
    }
}
