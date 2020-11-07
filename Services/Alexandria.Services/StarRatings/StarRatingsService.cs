namespace Alexandria.Services.StarRatings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Alexandria.Data;
    using Alexandria.Data.Models;
    using Alexandria.Services.Common;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;

    public class StarRatingsService : IStarRatingsService
    {
        private readonly AlexandriaDbContext db;
        private readonly IMapper mapper;

        public StarRatingsService(AlexandriaDbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task CreateRatingAsync(int rate, string userId, int bookId)
        {
            var starRating = await this.db.StarRatings.FirstOrDefaultAsync(sr => sr.UserId == userId && sr.BookId == bookId);

            if (starRating != null)
            {
                throw new ArgumentException(ExceptionMessages.ExistingRating);
            }

            var rating = new StarRating
            {
                Rate = rate,
                UserId = userId,
                BookId = bookId,
            };

            await this.db.StarRatings.AddAsync(rating);
            await this.db.SaveChangesAsync();
        }

        public async Task DeleteRatingAsync(int id)
        {
            var rating = await this.db.StarRatings.FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);

            rating.IsDeleted = true;
            rating.DeletedOn = DateTime.UtcNow;

            await this.db.SaveChangesAsync();
        }

        public async Task<IEnumerable<TModel>> GetAllRatesByBookIdAsync<TModel>(int bookId)
        {
            var ratings = await this.db.StarRatings.AsNoTracking()
                                             .Where(r => r.BookId == bookId && !r.IsDeleted)
                                             .ProjectTo<TModel>(this.mapper.ConfigurationProvider)
                                             .ToListAsync();

            return ratings;
        }

        public async Task<IEnumerable<TModel>> GetAllRatesByUserIdAsync<TModel>(string userId)
        {
            var ratings = await this.db.StarRatings.AsNoTracking()
                                             .Where(r => r.UserId == userId && !r.IsDeleted)
                                             .ProjectTo<TModel>(this.mapper.ConfigurationProvider)
                                             .ToListAsync();

            return ratings;
        }
    }
}
