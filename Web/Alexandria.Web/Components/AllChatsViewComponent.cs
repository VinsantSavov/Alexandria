namespace Alexandria.Web.Components
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Alexandria.Services.Messages;
    using Alexandria.Web.ViewModels.Chat;
    using Microsoft.AspNetCore.Mvc;

    [ViewComponent(Name = "AllChats")]
    public class AllChatsViewComponent : ViewComponent
    {
        private readonly IMessagesService messagesService;

        public AllChatsViewComponent(IMessagesService messagesService)
        {
            this.messagesService = messagesService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = this.UserClaimsPrincipal.FindFirst(ClaimTypes.NameIdentifier);

            var viewModel = new ChatAllChatsViewModel();

            var distinct = await this.messagesService.GetAllDistinctChatsAsync(userId.Value);

            foreach (var tuple in distinct)
            {
                var chat = await this.messagesService.GetLatestChatMessagesAsync<ChatSingleChatViewModel>(tuple.Item1, tuple.Item2);

                viewModel.Chats.Add(chat);
            }

            return this.View(viewModel);
        }
    }
}
