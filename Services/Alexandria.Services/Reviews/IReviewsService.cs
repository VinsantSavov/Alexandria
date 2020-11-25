namespace Alexandria.Services.Reviews
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Alexandria.Data.Models.Enums;

    public interface IReviewsService
    {
        Task<int> CreateReviewAsync(string description, string authorId, int bookId, ReadingProgress readingProgress, bool thisEdition, int? parentId = null);

        Task<TModel> GetReviewByIdAsync<TModel>(int id);

        Task DeleteReviewByIdAsync(int id);

        Task EditReviewAsync(int id, string description);

        Task<bool> DoesReviewIdExistAsync(int id);

        Task<bool> AreReviewsAboutSameBookAsync(int reviewId, int bookId);

        Task MakeBestReviewAsync(int id);

        Task<IEnumerable<TModel>> GetChildrenReviewsByReviewIdAsync<TModel>(int reviewId);

        Task<IEnumerable<TModel>> GetAllReviewsByAuthorIdAsync<TModel>(string authorId);

        Task<IEnumerable<TModel>> GetAllReviewsByBookIdAsync<TModel>(int bookId);

        Task<IEnumerable<TModel>> GetTopReviewsByBookIdAsync<TModel>(int bookId, int count);
    }
}
