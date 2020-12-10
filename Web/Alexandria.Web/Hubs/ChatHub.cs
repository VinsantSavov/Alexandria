namespace Alexandria.Web.Hubs
{
    using System;
    using System.Threading.Tasks;

    using Alexandria.Services.Messages;
    using Alexandria.Services.Users;
    using Alexandria.Web.ViewModels.Chat;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SignalR;

    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IMessagesService messagesService;
        private readonly IUsersService usersService;

        public ChatHub(
            IMessagesService messagesService,
            IUsersService usersService)
        {
            this.messagesService = messagesService;
            this.usersService = usersService;
        }

        public async Task SendMessage(string message, string receiverId)
        {
            var authorId = this.Context.UserIdentifier;
            var author = await this.usersService.GetUserByIdAsync<ChatUserViewModel>(authorId);

            await this.messagesService.CreateMessageAsync(authorId, receiverId, message);

            await this.Clients.All.SendAsync(
                "NewMessage",
                new ChatNewMessageViewModel
                {
                    AuthorId = author.Id,
                    AuthorUsername = author.Username,
                    AuthorProfilePicture = author.ProfilePicture,
                    Content = message,
                    CreatedOn = DateTime.UtcNow,
                });
        }
    }
}
