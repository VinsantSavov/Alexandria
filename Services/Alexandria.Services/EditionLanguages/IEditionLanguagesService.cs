namespace Alexandria.Services.EditionLanguages
{
    using System.Threading.Tasks;

    public interface IEditionLanguagesService
    {
        Task CreateEditionLanguageAsync(string name);

        Task<TModel> GetEditionLanguageByIdAsync<TModel>(int id);
    }
}
