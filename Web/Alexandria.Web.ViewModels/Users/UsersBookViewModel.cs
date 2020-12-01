namespace Alexandria.Web.ViewModels.Users
{
    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;

    public class UsersBookViewModel : IMapFrom<Book>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string PictureURL { get; set; }

        public UsersBookAuthorViewModel Author { get; set; }
    }
}
