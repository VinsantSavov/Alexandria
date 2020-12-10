namespace Alexandria.Web.Controllers
{
    using System.Threading.Tasks;

    using Alexandria.Services.Messages;
    using Alexandria.Services.Users;
    using Alexandria.Web.Infrastructure.Extensions;
    using Alexandria.Web.ViewModels.Chat;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class ChatController : Controller
    {
        private const int LatestMessagesCount = 20;

        private readonly IUsersService usersService;
        private readonly IMessagesService messagesService;

        public ChatController(
            IUsersService usersService,
            IMessagesService messagesService)
        {
            this.usersService = usersService;
            this.messagesService = messagesService;
        }

        public async Task<IActionResult> ChatWithUser(string id)
        {
            var currentUserId = this.User.GetUserId();

            var viewModel = new ChatWithUserViewModel
            {
                User = await this.usersService.GetUserByIdAsync<ChatUserViewModel>(id),
                LatestMessages = await this.messagesService.GetAllMessagesByUserIdAsync<ChatMessageViewModel>(currentUserId, id, LatestMessagesCount),
            };

            return this.View(viewModel);
        }

        public IActionResult Chat() => this.View();
    }
}
