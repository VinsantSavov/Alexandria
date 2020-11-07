namespace Alexandria.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class BooksController : Controller
    {
        public IActionResult Details()
        {
            return this.View();
        }

        public IActionResult AllBooks()
        {
            return this.View();
        }
    }
}
