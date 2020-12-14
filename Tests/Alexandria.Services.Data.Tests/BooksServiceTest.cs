namespace Alexandria.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using Alexandria.Data;
    using Alexandria.Data.Models;
    using Alexandria.Services.Books;
    using Alexandria.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    using Xunit;

    public class BooksServiceTest
    {
        public BooksServiceTest()
        {
            AutoMapperConfig.RegisterMappings(typeof(BookTestModel).GetTypeInfo().Assembly);
        }

        [Fact]
        public async Task CreateBookMethodShouldAddBookInDatabase()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            var booksService = new BooksService(db);
            var genresIds = new List<int> { 1, 2, 3 };
            var tagsIds = new List<int> { 1, 2, 3 };
            var awardsIds = new List<int> { 1, 2, 3 };

            int resultId = await booksService
                .CreateBookAsync("test", 1, "summary", DateTime.UtcNow, 1, "picture", 1, "amazon", genresIds, tagsIds, awardsIds);

            Assert.Equal(1, resultId);
            Assert.Equal(1, await db.Books.CountAsync());
        }

        [Fact]
        public async Task CreateBookMethodShouldAddRightBook()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            var booksService = new BooksService(db);
            var genresIds = new List<int> { 1, 2, 3 };
            var tagsIds = new List<int> { 1, 2, 3 };
            var awardsIds = new List<int> { 1, 2, 3 };

            int resultId = await booksService
                .CreateBookAsync("test", 1, "summary", DateTime.UtcNow, 1, "picture", 1, "amazon", genresIds, tagsIds, awardsIds);

            var book = await db.Books.FirstOrDefaultAsync();

            Assert.Equal("test", book.Title);
            Assert.Equal("summary", book.Summary);
            Assert.Equal("picture", book.PictureURL);
            Assert.Equal(1, book.AuthorId);
        }

        [Fact]
        public async Task CreateBookMethodShouldAddGenresTagsAndAwards()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            var booksService = new BooksService(db);
            var genresIds = new List<int> { 1, 2, 3 };
            var tagsIds = new List<int> { 1, 2, 3 };
            var awardsIds = new List<int> { 1, 2, 3 };

            int resultId = await booksService
                .CreateBookAsync("test", 1, "summary", DateTime.UtcNow, 1, "picture", 1, "amazon", genresIds, tagsIds, awardsIds);

            var book = await db.Books.FirstOrDefaultAsync();

            Assert.Equal(genresIds, book.Genres.Select(g => g.GenreId));
            Assert.Equal(tagsIds, book.Tags.Select(g => g.TagId));
            Assert.Equal(awardsIds, book.Awards.Select(g => g.AwardId));
        }

        [Fact]
        public async Task CreateBookMethodShouldNotAddExistingGenresTagsAndAwards()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            var booksService = new BooksService(db);
            var genresIds = new List<int> { 1, 2, 3, 3, 3 };
            var tagsIds = new List<int> { 1, 2, 3, 3, 3 };
            var awardsIds = new List<int> { 1, 2, 3, 3, 3 };

            int resultId = await booksService
                .CreateBookAsync("test", 1, "summary", DateTime.UtcNow, 1, "picture", 1, "amazon", genresIds, tagsIds, awardsIds);

            var book = await db.Books.FirstOrDefaultAsync();

            Assert.NotEqual(genresIds, book.Genres.Select(g => g.GenreId));
            Assert.NotEqual(tagsIds, book.Tags.Select(g => g.TagId));
            Assert.NotEqual(awardsIds, book.Awards.Select(g => g.AwardId));
        }

        [Fact]
        public async Task DeleteBookMethodShouldSetIsDeletedAndDeletedOn()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.Books.AddAsync(
                new Book
                {
                    Title = "test",
                    CreatedOn = DateTime.UtcNow,
                });
            await db.SaveChangesAsync();

            var booksService = new BooksService(db);

            await booksService.DeleteByIdAsync(1);

            var book = await db.Books.FirstOrDefaultAsync();

            Assert.Equal("test", book.Title);
            Assert.True(book.IsDeleted);
            Assert.NotNull(book.DeletedOn);
        }

        [Theory]
        [InlineData("test1", "summary1", 1, "picture1", "amazon1")]
        [InlineData("test2", "summary2", 2, "picture2", "amazon2")]
        [InlineData("test3", "summary3", 3, "picture3", "amazon3")]
        public async Task EditBookMethodShouldChangeProperties(string title, string summary, int pages, string pictureUrl, string amazonLink)
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.Books.AddAsync(
                new Book
                {
                    Title = "test",
                    Summary = "summary",
                    CreatedOn = DateTime.UtcNow,
                });
            await db.SaveChangesAsync();

            var booksService = new BooksService(db);

            await booksService.EditBookAsync(1, title, summary, DateTime.UtcNow, pages, pictureUrl, amazonLink);

            var book = await db.Books.FirstOrDefaultAsync();

            Assert.Equal(title, book.Title);
            Assert.Equal(summary, book.Summary);
            Assert.Equal(pages, book.Pages);
            Assert.Equal(pictureUrl, book.PictureURL);
            Assert.Equal(amazonLink, book.AmazonLink);
        }

        [Fact]
        public async Task GetPictureUrlShouldReturnCorrectPicture()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.Books.AddAsync(
                new Book
                {
                    PictureURL = "picture1",
                });
            await db.SaveChangesAsync();

            var booksService = new BooksService(db);

            var result = await booksService.GetPictureUrlByBookIdAsync(1);

            Assert.Equal("picture1", result);
        }

        [Fact]
        public async Task GetBookByIdShouldReturnRightBook()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.Books.AddRangeAsync(
                new Book
                {
                    Title = "title1",
                    Summary = "summary1",
                    CreatedOn = DateTime.UtcNow,
                },
                new Book
                {
                    Title = "title2",
                    Summary = "summary2",
                    CreatedOn = DateTime.UtcNow,
                },
                new Book
                {
                    Title = "title3",
                    Summary = "summary3",
                    CreatedOn = DateTime.UtcNow,
                });
            await db.SaveChangesAsync();

            var booksService = new BooksService(db);

            var firstResult = await booksService.GetBookByIdAsync<BookTestModel>(1);
            var secondResult = await booksService.GetBookByIdAsync<BookTestModel>(2);
            var thirdResult = await booksService.GetBookByIdAsync<BookTestModel>(3);

            Assert.Equal("title1", firstResult.Title);
            Assert.Equal("summary1", firstResult.Summary);
            Assert.Equal("title2", secondResult.Title);
            Assert.Equal("summary2", secondResult.Summary);
            Assert.Equal("title3", thirdResult.Title);
            Assert.Equal("summary3", thirdResult.Summary);
        }

        [Fact]
        public async Task GetBookByIdShouldReturnNullWhenBookIsDeleted()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.Books.AddAsync(
                new Book
                {
                    Title = "title1",
                    Summary = "summary1",
                    CreatedOn = DateTime.UtcNow,
                    IsDeleted = true,
                    DeletedOn = DateTime.UtcNow,
                });
            await db.SaveChangesAsync();

            var booksService = new BooksService(db);

            var firstResult = await booksService.GetBookByIdAsync<BookTestModel>(1);

            Assert.Null(firstResult);
        }

        [Fact]
        public async Task GetBookByIdShouldReturnNullWhenNotFound()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.Books.AddAsync(
                new Book
                {
                    Title = "title1",
                    Summary = "summary1",
                    CreatedOn = DateTime.UtcNow,
                });
            await db.SaveChangesAsync();

            var booksService = new BooksService(db);

            var firstResult = await booksService.GetBookByIdAsync<BookTestModel>(5);

            Assert.Null(firstResult);
        }

        [Fact]
        public async Task SearchBooksByTitleShouldReturnCorrectCount()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.Books.AddRangeAsync(
                new Book
                {
                    Title = "title1",
                    CreatedOn = DateTime.UtcNow,
                },
                new Book
                {
                    Title = "title2",
                    CreatedOn = DateTime.UtcNow,
                },
                new Book
                {
                    Title = "title3",
                    CreatedOn = DateTime.UtcNow,
                },
                new Book
                {
                    Title = "bot",
                    CreatedOn = DateTime.UtcNow,
                });
            await db.SaveChangesAsync();

            var booksService = new BooksService(db);

            var result = await booksService.SearchBooksByTitleAsync<BookTestModel>("title");

            Assert.Equal(3, result.Count());
            Assert.NotNull(result);
        }

        [Fact]
        public async Task SearchBooksByTitleShouldBeEmptyWhenTitleIsNotFound()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.Books.AddRangeAsync(
                new Book
                {
                    Title = "title1",
                    CreatedOn = DateTime.UtcNow,
                },
                new Book
                {
                    Title = "title2",
                    CreatedOn = DateTime.UtcNow,
                },
                new Book
                {
                    Title = "title3",
                    CreatedOn = DateTime.UtcNow,
                },
                new Book
                {
                    Title = "bot",
                    CreatedOn = DateTime.UtcNow,
                });
            await db.SaveChangesAsync();

            var booksService = new BooksService(db);

            var result = await booksService.SearchBooksByTitleAsync<BookTestModel>("kek");

            Assert.Empty(result);
        }

        [Fact]
        public async Task SearchBooksByTitleShouldReturnAllWhenTitleIsNull()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.Books.AddRangeAsync(
                new Book
                {
                    Title = "title1",
                    CreatedOn = DateTime.UtcNow,
                },
                new Book
                {
                    Title = "title2",
                    CreatedOn = DateTime.UtcNow,
                },
                new Book
                {
                    Title = "title3",
                    CreatedOn = DateTime.UtcNow,
                },
                new Book
                {
                    Title = "bot",
                    CreatedOn = DateTime.UtcNow,
                });
            await db.SaveChangesAsync();

            var booksService = new BooksService(db);

            var result = await booksService.SearchBooksByTitleAsync<BookTestModel>(null);

            Assert.Equal(4, result.Count());
        }

        [Fact]
        public async Task SearchBooksByTitleShouldWorkWhenTakeAndSkipAreGiven()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.Books.AddRangeAsync(
                new Book
                {
                    Title = "title1",
                    CreatedOn = DateTime.UtcNow,
                },
                new Book
                {
                    Title = "title2",
                    CreatedOn = DateTime.UtcNow,
                },
                new Book
                {
                    Title = "title3",
                    CreatedOn = DateTime.UtcNow,
                },
                new Book
                {
                    Title = "bot",
                    CreatedOn = DateTime.UtcNow,
                });
            await db.SaveChangesAsync();

            var booksService = new BooksService(db);

            var result = await booksService.SearchBooksByTitleAsync<BookTestModel>("title", 3, 1);

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetRandomBooksShouldReturnCorrectCount()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.Books.AddRangeAsync(
                new Book
                {
                    Title = "title1",
                    CreatedOn = DateTime.UtcNow,
                },
                new Book
                {
                    Title = "title2",
                    CreatedOn = DateTime.UtcNow,
                },
                new Book
                {
                    Title = "title3",
                    CreatedOn = DateTime.UtcNow,
                },
                new Book
                {
                    Title = "bot",
                    CreatedOn = DateTime.UtcNow,
                });
            await db.SaveChangesAsync();

            var booksService = new BooksService(db);

            var result = await booksService.GetRandomBooksAsync<BookTestModel>(2);

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task DoesBookIdExistShouldReturnTrueWhenExists()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.Books.AddAsync(
                new Book
                {
                    Title = "title1",
                    CreatedOn = DateTime.UtcNow,
                });
            await db.SaveChangesAsync();

            var booksService = new BooksService(db);

            var result = await booksService.DoesBookIdExistAsync(1);

            Assert.True(result);
        }

        [Fact]
        public async Task DoesBookIdExistShouldReturnFalseWhenDeleted()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.Books.AddAsync(
                new Book
                {
                    Title = "title1",
                    CreatedOn = DateTime.UtcNow,
                    IsDeleted = true,
                    DeletedOn = DateTime.UtcNow,
                });
            await db.SaveChangesAsync();

            var booksService = new BooksService(db);

            var result = await booksService.DoesBookIdExistAsync(1);

            Assert.False(result);
        }

        [Fact]
        public async Task GetBooksCountShouldReturnCorrectCountWithoutSearch()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.Books.AddRangeAsync(
                new Book
                {
                    Title = "title1",
                    CreatedOn = DateTime.UtcNow,
                },
                new Book
                {
                    Title = "title2",
                    CreatedOn = DateTime.UtcNow,
                },
                new Book
                {
                    Title = "title3",
                    CreatedOn = DateTime.UtcNow,
                },
                new Book
                {
                    Title = "bot",
                    CreatedOn = DateTime.UtcNow,
                });
            await db.SaveChangesAsync();

            var booksService = new BooksService(db);

            var result = await booksService.GetBooksCountAsync();

            Assert.Equal(4, result);
        }

        [Fact]
        public async Task GetBooksCountShouldReturnCorrectCountWithSearch()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.Books.AddRangeAsync(
                new Book
                {
                    Title = "title1",
                    CreatedOn = DateTime.UtcNow,
                },
                new Book
                {
                    Title = "title2",
                    CreatedOn = DateTime.UtcNow,
                },
                new Book
                {
                    Title = "title3",
                    CreatedOn = DateTime.UtcNow,
                },
                new Book
                {
                    Title = "bot",
                    CreatedOn = DateTime.UtcNow,
                });
            await db.SaveChangesAsync();

            var booksService = new BooksService(db);

            var result = await booksService.GetBooksCountAsync("title");

            Assert.Equal(3, result);
        }

        [Fact]
        public async Task GetBooksCountByAuthorIdShouldReturnCorrectCount()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.Books.AddRangeAsync(
                new Book
                {
                    Title = "title1",
                    CreatedOn = DateTime.UtcNow,
                    AuthorId = 1,
                },
                new Book
                {
                    Title = "title2",
                    CreatedOn = DateTime.UtcNow,
                    AuthorId = 1,
                },
                new Book
                {
                    Title = "title3",
                    CreatedOn = DateTime.UtcNow,
                    AuthorId = 1,
                },
                new Book
                {
                    Title = "bot",
                    CreatedOn = DateTime.UtcNow,
                });
            await db.SaveChangesAsync();

            var booksService = new BooksService(db);

            var result = await booksService.GetBooksCountByAuthorIdAsync(1);

            Assert.Equal(3, result);
        }

        [Fact]
        public async Task GetBooksCountByGenreIdShouldReturnCorrectCount()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            await db.Books.AddRangeAsync(
                    new Book
                    {
                        Title = "title1",
                        CreatedOn = DateTime.UtcNow,
                        Genres = new List<BookGenre> { new BookGenre { BookId = 1, GenreId = 1 } },
                    },
                    new Book
                    {
                        Title = "title2",
                        CreatedOn = DateTime.UtcNow,
                        Genres = new List<BookGenre> { new BookGenre { BookId = 2, GenreId = 1 } },
                    },
                    new Book
                    {
                        Title = "title3",
                        CreatedOn = DateTime.UtcNow,
                        Genres = new List<BookGenre> { new BookGenre { BookId = 3, GenreId = 1 } },
                        IsDeleted = true,
                        DeletedOn = DateTime.UtcNow,
                    },
                    new Book
                    {
                        Title = "bot",
                        CreatedOn = DateTime.UtcNow,
                    });
            await db.SaveChangesAsync();

            var booksService = new BooksService(db);

            var result = await booksService.GetBooksCountByGenreIdAsync(1);

            Assert.Equal(2, result);
        }

        [Fact]
        public async Task NewRealesedBooksByGenreIdShouldReturnCorrectCount()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            await db.Books.AddRangeAsync(
                    new Book
                    {
                        Title = "title1",
                        CreatedOn = DateTime.UtcNow,
                        Genres = new List<BookGenre> { new BookGenre { BookId = 1, GenreId = 1 } },
                    },
                    new Book
                    {
                        Title = "title2",
                        CreatedOn = DateTime.UtcNow,
                        Genres = new List<BookGenre> { new BookGenre { BookId = 2, GenreId = 1 } },
                    },
                    new Book
                    {
                        Title = "title3",
                        CreatedOn = DateTime.UtcNow,
                        Genres = new List<BookGenre> { new BookGenre { BookId = 3, GenreId = 1 } },
                        IsDeleted = true,
                        DeletedOn = DateTime.UtcNow,
                    },
                    new Book
                    {
                        Title = "bot",
                        CreatedOn = DateTime.UtcNow,
                    });
            await db.SaveChangesAsync();

            var booksService = new BooksService(db);

            var result = await booksService.NewRealesedBooksByGenreIdAsync<BookTestModel>(1);

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task NewRealesedBooksByGenreIdShouldReturnCorrectCountWithTakeAndSkip()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            await db.Books.AddRangeAsync(
                    new Book
                    {
                        Title = "title1",
                        CreatedOn = DateTime.UtcNow,
                        Genres = new List<BookGenre> { new BookGenre { BookId = 1, GenreId = 1 } },
                    },
                    new Book
                    {
                        Title = "title2",
                        CreatedOn = DateTime.UtcNow,
                        Genres = new List<BookGenre> { new BookGenre { BookId = 2, GenreId = 1 } },
                    },
                    new Book
                    {
                        Title = "title3",
                        CreatedOn = DateTime.UtcNow,
                        Genres = new List<BookGenre> { new BookGenre { BookId = 3, GenreId = 1 } },
                    },
                    new Book
                    {
                        Title = "bot",
                        CreatedOn = DateTime.UtcNow,
                    });
            await db.SaveChangesAsync();

            var booksService = new BooksService(db);

            var result = await booksService.NewRealesedBooksByGenreIdAsync<BookTestModel>(1, 5, 1);

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task TopRatedBooksByGenreIdShouldReturnRightResult()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            await db.Books.AddRangeAsync(
                    new Book
                    {
                        Title = "title1",
                        Ratings = new List<StarRating> { new StarRating { BookId = 1, UserId = "userId", Rate = 1 } },
                        CreatedOn = DateTime.UtcNow,
                        Genres = new List<BookGenre> { new BookGenre { BookId = 1, GenreId = 1 } },
                    },
                    new Book
                    {
                        Title = "title2",
                        Ratings = new List<StarRating> { new StarRating { BookId = 2, UserId = "userId", Rate = 5 } },
                        CreatedOn = DateTime.UtcNow,
                        Genres = new List<BookGenre> { new BookGenre { BookId = 2, GenreId = 1 } },
                    },
                    new Book
                    {
                        Title = "title3",
                        Ratings = new List<StarRating> { new StarRating { BookId = 3, UserId = "userId", Rate = 3 } },
                        CreatedOn = DateTime.UtcNow,
                        Genres = new List<BookGenre> { new BookGenre { BookId = 3, GenreId = 1 } },
                    },
                    new Book
                    {
                        Title = "bot",
                        CreatedOn = DateTime.UtcNow,
                    });
            await db.SaveChangesAsync();

            var booksService = new BooksService(db);

            var result = await booksService.TopRatedBooksByGenreIdAsync<BookTestModel>(1);
            var resultBook = result.FirstOrDefault();

            Assert.Equal(3, result.Count());
            Assert.Equal(2, resultBook.Id);
            Assert.Equal("title2", resultBook.Title);
        }

        [Fact]
        public async Task TopRatedBooksByGenreIdShouldReturnRightResultWithTakeAndSkip()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            await db.Books.AddRangeAsync(
                    new Book
                    {
                        Title = "title1",
                        Ratings = new List<StarRating> { new StarRating { BookId = 1, UserId = "userId", Rate = 1 } },
                        CreatedOn = DateTime.UtcNow,
                        Genres = new List<BookGenre> { new BookGenre { BookId = 1, GenreId = 1 } },
                    },
                    new Book
                    {
                        Title = "title2",
                        Ratings = new List<StarRating> { new StarRating { BookId = 2, UserId = "userId", Rate = 5 } },
                        CreatedOn = DateTime.UtcNow,
                        Genres = new List<BookGenre> { new BookGenre { BookId = 2, GenreId = 1 } },
                    },
                    new Book
                    {
                        Title = "title3",
                        Ratings = new List<StarRating> { new StarRating { BookId = 3, UserId = "userId", Rate = 3 } },
                        CreatedOn = DateTime.UtcNow,
                        Genres = new List<BookGenre> { new BookGenre { BookId = 3, GenreId = 1 } },
                    },
                    new Book
                    {
                        Title = "bot",
                        CreatedOn = DateTime.UtcNow,
                    });
            await db.SaveChangesAsync();

            var booksService = new BooksService(db);

            var result = await booksService.TopRatedBooksByGenreIdAsync<BookTestModel>(1, 5, 1);
            var resultBook = result.FirstOrDefault();

            Assert.Equal(2, result.Count());
            Assert.Equal(3, resultBook.Id);
            Assert.Equal("title3", resultBook.Title);
        }

        [Fact]
        public async Task GetLatestPublishedBooksShouldReturnRightResult()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            await db.Books.AddRangeAsync(
                    new Book
                    {
                        Title = "title1",
                        Ratings = new List<StarRating> { new StarRating { BookId = 1, UserId = "userId", Rate = 1 } },
                        CreatedOn = DateTime.UtcNow,
                        PublishedOn = new DateTime(2020, 4, 12),
                    },
                    new Book
                    {
                        Title = "title2",
                        Ratings = new List<StarRating> { new StarRating { BookId = 2, UserId = "userId", Rate = 5 } },
                        CreatedOn = DateTime.UtcNow,
                        PublishedOn = new DateTime(2018, 4, 12),
                    },
                    new Book
                    {
                        Title = "title3",
                        Ratings = new List<StarRating> { new StarRating { BookId = 3, UserId = "userId", Rate = 3 } },
                        CreatedOn = DateTime.UtcNow,
                        PublishedOn = new DateTime(2017, 4, 12),
                    });
            await db.SaveChangesAsync();

            var booksService = new BooksService(db);

            var result = await booksService.GetLatestPublishedBooksAsync<BookTestModel>();
            var resultBook = result.FirstOrDefault();

            Assert.Equal(3, result.Count());
            Assert.Equal(1, resultBook.Id);
            Assert.Equal("title1", resultBook.Title);
        }

        [Fact]
        public async Task GetLatestPublishedBooksShouldReturnRightResultWithTakeAndSkip()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            await db.Books.AddRangeAsync(
                    new Book
                    {
                        Title = "title1",
                        Ratings = new List<StarRating> { new StarRating { BookId = 1, UserId = "userId", Rate = 1 } },
                        CreatedOn = DateTime.UtcNow,
                        PublishedOn = new DateTime(2020, 4, 12),
                    },
                    new Book
                    {
                        Title = "title2",
                        Ratings = new List<StarRating> { new StarRating { BookId = 2, UserId = "userId", Rate = 5 } },
                        CreatedOn = DateTime.UtcNow,
                        PublishedOn = new DateTime(2018, 4, 12),
                    },
                    new Book
                    {
                        Title = "title3",
                        Ratings = new List<StarRating> { new StarRating { BookId = 3, UserId = "userId", Rate = 3 } },
                        CreatedOn = DateTime.UtcNow,
                        PublishedOn = new DateTime(2017, 4, 12),
                    });
            await db.SaveChangesAsync();

            var booksService = new BooksService(db);

            var result = await booksService.GetLatestPublishedBooksAsync<BookTestModel>(1, 1);
            var resultBook = result.FirstOrDefault();

            Assert.Single(result);
            Assert.Equal(2, resultBook.Id);
            Assert.Equal("title2", resultBook.Title);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task GetAllBooksByAuthorIdShouldReturnCorrectCount(int authorId)
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            await db.Books.AddRangeAsync(
                    new Book
                    {
                        Title = "title1",
                        AuthorId = authorId,
                        Ratings = new List<StarRating> { new StarRating { BookId = 1, UserId = "userId", Rate = 1 } },
                        CreatedOn = DateTime.UtcNow,
                        PublishedOn = new DateTime(2020, 4, 12),
                    },
                    new Book
                    {
                        Title = "title2",
                        AuthorId = authorId,
                        Ratings = new List<StarRating> { new StarRating { BookId = 2, UserId = "userId", Rate = 5 } },
                        CreatedOn = DateTime.UtcNow,
                        PublishedOn = new DateTime(2018, 4, 12),
                    },
                    new Book
                    {
                        Title = "title3",
                        AuthorId = authorId,
                        Ratings = new List<StarRating> { new StarRating { BookId = 3, UserId = "userId", Rate = 3 } },
                        CreatedOn = DateTime.UtcNow,
                        PublishedOn = new DateTime(2017, 4, 12),
                    });
            await db.SaveChangesAsync();

            var booksService = new BooksService(db);

            var result = await booksService.GetAllBooksByAuthorIdAsync<BookTestModel>(authorId);

            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task GetAllBooksByAuthorIdShouldReturnCorrectOrderWithTakeAndSkip()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            await db.Books.AddRangeAsync(
                    new Book
                    {
                        Title = "title1",
                        AuthorId = 1,
                        Ratings = new List<StarRating> { new StarRating { BookId = 1, UserId = "userId", Rate = 1 } },
                        CreatedOn = DateTime.UtcNow,
                        PublishedOn = new DateTime(2020, 4, 12),
                    },
                    new Book
                    {
                        Title = "title2",
                        AuthorId = 1,
                        Ratings = new List<StarRating> { new StarRating { BookId = 2, UserId = "userId", Rate = 5 } },
                        CreatedOn = DateTime.UtcNow,
                        PublishedOn = new DateTime(2018, 4, 12),
                    },
                    new Book
                    {
                        Title = "title3",
                        AuthorId = 1,
                        Ratings = new List<StarRating> { new StarRating { BookId = 3, UserId = "userId", Rate = 3 } },
                        CreatedOn = DateTime.UtcNow,
                        PublishedOn = new DateTime(2017, 4, 12),
                    });
            await db.SaveChangesAsync();

            var booksService = new BooksService(db);

            var result = await booksService.GetAllBooksByAuthorIdAsync<BookTestModel>(1, 2, 1);
            var resultBook = result.FirstOrDefault();

            Assert.Equal(2, result.Count());
            Assert.Equal("title3", resultBook.Title);
            Assert.Equal(3, resultBook.Id);
        }

        [Fact]
        public async Task GetTopRatedBooksByAuthorIdShouldReturnCorrectCount()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            await db.Books.AddRangeAsync(
                    new Book
                    {
                        Title = "title1",
                        AuthorId = 1,
                        Ratings = new List<StarRating> { new StarRating { BookId = 1, UserId = "userId", Rate = 1 } },
                    },
                    new Book
                    {
                        Title = "title2",
                        AuthorId = 1,
                        Ratings = new List<StarRating> { new StarRating { BookId = 2, UserId = "userId", Rate = 5 } },
                    },
                    new Book
                    {
                        Title = "title3",
                        AuthorId = 1,
                        Ratings = new List<StarRating> { new StarRating { BookId = 3, UserId = "userId", Rate = 3 } },
                    });
            await db.SaveChangesAsync();

            var booksService = new BooksService(db);

            var result = await booksService.GetTopRatedBooksByAuthorIdAsync<BookTestModel>(1, 10);

            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task GetTopRatedBooksByAuthorIdShouldReturnCollectionWithCorrectOrder()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            await db.Books.AddRangeAsync(
                    new Book
                    {
                        Title = "title1",
                        AuthorId = 1,
                        Ratings = new List<StarRating> { new StarRating { BookId = 1, UserId = "userId", Rate = 1 } },
                    },
                    new Book
                    {
                        Title = "title2",
                        AuthorId = 1,
                        Ratings = new List<StarRating> { new StarRating { BookId = 2, UserId = "userId", Rate = 5 } },
                    },
                    new Book
                    {
                        Title = "title3",
                        AuthorId = 1,
                        Ratings = new List<StarRating> { new StarRating { BookId = 3, UserId = "userId", Rate = 3 } },
                    });
            await db.SaveChangesAsync();

            var booksService = new BooksService(db);

            var result = await booksService.GetTopRatedBooksByAuthorIdAsync<BookTestModel>(1, 10);
            var resultBook = result.FirstOrDefault();

            Assert.Equal(3, result.Count());
            Assert.Equal(2, resultBook.Id);
            Assert.Equal("title2", resultBook.Title);
        }

        [Fact]
        public async Task GetAllBooksShouldReturnAllBooks()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            await db.Books.AddRangeAsync(
                    new Book
                    {
                        Title = "title1",
                    },
                    new Book
                    {
                        Title = "title2",
                    },
                    new Book
                    {
                        Title = "title3",
                    });
            await db.SaveChangesAsync();

            var booksService = new BooksService(db);

            var result = await booksService.GetAllBooksAsync<BookTestModel>();

            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task GetAllBooksShouldNotReturnDeletedBooks()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            await db.Books.AddRangeAsync(
                    new Book
                    {
                        Title = "title1",
                    },
                    new Book
                    {
                        Title = "title2",
                    },
                    new Book
                    {
                        Title = "title3",
                        IsDeleted = true,
                        DeletedOn = DateTime.UtcNow,
                    });
            await db.SaveChangesAsync();

            var booksService = new BooksService(db);

            var result = await booksService.GetAllBooksAsync<BookTestModel>();

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetAllBooksShouldReturnCorrectBooksCountWhenTakeAndSkipAreGiven()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            await db.Books.AddRangeAsync(
                    new Book
                    {
                        Title = "title1",
                    },
                    new Book
                    {
                        Title = "title2",
                    },
                    new Book
                    {
                        Title = "title3",
                    });
            await db.SaveChangesAsync();

            var booksService = new BooksService(db);

            var result = await booksService.GetAllBooksAsync<BookTestModel>(1, 1);

            Assert.Single(result);
        }

        [Fact]
        public async Task GetTopRatedBooksShouldReturnAllTopRatedBooks()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            await db.Books.AddRangeAsync(
                    new Book
                    {
                        Title = "title1",
                        AuthorId = 1,
                        Ratings = new List<StarRating> { new StarRating { BookId = 1, UserId = "userId", Rate = 1 } },
                    },
                    new Book
                    {
                        Title = "title2",
                        AuthorId = 1,
                        Ratings = new List<StarRating> { new StarRating { BookId = 2, UserId = "userId", Rate = 5 } },
                    },
                    new Book
                    {
                        Title = "title3",
                        AuthorId = 1,
                        Ratings = new List<StarRating> { new StarRating { BookId = 3, UserId = "userId", Rate = 3 } },
                    });
            await db.SaveChangesAsync();

            var booksService = new BooksService(db);

            var result = await booksService.GetTopRatedBooksAsync<BookTestModel>();
            var resultBook = result.FirstOrDefault();

            Assert.Equal(3, result.Count());
            Assert.Equal(2, resultBook.Id);
            Assert.Equal("title2", resultBook.Title);
        }

        [Fact]
        public async Task GetTopRatedBooksShouldReturnTopRatedBooksWithTakeAndSkip()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            await db.Books.AddRangeAsync(
                    new Book
                    {
                        Title = "title1",
                        AuthorId = 1,
                        Ratings = new List<StarRating> { new StarRating { BookId = 1, UserId = "userId", Rate = 1 } },
                    },
                    new Book
                    {
                        Title = "title2",
                        AuthorId = 1,
                        Ratings = new List<StarRating> { new StarRating { BookId = 2, UserId = "userId", Rate = 5 } },
                    },
                    new Book
                    {
                        Title = "title3",
                        AuthorId = 1,
                        Ratings = new List<StarRating> { new StarRating { BookId = 3, UserId = "userId", Rate = 3 } },
                    });
            await db.SaveChangesAsync();

            var booksService = new BooksService(db);

            var result = await booksService.GetTopRatedBooksAsync<BookTestModel>(2, 1);
            var resultBook = result.FirstOrDefault();

            Assert.Equal(2, result.Count());
            Assert.Equal(3, resultBook.Id);
            Assert.Equal("title3", resultBook.Title);
        }

        [Fact]
        public async Task GetTopRatedBooksShouldNotReturnDeletedBooks()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            await db.Books.AddRangeAsync(
                    new Book
                    {
                        Title = "title1",
                        AuthorId = 1,
                        Ratings = new List<StarRating> { new StarRating { BookId = 1, UserId = "userId", Rate = 1 } },
                        IsDeleted = true,
                        DeletedOn = DateTime.UtcNow,
                    },
                    new Book
                    {
                        Title = "title2",
                        AuthorId = 1,
                        Ratings = new List<StarRating> { new StarRating { BookId = 2, UserId = "userId", Rate = 5 } },
                    },
                    new Book
                    {
                        Title = "title3",
                        AuthorId = 1,
                        Ratings = new List<StarRating> { new StarRating { BookId = 3, UserId = "userId", Rate = 3 } },
                    });
            await db.SaveChangesAsync();

            var booksService = new BooksService(db);

            var result = await booksService.GetTopRatedBooksAsync<BookTestModel>();

            Assert.Equal(2, result.Count());
        }

        public class BookTestModel : IMapFrom<Book>
        {
            public int Id { get; set; }

            public string Title { get; set; }

            public string Summary { get; set; }

            public DateTime CreatedOn { get; set; }

            public bool IsDeleted { get; set; }

            public DateTime DeletedOn { get; set; }
        }
    }
}
