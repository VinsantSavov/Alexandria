using System.ComponentModel.DataAnnotations;

namespace Alexandria.Web.InputModels.Users
{
    public class UsersLoginInputModel
    {
        public string Username { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
