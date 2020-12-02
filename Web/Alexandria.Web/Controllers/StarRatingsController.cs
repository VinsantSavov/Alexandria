namespace Alexandria.Web.Controllers
{
    using System.Threading.Tasks;

    using Alexandria.Services.StarRatings;
    using Alexandria.Web.Infrastructure.Extensions;
    using Alexandria.Web.InputModels.StarRatings;
    using Alexandria.Web.ViewModels.StarRatings;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class StarRatingsController : BaseController
    {
        private readonly IStarRatingsService ratingsService;

        public StarRatingsController(IStarRatingsService ratingsService)
        {
            this.ratingsService = ratingsService;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<StarRatingResponseModel>> Create(StarRatingInputModel input)
        {
            var userId = this.User.GetUserId();
            await this.ratingsService.CreateRatingAsync(input.Rate, userId, input.BookId);
            var ratings = await this.ratingsService.GetRatesCountByBookIdAsync(input.BookId);
            var averageRating = await this.ratingsService.GetAverageRatingByBookIdAsync(input.BookId);

            return new StarRatingResponseModel { RatingsCount = ratings, AverageRating = averageRating };
        }
    }
}
