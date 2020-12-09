namespace Alexandria.Web.ViewModels.Administration.Books
{
    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;

    public class ABooksEditionLanguageViewModel : IMapFrom<EditionLanguage>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
