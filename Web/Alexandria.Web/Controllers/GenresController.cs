namespace Alexandria.Web.Controllers
{
    using System.Threading.Tasks;

    using Alexandria.Services.Books;
    using Alexandria.Services.Genres;
    using Alexandria.Web.ViewModels.Genres;
    using Microsoft.AspNetCore.Mvc;

    public class GenresController : Controller
    {
        private readonly IGenresService genresService;
        private readonly IBooksService booksService;

        public GenresController(IGenresService genresService, IBooksService booksService)
        {
            this.genresService = genresService;
            this.booksService = booksService;
        }

        public async Task<IActionResult> Details(int id)
        {
            var genre = await this.genresService.GetGenreByIdAsync<GenresDetailsViewModel>(id);

            if (genre == null)
            {
                // throw Exception
            }

            genre.NewReleasedBooks = await this.booksService.NewRealesedBooksByGenreIdAsync<GenresBookDetailsViewModel>(id, 8);
            genre.TopRatedBooks = await this.booksService.TopRatedBooksByGenreIdAsync<GenresBookDetailsViewModel>(id, 8);

            return this.View(genre);
        }

        public async Task<IActionResult> NewReleases(int id)
        {
            this.ViewData["Subtitle"] = "New Releases";
            var genre = await this.genresService.GetGenreByIdAsync<GenresAllBooksViewModel>(id);

            if (genre == null)
            {
                // throw Exception
            }

            genre.AllBooks = await this.booksService.NewRealesedBooksByGenreIdAsync<GenresBookDetailsViewModel>(id, 10);

            return this.View(genre);
        }

        public async Task<IActionResult> TopRated(int id)
        {
            this.ViewData["Subtitle"] = "Top Rated";

            var genre = await this.genresService.GetGenreByIdAsync<GenresAllBooksViewModel>(id);

            if (genre == null)
            {
                // throw exception
            }

            genre.AllBooks = await this.booksService.TopRatedBooksByGenreIdAsync<GenresBookDetailsViewModel>(id, 10);

            return this.View("NewReleases", genre);
        }
    }
}
