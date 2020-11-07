namespace Alexandria.Services.StarRatings
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IStarRatingsService
    {
        Task CreateRatingAsync(int rate, string userId, int bookId);

        Task DeleteRatingAsync(int id);

        Task<IEnumerable<TModel>> GetAllRatesByUserIdAsync<TModel>(string userId);

        Task<IEnumerable<TModel>> GetAllRatesByBookIdAsync<TModel>(int bookId);
    }
}
