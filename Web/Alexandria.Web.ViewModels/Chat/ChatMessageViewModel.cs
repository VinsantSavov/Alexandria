namespace Alexandria.Web.ViewModels.Chat
{
    using System;

    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;

    public class ChatMessageViewModel : IMapFrom<Message>
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public ChatUserViewModel Author { get; set; }
    }
}
