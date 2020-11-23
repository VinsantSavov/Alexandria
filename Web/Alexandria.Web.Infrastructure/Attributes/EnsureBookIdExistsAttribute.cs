namespace Alexandria.Web.Infrastructure.Attributes
{
    using System.ComponentModel.DataAnnotations;

    using Alexandria.Services.Books;

    public class EnsureBookIdExistsAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var booksService = (IBooksService)validationContext.GetService(typeof(IBooksService));
            var doesExist = booksService.DoesBookIdExistAsync((int)value).GetAwaiter().GetResult();

            if (doesExist)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(this.ErrorMessage);
        }
    }
}
