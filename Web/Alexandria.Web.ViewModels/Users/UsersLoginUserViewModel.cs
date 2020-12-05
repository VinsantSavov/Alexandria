namespace Alexandria.Web.ViewModels.Users
{
    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;

    public class UsersLoginUserViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string ProfilePicture { get; set; }
    }
}
