namespace Alexandria.Web.ViewModels.Books
{
    using System.Collections.Generic;

    public class BooksSearchViewModel : PagingViewModel
    {
        public int Id { get; set; }

        public string SearchString { get; set; }

        public IEnumerable<BooksSingleViewModel> Books { get; set; }

        public override string GetId()
        {
            return this.Id.ToString();
        }
    }
}
