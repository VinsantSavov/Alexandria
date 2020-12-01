namespace Alexandria.Web.Controllers
{
    using System.Threading.Tasks;

    using Alexandria.Services.Reviews;
    using Alexandria.Services.StarRatings;
    using Alexandria.Services.UserFollowers;
    using Alexandria.Services.Users;
    using Alexandria.Web.ViewModels.Users;
    using Microsoft.AspNetCore.Mvc;

    public class UsersController : Controller
    {
        private const int RatingsCount = 5;
        private const int ReviewsCount = 5;

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

            user.TopRatings = await this.starRatingsService.GetAllRatesByUserIdAsync<UsersRatingViewModel>(id, RatingsCount);
            user.TopReviews = await this.reviewsService.GetAllReviewsByAuthorIdAsync<UsersReviewViewModel>(id, ReviewsCount);

            return this.View(user);
        }

        public async Task<IActionResult> Ratings(string id)
        {
            return this.View();
        }

        public async Task<IActionResult> Reviews(string id)
        {
            return this.View();
        }
    }
}
