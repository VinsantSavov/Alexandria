namespace Alexandria.Web.Controllers
{
    using System.Threading.Tasks;

    using Alexandria.Data.Models;
    using Alexandria.Services.Likes;
    using Alexandria.Web.InputModels.Likes;
    using Alexandria.Web.ViewModels.Likes;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class LikesController : BaseController
    {
        private readonly ILikesService likesService;
        private readonly UserManager<ApplicationUser> userManager;

        public LikesController(
            ILikesService likesService,
            UserManager<ApplicationUser> userManager)
        {
            this.likesService = likesService;
            this.userManager = userManager;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<LikesResponseModel>> Create(LikesCreateInputModel input)
        {
            var userId = this.userManager.GetUserId(this.User);
            await this.likesService.CreateLikeAsync(userId, input.ReviewId, input.IsLiked);
            var likesCount = await this.likesService.GetLikesCountByReviewIdAsync(input.ReviewId);

            return new LikesResponseModel { LikesCount = likesCount };
        }
    }
}
