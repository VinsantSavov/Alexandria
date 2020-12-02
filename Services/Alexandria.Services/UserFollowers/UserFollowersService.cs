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
            var userFollower = await this.db.UserFollowers
                    .FirstOrDefaultAsync(uf => uf.UserId == userId && uf.FollowerId == followerId);

            if (userFollower == null)
            {
                userFollower = new UserFollower
                {
                    UserId = userId,
                    FollowerId = followerId,
                    CreatedOn = DateTime.UtcNow,
                };

                await this.db.UserFollowers.AddAsync(userFollower);
            }
            else
            {
                if (userFollower.IsDeleted)
                {
                    userFollower.IsDeleted = false;
                    userFollower.DeletedOn = null;
                    userFollower.CreatedOn = DateTime.UtcNow;
                    userFollower.ModifiedOn = DateTime.UtcNow;
                }
                else
                {
                    userFollower.IsDeleted = true;
                    userFollower.DeletedOn = DateTime.UtcNow;
                }
            }

            await this.db.SaveChangesAsync();
        }

        public async Task<bool> DoesUserFollowerExistAsync(string userId, string followerId)
           => await this.db.UserFollowers.AnyAsync(uf => uf.UserId == userId
                                                  && uf.FollowerId == followerId
                                                  && !uf.IsDeleted);

        public async Task<IEnumerable<TModel>> GetAllFollowersByUserIdAsync<TModel>(string userId, int? take = null, int skip = 0)
        {
            var followers = this.db.UserFollowers.AsNoTracking()
                                                 .Where(uf => uf.UserId == userId
                                                    && !uf.IsDeleted
                                                    && !uf.Follower.IsDeleted)
                                                 .Skip(skip);

            if (take.HasValue)
            {
                followers = followers.Take(take.Value);
            }

            return await followers.Select(uf => uf.Follower).To<TModel>().ToListAsync();
        }

        public async Task<IEnumerable<TModel>> GetAllFollowingByUserIdAsync<TModel>(string userId, int? take = null, int skip = 0)
        {
            var following = this.db.UserFollowers.AsNoTracking()
                                                 .Where(uf => uf.FollowerId == userId
                                                     && !uf.IsDeleted
                                                     && !uf.User.IsDeleted)
                                                 .Skip(skip);

            if (take.HasValue)
            {
                following = following.Take(take.Value);
            }

            return await following.Select(uf => uf.User).To<TModel>().ToListAsync();
        }

        public async Task<int> GetFollowersCountByUserIdAsync(string userId)
            => await this.db.UserFollowers.Where(uf => uf.UserId == userId && !uf.IsDeleted)
                                          .CountAsync();

        public async Task<int> GetFollowingCountByUserIdAsync(string userId)
            => await this.db.UserFollowers.Where(uf => uf.FollowerId == userId && !uf.IsDeleted)
                                          .CountAsync();
    }
}
