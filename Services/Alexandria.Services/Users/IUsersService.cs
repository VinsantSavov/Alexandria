namespace Alexandria.Services.Users
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Alexandria.Data.Models.Enums;

    public interface IUsersService
    {
        Task DeleteUserAsync(string id);

        Task<TModel> GetUserByIdAsync<TModel>(string id);

        Task EditUserAsync(string id, GenderType gender, string profilePicture, string biography);

        Task<IEnumerable<TModel>> GetAllUsersAsync<TModel>();

        Task<bool> IsUsernameUsedAsync(string username);

        Task<bool> IsUserDeletedAsync(string username);
    }
}
