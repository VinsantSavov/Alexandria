namespace Alexandria.Services.Authors
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Alexandria.Data.Models;

    public interface IAuthorsService
    {
        Task<TModel> GetAuthorByIdAsync<TModel>(int id);

        Task<bool> DoesAuthorIdExistAsync(int id);

        Task<IEnumerable<TModel>> GetAllAuthorsAsync<TModel>();
    }
}
