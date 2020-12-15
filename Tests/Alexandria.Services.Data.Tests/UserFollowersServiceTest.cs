namespace Alexandria.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using Alexandria.Data;
    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;
    using Alexandria.Services.UserFollowers;
    using Microsoft.EntityFrameworkCore;

    using Xunit;

    public class UserFollowersServiceTest
    {
        public UserFollowersServiceTest()
        {
            AutoMapperConfig.RegisterMappings(typeof(UserFollowerTestModel).GetTypeInfo().Assembly);
        }

        [Theory]
        [InlineData("user1", "user2")]
        [InlineData("user2", "user3")]
        [InlineData("user3", "user4")]
        public async Task CreateUserFollowerShouldAddInDatabaseIfNotExisting(string userId, string followerId)
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            var userFollowerService = new UserFollowersService(db);

            await userFollowerService.CreateUserFollowerAsync(userId, followerId);

            Assert.Equal(1, await db.UserFollowers.CountAsync());
        }

        [Fact]
        public async Task CreateUserFollowerShouldRestoreUserFollowerIfDeleted()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.UserFollowers.AddAsync(
                new UserFollower
                {
                    UserId = "user1",
                    FollowerId = "user2",
                    IsDeleted = true,
                });
            await db.SaveChangesAsync();

            var userFollowerService = new UserFollowersService(db);

            await userFollowerService.CreateUserFollowerAsync("user1", "user2");
            var result = await db.UserFollowers.FirstOrDefaultAsync();

            Assert.False(result.IsDeleted);
            Assert.Null(result.DeletedOn);
        }

        [Fact]
        public async Task CreateUserFollowerShouldDeleteUserFollowerIfExisting()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.UserFollowers.AddAsync(
                new UserFollower
                {
                    UserId = "user1",
                    FollowerId = "user2",
                });
            await db.SaveChangesAsync();

            var userFollowerService = new UserFollowersService(db);

            await userFollowerService.CreateUserFollowerAsync("user1", "user2");
            var result = await db.UserFollowers.FirstOrDefaultAsync();

            Assert.True(result.IsDeleted);
            Assert.NotNull(result.DeletedOn);
        }

        [Fact]
        public async Task DoesUserFollowUserShouldReturnTrueIfCorrect()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.UserFollowers.AddAsync(
                new UserFollower
                {
                    UserId = "user1",
                    FollowerId = "user2",
                });
            await db.SaveChangesAsync();

            var userFollowerService = new UserFollowersService(db);

            var result = await userFollowerService.DoesUserFollowUserAsync("user1", "user2");

            Assert.True(result);
        }

        [Fact]
        public async Task DoesUserFollowUserShouldReturnFalseIfDeleted()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.UserFollowers.AddAsync(
                new UserFollower
                {
                    UserId = "user1",
                    FollowerId = "user2",
                    IsDeleted = true,
                });
            await db.SaveChangesAsync();

            var userFollowerService = new UserFollowersService(db);

            var result = await userFollowerService.DoesUserFollowUserAsync("user1", "user2");

            Assert.False(result);
        }

        [Fact]
        public async Task GetAllFollowersShouldReturnAllFollowers()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            var follower1 = new ApplicationUser { Id = "user2", UserName = "username1", IsDeleted = false };
            var follower2 = new ApplicationUser { Id = "user3", UserName = "username2", IsDeleted = false };
            var follower3 = new ApplicationUser { Id = "user4", UserName = "username3", IsDeleted = false };

            await db.UserFollowers.AddRangeAsync(
                new UserFollower
                {
                    UserId = "user1",
                    Follower = follower1,
                },
                new UserFollower
                {
                    UserId = "user1",
                    Follower = follower2,
                },
                new UserFollower
                {
                    UserId = "user1",
                    Follower = follower3,
                });
            await db.SaveChangesAsync();

            var userFollowerService = new UserFollowersService(db);

            var result = await userFollowerService.GetAllFollowersByUserIdAsync<UserFollowerTestModel>("user1");

            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task GetAllFollowersShouldNotReturnDeletedFollowers()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            var follower1 = new ApplicationUser { Id = "user2", UserName = "username1", IsDeleted = false };
            var follower2 = new ApplicationUser { Id = "user3", UserName = "username2", IsDeleted = true };
            var follower3 = new ApplicationUser { Id = "user4", UserName = "username3", IsDeleted = false };

            await db.UserFollowers.AddRangeAsync(
                new UserFollower
                {
                    UserId = "user1",
                    Follower = follower1,
                },
                new UserFollower
                {
                    UserId = "user1",
                    Follower = follower2,
                },
                new UserFollower
                {
                    UserId = "user1",
                    Follower = follower3,
                    IsDeleted = true,
                });
            await db.SaveChangesAsync();

            var userFollowerService = new UserFollowersService(db);

            var result = await userFollowerService.GetAllFollowersByUserIdAsync<UserFollowerTestModel>("user1");

            Assert.Single(result);
        }

        [Fact]
        public async Task GetAllFollowersShouldReturnCorrectResultWithTakeAndSkip()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            var follower1 = new ApplicationUser { Id = "user2", UserName = "username1", IsDeleted = false };
            var follower2 = new ApplicationUser { Id = "user3", UserName = "username2", IsDeleted = false };
            var follower3 = new ApplicationUser { Id = "user4", UserName = "username3", IsDeleted = false };

            await db.UserFollowers.AddRangeAsync(
                new UserFollower
                {
                    UserId = "user1",
                    Follower = follower1,
                },
                new UserFollower
                {
                    UserId = "user1",
                    Follower = follower2,
                },
                new UserFollower
                {
                    UserId = "user1",
                    Follower = follower3,
                });
            await db.SaveChangesAsync();

            var userFollowerService = new UserFollowersService(db);

            var result = await userFollowerService.GetAllFollowersByUserIdAsync<UserFollowerTestModel>("user1", 5, 1);

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetAllFollowingShouldReturnAllFollowing()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            var user1 = new ApplicationUser { Id = "user1", UserName = "username1", IsDeleted = false };
            var user2 = new ApplicationUser { Id = "user2", UserName = "username2", IsDeleted = false };
            var user3 = new ApplicationUser { Id = "user3", UserName = "username3", IsDeleted = false };

            await db.UserFollowers.AddRangeAsync(
                new UserFollower
                {
                    User = user1,
                    FollowerId = "follower1",
                },
                new UserFollower
                {
                    User = user2,
                    FollowerId = "follower1",
                },
                new UserFollower
                {
                    User = user3,
                    FollowerId = "follower1",
                });
            await db.SaveChangesAsync();

            var userFollowerService = new UserFollowersService(db);

            var result = await userFollowerService.GetAllFollowingByUserIdAsync<UserFollowerTestModel>("follower1");

            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task GetAllFollowingShouldNotReturnDeletedUsers()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            var user1 = new ApplicationUser { Id = "user1", UserName = "username1", IsDeleted = false };
            var user2 = new ApplicationUser { Id = "user2", UserName = "username2", IsDeleted = true };
            var user3 = new ApplicationUser { Id = "user3", UserName = "username3", IsDeleted = false };

            await db.UserFollowers.AddRangeAsync(
                new UserFollower
                {
                    User = user1,
                    FollowerId = "follower1",
                },
                new UserFollower
                {
                    User = user2,
                    FollowerId = "follower1",
                },
                new UserFollower
                {
                    User = user3,
                    FollowerId = "follower1",
                    IsDeleted = true,
                });
            await db.SaveChangesAsync();

            var userFollowerService = new UserFollowersService(db);

            var result = await userFollowerService.GetAllFollowingByUserIdAsync<UserFollowerTestModel>("follower1");

            Assert.Single(result);
        }

        [Fact]
        public async Task GetAllFollowingShouldReturnCorrectResultWithTakeAndSkip()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            var user1 = new ApplicationUser { Id = "user1", UserName = "username1", IsDeleted = false };
            var user2 = new ApplicationUser { Id = "user2", UserName = "username2", IsDeleted = false };
            var user3 = new ApplicationUser { Id = "user3", UserName = "username3", IsDeleted = false };

            await db.UserFollowers.AddRangeAsync(
                new UserFollower
                {
                    User = user1,
                    FollowerId = "follower1",
                },
                new UserFollower
                {
                    User = user2,
                    FollowerId = "follower1",
                },
                new UserFollower
                {
                    User = user3,
                    FollowerId = "follower1",
                });
            await db.SaveChangesAsync();

            var userFollowerService = new UserFollowersService(db);

            var result = await userFollowerService.GetAllFollowingByUserIdAsync<UserFollowerTestModel>("follower1", 5, 1);

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetFollowersCountShouldReturnCorrectCount()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            var follower1 = new ApplicationUser { Id = "user1", UserName = "username1", IsDeleted = false };
            var follower2 = new ApplicationUser { Id = "user2", UserName = "username2", IsDeleted = false };
            var follower3 = new ApplicationUser { Id = "user3", UserName = "username3", IsDeleted = false };

            await db.UserFollowers.AddRangeAsync(
                new UserFollower
                {
                    UserId = "user1",
                    Follower = follower1,
                },
                new UserFollower
                {
                    UserId = "user1",
                    Follower = follower2,
                },
                new UserFollower
                {
                    UserId = "user1",
                    Follower = follower3,
                });
            await db.SaveChangesAsync();

            var userFollowerService = new UserFollowersService(db);

            var result = await userFollowerService.GetFollowersCountByUserIdAsync("user1");

            Assert.Equal(3, result);
        }

        [Fact]
        public async Task GetFollowersCountShouldNotReturnDeleted()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            var follower1 = new ApplicationUser { Id = "user1", UserName = "username1", IsDeleted = false };
            var follower2 = new ApplicationUser { Id = "user2", UserName = "username2", IsDeleted = true };
            var follower3 = new ApplicationUser { Id = "user3", UserName = "username3", IsDeleted = false };

            await db.UserFollowers.AddRangeAsync(
                new UserFollower
                {
                    UserId = "user1",
                    Follower = follower1,
                },
                new UserFollower
                {
                    UserId = "user1",
                    Follower = follower2,
                },
                new UserFollower
                {
                    UserId = "user1",
                    Follower = follower3,
                    IsDeleted = true,
                });
            await db.SaveChangesAsync();

            var userFollowerService = new UserFollowersService(db);

            var result = await userFollowerService.GetFollowersCountByUserIdAsync("user1");

            Assert.Equal(1, result);
        }

        [Fact]
        public async Task GetFollowingCountShouldReturnCorrectCount()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            var user1 = new ApplicationUser { Id = "user1", UserName = "username1", IsDeleted = false };
            var user2 = new ApplicationUser { Id = "user2", UserName = "username2", IsDeleted = false };
            var user3 = new ApplicationUser { Id = "user3", UserName = "username3", IsDeleted = false };

            await db.UserFollowers.AddRangeAsync(
                new UserFollower
                {
                    User = user1,
                    FollowerId = "follower1",
                },
                new UserFollower
                {
                    User = user2,
                    FollowerId = "follower1",
                },
                new UserFollower
                {
                    User = user3,
                    FollowerId = "follower1",
                });
            await db.SaveChangesAsync();

            var userFollowerService = new UserFollowersService(db);

            var result = await userFollowerService.GetFollowingCountByUserIdAsync("follower1");

            Assert.Equal(3, result);
        }

        [Fact]
        public async Task GetFollowingCountShouldNotReturnDeleted()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            var user1 = new ApplicationUser { Id = "user1", UserName = "username1", IsDeleted = false };
            var user2 = new ApplicationUser { Id = "user2", UserName = "username2", IsDeleted = true };
            var user3 = new ApplicationUser { Id = "user3", UserName = "username3", IsDeleted = false };

            await db.UserFollowers.AddRangeAsync(
                new UserFollower
                {
                    User = user1,
                    FollowerId = "follower1",
                },
                new UserFollower
                {
                    User = user2,
                    FollowerId = "follower1",
                },
                new UserFollower
                {
                    User = user3,
                    FollowerId = "follower1",
                    IsDeleted = true,
                });
            await db.SaveChangesAsync();

            var userFollowerService = new UserFollowersService(db);

            var result = await userFollowerService.GetFollowingCountByUserIdAsync("follower1");

            Assert.Equal(1, result);
        }

        public class UserFollowerTestModel : IMapFrom<ApplicationUser>
        {
            public string Id { get; set; }

            public string UserName { get; set; }
        }
    }
}
