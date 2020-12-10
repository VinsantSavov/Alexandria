namespace Alexandria.Services.Messages
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IMessagesService
    {
        Task CreateMessageAsync(string authorId, string receiverId, string content);

        Task<IEnumerable<TModel>> GetAllMessagesByUserIdAsync<TModel>(string currentUserId, string userId, int? take = null, int skip = 0);
    }
}
