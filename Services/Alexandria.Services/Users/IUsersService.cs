namespace Alexandria.Services.Users
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IUsersService
    {
        Task<TModel> GetUserByIdAsync<TModel>(string id);

        Task<IEnumerable<TModel>> GetChatUsersAsync<TModel>(string id);

        Task<bool> DoesUserIdExistAsync(string userId);

        Task<bool> IsUsernameUsedAsync(string username);

        Task<bool> IsUserDeletedAsync(string username);
    }
}
