namespace Alexandria.Web.ViewModels.Administration.Books
{
    using System.Collections.Generic;

    public class ABooksAllBooksViewModel : PagingViewModel
    {
        public IEnumerable<ABooksListingViewModel> Books { get; set; }
    }
}
