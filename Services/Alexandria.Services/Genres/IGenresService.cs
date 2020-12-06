namespace Alexandria.Services.Genres
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IGenresService
    {
        Task CreateGenreAsync(string name, string description);

        Task DeleteGenreByIdAsync(int id);

        Task<TModel> GetGenreByIdAsync<TModel>(int id);

        Task<IEnumerable<TModel>> GetRandomGenresAsync<TModel>(int count);
    }
}
