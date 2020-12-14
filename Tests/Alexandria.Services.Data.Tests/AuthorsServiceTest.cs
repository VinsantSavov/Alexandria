namespace Alexandria.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using Alexandria.Data;
    using Alexandria.Data.Models;
    using Alexandria.Services.Authors;
    using Alexandria.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    using Xunit;

    public class AuthorsServiceTest
    {
        public AuthorsServiceTest()
        {
            AutoMapperConfig.RegisterMappings(typeof(AuthorTestModel).GetTypeInfo().Assembly);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task GetAuthorByIdShouldReturnRightAuthor(int authorId)
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            await db.Authors.AddRangeAsync(
                new Author
                {
                    FirstName = "first1",
                    SecondName = "second1",
                    LastName = "last1",
                },
                new Author
                {
                    FirstName = "first1",
                    SecondName = "second1",
                    LastName = "last1",
                },
                new Author
                {
                    FirstName = "first1",
                    SecondName = "second1",
                    LastName = "last1",
                });

            await db.SaveChangesAsync();

            var authorsService = new AuthorsService(db);

            var result = authorsService.GetAuthorByIdAsync<AuthorTestModel>(authorId);

            Assert.Equal(authorId, result.Id);
        }

        [Fact]
        public async Task GetAuthorByIdShouldReturnNullWhenDeleted()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            await db.Authors.AddAsync(
                new Author
                {
                    FirstName = "first1",
                    SecondName = "second1",
                    LastName = "last1",
                    IsDeleted = true,
                    DeletedOn = DateTime.UtcNow,
                });

            await db.SaveChangesAsync();

            var authorsService = new AuthorsService(db);

            var result = await authorsService.GetAuthorByIdAsync<AuthorTestModel>(1);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetAuthorByIdShouldReturnNullWhenNotFound()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            var authorsService = new AuthorsService(db);

            var result = await authorsService.GetAuthorByIdAsync<AuthorTestModel>(1);

            Assert.Null(result);
        }

        [Fact]
        public async Task DoesAuthorIdExistShouldReturnTrueIfExists()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            await db.Authors.AddAsync(
                new Author
                {
                    FirstName = "first1",
                    SecondName = "second1",
                    LastName = "last1",
                });

            await db.SaveChangesAsync();

            var authorsService = new AuthorsService(db);

            var result = await authorsService.DoesAuthorIdExistAsync(1);

            Assert.True(result);
        }

        [Fact]
        public async Task DoesAuthorIdExistShouldReturnFalseIfDeleted()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            await db.Authors.AddAsync(
                new Author
                {
                    FirstName = "first1",
                    SecondName = "second1",
                    LastName = "last1",
                    IsDeleted = true,
                    DeletedOn = DateTime.UtcNow,
                });

            await db.SaveChangesAsync();

            var authorsService = new AuthorsService(db);

            var result = await authorsService.DoesAuthorIdExistAsync(1);

            Assert.False(result);
        }

        [Fact]
        public async Task DoesAuthorIdExistShouldReturnFalseWhenNotFound()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            await db.Authors.AddAsync(
                new Author
                {
                    FirstName = "first1",
                    SecondName = "second1",
                    LastName = "last1",
                });

            await db.SaveChangesAsync();

            var authorsService = new AuthorsService(db);

            var result = await authorsService.DoesAuthorIdExistAsync(5);

            Assert.False(result);
        }

        [Fact]
        public async Task GetAllAuthorsShouldReturnAllAuthors()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            await db.Authors.AddRangeAsync(
                new Author
                {
                    FirstName = "first1",
                    SecondName = "second1",
                    LastName = "last1",
                },
                new Author
                {
                    FirstName = "first2",
                    SecondName = "second2",
                    LastName = "last2",
                },
                new Author
                {
                    FirstName = "first3",
                    SecondName = "second3",
                    LastName = "last3",
                });

            await db.SaveChangesAsync();

            var authorsService = new AuthorsService(db);

            var result = await authorsService.GetAllAuthorsAsync<AuthorTestModel>();

            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task GetAllAuthorsShouldNotReturnDeletedAuthors()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            await db.Authors.AddRangeAsync(
                new Author
                {
                    FirstName = "first1",
                    SecondName = "second1",
                    LastName = "last1",
                },
                new Author
                {
                    FirstName = "first2",
                    SecondName = "second2",
                    LastName = "last2",
                },
                new Author
                {
                    FirstName = "first3",
                    SecondName = "second3",
                    LastName = "last3",
                    IsDeleted = true,
                    DeletedOn = DateTime.UtcNow,
                });

            await db.SaveChangesAsync();

            var authorsService = new AuthorsService(db);

            var result = await authorsService.GetAllAuthorsAsync<AuthorTestModel>();

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetAllAuthorsShouldReturnAllAuthorsInRightOrder()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            await db.Authors.AddRangeAsync(
                new Author
                {
                    FirstName = "first1",
                    SecondName = "second1",
                    LastName = "last1",
                },
                new Author
                {
                    FirstName = "first2",
                    SecondName = "second2",
                    LastName = "last2",
                },
                new Author
                {
                    FirstName = "afirst3",
                    SecondName = "asecond3",
                    LastName = "alast3",
                });

            await db.SaveChangesAsync();

            var authorsService = new AuthorsService(db);

            var result = await authorsService.GetAllAuthorsAsync<AuthorTestModel>();
            var resultAuthor = result.FirstOrDefault();

            Assert.Equal(3, result.Count());
            Assert.Equal(3, resultAuthor.Id);
            Assert.Equal("afirst3", resultAuthor.FirstName);
        }

        public class AuthorTestModel : IMapFrom<Author>
        {
            public int Id { get; set; }

            public string FirstName { get; set; }

            public string SecondName { get; set; }

            public string LastName { get; set; }
        }
    }
}
