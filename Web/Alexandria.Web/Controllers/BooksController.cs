﻿namespace Alexandria.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Alexandria.Data.Models;
    using Alexandria.Services.Books;
    using Alexandria.Services.Likes;
    using Alexandria.Services.Reviews;
    using Alexandria.Web.Infrastructure.Extensions;
    using Alexandria.Web.ViewModels;
    using Alexandria.Web.ViewModels.Books;
    using Microsoft.AspNetCore.Mvc;

    public class BooksController : Controller
    {
        private const int BooksPerPage = 10;
        private const int ReviewsCount = 5;
        private const string ControllerName = "Books";

        private readonly IBooksService booksService;
        private readonly IReviewsService reviewsService;
        private readonly ILikesService likesService;

        public BooksController(
            IBooksService booksService,
            IReviewsService reviewsService,
            ILikesService likesService)
        {
            this.booksService = booksService;
            this.reviewsService = reviewsService;
            this.likesService = likesService;
        }

        public async Task<IActionResult> Details(int id)
        {
            var book = await this.booksService.GetBookByIdAsync<BooksDetailsViewModel>(id);
            if (book == null)
            {
                return this.NotFound();
            }

            book.CommunityReviews = await this.reviewsService.GetTopReviewsByBookIdAsync<ReviewListingViewModel>(id, ReviewsCount);
            var userId = this.User.GetUserId();

            foreach (var review in book.CommunityReviews)
            {
                review.UserLikedReview = await this.likesService.DoesUserLikeReviewAsync(userId, review.Id);
            }

            return this.View(book);
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

        public async Task<IActionResult> Search(string search, int page = 1)
        {
            var viewModel = new BooksSearchViewModel();
            viewModel.SearchString = search;

            int booksCount = await this.booksService.GetBooksCountAsync(search);

            viewModel.Books = await this.booksService.SearchBooksByTitleAsync<BooksSingleViewModel>(search, BooksPerPage, (page - 1) * BooksPerPage);
            viewModel.CurrentPage = page;
            viewModel.PagesCount = (int)Math.Ceiling((double)booksCount / BooksPerPage);
            viewModel.ControllerName = ControllerName;
            viewModel.ActionName = nameof(this.Search);

            return this.View(viewModel);
        }
    }
}
