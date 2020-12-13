namespace Alexandria.Services.Awards
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IAwardsService
    {
        Task<bool> DoesAwardIdExistAsync(int id);

        Task<IEnumerable<TModel>> GetAllAwardsAsync<TModel>();
    }
}
