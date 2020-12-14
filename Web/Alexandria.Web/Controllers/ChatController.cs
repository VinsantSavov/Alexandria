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
        private const int LatestMessagesCount = 5;

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
                LatestMessages = await this.messagesService.GetAllMessagesBetweenUsersAsync<ChatMessageViewModel>(currentUserId, id, LatestMessagesCount),
            };

            return this.View(viewModel);
        }

        public async Task<IActionResult> SendMessage()
        {
            var userId = this.User.GetUserId();
            var viewModel = new ChatSendMessageInputModel();
            viewModel.Users = await this.usersService.GetChatUsersAsync<ChatUserViewModel>(userId);

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(ChatSendMessageInputModel input)
        {
            var userId = this.User.GetUserId();

            if (!this.ModelState.IsValid)
            {
                input.Users = await this.usersService.GetChatUsersAsync<ChatUserViewModel>(userId);

                return this.View(input);
            }

            await this.messagesService.CreateMessageAsync(userId, input.UserId, input.Content);

            return this.RedirectToAction(nameof(this.ChatWithUser), new { Id = input.UserId });
        }
    }
}
