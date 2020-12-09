namespace Alexandria.Services.EditionLanguages
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Alexandria.Data;
    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class EditionLanguagesService : IEditionLanguagesService
    {
        private readonly AlexandriaDbContext db;

        public EditionLanguagesService(AlexandriaDbContext db)
        {
            this.db = db;
        }

        public async Task CreateEditionLanguageAsync(string name)
        {
            var language = new EditionLanguage
            {
                Name = name,
            };

            await this.db.EditionLanguages.AddAsync(language);
            await this.db.SaveChangesAsync();
        }

        public async Task<bool> DoesEditionLanguageIdExistAsync(int id)
            => await this.db.EditionLanguages.AnyAsync(l => l.Id == id);

        public async Task<IEnumerable<TModel>> GetAllLanguagesAsync<TModel>()
        {
            var languages = await this.db.EditionLanguages.AsNoTracking()
                                                          .OrderBy(l => l.Name)
                                                          .To<TModel>()
                                                          .ToListAsync();

            return languages;
        }

        public async Task<TModel> GetEditionLanguageByIdAsync<TModel>(int id)
        {
            var language = await this.db.EditionLanguages.Where(l => l.Id == id)
                                                   .To<TModel>()
                                                   .FirstOrDefaultAsync();

            return language;
        }
    }
}
