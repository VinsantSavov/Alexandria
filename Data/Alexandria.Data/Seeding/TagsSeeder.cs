namespace Alexandria.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Alexandria.Data.Models;

    public class TagsSeeder : ISeeder
    {
        public async Task SeedAsync(AlexandriaDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Tags.Any())
            {
                return;
            }

            var tags = new List<string> { };

            foreach (var tag in tags)
            {
                await dbContext.AddAsync(new Tag
                {
                    Name = tag,
                    CreatedOn = DateTime.UtcNow,
                });
            }
        }
    }
}
