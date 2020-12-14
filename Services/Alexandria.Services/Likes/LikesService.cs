namespace Alexandria.Services.Likes
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Alexandria.Data;
    using Alexandria.Data.Models;
    using Microsoft.EntityFrameworkCore;

    public class LikesService : ILikesService
    {
        private readonly AlexandriaDbContext db;

        public LikesService(AlexandriaDbContext db)
        {
            this.db = db;
        }

        public async Task CreateLikeAsync(string userId, int reviewId, bool isLiked)
        {
            var like = await this.db.Likes.FirstOrDefaultAsync(l => l.UserId == userId && l.ReviewId == reviewId);

            if (like == null)
            {
                like = new Like
                {
                    IsLiked = true,
                    UserId = userId,
                    ReviewId = reviewId,
                    CreatedOn = DateTime.UtcNow,
                };

                await this.db.Likes.AddAsync(like);
            }
            else
            {
                like.IsLiked = isLiked;
                like.ModifiedOn = DateTime.UtcNow;
            }

            await this.db.SaveChangesAsync();
        }

        public async Task<bool> DoesUserLikeReviewAsync(string userId, int reviewId)
            => await this.db.Likes.AnyAsync(l => l.UserId == userId && l.ReviewId == reviewId && l.IsLiked);

        public async Task<int> GetLikesCountByReviewIdAsync(int reviewId)
            => await this.db.Likes.Where(l => l.ReviewId == reviewId && l.IsLiked).CountAsync();
    }
}
