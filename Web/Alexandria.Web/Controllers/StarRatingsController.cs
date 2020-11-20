namespace Alexandria.Web.Controllers
{
    using System.Threading.Tasks;

    using Alexandria.Data.Models;
    using Alexandria.Services.StarRatings;
    using Alexandria.Web.InputModels.StarRatings;
    using Alexandria.Web.ViewModels.StarRatings;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class StarRatingsController : BaseController
    {
        private readonly IStarRatingsService ratingsService;
        private readonly UserManager<ApplicationUser> userManager;

        public StarRatingsController(IStarRatingsService ratingsService, UserManager<ApplicationUser> userManager)
        {
            this.ratingsService = ratingsService;
            this.userManager = userManager;
        }

        [HttpPost]
        public async Task<ActionResult<StarRatingResponseModel>> Create(StarRatingInputModel input)
        {
            var userId = this.userManager.GetUserId(this.User);
            await this.ratingsService.CreateRatingAsync(input.Rate, userId, input.BookId);
            var ratings = await this.ratingsService.GetAllRatesByBookIdAsync(input.BookId);

            return new StarRatingResponseModel { RatingsCount = ratings };
        }
    }
}
