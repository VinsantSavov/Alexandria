namespace Alexandria.Web.Controllers
{
    using System.Threading.Tasks;

    using Alexandria.Services.Authors;
    using Alexandria.Web.ViewModels.Authors;
    using Microsoft.AspNetCore.Mvc;

    public class AuthorsController : Controller
    {
        private readonly IAuthorsService authorsService;

        public AuthorsController(IAuthorsService authorsService)
        {
            this.authorsService = authorsService;
        }

        public async Task<IActionResult> Details(int id)
        {
            var author = await this.authorsService.GetAuthorByIdAsync<AuthorsDetailsViewModel>(id);

            return this.View(author);
        }

        public async Task<IActionResult> AllBooks(int id)
        {
            var author = await this.authorsService.GetAuthorByIdAsync<AuthorsAllBooksDetailsViewModel>(id);

            return this.View(author);
        }
    }
}
