namespace Alexandria.Web.Infrastructure.Attributes
{
    using System.ComponentModel.DataAnnotations;

    using Alexandria.Services.Users;

    public class EnsureUserIdExistsAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var id = (string)value;
                var usersService = (IUsersService)validationContext.GetService(typeof(IUsersService));

                var doExist = usersService.DoesUserIdExistAsync(id).GetAwaiter().GetResult();

                if (doExist)
                {
                    return ValidationResult.Success;
                }
            }

            return new ValidationResult(this.ErrorMessage);
        }
    }
}
