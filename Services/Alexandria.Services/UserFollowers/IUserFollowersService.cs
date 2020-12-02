namespace Alexandria.Services.UserFollowers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IUserFollowersService
    {
        Task CreateUserFollowerAsync(string userId, string followerId);

        Task<bool> DoesUserFollowerExistAsync(string userId, string followerId);

        Task<int> GetFollowersCountByUserIdAsync(string userId);

        Task<int> GetFollowingCountByUserIdAsync(string userId);

        Task<IEnumerable<TModel>> GetAllFollowersByUserIdAsync<TModel>(string userId, int? take = null, int skip = 0);

        Task<IEnumerable<TModel>> GetAllFollowingByUserIdAsync<TModel>(string userId, int? take = null, int skip = 0);
    }
}
