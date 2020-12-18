namespace Alexandria.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Alexandria.Services.Books;
    using Alexandria.Services.Genres;
    using Alexandria.Web.ViewModels.Genres;
    using Microsoft.AspNetCore.Mvc;

    public class GenresController : Controller
    {
        private const int BooksPerPage = 12;
        private const int GenreBooks = 8;
        private const string ControllerName = "Genres";

        private readonly IGenresService genresService;
        private readonly IBooksService booksService;

        public GenresController(
            IGenresService genresService,
            IBooksService booksService)
        {
            this.genresService = genresService;
            this.booksService = booksService;
        }

        public async Task<IActionResult> Details(int id)
        {
            var genre = await this.genresService.GetGenreByIdAsync<GenresDetailsViewModel>(id);
            if (genre == null)
            {
                return this.NotFound();
            }

            genre.NewReleasedBooks = await this.booksService.NewRealesedBooksByGenreIdAsync<GenresBookDetailsViewModel>(id, GenreBooks);
            genre.TopRatedBooks = await this.booksService.TopRatedBooksByGenreIdAsync<GenresBookDetailsViewModel>(id, GenreBooks);

            return this.View(genre);
        }

        public async Task<IActionResult> NewReleases(int id, int page = 1)
        {
            var genre = await this.genresService.GetGenreByIdAsync<GenresAllBooksViewModel>(id);
            if (genre == null)
            {
                return this.NotFound();
            }

            int booksCount = await this.booksService.GetBooksCountByGenreIdAsync(id);

            genre.AllBooks = await this.booksService.NewRealesedBooksByGenreIdAsync<GenresBookDetailsViewModel>(genre.Id, BooksPerPage, (page - 1) * BooksPerPage);
            genre.PagesCount = (int)Math.Ceiling((double)booksCount / BooksPerPage);
            genre.CurrentPage = page;
            genre.ControllerName = ControllerName;
            genre.ActionName = nameof(this.NewReleases);

            return this.View(genre);
        }

        public async Task<IActionResult> TopRated(int id, int page = 1)
        {
            var genre = await this.genresService.GetGenreByIdAsync<GenresAllBooksViewModel>(id);
            if (genre == null)
            {
                return this.NotFound();
            }

            int booksCount = await this.booksService.GetBooksCountByGenreIdAsync(id);

            genre.AllBooks = await this.booksService.TopRatedBooksByGenreIdAsync<GenresBookDetailsViewModel>(id, BooksPerPage, (page - 1) * BooksPerPage);
            genre.PagesCount = (int)Math.Ceiling((double)booksCount / BooksPerPage);
            genre.CurrentPage = page;
            genre.ControllerName = ControllerName;
            genre.ActionName = nameof(this.NewReleases);

            return this.View(genre);
        }
    }
}
