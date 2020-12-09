namespace Alexandria.Services.Awards
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IAwardsService
    {
        Task CreateAwardAsync(string name);

        Task DeleteAwardByIdAsync(int id);

        Task<bool> DoesAwardIdExistAsync(int id);

        Task<IEnumerable<TModel>> GetAllAwardsAsync<TModel>();
    }
}
