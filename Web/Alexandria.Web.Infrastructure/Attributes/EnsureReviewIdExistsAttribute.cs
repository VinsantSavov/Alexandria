namespace Alexandria.Web.Infrastructure.Attributes
{
    using System.ComponentModel.DataAnnotations;

    using Alexandria.Services.Reviews;

    public class EnsureReviewIdExistsAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var reviewsService = (IReviewsService)validationContext.GetService(typeof(IReviewsService));

                var doesExist = reviewsService.DoesReviewIdExistAsync((int)value).GetAwaiter().GetResult();

                if (doesExist)
                {
                    return ValidationResult.Success;
                }

                return new ValidationResult(this.ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
