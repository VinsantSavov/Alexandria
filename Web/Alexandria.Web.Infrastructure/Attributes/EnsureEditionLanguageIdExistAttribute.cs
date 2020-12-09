namespace Alexandria.Web.Infrastructure.Attributes
{
    using System.ComponentModel.DataAnnotations;

    using Alexandria.Services.EditionLanguages;

    public class EnsureEditionLanguageIdExistAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var id = (int)value;
                var languagesService = (IEditionLanguagesService)validationContext.GetService(typeof(IEditionLanguagesService));

                if (languagesService.DoesEditionLanguageIdExistAsync(id).GetAwaiter().GetResult())
                {
                    return ValidationResult.Success;
                }
            }

            return new ValidationResult(this.ErrorMessage);
        }
    }
}
