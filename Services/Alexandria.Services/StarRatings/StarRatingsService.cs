namespace Alexandria.Services.StarRatings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Alexandria.Data;
    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class StarRatingsService : IStarRatingsService
    {
        private readonly AlexandriaDbContext db;

        public StarRatingsService(AlexandriaDbContext db)
        {
            this.db = db;
        }

        public async Task CreateRatingAsync(int rate, string userId, int bookId)
        {
            var starRating = await this.db.StarRatings.FirstOrDefaultAsync(sr => sr.UserId == userId && sr.BookId == bookId);

            if (starRating != null)
            {
                starRating.Rate = rate;
                starRating.ModifiedOn = DateTime.UtcNow;
            }
            else
            {
                starRating = new StarRating
                {
                    Rate = rate,
                    UserId = userId,
                    BookId = bookId,
                    CreatedOn = DateTime.UtcNow,
                };

                await this.db.StarRatings.AddAsync(starRating);
            }

            await this.db.SaveChangesAsync();
        }

        public async Task<int> GetRatesCountByUserIdAsync(string userId)
                  => await this.db.StarRatings.Where(sr => sr.UserId == userId)
                                              .CountAsync();

        public async Task<int> GetRatesCountByBookIdAsync(int bookId)
                  => await this.db.StarRatings.Where(r => r.BookId == bookId)
                                              .CountAsync();

        public async Task<IEnumerable<TModel>> GetAllRatesByUserIdAsync<TModel>(string userId, int? take = null, int skip = 0)
        {
            var queryable = this.db.StarRatings.AsNoTracking()
                                               .Where(r => r.UserId == userId)
                                               .OrderByDescending(r => r.Rate)
                                               .Skip(skip);

            if (take.HasValue)
            {
                queryable = queryable.Take(take.Value);
            }

            return await queryable.To<TModel>().ToListAsync();
        }

        public async Task<double> GetAverageRatingByBookIdAsync(int bookId)
        {
            var count = await this.db.StarRatings.Where(r => r.BookId == bookId)
                                                 .CountAsync();

            if (count == 0)
            {
                return 0;
            }

            return await this.db.StarRatings.Where(r => r.BookId == bookId)
                                            .AverageAsync(r => r.Rate);
        }
    }
}
