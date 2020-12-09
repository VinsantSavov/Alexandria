namespace Alexandria.Web.Infrastructure.Attributes
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Alexandria.Services.Tags;

    public class EnsureTagsIdsExistAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var ids = value as IEnumerable<int>;

                var tagsService = (ITagsService)validationContext.GetService(typeof(ITagsService));

                foreach (var id in ids)
                {
                    if (!tagsService.DoesTagIdExistAsync(id).GetAwaiter().GetResult())
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
