namespace Alexandria.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class GenresController : Controller
    {
        public IActionResult Details()
        {
            return this.View();
        }
    }
}
