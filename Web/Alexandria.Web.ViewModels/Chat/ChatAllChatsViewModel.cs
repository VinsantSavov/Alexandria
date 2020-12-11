namespace Alexandria.Web.ViewModels.Chat
{
    using System.Collections.Generic;

    public class ChatAllChatsViewModel
    {
        public ChatAllChatsViewModel()
        {
            this.Chats = new List<ChatSingleChatViewModel>();
        }

        public IList<ChatSingleChatViewModel> Chats { get; set; }
    }
}
