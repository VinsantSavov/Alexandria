namespace Alexandria.Services.StarRatings
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IStarRatingsService
    {
        Task CreateRatingAsync(int rate, string userId, int bookId);

        Task<IEnumerable<TModel>> GetAllRatesByUserIdAsync<TModel>(string userId);

        Task<int> GetAllRatesByBookIdAsync(int bookId);

        Task<double> GetAverageRatingByBookIdAsync(int bookId);
    }
}
