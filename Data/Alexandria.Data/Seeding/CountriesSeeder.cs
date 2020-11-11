namespace Alexandria.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Alexandria.Data.Models;

    public class CountriesSeeder : ISeeder
    {
        public async Task SeedAsync(AlexandriaDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Countries.Any())
            {
                return;
            }

            var countries = new List<string>
            {
                "United States of America",
                "United Kingdom",
                "Vietnam",
                "Switzerland",
                "Sweden",
                "Spain",
                "South Korea",
                "South Africa",
                "Slovenia",
                "Slovakia",
                "Austria",
                "Australia",
                "Belgium",
                "Brazil",
                "Bulgaria",
                "Croatia",
                "Czechia (Czech Republic)",
                "Finland",
                "France",
                "Germany",
                "Greece",
                "Hungary",
                "Iceland",
                "Ireland",
                "Iraq",
                "Italy",
                "Japan",
                "Mexico",
                "Netherlands",
                "Poland",
                "Portugal",
                "Russia",
                "Turkey",
            };

            foreach (var country in countries)
            {
                await dbContext.Countries.AddAsync(new Country
                {
                    Name = country,
                });
            }
        }
    }
}
