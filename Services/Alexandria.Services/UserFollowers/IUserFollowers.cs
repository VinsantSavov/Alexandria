namespace Alexandria.Services.UserFollowers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IUserFollowers
    {
        Task CreateUserFollowerAsync(string userId, string followerId);

        Task DeleteUserFollowerAsync(string userId, string followerId);

        Task<bool> DoesUserFollowerExistAsync(string userId, string followerId);

        Task<int> GetFollowersCountByUserIdAsync(string userId);

        Task<IEnumerable<TModel>> GetAllFollowersByUserIdAsync<TModel>(string userId);
    }
}
