namespace Alexandria.Web.ViewModels.Chat
{
    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;

    public class ChatUserViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string ProfilePicture { get; set; }
    }
}
