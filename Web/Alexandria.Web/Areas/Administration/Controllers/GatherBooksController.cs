namespace Alexandria.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Alexandria.Services.Scrapers;
    using Microsoft.AspNetCore.Mvc;

    public class GatherBooksController : AdministrationController
    {
        private readonly IGoodReadsScraperService scraper;

        public GatherBooksController(IGoodReadsScraperService scraper)
        {
            this.scraper = scraper;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        public async Task<IActionResult> Add()
        {
            await this.scraper.PopulateDatabaseWithBooks(500);

            return this.View();
        }
    }
}
