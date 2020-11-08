namespace Alexandria.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class GenresController : Controller
    {
        public IActionResult Details()
        {
            return this.View();
        }

        public IActionResult NewReleases()
        {
            return this.View();
        }

        public IActionResult TopRated()
        {
            return this.View();
        }
    }
}
