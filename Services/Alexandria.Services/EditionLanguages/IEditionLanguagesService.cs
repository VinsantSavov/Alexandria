namespace Alexandria.Services.EditionLanguages
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IEditionLanguagesService
    {
        Task CreateEditionLanguageAsync(string name);

        Task<bool> DoesEditionLanguageIdExistAsync(int id);

        Task<IEnumerable<TModel>> GetAllLanguagesAsync<TModel>();

        Task<TModel> GetEditionLanguageByIdAsync<TModel>(int id);
    }
}
