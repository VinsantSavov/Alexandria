namespace Alexandria.Data.Seeding
{
    using Alexandria.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class EditionLanguagesSeeder : ISeeder
    {
        public async Task SeedAsync(AlexandriaDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.EditionLanguages.Any())
            {
                return;
            }

            var languages = new List<string>
            {
                "English",
                "Spanish",
                "Portuguese",
                "Russian",
                "Japanese",
                "Mandarin",
                "Turkish",
                "Korean",
                "French",
                "German",
                "Italian",
                "Vietnamese",
                "Polish",
                "Dutch",
                "Bulgarian",
                "Czech",
            };

            foreach (var language in languages)
            {
                await dbContext.EditionLanguages.AddAsync(new EditionLanguage
                {
                    Name = language,
                });
            }
        }
    }
}
