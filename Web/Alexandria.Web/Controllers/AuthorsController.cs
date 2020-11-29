namespace Alexandria.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Alexandria.Services.Authors;
    using Alexandria.Services.Books;
    using Alexandria.Web.ViewModels.Authors;
    using Microsoft.AspNetCore.Mvc;

    public class AuthorsController : Controller
    {
        private const int BooksPerPage = 10;
        private const int BooksCount = 4;
        private const string ControllerName = "Authors";

        private readonly IAuthorsService authorsService;
        private readonly IBooksService booksService;

        public AuthorsController(
            IAuthorsService authorsService,
            IBooksService booksService)
        {
            this.authorsService = authorsService;
            this.booksService = booksService;
        }

        public async Task<IActionResult> Details(int id)
        {
            var author = await this.authorsService.GetAuthorByIdAsync<AuthorsDetailsViewModel>(id);

            author.AllBooks = await this.booksService.GetTopRatedBooksByAuthorIdAsync<AuthorsBookDetailsViewModel>(id, BooksCount);

            return this.View(author);
        }

        public async Task<IActionResult> AllBooks(int id, int page = 1)
        {
            var author = await this.authorsService.GetAuthorByIdAsync<AuthorsAllBooksDetailsViewModel>(id);

            var booksCount = await this.booksService.GetBooksCountByAuthorIdAsync(id);

            author.AllBooks = await this.booksService.GetAllBooksByAuthorIdAsync<AuthorsAllBooksBookViewModel>(id, BooksPerPage, (page - 1) * BooksPerPage);
            author.PagesCount = (int)Math.Ceiling((double)booksCount / BooksPerPage);
            author.CurrentPage = page;
            author.ControllerName = ControllerName;
            author.ActionName = nameof(this.AllBooks);

            return this.View(author);
        }
    }
}
