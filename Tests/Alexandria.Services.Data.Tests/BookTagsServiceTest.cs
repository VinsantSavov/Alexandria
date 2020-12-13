namespace Alexandria.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Alexandria.Data;
    using Alexandria.Data.Models;
    using Alexandria.Services.BookTags;
    using Microsoft.EntityFrameworkCore;

    using Xunit;

    public class BookTagsServiceTest
    {
        [Fact]
        public async Task AddTagsToBookMethodShouldAddBookTagsInDatabase()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            var bookTagsService = new BookTagsService(db);

            var tags = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            await bookTagsService.AddTagsToBookAsync(1, tags);

            Assert.Equal(10, await db.BookTags.CountAsync());
        }

        [Fact]
        public async Task AddTagsToBookMethodShouldNotAddDuplicateTags()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            var bookTagsService = new BookTagsService(db);

            var tags = new List<int> { 1, 2, 3, 4, 5, 5, 5, 5, 5, 5 };

            await bookTagsService.AddTagsToBookAsync(1, tags);

            Assert.Equal(5, await db.BookTags.CountAsync());
        }

        [Fact]
        public async Task AddTagsToBookMethodShouldNotAddBookTagsWhichExist()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            await db.BookTags.AddRangeAsync(
                new BookTag
                {
                    BookId = 1,
                    TagId = 1,
                },
                new BookTag
                {
                    BookId = 1,
                    TagId = 2,
                });
            await db.SaveChangesAsync();

            var bookTagsService = new BookTagsService(db);

            var addTags = new List<int> { 1, 2, 3, 4, 5, 5, 5, 5, 5, 5 };

            await bookTagsService.AddTagsToBookAsync(1, addTags);

            Assert.Equal(5, await db.BookTags.CountAsync());
        }
    }
}
