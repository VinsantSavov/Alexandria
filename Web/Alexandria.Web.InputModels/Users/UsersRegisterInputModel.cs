namespace Alexandria.Web.InputModels.Users
{
    using System.ComponentModel.DataAnnotations;

    using Alexandria.Data.Models.Enums;
    using Microsoft.AspNetCore.Http;

    public class UsersRegisterInputModel
    {
        public string Username { get; set; }

        public GenderType Gender { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Tell something about you")]
        public string Biography { get; set; }

        public IFormFile ProfilePicture { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string ConfirmPassword { get; set; }
    }
}
