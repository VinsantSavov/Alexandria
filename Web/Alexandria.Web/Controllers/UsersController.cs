namespace Alexandria.Web.Controllers
{
    using System.Threading.Tasks;

    using Alexandria.Services.UserFollowers;
    using Alexandria.Services.Users;
    using Alexandria.Web.ViewModels.Users;
    using Microsoft.AspNetCore.Mvc;

    public class UsersController : Controller
    {
        private readonly IUsersService usersService;
        private readonly IUserFollowersService userFollowers;

        public UsersController(
            IUsersService usersService,
            IUserFollowersService userFollowers)
        {
            this.usersService = usersService;
            this.userFollowers = userFollowers;
        }

        public async Task<IActionResult> Details(string id)
        {
            var user = await this.usersService.GetUserByIdAsync<UsersDetailsViewModel>(id);
            if (user == null)
            {
                return this.NotFound();
            }

            return this.View(user);
        }
    }
}
