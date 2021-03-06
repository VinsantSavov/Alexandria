﻿namespace Alexandria.Services.Books
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IBooksService
    {
        Task<int> CreateBookAsync(string title, int authorId, string summary, DateTime publishedOn, int pages, string pictureUrl, int editionLanguageId, string amazonLink, IEnumerable<int> genresIds, IEnumerable<int> tagsIds, IEnumerable<int> awardsIds);

        Task<TModel> GetBookByIdAsync<TModel>(int id);

        Task<IEnumerable<TModel>> SearchBooksByTitleAsync<TModel>(string search, int? take = null, int skip = 0);

        Task DeleteByIdAsync(int id);

        Task EditBookAsync(int id, string title, string summary, DateTime publishedOn, int pages, string pictureUrl, string amazonLink);

        Task<string> GetPictureUrlByBookIdAsync(int id);

        Task<bool> DoesBookIdExistAsync(int id);

        Task<int> GetBooksCountAsync(string search = null);

        Task<int> GetBooksCountByAuthorIdAsync(int authorId);

        Task<int> GetBooksCountByGenreIdAsync(int genreId);

        Task<IEnumerable<TModel>> GetRandomBooksAsync<TModel>(int count);

        Task<IEnumerable<TModel>> NewRealesedBooksByGenreIdAsync<TModel>(int genreId, int? take = null, int skip = 0);

        Task<IEnumerable<TModel>> TopRatedBooksByGenreIdAsync<TModel>(int genreId, int? take = null, int skip = 0);

        Task<IEnumerable<TModel>> GetAllBooksAsync<TModel>(int? take = null, int skip = 0);

        Task<IEnumerable<TModel>> GetTopRatedBooksAsync<TModel>(int? take = null, int skip = 0);

        Task<IEnumerable<TModel>> GetLatestPublishedBooksAsync<TModel>(int? take = null, int skip = 0);

        Task<IEnumerable<TModel>> GetAllBooksByAuthorIdAsync<TModel>(int authorId, int? take = null, int skip = 0);

        Task<IEnumerable<TModel>> GetTopRatedBooksByAuthorIdAsync<TModel>(int authorId, int count = 0);
    }
}
