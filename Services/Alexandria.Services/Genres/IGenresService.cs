namespace Alexandria.Services.Genres
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IGenresService
    {
        Task<int> CreateGenreAsync(string name, string description);

        Task DeleteGenreByIdAsync(int id);

        Task EditGenreAsync(int id, string name, string description);

        Task<bool> DoesGenreIdExistAsync(int id);

        Task<bool> DoesGenreNameExistAsync(string name);

        Task<TModel> GetGenreByIdAsync<TModel>(int id);

        Task<int> GetGenresCount();

        Task<IEnumerable<TModel>> GetRandomGenresAsync<TModel>(int count);

        Task<IEnumerable<TModel>> GetAllGenresAsync<TModel>(int? take = null, int skip = 0);
    }
}
