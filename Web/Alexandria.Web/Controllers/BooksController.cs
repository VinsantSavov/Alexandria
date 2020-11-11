namespace Alexandria.Web.Controllers
{
    using System.Threading.Tasks;

    using Alexandria.Services.Books;
    using Alexandria.Web.ViewModels.Books;
    using Microsoft.AspNetCore.Mvc;

    public class BooksController : Controller
    {
        private readonly IBooksService booksService;

        public BooksController(IBooksService booksService)
        {
            this.booksService = booksService;
        }

        public IActionResult Details()
        {
            return this.View();
        }

        public IActionResult All()
        {

            return this.View();
        }

        public async Task<IActionResult> NewReleases()
        {
            var books = await this.booksService.GetLatestPublishedBooksAsync<BooksAllViewModel>(8);

            return this.View(books);
        }

        public IActionResult TopRated()
        {
            return this.View();
        }
    }
}
