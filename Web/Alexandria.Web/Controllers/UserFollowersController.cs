namespace Alexandria.Web.Controllers
{
    using System.Threading.Tasks;

    using Alexandria.Services.UserFollowers;
    using Alexandria.Web.Infrastructure.Extensions;
    using Alexandria.Web.ViewModels.UserFollowers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserFollowersController : BaseController
    {
        private readonly IUserFollowersService usersFollowersService;

        public UserFollowersController(IUserFollowersService usersFollowersService)
        {
            this.usersFollowersService = usersFollowersService;
        }

        [HttpPost]
        public async Task<ActionResult<UserFollowersResponseModel>> Follow(UserFollowersInputModel input)
        {
            var followerId = this.User.GetUserId();
            if (followerId == input.Id)
            {
                return this.BadRequest();
            }

            await this.usersFollowersService.CreateUserFollowerAsync(input.Id, followerId);

            var followersCount = await this.usersFollowersService.GetFollowersCountByUserIdAsync(input.Id);

            return new UserFollowersResponseModel { FollowersCount = followersCount };
        }
    }
}
