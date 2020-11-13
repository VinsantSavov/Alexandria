namespace Alexandria.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Alexandria.Services.Books;
    using Alexandria.Web.ViewModels.Books;
    using Microsoft.AspNetCore.Mvc;

    public class BooksController : Controller
    {
        private const int BooksPerPage = 10;

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

        public async Task<IActionResult> NewReleases(int page = 1)
        {
            var viewModel = new BooksAllViewModel();

            int booksCount = await this.booksService.GetBooksCountAsync();

            viewModel.Books = await this.booksService.GetLatestPublishedBooksAsync<BooksSingleViewModel>(BooksPerPage, (page - 1) * BooksPerPage);
            viewModel.CurrentPage = page;
            viewModel.PagesCount = (int)Math.Ceiling((double)booksCount / BooksPerPage);

            return this.View(viewModel);
        }

        public async Task<IActionResult> TopRated(int page = 1)
        {
            var viewModel = new BooksAllViewModel();

            int booksCount = await this.booksService.GetBooksCountAsync();

            viewModel.Books = await this.booksService.GetTopRatedBooksAsync<BooksSingleViewModel>(BooksPerPage, (page - 1) * BooksPerPage);
            viewModel.CurrentPage = page;
            viewModel.PagesCount = (int)Math.Ceiling((double)booksCount / BooksPerPage);

            return this.View(viewModel);
        }

        public async Task<IActionResult> Test(int id)
        {
            return this.View();
        }
    }
}
