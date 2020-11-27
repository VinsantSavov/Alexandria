namespace Alexandria.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Alexandria.Data.Models;
    using Alexandria.Services.Books;
    using Alexandria.Services.Reviews;
    using Alexandria.Web.ViewModels;
    using Alexandria.Web.ViewModels.Reviews;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class ReviewsController : Controller
    {
        private const int ReviewsPerPage = 3;
        private const int CommentsPerPage = 3;
        private const string ControllerName = "Reviews";

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

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id, int page = 1)
        {
            var review = await this.reviewsService.GetReviewByIdAsync<ReviewsDetailsViewModel>(id);

            var reviewsCount = await this.reviewsService.GetChildrenReviewsCountByReviewIdAsync(id);

            review.PagesCount = (int)Math.Ceiling((double)reviewsCount / CommentsPerPage);
            review.CurrentPage = page;
            review.ControllerName = ControllerName;
            review.ActionName = nameof(this.Details);
            review.Comments = await this.reviewsService.GetChildrenReviewsByReviewIdAsync<ReviewListingViewModel>(id, CommentsPerPage, (page - 1) * CommentsPerPage);

            return this.View(review);
        }

        [AllowAnonymous]
        public async Task<IActionResult> All(int id, int page = 1)
        {
            var viewModel = await this.booksService.GetBookByIdAsync<ReviewsAllViewModel>(id);

            var reviewsCount = await this.reviewsService.GetReviewsCountByBookIdAsync(id);

            viewModel.PagesCount = (int)Math.Ceiling((double)reviewsCount / ReviewsPerPage);
            viewModel.CurrentPage = page;
            viewModel.ControllerName = ControllerName;
            viewModel.ActionName = nameof(this.All);
            viewModel.AllReviews = (ICollection<ReviewListingViewModel>)await this.reviewsService.GetAllReviewsByBookIdAsync<ReviewListingViewModel>(id, ReviewsPerPage, (page - 1) * ReviewsPerPage);

            var all = await this.reviewsService.GetChildrenReviewsToReviewsAsync<ReviewListingViewModel>(viewModel.AllReviews.Select(r => r.Id).ToList(), id);

            foreach (var review in all)
            {
                viewModel.AllReviews.Add(review);
            }

            return this.View(viewModel);
        }

        public async Task<IActionResult> Create(int id)
        {
            var viewModel = await this.booksService.GetBookByIdAsync<ReviewsCreateInputModel>(id);

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ReviewsCreateInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                var bookInfo = await this.booksService.GetBookByIdAsync<ReviewsCreateInputModel>(input.BookId);
                input.PictureURL = bookInfo.PictureURL;
                input.Title = bookInfo.Title;
                input.Author = bookInfo.Author;
                input.AuthorId = bookInfo.AuthorId;

                return this.View(input);
            }

            if (input.ReviewId.HasValue)
            {
                var areAboutSameBook = await this.reviewsService.AreReviewsAboutSameBookAsync(input.ReviewId.Value, input.BookId);

                if (!areAboutSameBook)
                {
                    return this.BadRequest();
                }
            }

            var userId = this.userManager.GetUserId(this.User);
            var reviewId = await this.reviewsService.CreateReviewAsync(input.Description, userId, input.BookId, input.ReadingProgress, input.ThisEdition, input.ReviewId);

            if (input.ReviewId != null)
            {
                return this.RedirectToAction(nameof(this.Details), new { id = input.ReviewId });
            }

            return this.RedirectToAction(nameof(this.Details), new { id = reviewId });
        }
    }
}
