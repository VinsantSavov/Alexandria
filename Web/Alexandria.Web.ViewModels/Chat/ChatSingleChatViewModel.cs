namespace Alexandria.Web.ViewModels.Chat
{
    using System;

    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;

    public class ChatSingleChatViewModel : IMapFrom<Message>
    {
        public ChatUserViewModel Author { get; set; }

        public ChatUserViewModel Receiver { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
