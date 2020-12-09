namespace Alexandria.Services.Authors
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Alexandria.Data.Models;

    public interface IAuthorsService
    {
        Task CreateAuthorAsync(string firstName, string secondName, string lastName, string profilePicture, int countryId, string biography, DateTime bornOn);

        Task<TModel> GetAuthorByIdAsync<TModel>(int id);

        Task DeleteAuthorByIdAsync(int id);

        Task EditAuthorBiographyAsync(int id, string biography);

        Task<bool> DoesAuthorIdExistAsync(int id);

        Task<IEnumerable<TModel>> GetAllAuthorsAsync<TModel>();

        Task<IEnumerable<TModel>> GetMostPopularAuthorsByCountryAsync<TModel>(int countryId, int count = 0);
    }
}
