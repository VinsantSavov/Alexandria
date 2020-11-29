namespace Alexandria.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Alexandria.Services.Books;
    using Alexandria.Services.Likes;
    using Alexandria.Services.Reviews;
    using Alexandria.Web.Infrastructure.Extensions;
    using Alexandria.Web.ViewModels;
    using Alexandria.Web.ViewModels.Reviews;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class ReviewsController : Controller
    {
        private const int ReviewsPerPage = 3;
        private const int CommentsPerPage = 3;
        private const string ControllerName = "Reviews";

        private readonly IBooksService booksService;
        private readonly IReviewsService reviewsService;
        private readonly ILikesService likesService;

        public ReviewsController(
            IBooksService booksService,
            IReviewsService reviewsService,
            ILikesService likesService)
        {
            this.booksService = booksService;
            this.reviewsService = reviewsService;
            this.likesService = likesService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id, int page = 1)
        {
            var review = await this.reviewsService.GetReviewByIdAsync<ReviewsDetailsViewModel>(id);
            var userId = this.User.GetUserId();

            var reviewsCount = await this.reviewsService.GetChildrenReviewsCountByReviewIdAsync(id);

            review.PagesCount = (int)Math.Ceiling((double)reviewsCount / CommentsPerPage);
            review.CurrentPage = page;
            review.ControllerName = ControllerName;
            review.ActionName = nameof(this.Details);
            review.Comments = await this.reviewsService.GetChildrenReviewsByReviewIdAsync<ReviewListingViewModel>(id, CommentsPerPage, (page - 1) * CommentsPerPage);

            review.UserLikedReview = await this.likesService.DoesUserLikeReviewAsync(userId, review.Id);

            foreach (var comment in review.Comments)
            {
                comment.UserLikedReview = await this.likesService.DoesUserLikeReviewAsync(userId, comment.Id);
            }

            return this.View(review);
        }

        [AllowAnonymous]
        public async Task<IActionResult> All(int id, int page = 1)
        {
            var viewModel = await this.booksService.GetBookByIdAsync<ReviewsAllViewModel>(id);
            var userId = this.User.GetUserId();

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

            foreach (var review in viewModel.AllReviews)
            {
                review.UserLikedReview = await this.likesService.DoesUserLikeReviewAsync(userId, review.Id);
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
                var bookInfo = await this.booksService.GetBookByIdAsync<ReviewsCreateInputModel>(input.Id);
                bookInfo.Description = input.Description;
                bookInfo.ReadingProgress = input.ReadingProgress;
                bookInfo.ThisEdition = input.ThisEdition;

                return this.View(bookInfo);
            }

            if (input.ReviewId.HasValue)
            {
                var areAboutSameBook = await this.reviewsService.AreReviewsAboutSameBookAsync(input.ReviewId.Value, input.Id);

                if (!areAboutSameBook)
                {
                    return this.BadRequest();
                }
            }

            var userId = this.User.GetUserId();
            var reviewId = await this.reviewsService.CreateReviewAsync(input.Description, userId, input.Id, input.ReadingProgress, input.ThisEdition, input.ReviewId);

            if (input.ReviewId != null)
            {
                return this.RedirectToAction(nameof(this.Details), new { id = input.ReviewId });
            }

            return this.RedirectToAction(nameof(this.Details), new { id = reviewId });
        }
    }
}
