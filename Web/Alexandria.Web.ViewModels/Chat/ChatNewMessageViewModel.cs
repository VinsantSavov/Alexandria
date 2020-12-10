namespace Alexandria.Web.ViewModels.Chat
{
    using System;

    public class ChatNewMessageViewModel
    {
        public string AuthorId { get; set; }

        public string AuthorUsername { get; set; }

        public string AuthorProfilePicture { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
