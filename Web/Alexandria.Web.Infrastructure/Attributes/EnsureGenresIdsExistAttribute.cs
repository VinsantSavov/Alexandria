namespace Alexandria.Web.Infrastructure.Attributes
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Alexandria.Services.Genres;

    public class EnsureGenresIdsExistAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var genresIds = value as IEnumerable<int>;
                var genresService = (IGenresService)validationContext.GetService(typeof(IGenresService));

                foreach (var id in genresIds)
                {
                    if (!genresService.DoesGenreIdExistAsync(id).GetAwaiter().GetResult())
                    {
                        return new ValidationResult(this.ErrorMessage);
                    }
                }

                return ValidationResult.Success;
            }

            return new ValidationResult(this.ErrorMessage);
        }
    }
}
