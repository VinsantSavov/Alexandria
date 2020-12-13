namespace Alexandria.Services.EditionLanguages
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IEditionLanguagesService
    {
        Task<bool> DoesEditionLanguageIdExistAsync(int id);

        Task<IEnumerable<TModel>> GetAllLanguagesAsync<TModel>();
    }
}
