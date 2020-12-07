namespace Alexandria.Web.Infrastructure.Attributes
{
    using System.ComponentModel.DataAnnotations;

    using Alexandria.Services.Genres;

    public class EnsureGenreIdExistsAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var genresService = (IGenresService)validationContext.GetService(typeof(IGenresService));

            if (value != null)
            {
                var doExist = genresService.DoesGenreIdExist((int)value).GetAwaiter().GetResult();

                if (doExist)
                {
                    return ValidationResult.Success;
                }
            }

            return new ValidationResult(this.ErrorMessage);
        }
    }
}
