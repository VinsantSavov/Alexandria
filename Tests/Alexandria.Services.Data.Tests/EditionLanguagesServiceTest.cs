namespace Alexandria.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using Alexandria.Data;
    using Alexandria.Data.Models;
    using Alexandria.Services.EditionLanguages;
    using Alexandria.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    using Xunit;

    public class EditionLanguagesServiceTest
    {
        public EditionLanguagesServiceTest()
        {
            AutoMapperConfig.RegisterMappings(typeof(LanguageTestModel).GetTypeInfo().Assembly);
        }

        [Fact]
        public async Task DoesEditionLanguageExistShouldReturnTrueIfExists()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            await db.EditionLanguages.AddRangeAsync(
                new EditionLanguage
                {
                    Name = "test1",
                },
                new EditionLanguage
                {
                    Name = "test2",
                },
                new EditionLanguage
                {
                    Name = "test3",
                });

            await db.SaveChangesAsync();

            var languagesService = new EditionLanguagesService(db);

            Assert.True(await languagesService.DoesEditionLanguageIdExistAsync(1));
        }

        [Fact]
        public async Task DoesEditionLanguageExistShouldReturnFalseWhenNotFound()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            await db.EditionLanguages.AddRangeAsync(
                new EditionLanguage
                {
                    Name = "test1",
                },
                new EditionLanguage
                {
                    Name = "test2",
                },
                new EditionLanguage
                {
                    Name = "test3",
                });

            await db.SaveChangesAsync();

            var languagesService = new EditionLanguagesService(db);

            Assert.False(await languagesService.DoesEditionLanguageIdExistAsync(5));
        }

        [Fact]
        public async Task GetAllLanguagesShouldReturnTheCorrectCount()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            var languages = new List<EditionLanguage>();

            for (int i = 1; i <= 10; i++)
            {
                languages.Add(
                    new EditionLanguage
                    {
                        Name = $"test{i}",
                    });
            }

            await db.EditionLanguages.AddRangeAsync(languages);
            await db.SaveChangesAsync();

            var languagesService = new EditionLanguagesService(db);

            var result = await languagesService.GetAllLanguagesAsync<LanguageTestModel>();

            Assert.Equal(10, result.Count());
        }

        public class LanguageTestModel : IMapFrom<EditionLanguage>
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }
    }
}
