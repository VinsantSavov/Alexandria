namespace Alexandria.Web.Areas.Identity.Pages.Account.Manage
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;

    using Alexandria.Common;
    using Alexandria.Data.Models;
    using Alexandria.Data.Models.Enums;
    using Alexandria.Services.Cloudinary;
    using Alexandria.Web.Infrastructure.Attributes;
    using Alexandria.Web.Infrastructure.Extensions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ICloudinaryService cloudinaryService;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ICloudinaryService cloudinaryService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.cloudinaryService = cloudinaryService;
        }

        public string Username { get; set; }

        public string ProfilePicture { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = GlobalConstants.PhoneNumberDisplayName)]
            public string PhoneNumber { get; set; }

            [Required]
            [EnumDataType(typeof(GenderType), ErrorMessage = ErrorMessages.UserInvalidGender)]
            public GenderType Gender { get; set; }

            [DataType(DataType.Upload)]
            [EnsureImageExtensionIsValid(ErrorMessage = ErrorMessages.InvalidExtension)]
            [Display(Name = GlobalConstants.ProfilePictureDisplayName)]
            public IFormFile ProfilePicture { get; set; }

            [StringLength(GlobalConstants.UserBiographyMaxLength, ErrorMessage = ErrorMessages.UserInvalidBiography)]
            public string Biography { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this.User.GetUserId()}'.");
            }

            await this.LoadAsync(user);
            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this.User.GetUserId()}'.");
            }

            if (!this.ModelState.IsValid)
            {
                await this.LoadAsync(user);
                return this.Page();
            }

            user.Gender = this.Input.Gender;
            user.Biography = this.Input.Biography;

            var phoneNumber = await this.userManager.GetPhoneNumberAsync(user);
            if (this.Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await this.userManager.SetPhoneNumberAsync(user, this.Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    this.StatusMessage = "Unexpected error when trying to set phone number.";
                    return this.RedirectToPage();
                }
            }

            if (this.Input.ProfilePicture != null)
            {
                var image = await this.cloudinaryService
                    .UploadImageAsync(this.Input.ProfilePicture, user.Id.ToString());

                user.ProfilePicture = image;
            }

            var result = await this.userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var userId = this.User.GetUserId();
                throw new InvalidOperationException($"Unexpected error occured with user with Id {userId}");
            }

            await this.signInManager.RefreshSignInAsync(user);
            this.StatusMessage = "Your profile has been updated";
            return this.RedirectToPage();
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await this.userManager.GetUserNameAsync(user);
            var phoneNumber = await this.userManager.GetPhoneNumberAsync(user);

            this.ProfilePicture = user.ProfilePicture;
            this.Username = userName;

            this.Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                Biography = user.Biography,
                Gender = user.Gender,
            };
        }
    }
}
