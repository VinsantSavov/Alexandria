namespace Alexandria.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using Alexandria.Data;
    using Alexandria.Data.Models;
    using Alexandria.Services.Genres;
    using Alexandria.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    using Xunit;

    public class GenresServiceTest
    {
        public GenresServiceTest()
        {
            AutoMapperConfig.RegisterMappings(typeof(GenreTestModel).GetTypeInfo().Assembly);
        }

        [Fact]
        public async Task CreateGenreShouldAddToDatabase()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            var genresService = new GenresService(db);

            await genresService.CreateGenreAsync("test", "test description");

            Assert.Equal(1, await db.Genres.CountAsync());
        }

        [Fact]
        public async Task CreateGenreShouldCreateCorrectGenre()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            var genresService = new GenresService(db);

            await genresService.CreateGenreAsync("test", "test description");
            var result = await db.Genres.FirstOrDefaultAsync();

            Assert.Equal("test", result.Name);
            Assert.Equal("test description", result.Description);
        }

        [Fact]
        public async Task CreateGenreShouldReturnCorrectId()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            var genresService = new GenresService(db);

            var id = await genresService.CreateGenreAsync("test", "test description");
            var result = await db.Genres.FirstOrDefaultAsync();

            Assert.Equal(id, result.Id);
        }

        [Theory]
        [InlineData("test1", "description1")]
        [InlineData("test2", "description2")]
        [InlineData("test3", "description3")]
        public async Task EditGenreShouldWorkCorrectly(string name, string description)
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.Genres.AddAsync(
                new Genre
                {
                    Name = "test",
                    Description = "description",
                });
            await db.SaveChangesAsync();

            var genresService = new GenresService(db);

            await genresService.EditGenreAsync(1, name, description);
            var result = await db.Genres.FirstOrDefaultAsync();

            Assert.Equal(name, result.Name);
            Assert.Equal(description, result.Description);
            Assert.NotNull(result.ModifiedOn);
        }

        [Fact]
        public async Task DeleteGenreShouldSetIsDeletedAndDeletedOn()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.Genres.AddAsync(
                new Genre
                {
                    Name = "test",
                    Description = "description",
                });
            await db.SaveChangesAsync();

            var genresService = new GenresService(db);

            await genresService.DeleteGenreByIdAsync(1);
            var result = await db.Genres.FirstOrDefaultAsync();

            Assert.True(result.IsDeleted);
            Assert.NotNull(result.DeletedOn);
        }

        [Fact]
        public async Task DoesGenreIdExistShouldReturnTrueIfExists()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.Genres.AddAsync(
                new Genre
                {
                    Name = "test",
                    Description = "description",
                });
            await db.SaveChangesAsync();

            var genresService = new GenresService(db);

            var result = await genresService.DoesGenreIdExistAsync(1);

            Assert.True(result);
        }

        [Fact]
        public async Task DoesGenreIdExistShouldReturnFalseIfDeleted()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.Genres.AddAsync(
                new Genre
                {
                    Name = "test",
                    Description = "description",
                    IsDeleted = true,
                });
            await db.SaveChangesAsync();

            var genresService = new GenresService(db);

            var result = await genresService.DoesGenreIdExistAsync(1);

            Assert.False(result);
        }

        [Fact]
        public async Task DoesGenreIdExistShouldReturnFalseIfNotFound()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.Genres.AddAsync(
                new Genre
                {
                    Name = "test",
                    Description = "description",
                });
            await db.SaveChangesAsync();

            var genresService = new GenresService(db);

            var result = await genresService.DoesGenreIdExistAsync(3);

            Assert.False(result);
        }

        [Fact]
        public async Task DoesGenreNameExistShouldReturnTrueWhenExists()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.Genres.AddAsync(
                new Genre
                {
                    Name = "test",
                    Description = "description",
                });
            await db.SaveChangesAsync();

            var genresService = new GenresService(db);

            var result = await genresService.DoesGenreNameExistAsync("test");

            Assert.True(result);
        }

        [Fact]
        public async Task DoesGenreNameExistShouldReturnTrueEvenIfDeleted()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.Genres.AddAsync(
                new Genre
                {
                    Name = "test",
                    Description = "description",
                    IsDeleted = true,
                    DeletedOn = DateTime.UtcNow,
                });
            await db.SaveChangesAsync();

            var genresService = new GenresService(db);

            var result = await genresService.DoesGenreNameExistAsync("test");

            Assert.True(result);
        }

        [Fact]
        public async Task DoesGenreNameExistShouldReturnFalseIfNotFound()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.Genres.AddAsync(
                new Genre
                {
                    Name = "test",
                    Description = "description",
                });
            await db.SaveChangesAsync();

            var genresService = new GenresService(db);

            var result = await genresService.DoesGenreNameExistAsync("test1");

            Assert.False(result);
        }

        [Fact]
        public async Task GetAllGenresShouldReturnAllGenres()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.Genres.AddRangeAsync(
                new Genre
                {
                    Name = "test1",
                    Description = "description1",
                },
                new Genre
                {
                    Name = "test2",
                    Description = "description2",
                },
                new Genre
                {
                    Name = "test3",
                    Description = "description3",
                });
            await db.SaveChangesAsync();

            var genresService = new GenresService(db);

            var result = await genresService.GetAllGenresAsync<GenreTestModel>();

            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task GetAllGenresShouldNotReturnDeleted()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.Genres.AddRangeAsync(
                new Genre
                {
                    Name = "test1",
                    Description = "description1",
                },
                new Genre
                {
                    Name = "test2",
                    Description = "description2",
                },
                new Genre
                {
                    Name = "test3",
                    Description = "description3",
                    IsDeleted = true,
                });
            await db.SaveChangesAsync();

            var genresService = new GenresService(db);

            var result = await genresService.GetAllGenresAsync<GenreTestModel>();

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetAllGenresShouldReturnCorrectGenresWhenTakeAndSkip()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.Genres.AddRangeAsync(
                new Genre
                {
                    Name = "test1",
                    Description = "description1",
                },
                new Genre
                {
                    Name = "test2",
                    Description = "description2",
                },
                new Genre
                {
                    Name = "test3",
                    Description = "description3",
                });
            await db.SaveChangesAsync();

            var genresService = new GenresService(db);

            var result = await genresService.GetAllGenresAsync<GenreTestModel>(2, 1);

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetGenresCountShouldReturnCorrectCount()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.Genres.AddRangeAsync(
                new Genre
                {
                    Name = "test1",
                    Description = "description1",
                },
                new Genre
                {
                    Name = "test2",
                    Description = "description2",
                },
                new Genre
                {
                    Name = "test3",
                    Description = "description3",
                });
            await db.SaveChangesAsync();

            var genresService = new GenresService(db);

            var result = await genresService.GetGenresCountAsync();

            Assert.Equal(3, result);
        }

        [Fact]
        public async Task GetGenresCountShouldReturnCorrectCountIfThereAreDeleted()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.Genres.AddRangeAsync(
                new Genre
                {
                    Name = "test1",
                    Description = "description1",
                },
                new Genre
                {
                    Name = "test2",
                    Description = "description2",
                },
                new Genre
                {
                    Name = "test3",
                    Description = "description3",
                    IsDeleted = true,
                });
            await db.SaveChangesAsync();

            var genresService = new GenresService(db);

            var result = await genresService.GetGenresCountAsync();

            Assert.Equal(2, result);
        }

        [Fact]
        public async Task GetGenreShouldReturnCorrectGenre()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.Genres.AddRangeAsync(
                new Genre
                {
                    Name = "test1",
                    Description = "description1",
                },
                new Genre
                {
                    Name = "test2",
                    Description = "description2",
                },
                new Genre
                {
                    Name = "test3",
                    Description = "description3",
                    IsDeleted = true,
                });
            await db.SaveChangesAsync();

            var genresService = new GenresService(db);

            var result = await genresService.GetGenreByIdAsync<GenreTestModel>(1);

            Assert.Equal("test1", result.Name);
            Assert.Equal("description1", result.Description);
        }

        [Fact]
        public async Task GetGenreShouldReturnNullWhenDeleted()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.Genres.AddRangeAsync(
                new Genre
                {
                    Name = "test1",
                    Description = "description1",
                    IsDeleted = true,
                },
                new Genre
                {
                    Name = "test2",
                    Description = "description2",
                },
                new Genre
                {
                    Name = "test3",
                    Description = "description3",
                    IsDeleted = true,
                });
            await db.SaveChangesAsync();

            var genresService = new GenresService(db);

            var result = await genresService.GetGenreByIdAsync<GenreTestModel>(1);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetGenreShouldReturnNullWhenNotFound()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            var genresService = new GenresService(db);

            var result = await genresService.GetGenreByIdAsync<GenreTestModel>(1);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetRandomGenresShouldReturnRandomGenres()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.Genres.AddRangeAsync(
                new Genre
                {
                    Name = "test1",
                    Description = "description1",
                },
                new Genre
                {
                    Name = "test2",
                    Description = "description2",
                },
                new Genre
                {
                    Name = "test3",
                    Description = "description3",
                });
            await db.SaveChangesAsync();

            var genresService = new GenresService(db);

            var result = await genresService.GetRandomGenresAsync<GenreTestModel>(2);

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetRandomGenresShouldNotReturnDeletedGenres()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.Genres.AddRangeAsync(
                new Genre
                {
                    Name = "test1",
                    Description = "description1",
                    IsDeleted = true,
                },
                new Genre
                {
                    Name = "test2",
                    Description = "description2",
                    IsDeleted = true,
                },
                new Genre
                {
                    Name = "test3",
                    Description = "description3",
                });
            await db.SaveChangesAsync();

            var genresService = new GenresService(db);

            var result = await genresService.GetRandomGenresAsync<GenreTestModel>(2);

            Assert.Single(result);
        }

        public class GenreTestModel : IMapFrom<Genre>
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public string Description { get; set; }
        }
    }
}
