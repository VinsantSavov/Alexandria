namespace Alexandria.Services.Countries
{
    using System.Linq;
    using System.Threading.Tasks;

    using Alexandria.Data;
    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class CountriesService : ICountriesService
    {
        private readonly AlexandriaDbContext db;

        public CountriesService(AlexandriaDbContext db)
        {
            this.db = db;
        }

        public async Task CreateCountryAsync(string name)
        {
            var country = new Country
            {
                Name = name,
            };

            await this.db.Countries.AddAsync(country);
            await this.db.SaveChangesAsync();
        }

        public async Task<TModel> GetCountryByIdAsync<TModel>(int countryId)
        {
            var country = await this.db.Countries.Where(c => c.Id == countryId)
                                           .To<TModel>()
                                           .FirstOrDefaultAsync();

            return country;
        }
    }
}
