namespace Alexandria.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Alexandria.Data.Models;

    public class AwardsSeeder : ISeeder
    {
        public async Task SeedAsync(AlexandriaDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Awards.Any())
            {
                return;
            }

            var awards = new List<string>
            {
                "Goodreads Choice Award",
                "RITA Award by Romance Writers of America",
                "Locus Award",
                "Dorothy Canfield Fisher Children's Book Award",
                "National Book Award Finalist",
                "Lambda Literary Award",
                "Edgar Award",
                "World Fantasy Award",
                "Kindle Book Award",
                "National Book Award",
                "National Book Critics Circle Award",
                "Independent Publisher Book Award (IPPY)",
            };

            foreach (var award in awards)
            {
                await dbContext.Awards.AddAsync(new Award
                {
                    Name = award,
                    CreatedOn = DateTime.UtcNow,
                });
            }
        }
    }
}
