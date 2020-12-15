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
    using Alexandria.Services.Tags;
    using Microsoft.EntityFrameworkCore;

    using Xunit;

    public class TagsServiceTest
    {
        public TagsServiceTest()
        {
            AutoMapperConfig.RegisterMappings(typeof(TagTestModel).GetTypeInfo().Assembly);
        }

        [Fact]
        public async Task CreateMethodShoudAddTagInDatabase()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var db = new AlexandriaDbContext(options.Options);
            var tagsService = new TagsService(db);

            await tagsService.CreateTagAsync("test");

            Assert.Equal(1, await db.Tags.CountAsync());
        }

        [Fact]
        public async Task CreateMethodShouldAddTheCorrectTagInDatabase()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            var tagsService = new TagsService(db);

            await tagsService.CreateTagAsync("test");
            var result = await db.Tags.FirstOrDefaultAsync();

            Assert.Equal("test", result.Name);
        }

        [Fact]
        public async Task DeleteMethodShouldSetIsDeletedAndDeletedOn()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            await db.Tags.AddAsync(new Tag
            {
                Name = "test",
                CreatedOn = DateTime.UtcNow,
            });

            await db.SaveChangesAsync();

            var tagsService = new TagsService(db);
            await tagsService.DeleteTagByIdAsync(1);
            var result = await db.Tags.FirstOrDefaultAsync();

            Assert.NotNull(result.DeletedOn);
            Assert.True(result.IsDeleted);
        }

        [Fact]
        public async Task DoesTagIdExistShouldReturnTrueIfIdExists()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            await db.Tags.AddAsync(new Tag
            {
                Name = "test",
                CreatedOn = DateTime.UtcNow,
            });

            await db.SaveChangesAsync();

            var tagsService = new TagsService(db);

            Assert.True(await tagsService.DoesTagIdExistAsync(1));
        }

        [Fact]
        public async Task DoesTagIdExistShouldReturnFalseIfIdDoesntExist()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            await db.Tags.AddAsync(new Tag
            {
                Name = "test",
                CreatedOn = DateTime.UtcNow,
            });

            await db.SaveChangesAsync();

            var tagsService = new TagsService(db);

            Assert.False(await tagsService.DoesTagIdExistAsync(2));
        }

        [Fact]
        public async Task DoesTagIdExistShouldReturnFalseIfIdExistButIsDeleted()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            await db.Tags.AddAsync(new Tag
            {
                Name = "test",
                CreatedOn = DateTime.UtcNow,
                IsDeleted = true,
                DeletedOn = DateTime.UtcNow,
            });

            await db.SaveChangesAsync();

            var tagsService = new TagsService(db);

            Assert.False(await tagsService.DoesTagIdExistAsync(1));
        }

        [Fact]
        public async Task DoesTagNameExistShouldReturnTrueIfNameExists()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            await db.Tags.AddAsync(new Tag
            {
                Name = "test",
                CreatedOn = DateTime.UtcNow,
            });

            await db.SaveChangesAsync();

            var tagsService = new TagsService(db);

            Assert.True(await tagsService.DoesTagNameExistAsync("test"));
        }

        [Fact]
        public async Task DoesTagNameExistShouldReturnFalseIfNameDoesntExist()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            await db.Tags.AddAsync(new Tag
            {
                Name = "test",
                CreatedOn = DateTime.UtcNow,
            });

            await db.SaveChangesAsync();

            var tagsService = new TagsService(db);

            Assert.False(await tagsService.DoesTagNameExistAsync("exist"));
        }

        [Fact]
        public async Task GetTagsCountMethodShouldReturnTheCorrectCount()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            await db.Tags.AddRangeAsync(
                new Tag
                {
                    Name = "test1",
                    CreatedOn = DateTime.UtcNow,
                },
                new Tag
                {
                    Name = "test2",
                    CreatedOn = DateTime.UtcNow,
                },
                new Tag
                {
                    Name = "test3",
                    CreatedOn = DateTime.UtcNow,
                });

            await db.SaveChangesAsync();

            var tagsService = new TagsService(db);

            Assert.Equal(3, await tagsService.GetTagsCountAsync());
        }

        [Fact]
        public async Task GetTagsCountMethodShouldReturnTheCorrectCountIfThereAreDeletedTags()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            await db.Tags.AddRangeAsync(
                new Tag
                {
                    Name = "test1",
                    CreatedOn = DateTime.UtcNow,
                },
                new Tag
                {
                    Name = "test2",
                    CreatedOn = DateTime.UtcNow,
                    IsDeleted = true,
                    DeletedOn = DateTime.UtcNow,
                },
                new Tag
                {
                    Name = "test3",
                    CreatedOn = DateTime.UtcNow,
                });

            await db.SaveChangesAsync();

            var tagsService = new TagsService(db);

            Assert.Equal(2, await tagsService.GetTagsCountAsync());
        }

        [Fact]
        public async Task GetTagsCountMethodShouldReturnZeroIfThereArentAnyTags()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            var tagsService = new TagsService(db);

            Assert.Equal(0, await tagsService.GetTagsCountAsync());
        }

        [Fact]
        public async Task GetTagsCountMethodShouldReturnZeroIfAllTagsAreDeleted()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            await db.Tags.AddRangeAsync(
                new Tag
                {
                    Name = "test1",
                    CreatedOn = DateTime.UtcNow,
                    IsDeleted = true,
                    DeletedOn = DateTime.UtcNow,
                },
                new Tag
                {
                    Name = "test2",
                    CreatedOn = DateTime.UtcNow,
                    IsDeleted = true,
                    DeletedOn = DateTime.UtcNow,
                },
                new Tag
                {
                    Name = "test3",
                    CreatedOn = DateTime.UtcNow,
                    IsDeleted = true,
                    DeletedOn = DateTime.UtcNow,
                });

            await db.SaveChangesAsync();

            var tagsService = new TagsService(db);

            Assert.Equal(0, await tagsService.GetTagsCountAsync());
        }

        [Fact]
        public async Task GetTagByIdMethodShouldReturnTheCorrectTag()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            var expectedTag = new Tag
            {
                Name = "test1",
                CreatedOn = DateTime.UtcNow,
            };

            await db.Tags.AddRangeAsync(
                expectedTag,
                new Tag
                {
                    Name = "test2",
                    CreatedOn = DateTime.UtcNow,
                },
                new Tag
                {
                    Name = "test3",
                    CreatedOn = DateTime.UtcNow,
                });

            await db.SaveChangesAsync();

            var tagsService = new TagsService(db);

            var result = await tagsService.GetTagByIdAsync<TagTestModel>(1);

            Assert.Equal(expectedTag.Name, result.Name);
            Assert.Equal(expectedTag.CreatedOn, result.CreatedOn);
        }

        [Fact]
        public async Task GetTagByIdMethodShouldReturnNullIfTagIsDeleted()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            var expectedTag = new Tag
            {
                Name = "test1",
                CreatedOn = DateTime.UtcNow,
                IsDeleted = true,
                DeletedOn = DateTime.UtcNow,
            };

            await db.Tags.AddRangeAsync(
                expectedTag,
                new Tag
                {
                    Name = "test2",
                    CreatedOn = DateTime.UtcNow,
                },
                new Tag
                {
                    Name = "test3",
                    CreatedOn = DateTime.UtcNow,
                });

            await db.SaveChangesAsync();

            var tagsService = new TagsService(db);

            var result = await tagsService.GetTagByIdAsync<TagTestModel>(1);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetTagByIdMethodShouldReturnNullIfTagIsNotFound()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            var expectedTag = new Tag
            {
                Name = "test1",
                CreatedOn = DateTime.UtcNow,
                IsDeleted = true,
                DeletedOn = DateTime.UtcNow,
            };

            await db.Tags.AddRangeAsync(
                expectedTag,
                new Tag
                {
                    Name = "test2",
                    CreatedOn = DateTime.UtcNow,
                },
                new Tag
                {
                    Name = "test3",
                    CreatedOn = DateTime.UtcNow,
                });

            await db.SaveChangesAsync();

            var tagsService = new TagsService(db);

            var result = await tagsService.GetTagByIdAsync<TagTestModel>(5);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllTagsMethodShouldReturnAllTagsIfTakeAndSkipAreNotChanged()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            var tags = new List<Tag>();

            for (int i = 1; i <= 10; i++)
            {
                tags.Add(new Tag
                {
                    Name = $"test{i}",
                    CreatedOn = DateTime.UtcNow,
                });
            }

            await db.AddRangeAsync(tags);
            await db.SaveChangesAsync();

            var tagsService = new TagsService(db);
            var resultTags = await tagsService.GetAllTagsAsync<TagTestModel>();

            Assert.Equal(tags.Count(), resultTags.ToList().Count());
        }

        [Fact]
        public async Task GetAllTagsMethodShouldReturnZeroIfTagsAreDeleted()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            var tags = new List<Tag>();

            for (int i = 1; i <= 10; i++)
            {
                tags.Add(new Tag
                {
                    Name = $"test{i}",
                    CreatedOn = DateTime.UtcNow,
                    IsDeleted = true,
                    DeletedOn = DateTime.UtcNow,
                });
            }

            await db.AddRangeAsync(tags);
            await db.SaveChangesAsync();

            var tagsService = new TagsService(db);
            var resultTags = await tagsService.GetAllTagsAsync<TagTestModel>();

            Assert.Empty(resultTags.ToList());
        }

        [Fact]
        public async Task GetAllTagsMethodShouldReturnCorrectCountWhenTakeIsGiven()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            var tags = new List<Tag>();

            for (int i = 1; i <= 10; i++)
            {
                tags.Add(new Tag
                {
                    Name = $"test{i}",
                    CreatedOn = DateTime.UtcNow,
                });
            }

            await db.AddRangeAsync(tags);
            await db.SaveChangesAsync();

            var tagsService = new TagsService(db);
            var resultTags = await tagsService.GetAllTagsAsync<TagTestModel>(5);

            Assert.Equal(5, resultTags.ToList().Count());
        }

        [Fact]
        public async Task GetAllTagsMethodShouldReturnCorrectCountWhenSkipIsGiven()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            var tags = new List<Tag>();

            for (int i = 1; i <= 10; i++)
            {
                tags.Add(new Tag
                {
                    Name = $"test{i}",
                    CreatedOn = DateTime.UtcNow,
                });
            }

            await db.AddRangeAsync(tags);
            await db.SaveChangesAsync();

            var tagsService = new TagsService(db);
            var resultTags = await tagsService.GetAllTagsAsync<TagTestModel>(null, 5);

            Assert.Equal(5, resultTags.ToList().Count());
        }

        [Fact]
        public async Task GetAllTagsMethodShouldReturnCorrectCountWhenBothTakeAndSkipAreGiven()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            var tags = new List<Tag>();

            for (int i = 1; i <= 10; i++)
            {
                tags.Add(new Tag
                {
                    Name = $"test{i}",
                    CreatedOn = DateTime.UtcNow,
                });
            }

            await db.AddRangeAsync(tags);
            await db.SaveChangesAsync();

            var tagsService = new TagsService(db);
            var resultTags = await tagsService.GetAllTagsAsync<TagTestModel>(5, 5);

            Assert.Equal(5, resultTags.ToList().Count());
        }

        public class TagTestModel : IMapFrom<Tag>
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public DateTime CreatedOn { get; set; }
        }
    }
}
