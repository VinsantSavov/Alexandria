namespace Alexandria.Services.Reviews
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Alexandria.Data;
    using Alexandria.Data.Models;
    using Alexandria.Data.Models.Enums;
    using Alexandria.Services.Common;
    using Alexandria.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class ReviewsService : IReviewsService
    {
        private readonly AlexandriaDbContext db;

        public ReviewsService(AlexandriaDbContext db)
        {
            this.db = db;
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

            if (review == null)
            {
                throw new NullReferenceException(string.Format(ExceptionMessages.ReviewNotFound, id));
            }

            review.IsDeleted = true;
            review.DeletedOn = DateTime.UtcNow;

            await this.db.SaveChangesAsync();
        }

        public async Task<bool> DoesReviewIdExistAsync(int id)
        {
            return await this.db.Reviews.AnyAsync(r => r.Id == id && !r.IsDeleted);
        }

        public async Task EditReviewAsync(int id, string description)
        {
            var review = await this.GetByIdAsync(id);

            if (review == null)
            {
                throw new NullReferenceException(string.Format(ExceptionMessages.ReviewNotFound, id));
            }

            review.Description = description;
            review.ModifiedOn = DateTime.UtcNow;

            await this.db.SaveChangesAsync();
        }

        public async Task<IEnumerable<TModel>> GetAllReviewsByAuthorIdAsync<TModel>(string authorId)
        {
            var reviews = await this.db.Reviews.AsNoTracking()
                                         .Where(r => r.AuthorId == authorId && !r.IsDeleted)
                                         .OrderBy(r => r.IsBestReview)
                                         .ThenByDescending(r => r.Likes)
                                         .ThenByDescending(r => r.CreatedOn)
                                         .To<TModel>()
                                         .ToListAsync();

            return reviews;
        }

        public async Task<IEnumerable<TModel>> GetAllReviewsByBookIdAsync<TModel>(int bookId)
        {
            var reviews = await this.db.Reviews.AsNoTracking()
                                         .Where(r => r.BookId == bookId && !r.IsDeleted)
                                         .OrderBy(r => r.IsBestReview)
                                         .ThenByDescending(r => r.Likes)
                                         .ThenByDescending(r => r.CreatedOn)
                                         .To<TModel>()
                                         .ToListAsync();

            return reviews;
        }

        public async Task<IEnumerable<TModel>> GetChildrenReviewsByReviewIdAsync<TModel>(int reviewId)
        {
            var reviews = await this.db.Reviews.AsNoTracking()
                                               .Where(r => r.ParentId == reviewId && !r.IsDeleted)
                                               .OrderByDescending(r => r.Likes)
                                               .ThenByDescending(r => r.CreatedOn)
                                               .To<TModel>()
                                               .ToListAsync();

            return reviews;
        }

        public async Task<TModel> GetReviewByIdAsync<TModel>(int id)
        {
            var review = await this.db.Reviews.Where(r => r.Id == id && !r.IsDeleted)
                                        .To<TModel>()
                                        .FirstOrDefaultAsync();

            if (review == null)
            {
                throw new NullReferenceException(string.Format(ExceptionMessages.ReviewNotFound, id));
            }

            return review;
        }

        public async Task<IEnumerable<TModel>> GetTopReviewsByBookIdAsync<TModel>(int bookId, int count)
        {
            var reviews = await this.db.Reviews.AsNoTracking()
                                         .Where(r => r.BookId == bookId && !r.IsDeleted)
                                         .OrderByDescending(r => r.Likes)
                                         .ThenByDescending(r => r.CreatedOn)
                                         .To<TModel>()
                                         .Take(count)
                                         .ToListAsync();

            return reviews;
        }

        public async Task MakeBestReviewAsync(int id)
        {
            var review = await this.GetByIdAsync(id);

            review.IsBestReview = true;

            var oldBestReview = await this.db.Reviews.FirstOrDefaultAsync(r => r.IsBestReview && !r.IsDeleted);

            if (oldBestReview != null)
            {
                oldBestReview.IsBestReview = false;
            }

            await this.db.SaveChangesAsync();
        }

        private async Task<Review> GetByIdAsync(int id)
            => await this.db.Reviews.FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);
    }
}
