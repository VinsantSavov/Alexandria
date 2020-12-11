namespace Alexandria.Web.ViewModels.Chat
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Alexandria.Common;
    using Alexandria.Web.Infrastructure.Attributes;

    public class ChatSendMessageInputModel
    {
        [Required]
        [MaxLength(GlobalConstants.MessageContentMaxLength, ErrorMessage = ErrorMessages.MessageContentLengthErrorMessage)]
        public string Content { get; set; }

        [EnsureUserIdExists(ErrorMessage = ErrorMessages.UserInvalidUser)]
        public string UserId { get; set; }

        public IEnumerable<ChatUserViewModel> Users { get; set; }
    }
}
