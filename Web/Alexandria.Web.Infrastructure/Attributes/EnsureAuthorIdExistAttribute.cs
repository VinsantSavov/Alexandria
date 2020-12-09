namespace Alexandria.Web.Infrastructure.Attributes
{
    using System.ComponentModel.DataAnnotations;

    using Alexandria.Services.Authors;

    public class EnsureAuthorIdExistAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var id = (int)value;
                var authorsService = (IAuthorsService)validationContext.GetService(typeof(IAuthorsService));

                if (authorsService.DoesAuthorIdExistAsync(id).GetAwaiter().GetResult())
                {
                    return ValidationResult.Success;
                }
            }

            return new ValidationResult(this.ErrorMessage);
        }
    }
}
