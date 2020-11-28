namespace Alexandria.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Alexandria.Data.Models;
    using Alexandria.Services.Books;
    using Alexandria.Services.Likes;
    using Alexandria.Services.Reviews;
    using Alexandria.Web.ViewModels;
    using Alexandria.Web.ViewModels.Books;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class BooksController : Controller
    {
        private const int BooksPerPage = 10;
        private const int ReviewsCount = 5;
        private const string ControllerName = "Books";

        private readonly IBooksService booksService;
        private readonly IReviewsService reviewsService;
        private readonly ILikesService likesService;
        private readonly UserManager<ApplicationUser> userManager;

        public BooksController(
            IBooksService booksService,
            IReviewsService reviewsService,
            ILikesService likesService,
            UserManager<ApplicationUser> userManager)
        {
            this.booksService = booksService;
            this.reviewsService = reviewsService;
            this.likesService = likesService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Details(int id)
        {
            var book = await this.booksService.GetBookByIdAsync<BooksDetailsViewModel>(id);
            book.CommunityReviews = await this.reviewsService.GetTopReviewsByBookIdAsync<ReviewListingViewModel>(id, ReviewsCount);
            var userId = this.userManager.GetUserId(this.User);

            foreach (var review in book.CommunityReviews)
            {
                review.UserLikedReview = await this.likesService.DoesUserLikeReviewAsync(userId, review.Id);
            }

            return this.View(book);
        }

        public IActionResult All()
        {
            return this.View();
        }

        public async Task<IActionResult> NewReleases(int page = 1)
        {
            var viewModel = new BooksAllViewModel();

            int booksCount = await this.booksService.GetBooksCountAsync();

            viewModel.Books = await this.booksService.GetLatestPublishedBooksAsync<BooksSingleViewModel>(BooksPerPage, (page - 1) * BooksPerPage);
            viewModel.CurrentPage = page;
            viewModel.PagesCount = (int)Math.Ceiling((double)booksCount / BooksPerPage);
            viewModel.ControllerName = ControllerName;
            viewModel.ActionName = nameof(this.NewReleases);

            return this.View(viewModel);
        }

        public async Task<IActionResult> TopRated(int page = 1)
        {
            var viewModel = new BooksAllViewModel();

            int booksCount = await this.booksService.GetBooksCountAsync();

            viewModel.Books = await this.booksService.GetTopRatedBooksAsync<BooksSingleViewModel>(BooksPerPage, (page - 1) * BooksPerPage);
            viewModel.CurrentPage = page;
            viewModel.PagesCount = (int)Math.Ceiling((double)booksCount / BooksPerPage);
            viewModel.ControllerName = ControllerName;
            viewModel.ActionName = nameof(this.TopRated);

            return this.View(viewModel);
        }

        public async Task<IActionResult> Test(int id)
        {
            var book = await this.booksService.GetBookByIdAsync<BooksDetailsViewModel>(id);
            book.CommunityReviews = await this.reviewsService.GetTopReviewsByBookIdAsync<ReviewListingViewModel>(id, ReviewsCount);

            return this.View(book);
        }
    }
}
