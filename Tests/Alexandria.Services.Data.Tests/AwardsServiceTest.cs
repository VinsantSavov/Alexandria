namespace Alexandria.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using Alexandria.Data;
    using Alexandria.Data.Models;
    using Alexandria.Services.Awards;
    using Alexandria.Services.Mapping;
    using Alexandria.Web.ViewModels;
    using Alexandria.Web.ViewModels.Administration.Books;
    using Microsoft.EntityFrameworkCore;

    using Xunit;

    public class AwardsServiceTest
    {
        public AwardsServiceTest()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);
        }

        [Fact]
        public async Task DoesAwardExistMethodShouldReturnTrueIfExists()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            var awards = new List<Award>
            {
                new Award
                {
                    Name = "test1",
                    CreatedOn = DateTime.UtcNow,
                },
                new Award
                {
                    Name = "test2",
                    CreatedOn = DateTime.UtcNow,
                },
                new Award
                {
                    Name = "test3",
                    CreatedOn = DateTime.UtcNow,
                },
            };

            await db.Awards.AddRangeAsync(awards);
            await db.SaveChangesAsync();

            var awardsService = new AwardsService(db);

            Assert.True(await awardsService.DoesAwardIdExistAsync(1));
        }

        [Fact]
        public async Task DoesAwardExistMethodShouldReturnFalseIfAwardIsDeleted()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            var awards = new List<Award>
            {
                new Award
                {
                    Name = "test1",
                    CreatedOn = DateTime.UtcNow,
                    IsDeleted = true,
                    DeletedOn = DateTime.UtcNow,
                },
                new Award
                {
                    Name = "test2",
                    CreatedOn = DateTime.UtcNow,
                },
                new Award
                {
                    Name = "test3",
                    CreatedOn = DateTime.UtcNow,
                },
            };

            await db.Awards.AddRangeAsync(awards);
            await db.SaveChangesAsync();

            var awardsService = new AwardsService(db);

            Assert.False(await awardsService.DoesAwardIdExistAsync(1));
        }

        [Fact]
        public async Task DoesAwardExistMethodShouldReturnFalseIfAwardIsNotFound()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            var awards = new List<Award>
            {
                new Award
                {
                    Name = "test1",
                    CreatedOn = DateTime.UtcNow,
                },
                new Award
                {
                    Name = "test2",
                    CreatedOn = DateTime.UtcNow,
                },
                new Award
                {
                    Name = "test3",
                    CreatedOn = DateTime.UtcNow,
                },
            };

            await db.Awards.AddRangeAsync(awards);
            await db.SaveChangesAsync();

            var awardsService = new AwardsService(db);

            Assert.False(await awardsService.DoesAwardIdExistAsync(5));
        }

        [Fact]
        public async Task GetAllAwardsShouldReturnCorrectCountAwards()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            var awards = new List<Award>();

            for (int i = 1; i <= 10; i++)
            {
                awards.Add(
                    new Award
                    {
                        Name = $"test{i}",
                        CreatedOn = DateTime.UtcNow,
                    });
            }

            await db.Awards.AddRangeAsync(awards);
            await db.SaveChangesAsync();

            var awardsService = new AwardsService(db);
            var result = await awardsService.GetAllAwardsAsync<ABooksAwardViewModel>();

            Assert.Equal(10, result.Count());
        }

        [Fact]
        public async Task GetAllAwardsShouldReturnZeroWhenAwardsAreEmpty()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            var awards = new List<Award>();

            var awardsService = new AwardsService(db);
            var result = await awardsService.GetAllAwardsAsync<ABooksAwardViewModel>();

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllAwardsShouldReturnZeroWhenAwardsAreDeleted()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            var awards = new List<Award>();

            for (int i = 1; i <= 10; i++)
            {
                awards.Add(
                    new Award
                    {
                        Name = $"test{i}",
                        CreatedOn = DateTime.UtcNow,
                        IsDeleted = true,
                        DeletedOn = DateTime.UtcNow,
                    });
            }

            await db.Awards.AddRangeAsync(awards);
            await db.SaveChangesAsync();

            var awardsService = new AwardsService(db);
            var result = await awardsService.GetAllAwardsAsync<ABooksAwardViewModel>();

            Assert.Empty(result);
        }
    }
}
