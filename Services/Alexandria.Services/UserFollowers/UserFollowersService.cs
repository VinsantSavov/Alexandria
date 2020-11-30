namespace Alexandria.Services.UserFollowers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Alexandria.Data;
    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class UserFollowersService : IUserFollowersService
    {
        private readonly AlexandriaDbContext db;

        public UserFollowersService(AlexandriaDbContext db)
        {
            this.db = db;
        }

        public async Task CreateUserFollowerAsync(string userId, string followerId)
        {
            var exists = await this.DoesUserFollowerExistAsync(userId, followerId);

            if (exists)
            {
                throw new ArgumentException();
            }

            var userFollower = new UserFollower
            {
                UserId = userId,
                FollowerId = followerId,
                CreatedOn = DateTime.UtcNow,
            };

            await this.db.UserFollowers.AddAsync(userFollower);

            await this.db.SaveChangesAsync();
        }

        public async Task DeleteUserFollowerAsync(string userId, string followerId)
        {
            var userFollower = await this.db.UserFollowers.Where(uf => uf.UserId == userId
                                                                 && uf.FollowerId == followerId
                                                                 && !uf.IsDeleted)
                                                          .FirstOrDefaultAsync();

            userFollower.IsDeleted = true;
            userFollower.DeletedOn = DateTime.UtcNow;

            await this.db.SaveChangesAsync();
        }

        public async Task<bool> DoesUserFollowerExistAsync(string userId, string followerId)
           => await this.db.UserFollowers.AnyAsync(uf => uf.UserId == userId
                                                  && uf.FollowerId == followerId
                                                  && !uf.IsDeleted);

        public async Task<IEnumerable<TModel>> GetAllFollowersByUserIdAsync<TModel>(string userId)
        {
            var followers = await this.db.UserFollowers.AsNoTracking()
                                                 .Where(uf => uf.UserId == userId && !uf.IsDeleted)
                                                 .To<TModel>()
                                                 .ToListAsync();

            return followers;
        }

        public async Task<int> GetFollowersCountByUserIdAsync(string userId)
            => await this.db.UserFollowers.Where(uf => uf.UserId == userId && !uf.IsDeleted)
                                          .CountAsync();
    }
}
