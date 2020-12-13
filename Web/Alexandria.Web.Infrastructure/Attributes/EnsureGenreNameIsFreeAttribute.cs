namespace Alexandria.Web.Infrastructure.Attributes
{
    using Alexandria.Services.Genres;
    using System.ComponentModel.DataAnnotations;

    public class EnsureGenreNameIsFreeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var genresService = (IGenresService)validationContext.GetService(typeof(IGenresService));

                var doExist = genresService.DoesGenreNameExistAsync(value as string).GetAwaiter().GetResult();

                if (!doExist)
                {
                    return ValidationResult.Success;
                }
            }

            return new ValidationResult(this.ErrorMessage);
        }
    }
}
