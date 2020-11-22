namespace Alexandria.Web.ViewModels.Books
{
    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;

    public class BooksAuthorViewModel : IMapFrom<Author>
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public string LastName { get; set; }

        public string FullName => string.IsNullOrWhiteSpace(this.SecondName) ? this.FirstName + " " + this.LastName : this.FirstName + " " + this.SecondName + " " + this.LastName;
    }
}
