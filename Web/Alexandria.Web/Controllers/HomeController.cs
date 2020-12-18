namespace Alexandria.Web.Controllers
{
    using System.Diagnostics;
    using System.Threading.Tasks;

    using Alexandria.Services.Books;
    using Alexandria.Web.ViewModels;
    using Alexandria.Web.ViewModels.Home;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        private const int TopRatedBooksCount = 4;
        private const int RandomBooksCount = 10;

        private readonly IBooksService booksService;

        public HomeController(IBooksService booksService)
        {
            this.booksService = booksService;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new HomeViewModel();
            viewModel.TopBooks = await this.booksService.GetTopRatedBooksAsync<HomeBookViewModel>(TopRatedBooksCount);
            viewModel.RandomBooks = await this.booksService.GetRandomBooksAsync<HomeBookViewModel>(RandomBooksCount);

            return this.View(viewModel);
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }

        public IActionResult NotFound404()
        {
            return this.View();
        }
    }
}
