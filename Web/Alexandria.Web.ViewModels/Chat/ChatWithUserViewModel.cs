namespace Alexandria.Web.ViewModels.Chat
{
    using System.Collections.Generic;

    public class ChatWithUserViewModel
    {
        public ChatUserViewModel User { get; set; }

        public IEnumerable<ChatMessageViewModel> LatestMessages { get; set; }
    }
}
