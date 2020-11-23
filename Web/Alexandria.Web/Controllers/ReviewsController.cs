﻿namespace Alexandria.Web.Controllers
{
    using System.Threading.Tasks;

    using Alexandria.Data.Models;
    using Alexandria.Services.Books;
    using Alexandria.Services.Reviews;
    using Alexandria.Web.ViewModels.Reviews;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class ReviewsController : Controller
    {
        private readonly IBooksService booksService;
        private readonly IReviewsService reviewsService;
        private readonly UserManager<ApplicationUser> userManager;

        public ReviewsController(
            IBooksService booksService,
            IReviewsService reviewsService,
            UserManager<ApplicationUser> userManager)
        {
            this.booksService = booksService;
            this.reviewsService = reviewsService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Details(int id)
        {
            var review = await this.reviewsService.GetReviewByIdAsync<ReviewsDetailsViewModel>(id);

            return this.View(review);
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

            var userId = this.userManager.GetUserId(this.User);
            var reviewId = await this.reviewsService.CreateReviewAsync(input.Description, null, userId, input.Id, input.ReadingProgress, input.ThisEdition);

            return this.RedirectToAction(nameof(this.Details), reviewId);
        }
    }
}
