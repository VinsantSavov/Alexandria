namespace Alexandria.Web.ViewModels.Books
{
    using System.Collections.Generic;

    public class BooksAllViewModel
    {
        public int CurrentPage { get; set; }

        public int PagesCount { get; set; }

        public IEnumerable<BooksSingleViewModel> Books { get; set; }
    }
}
