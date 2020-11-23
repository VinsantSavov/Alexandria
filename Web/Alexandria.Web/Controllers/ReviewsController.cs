namespace Alexandria.Web.Controllers
{
    using System.Threading.Tasks;

    using Alexandria.Services.Books;
    using Alexandria.Web.ViewModels.Reviews;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Components.Forms;
    using Microsoft.AspNetCore.Mvc;

    public class ReviewsController : Controller
    {
        private readonly IBooksService booksService;

        public ReviewsController(IBooksService booksService)
        {
            this.booksService = booksService;
        }

        [Authorize]
        public async Task<IActionResult> Create(int id)
        {
            var viewModel = await this.booksService.GetBookByIdAsync<ReviewsCreateInputModel>(id);

            return this.View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(ReviewsCreateInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                var bookInfo = await this.booksService.GetBookByIdAsync<ReviewsCreateInputModel>(input.Id);
                input.PictureURL = bookInfo.PictureURL;
                input.Title = bookInfo.Title;
                input.Author = bookInfo.Author;
                input.AuthorId = bookInfo.AuthorId;

                return this.View(input);
            }

            return this.Json(input);
            return this.RedirectToAction("Create");
        }
    }
}
