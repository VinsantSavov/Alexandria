namespace Alexandria.Web.Controllers
{
    using System.Threading.Tasks;

    using Alexandria.Services.Likes;
    using Alexandria.Web.Infrastructure.Extensions;
    using Alexandria.Web.InputModels.Likes;
    using Alexandria.Web.ViewModels.Likes;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class LikesController : BaseController
    {
        private readonly ILikesService likesService;

        public LikesController(ILikesService likesService)
        {
            this.likesService = likesService;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<LikesResponseModel>> Create(LikesCreateInputModel input)
        {
            var userId = this.User.GetUserId();
            await this.likesService.CreateLikeAsync(userId, input.ReviewId, input.IsLiked);
            var likesCount = await this.likesService.GetLikesCountByReviewIdAsync(input.ReviewId);

            return new LikesResponseModel { LikesCount = likesCount };
        }
    }
}
