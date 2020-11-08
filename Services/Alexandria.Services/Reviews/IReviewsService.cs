namespace Alexandria.Services.Reviews
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Alexandria.Data.Models.Enums;

    public interface IReviewsService
    {
        Task CreateReviewAsync(string description, int? parentId, string authorId, int bookId, ReadingProgress readingProgress, bool thisEdition);

        Task<TModel> GetReviewByIdAsync<TModel>(int id);

        Task DeleteReviewByIdAsync(int id);

        Task EditReviewAsync(int id, string description);

        Task MakeBestReviewAsync(int id);

        Task<IEnumerable<TModel>> GetAllReviewsByAuthorIdAsync<TModel>(string authorId);

        Task<IEnumerable<TModel>> GetAllReviewsByBookIdAsync<TModel>(int bookId);
    }
}
