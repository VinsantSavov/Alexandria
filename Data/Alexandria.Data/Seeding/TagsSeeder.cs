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

            var tags = new List<string>
            {
                "Scary",
                "Spooky",
                "Paranormal",
                "Apocalyptic",
                "Post-Apocalyptic",
                "Series",
                "Zombies",
                "Dystopia",
                "Adult",
                "Urban-Fantasy",
                "Gay",
                "Adult-Fiction",
                "Supernatural",
                "End Of The World",
                "Survival",
                "Young-Adult",
                "Sci-Fi Fantasy",
                "Trilogy",
                "News-Series",
                "Recommended",
                "Strong Female",
                "Death",
                "Utopia",
                "Pandemic",
                "Halloween",
                "Asia",
                "World War II",
                "Civil War",
                "Stuart",
                "Prehistoric",
                "Roman",
                "Regency",
                "Christian",
                "Muslim",
                "Western",
                "Erotic",
                "Gentle",
                "Violent",
                "Easy",
                "Demanding",
                "Unusual",
                "Optimistic",
                "Short",
                "Long",
                "Down To Earth",
                "Larger Than Life",
                "Funny",
                "Serious",
                "Happy",
                "Sad",
                "Unpredictable",
                "Conflict",
                "Quest",
                "Plot Twists",
                "Revelation",
                "Conventional",
                "Humor",
                "Accounting",
                "Algebra",
                "Division",
                "Geometry",
                "Subtraction",
                "Trigonometry",
                "Calculation",
                "Statistics",
                "Gardening",
                "Money",
                "Agriculture",
                "Computer Science",
                "Children",
                "Food",
                "Cookbooks",
                "Poetry",
                "Self Help",
                "Travel",
                "Superheroes",
            };

            foreach (var tag in tags)
            {
                await dbContext.Tags.AddAsync(new Tag
                {
                    Name = tag,
                    CreatedOn = DateTime.UtcNow,
                });
            }
        }
    }
}
