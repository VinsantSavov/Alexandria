namespace Alexandria.Services.Reviews
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Alexandria.Data;
    using Alexandria.Data.Models;
    using Alexandria.Data.Models.Enums;
    using Alexandria.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class ReviewsService : IReviewsService
    {
        private readonly AlexandriaDbContext db;

        public ReviewsService(AlexandriaDbContext db)
        {
            this.db = db;
        }

        public async Task<bool> AreReviewsAboutSameBookAsync(int reviewId, int bookId)
        {
            var reviewBookId = await this.db.Reviews.Where(r => r.Id == reviewId && !r.IsDeleted)
                                                    .Select(r => r.BookId)
                                                    .FirstOrDefaultAsync();

            return reviewBookId == bookId;
        }

        public async Task<int> CreateReviewAsync(string description, string authorId, int bookId, ReadingProgress readingProgress, bool thisEdition, int? parentId = null)
        {
            var review = new Review
            {
                Description = description,
                ParentId = parentId,
                AuthorId = authorId,
                BookId = bookId,
                ReadingProgress = readingProgress,
                ThisEdition = thisEdition,
                CreatedOn = DateTime.UtcNow,
            };

            await this.db.Reviews.AddAsync(review);
            await this.db.SaveChangesAsync();

            return review.Id;
        }

        public async Task DeleteReviewByIdAsync(int id)
        {
            var review = await this.GetByIdAsync(id);

            review.IsDeleted = true;
            review.DeletedOn = DateTime.UtcNow;

            await this.db.SaveChangesAsync();
        }

        public async Task<int> GetChildrenReviewsCountByReviewIdAsync(int parentId)
            => await this.db.Reviews.Where(r => r.ParentId == parentId && !r.IsDeleted).CountAsync();

        public async Task<int> GetReviewsCountByBookIdAsync(int bookId)
            => await this.db.Reviews.Where(r => r.BookId == bookId
                                                    && r.ParentId == null
                                                    && !r.IsDeleted)
                                        .CountAsync();

        public async Task<int> GetReviewsCountByUserIdAsync(string userId)
            => await this.db.Reviews.Where(r => r.AuthorId == userId
                                                    && r.ParentId == null
                                                    && !r.IsDeleted)
                                        .CountAsync();

        public async Task<string> GetAuthorIdByIdAsync(int id)
            => await this.db.Reviews.Where(r => r.Id == id && !r.IsDeleted)
                                          .Select(r => r.AuthorId)
                                          .FirstOrDefaultAsync();

        public async Task<bool> DoesReviewIdExistAsync(int id)
            => await this.db.Reviews.AnyAsync(r => r.Id == id && !r.IsDeleted);

        public async Task EditReviewAsync(int id, string description, ReadingProgress readingProgress, bool thisEdition)
        {
            var review = await this.GetByIdAsync(id);

            review.Description = description;
            review.ReadingProgress = readingProgress;
            review.ThisEdition = thisEdition;
            review.ModifiedOn = DateTime.UtcNow;

            await this.db.SaveChangesAsync();
        }

        public async Task<IEnumerable<TModel>> GetAllReviewsByAuthorIdAsync<TModel>(string authorId, int? take = null, int skip = 0)
        {
            var queryable = this.db.Reviews.AsNoTracking()
                                           .Where(r => r.AuthorId == authorId
                                                        && !r.IsDeleted
                                                        && r.ParentId == null)
                                           .OrderByDescending(r => r.Likes.Count(l => l.IsLiked))
                                           .ThenByDescending(r => r.CreatedOn)
                                           .Skip(skip);

            if (take.HasValue)
            {
                queryable = queryable.Take(take.Value);
            }

            return await queryable.To<TModel>().ToListAsync();
        }

        public async Task<IEnumerable<TModel>> GetAllReviewsByBookIdAsync<TModel>(int bookId, int? take = null, int skip = 0)
        {
            var queryable = this.db.Reviews.AsNoTracking()
                                         .Where(r => r.BookId == bookId
                                                 && r.ParentId == null
                                                 && !r.IsDeleted)
                                         .OrderByDescending(r => r.Likes.Count(l => l.IsLiked))
                                         .ThenByDescending(r => r.CreatedOn)
                                         .Skip(skip);

            if (take.HasValue)
            {
                queryable = queryable.Take(take.Value);
            }

            return await queryable.To<TModel>().ToListAsync();
        }

        public async Task<IEnumerable<TModel>> GetChildrenReviewsByReviewIdAsync<TModel>(int reviewId, int? take = null, int skip = 0)
        {
            var queryable = this.db.Reviews.AsNoTracking()
                                           .Where(r => r.ParentId == reviewId && !r.IsDeleted)
                                           .OrderByDescending(r => r.Likes.Count(l => l.IsLiked))
                                           .ThenByDescending(r => r.CreatedOn)
                                           .Skip(skip);

            if (take.HasValue)
            {
                queryable = queryable.Take(take.Value);
            }

            return await queryable.To<TModel>().ToListAsync();
        }

        public async Task<TModel> GetReviewByIdAsync<TModel>(int id)
        {
            var review = await this.db.Reviews.Where(r => r.Id == id && !r.IsDeleted)
                                        .To<TModel>()
                                        .FirstOrDefaultAsync();

            return review;
        }

        public async Task<IEnumerable<TModel>> GetTopReviewsByBookIdAsync<TModel>(int bookId, int count)
        {
            var reviews = await this.db.Reviews.AsNoTracking()
                                         .Where(r => r.BookId == bookId
                                                && r.ParentId == null
                                                && !r.IsDeleted)
                                         .OrderByDescending(r => r.Likes.Count(l => l.IsLiked))
                                         .ThenByDescending(r => r.CreatedOn)
                                         .To<TModel>()
                                         .Take(count)
                                         .ToListAsync();

            return reviews;
        }

        public async Task<IEnumerable<TModel>> GetChildrenReviewsToReviewsAsync<TModel>(ICollection<int> reviewsIds, int bookId)
        {
            var childrenReviews = await this.db.Reviews.AsNoTracking()
                                                 .Where(r => reviewsIds
                                                    .Contains(r.ParentId == null ? 0 : r.ParentId.Value)
                                                    && r.BookId == bookId
                                                    && !r.IsDeleted)
                                                 .To<TModel>()
                                                 .ToListAsync();

            if (!childrenReviews.Any())
            {
                return childrenReviews;
            }

            childrenReviews.AddRange(await this.GetChildrenReviewsToReviewsAsync<TModel>(
                childrenReviews
                .Select(r => (int)r.GetType().GetProperty("Id").GetValue(r)).ToList(), bookId));

            return childrenReviews;
        }

        private async Task<Review> GetByIdAsync(int id)
            => await this.db.Reviews.FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);
    }
}
