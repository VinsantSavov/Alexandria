namespace Alexandria.Web.Components
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Alexandria.Services.Users;
    using Alexandria.Web.Infrastructure.Extensions;
    using Alexandria.Web.ViewModels.Users;
    using Microsoft.AspNetCore.Mvc;

    [ViewComponent(Name = "LoginUser")]
    public class LoginUserViewComponent : ViewComponent
    {
        private readonly IUsersService usersService;

        public LoginUserViewComponent(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        public async Task<IViewComponentResult> InvokeAsync(ClaimsPrincipal claimsPrincipal)
        {
            var userId = claimsPrincipal.GetUserId();
            var user = await this.usersService.GetUserByIdAsync<UsersLoginUserViewModel>(userId);

            return this.View(user);
        }
    }
}
