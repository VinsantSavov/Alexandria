namespace Alexandria.Web.InputModels.Users
{
    using System.ComponentModel.DataAnnotations;

    public class UsersLoginInputModel
    {
        public string Username { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
