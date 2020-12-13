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
    using Alexandria.Services.StarRatings;
    using Alexandria.Web.ViewModels;
    using Alexandria.Web.ViewModels.Administration.Books;
    using Alexandria.Web.ViewModels.Users;
    using Microsoft.EntityFrameworkCore;

    using Xunit;

    public class StarRatingsServiceTest
    {
        public StarRatingsServiceTest()
        {
            AutoMapperConfig.RegisterMappings(typeof(RatingTestModel).GetTypeInfo().Assembly);
        }

        [Fact]
        public async Task CreateRatingMethodShouldAddRatingInDatabase()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            var starRatingsService = new StarRatingsService(db);

            await starRatingsService.CreateRatingAsync(5, "userId", 1);

            Assert.Equal(1, db.StarRatings.Count());
        }

        [Fact]
        public async Task CreateRatingMethodShouldAddRightRating()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            var starRatingsService = new StarRatingsService(db);

            await starRatingsService.CreateRatingAsync(5, "userId", 1);
            var result = await db.StarRatings.FirstOrDefaultAsync();

            Assert.Equal(5, result.Rate);
            Assert.Equal("userId", result.UserId);
            Assert.Equal(1, result.BookId);
        }

        [Fact]
        public async Task CreateRatingMethodShouldReplaceRateIfExisting()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            await db.StarRatings.AddAsync(
                new StarRating
                {
                    Rate = 1,
                    UserId = "userId",
                    BookId = 1,
                });

            await db.SaveChangesAsync();

            var starRatingsService = new StarRatingsService(db);

            await starRatingsService.CreateRatingAsync(5, "userId", 1);
            var result = await db.StarRatings.FirstOrDefaultAsync();

            Assert.Equal(5, result.Rate);
            Assert.Equal("userId", result.UserId);
            Assert.Equal(1, result.BookId);
        }

        [Fact]
        public async Task CreateRatingMethodShouldNotChangeCountIfRatingExists()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            await db.StarRatings.AddAsync(
                new StarRating
                {
                    Rate = 1,
                    UserId = "userId",
                    BookId = 1,
                });

            await db.SaveChangesAsync();

            var starRatingsService = new StarRatingsService(db);

            await starRatingsService.CreateRatingAsync(5, "userId", 1);
            var result = await db.StarRatings.CountAsync();

            Assert.Equal(1, result);
        }

        [Fact]
        public async Task GetRatesCountByUserShouldReturnCorrectCount()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            var rates = new List<StarRating>();

            for (int i = 1; i <= 10; i++)
            {
                rates.Add(
                    new StarRating
                    {
                        Rate = i,
                        UserId = "userId",
                        BookId = i,
                    });
            }

            await db.StarRatings.AddRangeAsync(rates);

            await db.SaveChangesAsync();

            var ratingsService = new StarRatingsService(db);
            var result = await ratingsService.GetRatesCountByUserIdAsync("userId");

            Assert.Equal(10, result);
        }

        [Fact]
        public async Task GetRatesCountByUserShouldReturnZeroWhenNoRatings()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            var ratingsService = new StarRatingsService(db);
            var result = await ratingsService.GetRatesCountByUserIdAsync("userId");

            Assert.Equal(0, result);
        }

        [Fact]
        public async Task GetRatesCountByUserShouldReturnZeroWhenInvalidUserId()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            var rates = new List<StarRating>();

            for (int i = 1; i <= 10; i++)
            {
                rates.Add(
                    new StarRating
                    {
                        Rate = i,
                        UserId = "userId",
                        BookId = i,
                    });
            }

            await db.StarRatings.AddRangeAsync(rates);

            await db.SaveChangesAsync();

            var ratingsService = new StarRatingsService(db);
            var result = await ratingsService.GetRatesCountByUserIdAsync(null);

            Assert.Equal(0, result);
        }

        [Fact]
        public async Task GetRatesCountByBookIdShouldReturnCorrectCount()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            var rates = new List<StarRating>();

            for (int i = 1; i <= 10; i++)
            {
                rates.Add(new StarRating
                {
                    Rate = i,
                    UserId = i.ToString(),
                    BookId = 1,
                });
            }

            await db.AddRangeAsync(rates);
            await db.SaveChangesAsync();

            var ratingsService = new StarRatingsService(db);
            var result = await ratingsService.GetRatesCountByBookIdAsync(1);

            Assert.Equal(10, result);
        }

        [Fact]
        public async Task GetRatesCountByBookIdShouldReturnZeroWhenInvalidBookId()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            var rates = new List<StarRating>();

            for (int i = 1; i <= 10; i++)
            {
                rates.Add(new StarRating
                {
                    Rate = i,
                    UserId = i.ToString(),
                    BookId = 1,
                });
            }

            await db.AddRangeAsync(rates);
            await db.SaveChangesAsync();

            var ratingsService = new StarRatingsService(db);
            var result = await ratingsService.GetRatesCountByBookIdAsync(3);

            Assert.Equal(0, result);
        }

        [Fact]
        public async Task GetRatesCountByBookIdShouldReturnZeroWhenRatesIsEmpty()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            var ratingsService = new StarRatingsService(db);
            var result = await ratingsService.GetRatesCountByBookIdAsync(1);

            Assert.Equal(0, result);
        }

        [Fact]
        public async Task GetAllRatesByUserIdShouldReturnCorrectCount()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            var rates = new List<StarRating>();

            for (int i = 1; i <= 10; i++)
            {
                rates.Add(new StarRating
                {
                    Rate = i,
                    UserId = "userId",
                    BookId = i,
                });
            }

            await db.StarRatings.AddRangeAsync(rates);
            await db.SaveChangesAsync();

            var ratingsService = new StarRatingsService(db);

            var result = await ratingsService.GetAllRatesByUserIdAsync<RatingTestModel>("userId");

            Assert.Equal(10, result.Count());
        }

        [Fact]
        public async Task GetAllRatesByUserIdShouldReturnZeroWhenRatingsAreEmpty()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            var ratingsService = new StarRatingsService(db);

            var result = await ratingsService.GetAllRatesByUserIdAsync<RatingTestModel>("userId");

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllRatesByUserIdShouldReturnZeroWhenInvalidUserId()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            var rates = new List<StarRating>();

            for (int i = 1; i <= 10; i++)
            {
                rates.Add(new StarRating
                {
                    Rate = i,
                    UserId = "userId",
                    BookId = i,
                });
            }

            await db.StarRatings.AddRangeAsync(rates);
            await db.SaveChangesAsync();

            var ratingsService = new StarRatingsService(db);

            var result = await ratingsService.GetAllRatesByUserIdAsync<RatingTestModel>("id");

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllRatesByUserIdShouldReturnCorrectCountWhenTakeAndSkipAreUsed()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            var rates = new List<StarRating>();

            for (int i = 1; i <= 10; i++)
            {
                rates.Add(new StarRating
                {
                    Rate = i,
                    UserId = "userId",
                    BookId = i,
                });
            }

            await db.StarRatings.AddRangeAsync(rates);
            await db.SaveChangesAsync();

            var ratingsService = new StarRatingsService(db);

            var result = await ratingsService.GetAllRatesByUserIdAsync<RatingTestModel>("userId", 5, 5);

            Assert.Equal(5, result.Count());
        }

        [Fact]
        public async Task GetAverageRatingByBookIdShouldReturnCorrectResult()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            var ratings = new List<StarRating>();

            for (int i = 1; i <= 2; i++)
            {
                ratings.Add(
                    new StarRating
                    {
                        Rate = 5,
                        UserId = "userId",
                        BookId = 1,
                    });
            }

            await db.StarRatings.AddRangeAsync(ratings);
            await db.SaveChangesAsync();

            var ratingsService = new StarRatingsService(db);
            var result = await ratingsService.GetAverageRatingByBookIdAsync(1);

            Assert.Equal(5, result);
        }

        [Fact]
        public async Task GetAverageRatingByBookIdShouldZeroWhenRatingsIsEmpty()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            var ratingsService = new StarRatingsService(db);
            var result = await ratingsService.GetAverageRatingByBookIdAsync(1);

            Assert.Equal(0, result);
        }

        [Fact]
        public async Task GetAverageRatingByBookIdShouldZeroWhenBookIdIsInvalid()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            var ratings = new List<StarRating>();

            for (int i = 1; i <= 2; i++)
            {
                ratings.Add(
                    new StarRating
                    {
                        Rate = 5,
                        UserId = "userId",
                        BookId = 1,
                    });
            }

            await db.StarRatings.AddRangeAsync(ratings);
            await db.SaveChangesAsync();

            var ratingsService = new StarRatingsService(db);
            var result = await ratingsService.GetAverageRatingByBookIdAsync(5);

            Assert.Equal(0, result);
        }

        public class RatingTestModel : IMapFrom<StarRating>
        {
            public int Id { get; set; }

            public int Rate { get; set; }

            public string UserId { get; set; }

            public int BookId { get; set; }
        }
    }
}
