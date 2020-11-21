namespace Alexandria.Services.Users
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IUsersService
    {
        Task DeleteUserAsync(string id);

        Task<TModel> GetUserByIdAsync<TModel>(string id);

        Task<IEnumerable<TModel>> GetAllUsersAsync<TModel>();

        Task<bool> IsUsernameUsedAsync(string username);

        Task<bool> IsUserDeletedAsync(string username);
    }
}
