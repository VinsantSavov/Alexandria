namespace Alexandria.Services.Countries
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICountriesService
    {
        Task CreateCountryAsync(string name);

        Task<TModel> GetCountryByIdAsync<TModel>(int countryId);
    }
}
