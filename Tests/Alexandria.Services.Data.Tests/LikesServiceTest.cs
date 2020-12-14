namespace Alexandria.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using Alexandria.Data;
    using Alexandria.Data.Models;
    using Alexandria.Services.Likes;
    using Alexandria.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    using Xunit;

    public class LikesServiceTest
    {
        [Fact]
        public async Task CreateLikeShouldAddInDatabase()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            var likesService = new LikesService(db);

            await likesService.CreateLikeAsync("userId", 1, true);

            Assert.Single(db.Likes);
        }

        [Theory]
        [InlineData("userId", 1, false)]
        [InlineData("userId", 1, true)]
        public async Task CreateLikeShouldNotAddInDatabaseIfExists(string userId, int reviewId, bool liked)
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.AddAsync(new Like { UserId = "userId", ReviewId = 1 });
            await db.SaveChangesAsync();

            var likesService = new LikesService(db);

            await likesService.CreateLikeAsync(userId, reviewId, liked);
            var result = await db.Likes.FirstOrDefaultAsync();

            Assert.Equal(userId, result.UserId);
            Assert.Equal(reviewId, result.ReviewId);
            Assert.Equal(liked, result.IsLiked);
            Assert.Single(db.Likes);
        }

        [Theory]
        [InlineData("user1", 1)]
        [InlineData("user2", 2)]
        public async Task DoesUserLikeReviewShouldReturnTrueIfItIsLiked(string userId, int reviewId)
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.Likes.AddRangeAsync(
                new Like { UserId = "user1", ReviewId = 1, IsLiked = true },
                new Like { UserId = "user2", ReviewId = 2, IsLiked = true });
            await db.SaveChangesAsync();

            var likesService = new LikesService(db);

            var result = await likesService.DoesUserLikeReviewAsync(userId, reviewId);

            Assert.True(result);
        }

        [Fact]
        public async Task DoesUserLikeReviewShouldReturnFalseWhenItIsntLiked()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.Likes.AddRangeAsync(
                new Like { UserId = "user1", ReviewId = 1, IsLiked = false },
                new Like { UserId = "user2", ReviewId = 2, IsLiked = true });
            await db.SaveChangesAsync();

            var likesService = new LikesService(db);

            var result = await likesService.DoesUserLikeReviewAsync("user1", 1);

            Assert.False(result);
        }

        [Fact]
        public async Task GetLikesCountShouldReturnCorrectCount()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.Likes.AddRangeAsync(
                new Like { UserId = "user1", ReviewId = 1, IsLiked = true },
                new Like { UserId = "user2", ReviewId = 2, IsLiked = true },
                new Like { UserId = "user2", ReviewId = 1, IsLiked = true },
                new Like { UserId = "user2", ReviewId = 1, IsLiked = false });
            await db.SaveChangesAsync();

            var likesService = new LikesService(db);

            var result = await likesService.GetLikesCountByReviewIdAsync(1);

            Assert.Equal(2, result);
        }
    }
}
