﻿namespace Alexandria.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Alexandria.Services.Reviews;
    using Alexandria.Services.StarRatings;
    using Alexandria.Services.UserFollowers;
    using Alexandria.Services.Users;
    using Alexandria.Web.ViewModels.Users;
    using Microsoft.AspNetCore.Mvc;

    public class UsersController : Controller
    {
        private const int RatingsPerPage = 5;
        private const int ReviewsPerPage = 1;
        private const string ControllerName = "Users";

        private readonly IUsersService usersService;
        private readonly IUserFollowersService userFollowersService;
        private readonly IStarRatingsService starRatingsService;
        private readonly IReviewsService reviewsService;

        public UsersController(
            IUsersService usersService,
            IUserFollowersService userFollowersService,
            IStarRatingsService starRatingsService,
            IReviewsService reviewsService)
        {
            this.usersService = usersService;
            this.userFollowersService = userFollowersService;
            this.starRatingsService = starRatingsService;
            this.reviewsService = reviewsService;
        }

        public async Task<IActionResult> Details(string id)
        {
            var user = await this.usersService.GetUserByIdAsync<UsersDetailsViewModel>(id);
            if (user == null)
            {
                return this.NotFound();
            }

            user.TopRatings = await this.starRatingsService.GetAllRatesByUserIdAsync<UsersSingleRatingViewModel>(id, RatingsPerPage);
            user.TopReviews = await this.reviewsService.GetAllReviewsByAuthorIdAsync<UsersSingleReviewViewModel>(id, ReviewsPerPage);

            return this.View(user);
        }

        public async Task<IActionResult> Ratings(string id, int page = 1)
        {
            var user = await this.usersService.GetUserByIdAsync<UsersRatingViewModel>(id);
            if (user == null)
            {
                return this.NotFound();
            }

            int ratingsCount = await this.starRatingsService.GetRatesCountByUserIdAsync(id);

            user.CurrentPage = page;
            user.PagesCount = (int)Math.Ceiling((double)ratingsCount / RatingsPerPage);
            user.ControllerName = ControllerName;
            user.ActionName = nameof(this.Ratings);
            user.AllRatings = await this.starRatingsService.GetAllRatesByUserIdAsync<UsersSingleRatingViewModel>(id, RatingsPerPage, (page - 1) * RatingsPerPage);

            return this.View(user);
        }

        public async Task<IActionResult> Reviews(string id, int page = 1)
        {
            var user = await this.usersService.GetUserByIdAsync<UsersReviewViewModel>(id);
            if (user == null)
            {
                return this.NotFound();
            }

            var reviewsCount = await this.reviewsService.GetReviewsCountByUserIdAsync(id);

            user.CurrentPage = page;
            user.PagesCount = (int)Math.Ceiling((double)reviewsCount / ReviewsPerPage);
            user.ControllerName = ControllerName;
            user.ActionName = nameof(this.Reviews);
            user.AllReviews = await this.reviewsService.GetAllReviewsByAuthorIdAsync<UsersSingleReviewViewModel>(id, ReviewsPerPage, (page - 1) * ReviewsPerPage);

            return this.View(user);
        }
    }
}
