namespace Alexandria.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Alexandria.Data.Models;

    public class GenresSeeder : ISeeder
    {
        public async Task SeedAsync(AlexandriaDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Genres.Any())
            {
                return;
            }

            var genres = new List<(string Name, string Description)> { };

            foreach (var genre in genres)
            {
                await dbContext.AddAsync(new Genre
                {
                    Name = genre.Name,
                    Description = genre.Description,
                    CreatedOn = DateTime.UtcNow,
                });
            }
        }
    }
}
