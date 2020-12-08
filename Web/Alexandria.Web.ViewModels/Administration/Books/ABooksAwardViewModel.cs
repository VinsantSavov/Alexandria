namespace Alexandria.Web.ViewModels.Administration.Books
{
    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;

    public class ABooksAwardViewModel : IMapFrom<Award>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
