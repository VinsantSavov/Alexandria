namespace Alexandria.Web.ViewModels.Books
{
    using System.Collections.Generic;

    public class BooksAllViewModel : PagingViewModel
    {
        public IEnumerable<BooksSingleViewModel> Books { get; set; }
    }
}
