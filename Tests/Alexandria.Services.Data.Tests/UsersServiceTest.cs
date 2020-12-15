namespace Alexandria.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using Alexandria.Data;
    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;
    using Alexandria.Services.Users;
    using Microsoft.EntityFrameworkCore;

    using Xunit;

    public class UsersServiceTest
    {
        public UsersServiceTest()
        {
            AutoMapperConfig.RegisterMappings(typeof(UserTestModel).GetTypeInfo().Assembly);
        }

        [Fact]
        public async Task GetChatUsersShouldReturnChatUsers()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.Users.AddRangeAsync(
                new ApplicationUser
                {
                    Id = "userId1",
                    UserName = "user1",
                },
                new ApplicationUser
                {
                    Id = "userId2",
                    UserName = "user2",
                    SentMessages = new List<Message> { new Message { AuthorId = "userId2", ReceiverId = "userId1" } },
                },
                new ApplicationUser
                {
                    Id = "userId3",
                    UserName = "user3",
                    ReceivedMessages = new List<Message> { new Message { AuthorId = "userId1", ReceiverId = "userId3" } },
                });
            await db.SaveChangesAsync();

            var usersSevice = new UsersService(db);

            var result = await usersSevice.GetChatUsersAsync<UserTestModel>("userId1");

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetChatUsersShouldNotReturnDeletedUsers()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.Users.AddRangeAsync(
                new ApplicationUser
                {
                    Id = "userId1",
                    UserName = "user1",
                },
                new ApplicationUser
                {
                    Id = "userId2",
                    UserName = "user2",
                    SentMessages = new List<Message> { new Message { AuthorId = "userId2", ReceiverId = "userId1" } },
                    IsDeleted = true,
                },
                new ApplicationUser
                {
                    Id = "userId3",
                    UserName = "user3",
                    ReceivedMessages = new List<Message> { new Message { AuthorId = "userId1", ReceiverId = "userId3" } },
                });
            await db.SaveChangesAsync();

            var usersSevice = new UsersService(db);

            var result = await usersSevice.GetChatUsersAsync<UserTestModel>("userId1");

            Assert.Single(result);
        }

        [Fact]
        public async Task GetChatUsersShouldReturnCorrectResult()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.Users.AddRangeAsync(
                new ApplicationUser
                {
                    Id = "userId1",
                    UserName = "user1",
                },
                new ApplicationUser
                {
                    Id = "userId2",
                    UserName = "user2",
                    SentMessages = new List<Message> { new Message { AuthorId = "userId2", ReceiverId = "userId1" } },
                },
                new ApplicationUser
                {
                    Id = "userId3",
                    UserName = "user3",
                    ReceivedMessages = new List<Message> { new Message { AuthorId = "userId1", ReceiverId = "userId3" }, new Message { AuthorId = "userId2", ReceiverId = "userId3" } },
                },
                new ApplicationUser
                {
                    Id = "userId4",
                    UserName = "user4",
                    ReceivedMessages = new List<Message> { new Message { AuthorId = "userId2", ReceiverId = "userId3" } },
                });
            await db.SaveChangesAsync();

            var usersSevice = new UsersService(db);

            var result = await usersSevice.GetChatUsersAsync<UserTestModel>("userId1");

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetUserShouldReturnCorrectResult()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.Users.AddAsync(
                new ApplicationUser
                {
                    Id = "userId1",
                    UserName = "user1",
                });
            await db.SaveChangesAsync();

            var usersSevice = new UsersService(db);

            var result = await usersSevice.GetUserByIdAsync<UserTestModel>("userId1");

            Assert.Equal("userId1", result.Id);
            Assert.Equal("user1", result.Username);
        }

        [Fact]
        public async Task GetUserShouldReturnNullIfDeleted()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.Users.AddAsync(
                new ApplicationUser
                {
                    Id = "userId1",
                    UserName = "user1",
                    IsDeleted = true,
                });
            await db.SaveChangesAsync();

            var usersSevice = new UsersService(db);

            var result = await usersSevice.GetUserByIdAsync<UserTestModel>("userId1");

            Assert.Null(result);
        }

        [Fact]
        public async Task GetUserShouldReturnNullIfNotFound()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.Users.AddAsync(
                new ApplicationUser
                {
                    Id = "userId1",
                    UserName = "user1",
                });
            await db.SaveChangesAsync();

            var usersSevice = new UsersService(db);

            var result = await usersSevice.GetUserByIdAsync<UserTestModel>("userId2");

            Assert.Null(result);
        }

        [Fact]
        public async Task IsUserDeletedShouldReturnTrueIfDeletedAndExisting()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.Users.AddAsync(
                new ApplicationUser
                {
                    Id = "userId1",
                    UserName = "user1",
                    IsDeleted = true,
                });
            await db.SaveChangesAsync();

            var usersSevice = new UsersService(db);

            var result = await usersSevice.IsUserDeletedAsync("user1");

            Assert.True(result);
        }

        [Fact]
        public async Task IsUserDeletedShouldReturnFalseIfNotDeleted()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.Users.AddAsync(
                new ApplicationUser
                {
                    Id = "userId1",
                    UserName = "user1",
                });
            await db.SaveChangesAsync();

            var usersSevice = new UsersService(db);

            var result = await usersSevice.IsUserDeletedAsync("user1");

            Assert.False(result);
        }

        [Fact]
        public async Task IsUserDeletedShouldReturnFalseIfNotFound()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.Users.AddAsync(
                new ApplicationUser
                {
                    Id = "userId1",
                    UserName = "user1",
                    IsDeleted = true,
                });
            await db.SaveChangesAsync();

            var usersSevice = new UsersService(db);

            var result = await usersSevice.IsUserDeletedAsync("user2");

            Assert.False(result);
        }

        [Fact]
        public async Task IsUsernameUsedShouldReturnTrueIfUsed()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.Users.AddAsync(
                new ApplicationUser
                {
                    Id = "userId1",
                    UserName = "user1",
                });
            await db.SaveChangesAsync();

            var usersSevice = new UsersService(db);

            var result = await usersSevice.IsUsernameUsedAsync("user1");

            Assert.True(result);
        }

        [Fact]
        public async Task IsUsernameUsedShouldReturnFalseIfNotFound()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.Users.AddAsync(
                new ApplicationUser
                {
                    Id = "userId1",
                    UserName = "user1",
                });
            await db.SaveChangesAsync();

            var usersSevice = new UsersService(db);

            var result = await usersSevice.IsUsernameUsedAsync("user2");

            Assert.False(result);
        }

        [Fact]
        public async Task DoesUserIdExistShouldReturnTrueIfExists()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.Users.AddAsync(
                new ApplicationUser
                {
                    Id = "userId1",
                    UserName = "user1",
                });
            await db.SaveChangesAsync();

            var usersSevice = new UsersService(db);

            var result = await usersSevice.DoesUserIdExistAsync("userId1");

            Assert.True(result);
        }

        [Fact]
        public async Task DoesUserIdExistShouldReturnFalseIfDeleted()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.Users.AddAsync(
                new ApplicationUser
                {
                    Id = "userId1",
                    UserName = "user1",
                    IsDeleted = true,
                });
            await db.SaveChangesAsync();

            var usersSevice = new UsersService(db);

            var result = await usersSevice.DoesUserIdExistAsync("userId1");

            Assert.False(result);
        }

        [Fact]
        public async Task DoesUserIdExistShouldReturnFalseIfNotFound()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.Users.AddAsync(
                new ApplicationUser
                {
                    Id = "userId1",
                    UserName = "user1",
                });
            await db.SaveChangesAsync();

            var usersSevice = new UsersService(db);

            var result = await usersSevice.DoesUserIdExistAsync("userId2");

            Assert.False(result);
        }

        public class UserTestModel : IMapFrom<ApplicationUser>
        {
            public string Id { get; set; }

            public string Username { get; set; }
        }
    }
}
