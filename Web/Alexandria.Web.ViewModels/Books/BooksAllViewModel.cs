namespace Alexandria.Web.ViewModels.Books
{
    using System.Collections.Generic;

    public class BooksAllViewModel : PagingViewModel
    {
        public int Id { get; set; }

        public IEnumerable<BooksSingleViewModel> Books { get; set; }

        public override string GetId()
        {
            return this.Id.ToString();
        }
    }
}
