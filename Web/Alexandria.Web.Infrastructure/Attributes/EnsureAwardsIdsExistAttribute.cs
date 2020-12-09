namespace Alexandria.Web.Infrastructure.Attributes
{
    using Alexandria.Services.Awards;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class EnsureAwardsIdsExistAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var ids = value as IEnumerable<int>;
                var awardsService = (IAwardsService)validationContext.GetService(typeof(IAwardsService));

                foreach (var id in ids)
                {
                    if (!awardsService.DoesAwardIdExistAsync(id).GetAwaiter().GetResult())
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
