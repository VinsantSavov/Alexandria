namespace Alexandria.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using Alexandria.Data;
    using Alexandria.Data.Models;
    using Alexandria.Data.Models.Enums;
    using Alexandria.Services.Mapping;
    using Alexandria.Services.Reviews;
    using Microsoft.EntityFrameworkCore;

    using Xunit;

    public class ReviewsServiceTest
    {
        public ReviewsServiceTest()
        {
            AutoMapperConfig.RegisterMappings(typeof(ReviewTestModel).GetTypeInfo().Assembly);
        }

        [Theory]
        [InlineData("description1", "author1", 1, ReadingProgress.Finished, true)]
        [InlineData("description2", "author2", 2, ReadingProgress.StartedReading, false)]
        [InlineData("description3", "author3", 3, ReadingProgress.InTheMiddleOfReading, false)]
        public async Task CreateReviewShouldAddInDatabase(string description, string authorId, int bookId, ReadingProgress progress, bool thisEdition)
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            var reviewsService = new ReviewsService(db);

            await reviewsService.CreateReviewAsync(description, authorId, bookId, progress, thisEdition);

            var result = await db.Reviews.FirstOrDefaultAsync();

            Assert.Equal(1, await db.Reviews.CountAsync());
            Assert.Equal(description, result.Description);
            Assert.Equal(authorId, result.AuthorId);
            Assert.Equal(bookId, result.BookId);
            Assert.Equal(progress, result.ReadingProgress);
            Assert.Equal(thisEdition, result.ThisEdition);
            Assert.Null(result.ParentId);
        }

        [Fact]
        public async Task CreateReviewShouldAddParentId()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            var reviewsService = new ReviewsService(db);

            await reviewsService.CreateReviewAsync("description1", "author1", 1, ReadingProgress.ToRead, true, 10);

            var result = await db.Reviews.FirstOrDefaultAsync();

            Assert.Equal(1, await db.Reviews.CountAsync());
            Assert.Equal(10, result.ParentId);
        }

        [Fact]
        public async Task CreateReviewShouldReturnCorrectId()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            var reviewsService = new ReviewsService(db);

            var resultId = await reviewsService.CreateReviewAsync("description1", "author1", 1, ReadingProgress.ToRead, true, 10);

            Assert.Equal(1, resultId);
        }

        [Fact]
        public async Task DeleteReviewShouldSetIsDeletedAndDeletedOn()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.AddRangeAsync(
                new Review
                {
                    Description = "description1",
                    AuthorId = "author1",
                    BookId = 1,
                    ReadingProgress = ReadingProgress.Finished,
                },
                new Review
                {
                    Description = "description2",
                    AuthorId = "author2",
                    BookId = 1,
                    ReadingProgress = ReadingProgress.Finished,
                });
            await db.SaveChangesAsync();

            var reviewsService = new ReviewsService(db);

            await reviewsService.DeleteReviewByIdAsync(1);
            var result = await db.Reviews.FirstOrDefaultAsync();

            Assert.NotNull(result.DeletedOn);
            Assert.True(result.IsDeleted);
        }

        [Fact]
        public async Task AreReviewsAboutSameBookShouldReturnTrueIfTheyAre()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.AddRangeAsync(
                new Review
                {
                    Description = "description1",
                    AuthorId = "author1",
                    BookId = 1,
                    ReadingProgress = ReadingProgress.Finished,
                },
                new Review
                {
                    Description = "description2",
                    AuthorId = "author2",
                    BookId = 1,
                    ReadingProgress = ReadingProgress.Finished,
                });
            await db.SaveChangesAsync();

            var reviewsService = new ReviewsService(db);

            var result = await reviewsService.AreReviewsAboutSameBookAsync(1, 1);

            Assert.True(result);
        }

        [Fact]
        public async Task AreReviewsAboutSameBookShouldReturnFalseIfTheyAreNot()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.AddRangeAsync(
                new Review
                {
                    Description = "description1",
                    AuthorId = "author1",
                    BookId = 1,
                    ReadingProgress = ReadingProgress.Finished,
                },
                new Review
                {
                    Description = "description2",
                    AuthorId = "author2",
                    BookId = 1,
                    ReadingProgress = ReadingProgress.Finished,
                });
            await db.SaveChangesAsync();

            var reviewsService = new ReviewsService(db);

            var result = await reviewsService.AreReviewsAboutSameBookAsync(1, 3);

            Assert.False(result);
        }

        [Fact]
        public async Task AreReviewsAboutSameBookShouldReturnFalseIfDeleted()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.AddRangeAsync(
                new Review
                {
                    Description = "description1",
                    AuthorId = "author1",
                    BookId = 1,
                    ReadingProgress = ReadingProgress.Finished,
                    IsDeleted = true,
                },
                new Review
                {
                    Description = "description2",
                    AuthorId = "author2",
                    BookId = 1,
                    ReadingProgress = ReadingProgress.Finished,
                });
            await db.SaveChangesAsync();

            var reviewsService = new ReviewsService(db);

            var result = await reviewsService.AreReviewsAboutSameBookAsync(1, 1);

            Assert.False(result);
        }

        [Fact]
        public async Task GetChildrenReviewsCountShouldReturnCorrectCount()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.AddRangeAsync(
                new Review
                {
                    Description = "description1",
                    AuthorId = "author1",
                    BookId = 1,
                },
                new Review
                {
                    Description = "description2",
                    AuthorId = "author2",
                    BookId = 1,
                    ParentId = 1,
                },
                new Review
                {
                    Description = "description3",
                    AuthorId = "author3",
                    BookId = 1,
                    ParentId = 1,
                },
                new Review
                {
                    Description = "description4",
                    AuthorId = "author4",
                    BookId = 1,
                    ParentId = 1,
                    IsDeleted = true,
                });
            await db.SaveChangesAsync();

            var reviewsService = new ReviewsService(db);

            var result = await reviewsService.GetChildrenReviewsCountByReviewIdAsync(1);

            Assert.Equal(2, result);
        }

        [Fact]
        public async Task GetReviewsCountByBookIdShouldReturnCorrectCount()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.AddRangeAsync(
                new Review
                {
                    Description = "description1",
                    AuthorId = "author1",
                    BookId = 1,
                },
                new Review
                {
                    Description = "description2",
                    AuthorId = "author2",
                    BookId = 1,
                    ParentId = 1,
                },
                new Review
                {
                    Description = "description3",
                    AuthorId = "author3",
                    BookId = 1,
                },
                new Review
                {
                    Description = "description4",
                    AuthorId = "author4",
                    BookId = 1,
                });
            await db.SaveChangesAsync();

            var reviewsService = new ReviewsService(db);

            var result = await reviewsService.GetReviewsCountByBookIdAsync(1);

            Assert.Equal(3, result);
        }

        [Fact]
        public async Task GetReviewsCountByUserIdShouldReturnCorrectCount()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.AddRangeAsync(
                new Review
                {
                    Description = "description1",
                    AuthorId = "author1",
                    BookId = 1,
                },
                new Review
                {
                    Description = "description2",
                    AuthorId = "author1",
                    BookId = 1,
                    ParentId = 1,
                },
                new Review
                {
                    Description = "description3",
                    AuthorId = "author1",
                    BookId = 1,
                },
                new Review
                {
                    Description = "description4",
                    AuthorId = "author1",
                    BookId = 1,
                });
            await db.SaveChangesAsync();

            var reviewsService = new ReviewsService(db);

            var result = await reviewsService.GetReviewsCountByUserIdAsync("author1");

            Assert.Equal(3, result);
        }

        [Fact]
        public async Task GetAuthorIdByIdShouldReturnCorrectId()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.AddRangeAsync(
                new Review
                {
                    Description = "description1",
                    AuthorId = "author1",
                    BookId = 1,
                },
                new Review
                {
                    Description = "description2",
                    AuthorId = "author1",
                    BookId = 1,
                    ParentId = 1,
                });
            await db.SaveChangesAsync();

            var reviewsService = new ReviewsService(db);

            var result = await reviewsService.GetAuthorIdByIdAsync(1);

            Assert.Equal("author1", result);
        }

        [Fact]
        public async Task GetAuthorIdByIdShouldReturnNullIfDeleted()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.AddRangeAsync(
                new Review
                {
                    Description = "description1",
                    AuthorId = "author1",
                    BookId = 1,
                    IsDeleted = true,
                },
                new Review
                {
                    Description = "description2",
                    AuthorId = "author1",
                    BookId = 1,
                    ParentId = 1,
                });
            await db.SaveChangesAsync();

            var reviewsService = new ReviewsService(db);

            var result = await reviewsService.GetAuthorIdByIdAsync(1);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetAuthorIdByIdShouldReturnNullIfNotFound()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.AddRangeAsync(
                new Review
                {
                    Description = "description1",
                    AuthorId = "author1",
                    BookId = 1,
                },
                new Review
                {
                    Description = "description2",
                    AuthorId = "author1",
                    BookId = 1,
                    ParentId = 1,
                });
            await db.SaveChangesAsync();

            var reviewsService = new ReviewsService(db);

            var result = await reviewsService.GetAuthorIdByIdAsync(5);

            Assert.Null(result);
        }

        [Fact]
        public async Task DoesReviewIdExistShouldReturnTrueIfExists()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.AddRangeAsync(
                new Review
                {
                    Description = "description1",
                    AuthorId = "author1",
                    BookId = 1,
                },
                new Review
                {
                    Description = "description2",
                    AuthorId = "author1",
                    BookId = 1,
                    ParentId = 1,
                });
            await db.SaveChangesAsync();

            var reviewsService = new ReviewsService(db);

            var result = await reviewsService.DoesReviewIdExistAsync(1);

            Assert.True(result);
        }

        [Fact]
        public async Task DoesReviewIdExistShouldReturnFalseIfDeleted()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.AddRangeAsync(
                new Review
                {
                    Description = "description1",
                    AuthorId = "author1",
                    BookId = 1,
                    IsDeleted = true,
                },
                new Review
                {
                    Description = "description2",
                    AuthorId = "author1",
                    BookId = 1,
                    ParentId = 1,
                });
            await db.SaveChangesAsync();

            var reviewsService = new ReviewsService(db);

            var result = await reviewsService.DoesReviewIdExistAsync(1);

            Assert.False(result);
        }

        [Fact]
        public async Task DoesReviewIdExistShouldReturnFalseIfNotFound()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.AddRangeAsync(
                new Review
                {
                    Description = "description1",
                    AuthorId = "author1",
                    BookId = 1,
                },
                new Review
                {
                    Description = "description2",
                    AuthorId = "author1",
                    BookId = 1,
                    ParentId = 1,
                });
            await db.SaveChangesAsync();

            var reviewsService = new ReviewsService(db);

            var result = await reviewsService.DoesReviewIdExistAsync(5);

            Assert.False(result);
        }

        [Theory]
        [InlineData("description01", ReadingProgress.StartedReading, true)]
        [InlineData("description02", ReadingProgress.InTheMiddleOfReading, true)]
        [InlineData("description03", ReadingProgress.ToRead, false)]
        public async Task EditReviewShouldWorkCorrectly(string description, ReadingProgress progress, bool thisEdition)
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.AddAsync(
                new Review
                {
                    Description = "description1",
                    AuthorId = "author1",
                    BookId = 1,
                    ReadingProgress = ReadingProgress.Unknown,
                    ThisEdition = false,
                });
            await db.SaveChangesAsync();

            var reviewsService = new ReviewsService(db);

            await reviewsService.EditReviewAsync(1, description, progress, thisEdition);
            var result = await db.Reviews.FirstOrDefaultAsync();

            Assert.NotNull(result.ModifiedOn);
            Assert.Equal(description, result.Description);
            Assert.Equal(progress, result.ReadingProgress);
            Assert.Equal(thisEdition, result.ThisEdition);
        }

        [Fact]
        public async Task GetAllReviewsByAuthorIdShouldReturnAllReviews()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.AddRangeAsync(
                new Review
                {
                    Description = "description1",
                    AuthorId = "author1",
                },
                new Review
                {
                    Description = "description1",
                    AuthorId = "author1",
                },
                new Review
                {
                    Description = "description1",
                    AuthorId = "author1",
                });
            await db.SaveChangesAsync();

            var reviewsService = new ReviewsService(db);

            var result = await reviewsService.GetAllReviewsByAuthorIdAsync<ReviewTestModel>("author1");

            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task GetAllReviewsByAuthorIdShouldNotReturnDeletedAndReviewsWithParentId()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.AddRangeAsync(
                new Review
                {
                    Description = "description1",
                    AuthorId = "author1",
                },
                new Review
                {
                    Description = "description2",
                    AuthorId = "author1",
                },
                new Review
                {
                    Description = "description3",
                    AuthorId = "author1",
                },
                new Review
                {
                    Description = "description4",
                    AuthorId = "author1",
                    ParentId = 1,
                },
                new Review
                {
                    Description = "description5",
                    AuthorId = "author1",
                    IsDeleted = true,
                });
            await db.SaveChangesAsync();

            var reviewsService = new ReviewsService(db);

            var result = await reviewsService.GetAllReviewsByAuthorIdAsync<ReviewTestModel>("author1");

            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task GetAllReviewsByAuthorIdShouldReturnCorrectResultWithTakeAndSkip()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.AddRangeAsync(
                new Review
                {
                    Description = "description1",
                    AuthorId = "author1",
                },
                new Review
                {
                    Description = "description2",
                    AuthorId = "author1",
                },
                new Review
                {
                    Description = "description3",
                    AuthorId = "author1",
                },
                new Review
                {
                    Description = "description4",
                    AuthorId = "author1",
                    ParentId = 1,
                });
            await db.SaveChangesAsync();

            var reviewsService = new ReviewsService(db);

            var result = await reviewsService.GetAllReviewsByAuthorIdAsync<ReviewTestModel>("author1", 2, 1);

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetAllReviewsByAuthorIdShouldReturnCollectionWithCorrectOrder()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.AddRangeAsync(
                new Review
                {
                    Description = "description1",
                    AuthorId = "author1",
                    Likes = new List<Like> { new Like { ReviewId = 1, UserId = "1", IsLiked = true } },
                },
                new Review
                {
                    Description = "description2",
                    AuthorId = "author1",
                    Likes = new List<Like> { new Like { ReviewId = 2, UserId = "1", IsLiked = false } },
                },
                new Review
                {
                    Description = "description3",
                    AuthorId = "author1",
                    Likes = new List<Like> { new Like { ReviewId = 3, UserId = "1", IsLiked = true }, new Like { ReviewId = 3, UserId = "1", IsLiked = true } },
                },
                new Review
                {
                    Description = "description4",
                    AuthorId = "author1",
                });
            await db.SaveChangesAsync();

            var reviewsService = new ReviewsService(db);

            var result = await reviewsService.GetAllReviewsByAuthorIdAsync<ReviewTestModel>("author1");
            var resultReviews = result.ToArray();

            Assert.Equal(4, resultReviews.Count());
            Assert.Equal("description3", resultReviews[0].Description);
            Assert.Equal("description1", resultReviews[1].Description);
        }

        [Fact]
        public async Task GetAllReviewsByBookIdShouldReturnAllReviews()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.AddRangeAsync(
                new Review
                {
                    Description = "description1",
                    AuthorId = "author1",
                    BookId = 1,
                },
                new Review
                {
                    Description = "description2",
                    AuthorId = "author1",
                    BookId = 1,
                },
                new Review
                {
                    Description = "description3",
                    AuthorId = "author1",
                    BookId = 1,
                },
                new Review
                {
                    Description = "description4",
                    AuthorId = "author1",
                    BookId = 2,
                });
            await db.SaveChangesAsync();

            var reviewsService = new ReviewsService(db);

            var result = await reviewsService.GetAllReviewsByBookIdAsync<ReviewTestModel>(1);

            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task GetAllReviewsByBookIdShouldNotReturnDeletedAndReviewsWithParentId()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.AddRangeAsync(
                new Review
                {
                    Description = "description1",
                    AuthorId = "author1",
                    BookId = 1,
                },
                new Review
                {
                    Description = "description2",
                    AuthorId = "author1",
                    BookId = 1,
                    ParentId = 1,
                },
                new Review
                {
                    Description = "description3",
                    AuthorId = "author1",
                    BookId = 1,
                    IsDeleted = true,
                },
                new Review
                {
                    Description = "description4",
                    AuthorId = "author1",
                    BookId = 1,
                });
            await db.SaveChangesAsync();

            var reviewsService = new ReviewsService(db);

            var result = await reviewsService.GetAllReviewsByBookIdAsync<ReviewTestModel>(1);

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetAllReviewsByBookIdShouldReturnCorrectResultWhenTakeAndSkipAreUsed()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.AddRangeAsync(
                new Review
                {
                    Description = "description1",
                    AuthorId = "author1",
                    BookId = 1,
                },
                new Review
                {
                    Description = "description2",
                    AuthorId = "author1",
                    BookId = 1,
                },
                new Review
                {
                    Description = "description3",
                    AuthorId = "author1",
                    BookId = 1,
                },
                new Review
                {
                    Description = "description4",
                    AuthorId = "author1",
                    BookId = 1,
                });
            await db.SaveChangesAsync();

            var reviewsService = new ReviewsService(db);

            var result = await reviewsService.GetAllReviewsByBookIdAsync<ReviewTestModel>(1, 10, 1);

            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task GetChildrenReviewsByReviewIdShouldReturnOnlyChildrenReviews()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.AddRangeAsync(
                new Review
                {
                    Description = "description1",
                    AuthorId = "author1",
                    BookId = 1,
                },
                new Review
                {
                    Description = "description2",
                    AuthorId = "author1",
                    BookId = 1,
                    ParentId = 1,
                },
                new Review
                {
                    Description = "description3",
                    AuthorId = "author1",
                    BookId = 1,
                    ParentId = 1,
                },
                new Review
                {
                    Description = "description4",
                    AuthorId = "author1",
                    BookId = 1,
                    ParentId = 2,
                });
            await db.SaveChangesAsync();

            var reviewsService = new ReviewsService(db);

            var result = await reviewsService.GetChildrenReviewsByReviewIdAsync<ReviewTestModel>(1);
            var resultReview = result.FirstOrDefault();

            Assert.Equal(2, result.Count());
            Assert.Equal("description2", resultReview.Description);
        }

        [Fact]
        public async Task GetChildrenReviewsByReviewIdShouldReturnCorrectResultWhenTakeAndSkipAreUsed()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.AddRangeAsync(
                new Review
                {
                    Description = "description1",
                    AuthorId = "author1",
                    BookId = 1,
                },
                new Review
                {
                    Description = "description2",
                    AuthorId = "author1",
                    BookId = 1,
                    ParentId = 1,
                },
                new Review
                {
                    Description = "description3",
                    AuthorId = "author1",
                    BookId = 1,
                    ParentId = 1,
                },
                new Review
                {
                    Description = "description4",
                    AuthorId = "author1",
                    BookId = 1,
                    ParentId = 1,
                });
            await db.SaveChangesAsync();

            var reviewsService = new ReviewsService(db);

            var result = await reviewsService.GetChildrenReviewsByReviewIdAsync<ReviewTestModel>(1, 5, 1);
            var resultReview = result.FirstOrDefault();

            Assert.Equal(2, result.Count());
            Assert.Equal("description3", resultReview.Description);
        }

        [Fact]
        public async Task GetReviewByIdShouldReturnCorrectReview()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.AddRangeAsync(
                new Review
                {
                    Description = "description1",
                    AuthorId = "author1",
                    BookId = 1,
                },
                new Review
                {
                    Description = "description2",
                    AuthorId = "author1",
                    BookId = 1,
                    ParentId = 1,
                });
            await db.SaveChangesAsync();

            var reviewsService = new ReviewsService(db);

            var result = await reviewsService.GetReviewByIdAsync<ReviewTestModel>(2);

            Assert.Equal("description2", result.Description);
            Assert.NotNull(result.ParentId);
        }

        [Fact]
        public async Task GetReviewByIdShouldReturnNullIfDeleted()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.AddRangeAsync(
                new Review
                {
                    Description = "description1",
                    AuthorId = "author1",
                    BookId = 1,
                },
                new Review
                {
                    Description = "description2",
                    AuthorId = "author1",
                    BookId = 1,
                    ParentId = 1,
                    IsDeleted = true,
                });
            await db.SaveChangesAsync();

            var reviewsService = new ReviewsService(db);

            var result = await reviewsService.GetReviewByIdAsync<ReviewTestModel>(2);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetReviewByIdShouldReturnNullIfNotFound()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.AddRangeAsync(
                new Review
                {
                    Description = "description1",
                    AuthorId = "author1",
                    BookId = 1,
                },
                new Review
                {
                    Description = "description2",
                    AuthorId = "author1",
                    BookId = 1,
                });
            await db.SaveChangesAsync();

            var reviewsService = new ReviewsService(db);

            var result = await reviewsService.GetReviewByIdAsync<ReviewTestModel>(5);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetTopReviewsByBookIdShouldReturnTopReviews()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.AddRangeAsync(
                new Review
                {
                    Description = "description1",
                    AuthorId = "author1",
                    BookId = 1,
                },
                new Review
                {
                    Description = "description2",
                    AuthorId = "author1",
                    BookId = 1,
                    ParentId = 1,
                },
                new Review
                {
                    Description = "description3",
                    AuthorId = "author1",
                    BookId = 1,
                },
                new Review
                {
                    Description = "description4",
                    AuthorId = "author1",
                    BookId = 1,
                });
            await db.SaveChangesAsync();

            var reviewsService = new ReviewsService(db);

            var result = await reviewsService.GetTopReviewsByBookIdAsync<ReviewTestModel>(1, 5);

            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task GetTopReviewsByBookIdShouldNotReturnDeletedAndReviewsWithParentId()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.AddRangeAsync(
                new Review
                {
                    Description = "description1",
                    AuthorId = "author1",
                    BookId = 1,
                },
                new Review
                {
                    Description = "description2",
                    AuthorId = "author1",
                    BookId = 1,
                    ParentId = 1,
                },
                new Review
                {
                    Description = "description3",
                    AuthorId = "author1",
                    BookId = 1,
                },
                new Review
                {
                    Description = "description4",
                    AuthorId = "author1",
                    BookId = 1,
                    IsDeleted = true,
                },
                new Review
                {
                    Description = "description4",
                    AuthorId = "author1",
                    BookId = 2,
                });
            await db.SaveChangesAsync();

            var reviewsService = new ReviewsService(db);

            var result = await reviewsService.GetTopReviewsByBookIdAsync<ReviewTestModel>(1, 5);

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetChildrenReviewsToReviewsAsyncShouldReturnAllChildrenReviews()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.AddRangeAsync(
                new Review
                {
                    Description = "description1",
                    AuthorId = "author1",
                    BookId = 1,
                },
                new Review
                {
                    Description = "description2",
                    AuthorId = "author1",
                    BookId = 1,
                },
                new Review
                {
                    Description = "description3",
                    AuthorId = "author1",
                    BookId = 1,
                    ParentId = 1,
                },
                new Review
                {
                    Description = "description4",
                    AuthorId = "author1",
                    BookId = 1,
                    ParentId = 2,
                },
                new Review
                {
                    Description = "description4",
                    AuthorId = "author1",
                    BookId = 1,
                    ParentId = 3,
                },
                new Review
                {
                    Description = "description4",
                    AuthorId = "author1",
                    BookId = 1,
                    ParentId = 4,
                });
            await db.SaveChangesAsync();

            var reviewsService = new ReviewsService(db);

            var ids = new List<int> { 1, 2 };
            var result = await reviewsService.GetChildrenReviewsToReviewsAsync<ReviewTestModel>(ids, 1);

            Assert.Equal(4, result.Count());
        }

        [Fact]
        public async Task GetChildrenReviewsToReviewsAsyncShouldNotReturnOtherChildrenReviews()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.AddRangeAsync(
                new Review
                {
                    Description = "description1",
                    AuthorId = "author1",
                    BookId = 1,
                },
                new Review
                {
                    Description = "description2",
                    AuthorId = "author1",
                    BookId = 1,
                },
                new Review
                {
                    Description = "description3",
                    AuthorId = "author1",
                    BookId = 1,
                    ParentId = 1,
                },
                new Review
                {
                    Description = "description4",
                    AuthorId = "author1",
                    BookId = 1,
                    ParentId = 2,
                },
                new Review
                {
                    Description = "description4",
                    AuthorId = "author1",
                    BookId = 1,
                    ParentId = 3,
                },
                new Review
                {
                    Description = "description4",
                    AuthorId = "author1",
                    BookId = 1,
                    ParentId = 10,
                });
            await db.SaveChangesAsync();

            var reviewsService = new ReviewsService(db);

            var ids = new List<int> { 1, 2 };
            var result = await reviewsService.GetChildrenReviewsToReviewsAsync<ReviewTestModel>(ids, 1);

            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task GetChildrenReviewsToReviewsAsyncShouldNotReturnDeletedReviews()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.AddRangeAsync(
                new Review
                {
                    Description = "description1",
                    AuthorId = "author1",
                    BookId = 1,
                },
                new Review
                {
                    Description = "description2",
                    AuthorId = "author1",
                    BookId = 1,
                },
                new Review
                {
                    Description = "description3",
                    AuthorId = "author1",
                    BookId = 1,
                    ParentId = 1,
                },
                new Review
                {
                    Description = "description4",
                    AuthorId = "author1",
                    BookId = 1,
                    ParentId = 2,
                    IsDeleted = true,
                },
                new Review
                {
                    Description = "description4",
                    AuthorId = "author1",
                    BookId = 1,
                    ParentId = 3,
                },
                new Review
                {
                    Description = "description4",
                    AuthorId = "author1",
                    BookId = 1,
                    ParentId = 4,
                });
            await db.SaveChangesAsync();

            var reviewsService = new ReviewsService(db);

            var ids = new List<int> { 1, 2 };
            var result = await reviewsService.GetChildrenReviewsToReviewsAsync<ReviewTestModel>(ids, 1);

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetChildrenReviewsToReviewsAsyncShouldNotReturnReviewsWithOtherBookId()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.AddRangeAsync(
                new Review
                {
                    Description = "description1",
                    AuthorId = "author1",
                    BookId = 1,
                },
                new Review
                {
                    Description = "description2",
                    AuthorId = "author1",
                    BookId = 1,
                },
                new Review
                {
                    Description = "description3",
                    AuthorId = "author1",
                    BookId = 1,
                    ParentId = 1,
                },
                new Review
                {
                    Description = "description4",
                    AuthorId = "author1",
                    BookId = 2,
                    ParentId = 2,
                },
                new Review
                {
                    Description = "description4",
                    AuthorId = "author1",
                    BookId = 1,
                    ParentId = 3,
                },
                new Review
                {
                    Description = "description4",
                    AuthorId = "author1",
                    BookId = 1,
                    ParentId = 4,
                });
            await db.SaveChangesAsync();

            var reviewsService = new ReviewsService(db);

            var ids = new List<int> { 1, 2 };
            var result = await reviewsService.GetChildrenReviewsToReviewsAsync<ReviewTestModel>(ids, 1);

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetChildrenReviewsToReviewsAsyncShouldNotReturnReviewsWithNoParent()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.AddRangeAsync(
                new Review
                {
                    Description = "description1",
                    AuthorId = "author1",
                    BookId = 1,
                },
                new Review
                {
                    Description = "description2",
                    AuthorId = "author1",
                    BookId = 1,
                },
                new Review
                {
                    Description = "description3",
                    AuthorId = "author1",
                    BookId = 1,
                    ParentId = 1,
                },
                new Review
                {
                    Description = "description4",
                    AuthorId = "author1",
                    BookId = 1,
                },
                new Review
                {
                    Description = "description4",
                    AuthorId = "author1",
                    BookId = 1,
                    ParentId = 3,
                },
                new Review
                {
                    Description = "description4",
                    AuthorId = "author1",
                    BookId = 1,
                    ParentId = 4,
                });
            await db.SaveChangesAsync();

            var reviewsService = new ReviewsService(db);

            var ids = new List<int> { 1, 2 };
            var result = await reviewsService.GetChildrenReviewsToReviewsAsync<ReviewTestModel>(ids, 1);

            Assert.Equal(2, result.Count());
        }

        public class ReviewTestModel : IMapFrom<Review>
        {
            public int Id { get; set; }

            public string Description { get; set; }

            public string AuthorId { get; set; }

            public int BookId { get; set; }

            public int? ParentId { get; set; }
        }
    }
}
